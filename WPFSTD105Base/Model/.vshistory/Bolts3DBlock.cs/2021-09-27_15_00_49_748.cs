using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using GD_STD;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using WPFSTD105.Attribute;
using static WPFSTD105.Properties.SofSetting;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 螺栓群組
    /// </summary>
    public class Bolts3DBlock : Block, IBolts
    {
        /// <summary>
        /// 防止破圖
        /// </summary>
        const double h = 1.00e-2;
        /// <inheritdoc/>
        public GroupBoltsAttr Info { get; set; }

        /// <summary>
        /// 螺栓群組
        /// </summary>
        /// <param name="groupBoltsAttr">螺栓群組</param>
        /// <param name="name">圖塊名稱</param>
        public Bolts3DBlock(GroupBoltsAttr groupBoltsAttr) : base(groupBoltsAttr.GUID.ToString())
        {
            this.Info = groupBoltsAttr;
        }
        #region 私有屬性

        /// <summary>
        /// 螺栓 X 向列表
        /// </summary>
        /// <param name="boltY">y 向螺栓列表</param>
        /// <param name="value"></param>
        /// <returns></returns>
        private List<Mesh> BoltsX(List<Mesh> boltY, double value)
        {
            List<Mesh> result = new List<Mesh>();
            //List<Circle> circles1 = new List<Circle>();

            for (int i = 0; i < boltY.Count; i++)
            {
                Mesh bolt = (Mesh)boltY[i].Clone();
                BoltAttr boltAttr = (BoltAttr)((BoltAttr)boltY[i].EntityData).DeepClone();
                boltAttr.GUID = Guid.NewGuid();
                boltAttr.X += value;
                bolt.EntityData = boltAttr;

                bolt.Translate(value, 0, 0);
                result.Add(bolt);
            }
            //順便排序加工位置
            result.Reverse();
            return result;
        }
        /// <summary>
        /// 調整到適合的面
        /// </summary>
        /// <param name="angleInRadians"></param>
        /// <param name="axis"></param>
        private void Rotate(double angleInRadians, Vector3D axis)
        {
            if (Vector3D.AxisX == axis)
            {
                double value = 0d;
                if (angleInRadians > 0)
                    value = Info.Y;
                else
                    value = -Info.Y;
                Info.Y = Info.Z;
                Info.Z = value;
            }
        }
        #endregion

        #region 公開方法
        /// <summary>
        /// 創建螺栓群組
        /// </summary>
        public void CreateBolts()
        {
            List<Mesh> resultY = new List<Mesh>();//Y向螺栓結果(3D實體)，要給X向複製用
            //如果是打點改直徑
            if (this.Info.Mode == AXIS_MODE.POINT)
            {
                this.Info.Dia = 10;
            }
            double high = this.Info.t + h * 2;
            resultY.Add(Mesh.CreateCylinder(this.Info.Dia * 0.5, high, 36));//創建一個圓柱體
            //如果是打點改變顏色
            if (this.Info.Mode == AXIS_MODE.POINT)
            {
                resultY[0].Color = System.Drawing.ColorTranslator.FromHtml(Default.Point);
            }
            //建立螺栓參數
            resultY[0].EntityData = new BoltAttr()
            {
                Dia = this.Info.Dia,
                t = this.Info.t,
                Face = this.Info.Face,
                Mode = this.Info.Mode,
                X = Info.X,
                Y = Info.Y,
                GUID = Guid.NewGuid()
            };
            if (Info.Face == GD_STD.Enum.FACE.BACK || Info.Face == GD_STD.Enum.FACE.FRONT)
            {
                this.Rotate(Math.PI / 2, Vector3D.AxisX);
            }
            //查看要附加的面在哪
            switch (Info.Face)
            {
                case GD_STD.Enum.FACE.TOP:
                    //resultY[0].Translate(0, 0, -h);
                    resultY[0].Translate(Info.X, Info.Y, Info.Z - h);
                    break;
                case GD_STD.Enum.FACE.FRONT:
                case GD_STD.Enum.FACE.BACK:
                    resultY[0].Rotate(Math.PI / 2, Vector3D.AxisX);
                    //resultY[0].Translate(0, h, 0);
                    resultY[0].Translate(Info.X, Info.Y + h, Info.Z);
                    break;
                default:
                    break;
            }

            //resultY[0].Translate(this.Info.X, this.Info.Y, this.Info.Z - h); //使用相對移動到指定位置。
            //先產生Y向，原因是，三軸機先Y向移動工作效率較大
            List<double> dYList = Info.dYs;
            for (int i = 1; i < this.Info.yCount; i++)
            {
                //複製物件
                Mesh bolt = (Mesh)resultY[i - 1].Clone();
                BoltAttr boltAttr = (BoltAttr)((BoltAttr)resultY[i - 1].EntityData).DeepClone();
                boltAttr.GUID = Guid.NewGuid();
                double valueY = 0d;
                if (i - 1 < dYList.Count) //判斷孔位Y向矩陣列表是否有超出長度
                    valueY = dYList[i - 1];
                else
                    valueY = dYList[dYList.Count - 1];

                switch (Info.Face)
                {
                    case GD_STD.Enum.FACE.TOP:
                        bolt.Translate(0, valueY, 0);//使用相對移動到指定位置。
                        break;
                    case GD_STD.Enum.FACE.BACK:
                    case GD_STD.Enum.FACE.FRONT:
                        bolt.Translate(0, 0, valueY);//使用相對移動到指定位置。
                        break;
                    default:
                        break;
                }
                boltAttr.Y += valueY;
                bolt.EntityData = boltAttr;
                resultY.Add(bolt);//存取物件

                //複製一個圓形線條
            }
            this.Entities.AddRange(resultY);//存到列表內
                                            //產生X向螺栓
            List<double> dXList = Info.dXs;
            double valueX = 0d;
            for (int i = 1; i < this.Info.xCount; i++)
            {
                if (i < dXList.Count) //判斷孔位Y向矩陣列表是否有超出長度
                    valueX = dXList[i - 1];
                else
                    valueX = dXList[dXList.Count - 1];

                resultY = BoltsX(resultY, valueX);
                this.Entities.AddRange(resultY);
            }
        }
        /// <summary>
        /// 移動螺栓位置
        /// </summary>
        /// <param name="vector3D"></param>
        public void Move(Vector3D vector3D)
        {
            for (int i = 0; i < this.Entities.Count; i++)
            {
                this.Entities[i].Translate(vector3D);
            }
        }
        /// <summary>
        /// 加入螺栓圖塊到模型
        /// </summary>
        /// <param name="attr">螺栓設定檔</param>
        /// <param name="model">要被加入的模型</param>
        /// <param name="block">加入到模型的參考圖塊</param>
        /// <returns>加入到模型的圖塊</returns>
        public static Bolts3DBlock AddBolts(GroupBoltsAttr attr, devDept.Eyeshot.Model model, out BlockReference block)
        {
            Bolts3DBlock result = new Bolts3DBlock(attr); //產生孔位圖塊
            result.CreateBolts();//創建孔位群組
            model.Blocks.Add(result);//加入孔位群組圖塊到模型
            block = new BlockReference(0, 0, 0, result.Name, 1, 1, 1, 0);//產生孔位群組參考圖塊
            block.EntityData = result.Info;
            block.Attributes.Add("Bolts", new AttributeReference(0, 0, 0));
            model.Entities.Add(block);//加入參考圖塊到模型

            return result;
        }
        ///// <summary>
        ///// 轉換 Codesys Memory 可用資料 
        ///// </summary>
        ///// <returns></returns>
        //public Drill[] ToCodesys()
        //{
        //    Drill[] result = new Drill[this.Entities.Count];

        //    for (int i = 0; i < this.Entities.Count; i++)
        //    {
        //        result[i] = ((Bolt)this.Entities[i]).ToCodesys(Info.Face);
        //    }
        //    return result;
        //}
        #endregion
    }
}

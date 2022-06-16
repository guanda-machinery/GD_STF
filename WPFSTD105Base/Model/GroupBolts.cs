using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using GD_STD;
using System;
using System.Collections.Generic;
using WPFSTD105.Attribute;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 螺栓群組
    /// </summary>
    public class GroupBolts : Block
    {
        /// <summary>
        /// 防止破圖
        /// </summary>
        const double h = 1.00e-2;
        public GroupBoltsAttr Info { get; set; }

        /// <summary>
        /// 螺栓群組
        /// </summary>
        /// <param name="groupBoltsAttr">螺栓群組</param>
        /// <param name="name">圖塊名稱</param>
        public GroupBolts(GroupBoltsAttr groupBoltsAttr) : base(groupBoltsAttr.GUID.ToString())
        {
            this.Info = groupBoltsAttr;
            //this.Info.Z -= h; 
            //CreateBolts();
        }
        #region 私有屬性

        /// <summary>
        /// 螺栓 X 向列表
        /// </summary>
        /// <param name="boltY">y 向螺栓列表</param>
        /// <param name="dx">X向間距</param>
        /// <returns></returns>
        private List<Bolt> BoltsX(List<Bolt> boltY, double value)
        {
            List<Bolt> result = new List<Bolt>();


            for (int i = 0; i < boltY.Count; i++)
            {
                Bolt bolt = (Bolt)boltY[i].Clone();
                bolt.EntityData = boltY[i].EntityData;
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
            //for (int i = 0; i < this.Entities.Count; i++)
            //{
            //    this.Entities[i].Rotate(angleInRadians, axis);
            //}
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

            List<Bolt> resultY = new List<Bolt>();//Y向螺栓結果，要給X向複製用

            resultY.Add(Mesh.CreateCone<Bolt>(this.Info.Dia * 0.5, this.Info.Dia * 0.5, this.Info.t + h * 2, 22));//創建一個圓柱體

            if (Info.Face == GD_STD.Enum.FACE.BACK || Info.Face == GD_STD.Enum.FACE.FRONT)
            {
                this.Rotate(Math.PI / 2, Vector3D.AxisX);
            }
            //建立螺栓參數
            resultY[0].EntityData = new BoltAttr()
            {
                Dia = this.Info.Dia,
                t = this.Info.t,
                Face = this.Info.Face,
                Mode = this.Info.Mode
            };
            //查看要附加的面在哪
            switch (Info.Face)
            {
                case GD_STD.Enum.FACE.TOP:
                    resultY[0].Translate(0, 0, -h);
                    //this.Info.Z -= h;
                    break;
                case GD_STD.Enum.FACE.FRONT:
                case GD_STD.Enum.FACE.BACK:
                    resultY[0].Rotate(Math.PI / 2, Vector3D.AxisX);
                    //this.Info.Y += h;
                    resultY[0].Translate(0, h, 0);
                    break;
                default:
                    break;
            }
            //resultY[0].Translate(this.Info.X, this.Info.Y, this.Info.Z - h); //使用相對移動到指定位置。
            //先產生Y向，原因是，三軸機先Y向移動工作效率較大
            List<double> dYList = Info.dYs;
            for (int i = 1; i < this.Info.yCount; i++)
            {
                Bolt bolt = (Bolt)resultY[i - 1].Clone();//複製物件
                bolt.EntityData = ((BoltAttr)resultY[i - 1].EntityData).DeepClone();
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

                resultY.Add(bolt);//存取物件
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
        /// 轉換 Codesys Memory 可用資料 
        /// </summary>
        /// <returns></returns>
        public Drill[] ToCodesys()
        {
            Drill[] result = new Drill[this.Entities.Count];

            for (int i = 0; i < this.Entities.Count; i++)
            {
                result[i] = ((Bolt)this.Entities[i]).ToCodesys();
            }
            return result;
        }
        #endregion
    }
}

using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using GD_STD;
using GD_STD.Enum;
using SectionData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// 鋼構資訊
        /// </summary>
        public SteelAttr steelAttr { get; set; }
        /// <summary>
        /// 若檢查時，孔群已不在鋼上，變為true
        /// </summary>
        public bool hasOutSteel { get; set; } = false;  
        /// <summary>
        /// 以實體創建螺
        /// </summary>
        /// <param name="meshes"></param>
        /// <param name="groupBoltsAttr"></param>
        public Bolts3DBlock(EntityList meshes, GroupBoltsAttr groupBoltsAttr) : base(groupBoltsAttr.GUID.ToString())
        {
            this.Entities.AddRange(meshes);
            this.Info = groupBoltsAttr;
            this.hasOutSteel = false;   
        }

        /// <summary>
        /// 螺栓群組
        /// </summary>
        /// <param name="groupBoltsAttr">螺栓群組</param>
        public Bolts3DBlock(GroupBoltsAttr groupBoltsAttr) : base(groupBoltsAttr.GUID.ToString())
        {
            this.Info = groupBoltsAttr;
            this.hasOutSteel = false;
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
        public void CreateBolts(devDept.Eyeshot.Model model, ref bool check, List<Mesh> meshes = null,bool isRotate = true)
        {
            check = true;
            SteelAttr steelAttr = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
            List<Mesh> resultY = new List<Mesh>();//Y向螺栓結果(3D實體)，要給X向複製用
            var originalEntities = this.Entities;
            //如果是打點改直徑
            if (this.Info.Mode == AXIS_MODE.POINT || this.Info.Mode == AXIS_MODE.HypotenusePOINT)
            {
                this.Info.Dia = 10;
            }
            double high = this.Info.t + h * 2;
            resultY.Add(Mesh.CreateCylinder(this.Info.Dia * 0.5, high, 36));//創建一個圓柱體// Vertices XY有值 Z=0
            //建立螺栓參數
            resultY[0].EntityData = new BoltAttr()
            {
                Dia = this.Info.Dia,
                t = this.Info.t,
                Face = this.Info.Face,
                Mode = this.Info.Mode,
                X = Info.X,
                Y = Info.Y,
                Z = Info.Z,
                GUID = Guid.NewGuid(),
                BlockName = Info.BlockName,
            };

            // 若起始座標小於半徑，不可加入
            if (Info.X < this.Info.Dia / 2 && Info.dX!= "0" && this.Info.Mode != AXIS_MODE.POINT && this.Info.Mode != AXIS_MODE.HypotenusePOINT)
            {
                check = false;
            }
            else
            {
                if ((Info.Face == GD_STD.Enum.FACE.BACK || Info.Face == GD_STD.Enum.FACE.FRONT) && isRotate)
                {
                    // 原孔群資訊依照條件算出之視角為TOP視角，需轉成前後視角，y=z,z=y(only XYZ)
                    this.Rotate(Math.PI / 2, Vector3D.AxisX);
                }

                //取得螺栓資料 // 此時 若前後視角 XY未對調
                BoltAttr boltAttr = (BoltAttr)resultY[0].EntityData;

                // 加工區域計算
                List<double> list = WorkingRange(steelAttr.Type, boltAttr, steelAttr);

                double y, z;

                #region 依Face處理
                //查看要附加的面在哪
                switch (Info.Face)
                {
                    case GD_STD.Enum.FACE.TOP:
                        //resultY[0].Translate(0, 0, -h);
                        resultY[0].Translate(Info.X, Info.Y, Info.Z - h);
                        break;
                    case GD_STD.Enum.FACE.FRONT:
                    case GD_STD.Enum.FACE.BACK:
                        // Vertices y=z,z=y，轉二維
                        // 此時Vertices與YZ未轉換
                        // Front
                        // EntityData XYZ有值
                        // Vertices XY有值 Z=0
                        resultY[0].Rotate(Math.PI / 2, Vector3D.AxisX);
                        // 此時Vertices與YZ皆轉換Vertices
                        // EntityData XYZ有值(不變)
                        // Vertices X有值 YZ對調(Y=0)


                        //resultY[0].Translate(0, h, 0);
                        // 平移座標至第一個螺栓的位置，y軸起始為z+圓柱高度(因為轉90度)
                        // boltAttr.Z + boltAttr.t 移除 + boltAttr.t，因為boltAttr.Z在取值時，已經加上了
                        //double a = 10, b = 20, c = 30;
                        //resultY[0].Translate(a, b, c);
                        resultY[0].Translate(Info.X, Info.Y + h, Info.Z);
                        // 此時XYZ不變
                        // Vertices XYZ有值 各加上boltAttr.X, boltAttr.Y + h, boltAttr.Z
                        break;
                    default:
                        break;
                }
                #endregion
                // 孔 #調整顏色
                resultY[0].Color = ColorTranslator.FromHtml(Default.Hole);
                resultY[0].ColorMethod = colorMethodType.byEntity;

                //如果是打點改變顏色
                if (this.Info.Mode == AXIS_MODE.POINT)
                {
                    resultY[0].Color = System.Drawing.ColorTranslator.FromHtml(Default.Point);
                }
                // 與暐淇討論後 斜邊打點 綠色(3D) #調整顏色
                if (this.Info.Mode == AXIS_MODE.HypotenusePOINT)
                {
                    resultY[0].Color = System.Drawing.ColorTranslator.FromHtml(Default.Point);
                }



                //resultY[0].Translate(this.Info.X, this.Info.Y, this.Info.Z - h); //使用相對移動到指定位置。
                //先產生Y向，原因是，三軸機先Y向移動工作效率較大
                List<double> dYList = Info.dYs;
                List<Mesh> boltList = new List<Mesh>();
                for (int i = 1; i < this.Info.yCount; i++)
                {
                    if (this.Info.Mode== AXIS_MODE.HypotenusePOINT)
                    {
                        continue;
                    }
                    //複製物件
                    Mesh bolt = (Mesh)resultY[i - 1].Clone();
                    BoltAttr boltAttrEach = (BoltAttr)((BoltAttr)resultY[i - 1].EntityData).DeepClone();
                    // 此時 boltAttrEach XYZ 與 resultY 相同
                    boltAttrEach.GUID = Guid.NewGuid();
                    double valueY = 0d;
                    if (i - 1 < dYList.Count) //判斷孔位Y向矩陣列表是否有超出長度
                        valueY = dYList[i - 1];
                    else
                        valueY = dYList[dYList.Count - 1];

                    switch (Info.Face)
                    {
                        // this.Info.yCoun = 1 為斜邊打點
                        case GD_STD.Enum.FACE.TOP:
                            bolt.Translate(0, valueY, 0);//使用相對移動到指定位置。
                            boltAttrEach.Y = boltAttrEach.Y + valueY;
                            if (this.Info.yCount != 1)
                            {
                                check = CheckWorkingRange(boltAttrEach.Mode,Info.Face, steelAttr.Type, boltAttrEach.Y, list);
                            }
                            //if (boltAttrEach.Y < list[0] || boltAttrEach.Y > list[1])
                            //{
                            //    // 不能加入
                            //    check = false;
                            //}
                            break;
                        case GD_STD.Enum.FACE.BACK:
                        case GD_STD.Enum.FACE.FRONT:
                            //bolt.Rotate(Math.PI / 2, Vector3D.AxisX);
                            // 此時 EntityData XYZ 有值 與resultY相同
                            // 此時 Vertices 有值 與resultY相同
                            bolt.Translate(0, 0, valueY);//使用相對移動到指定位置。// Vertices Z增加valueY
                            // 此時 bolt.EntityData XYZ 有值 與resultY相同
                            // 此時 bolt.Vertices 有值 resultY.Vertices XYZ 各加上0, 0, valueY
                            // y = boltAttrEach.Z;
                            // z = boltAttrEach.Y;
                            // boltAttrEach.Y = y;
                            // boltAttrEach.Z = z;
                            boltAttrEach.Y = boltAttrEach.Y + valueY;
                            if (this.Info.yCount != 1)
                            {
                                check = CheckWorkingRange(boltAttrEach.Mode, Info.Face, steelAttr.Type, boltAttrEach.Y, list);
                            }
                            //switch (steelAttr.Type)
                            //{
                            //    case OBJECT_TYPE.RH:
                            //    case OBJECT_TYPE.CH:
                            //    case OBJECT_TYPE.BH:
                            //        if (boltAttrEach.Y < list[0] || (boltAttrEach.Y > list[1] && boltAttrEach.Y < list[2]) || boltAttrEach.Y > list[3])
                            //        {
                            //            // 不能加入
                            //            check = false;
                            //        }
                            //        break;
                            //    case OBJECT_TYPE.C:
                            //        if (boltAttrEach.Y < list[0] || boltAttrEach.Y > list[1])
                            //        {
                            //            // 不能加入
                            //            check = false;
                            //        }
                            //        break;
                            //    case OBJECT_TYPE.BOX:
                            //        if (boltAttrEach.Y < list[2] || boltAttrEach.Y > list[3])
                            //        {
                            //            // 不能加入
                            //            check = false;
                            //        }
                            //        break;
                            //    case OBJECT_TYPE.L:
                            //    case OBJECT_TYPE.PLATE:
                            //    case OBJECT_TYPE.RB:
                            //    case OBJECT_TYPE.FB:
                            //    case OBJECT_TYPE.PIPE:
                            //    case OBJECT_TYPE.Unknown:
                            //        break;
                            //    default:
                            //        break;
                            //}
                            //y = boltAttrEach.Z;
                            //z = boltAttrEach.Y;
                            //boltAttrEach.Y = y;
                            //boltAttrEach.Z = z;
                            break;
                        default:
                            break;
                    }
                    //boltAttr.Y += valueY;
                    //if (check)
                    //{
                        bolt.EntityData = boltAttrEach;
                        //boltList.Add(bolt);
                        resultY.Add(bolt);//存取物件
                    //}
                    //else
                    //{
                    //    break;
                    //}
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
                if (meshes != null)
                {
                    this.Entities.Clear();
                    this.Entities.AddRange(meshes);
                }
            }
        }

        /// <summary>
        /// 檢查是否在可加工範圍
        /// </summary>
        /// <param name="type"></param>
        /// <param name="face"></param>
        /// <param name="checkValue"></param>
        /// <param name="workingRange"></param>
        /// <returns></returns>
        public static bool CheckWorkingRange(AXIS_MODE Mode,FACE face,OBJECT_TYPE type, double checkValue, List<double> workingRange) 
        {
            bool check = true;
            // 無加工範圍設定或打點 不判斷
            if (workingRange.Count == 0 || (Mode == AXIS_MODE.POINT || Mode == AXIS_MODE.HypotenusePOINT) )
            {
                return check;
            }
            switch (face)
            {
                case GD_STD.Enum.FACE.TOP:
                    if (checkValue < workingRange[0] || 
                        checkValue > workingRange[1])
                    {
                        check = false;
                    }
                    break;
                case GD_STD.Enum.FACE.BACK:
                case GD_STD.Enum.FACE.FRONT:
                    switch (type)
                    {
                        case OBJECT_TYPE.RH:
                        case OBJECT_TYPE.CH:
                        case OBJECT_TYPE.BH:
                            if (checkValue < workingRange[0] || 
                                (checkValue > workingRange[1] && checkValue < workingRange[2]) || 
                                checkValue > workingRange[3])
                            {
                                check = false;
                            }
                            break;
                        case OBJECT_TYPE.C:
                            if (checkValue < workingRange[0] || 
                                checkValue > workingRange[1])
                            {
                                check = false;
                            }
                            break;
                        case OBJECT_TYPE.BOX:
                            if (checkValue < workingRange[2] || 
                                checkValue > workingRange[3])
                            {
                                check = false;
                            }
                            break;
                        case OBJECT_TYPE.L:
                        case OBJECT_TYPE.PLATE:
                        case OBJECT_TYPE.RB:
                        case OBJECT_TYPE.FB:
                        case OBJECT_TYPE.PIPE:
                        case OBJECT_TYPE.Unknown:
                            break;
                        default:
                            break;
                    }
                    break;
            }
            return check;
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
        /// 檢查孔是否合乎規則
        /// 1.工作範圍中
        /// 2.在鋼體上
        /// </summary>
        /// <param name="result"></param>
        /// <param name="model"></param>
        /// <param name="steelAttr"></param>
        /// <param name="rotate">步驟前 Front或Back有CreateBolt 不用翻轉(false)</param>
        /// <returns></returns>
        public static bool CheckBolts(devDept.Eyeshot.Model model,bool rotate = true, string guid = "")
        {
            // 最終檢驗值
            bool check = true;
            // 鋼構資訊
            //SteelAttr steelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
            SteelAttr steelAttr = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
            OBJECT_TYPE type = steelAttr.Type;
            // 取得所有孔
            for (int i = 0; i < model.Entities.Count; i++)//逐步產生 螺栓 3d 模型實體
            {
                if (model.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                {
                    if (!string.IsNullOrEmpty(guid))
                    {
                        //if (((GroupBoltsAttr)model.Entities[i].EntityData).GUID.Value != Guid.Parse(guid))
                        if (((GroupBoltsAttr)model.Entities[i].EntityData).GUID.Value != Guid.Parse(guid))
                        {
                            continue;
                        }
                    }
                    BlockReference blockReference = (BlockReference)model.Entities[i]; //取得參考圖塊
                    BoltAttr bolt = (BoltAttr)model.Entities[i].EntityData;
                    // 加工區域計算
                    List<double> list = WorkingRange(type, bolt, steelAttr);
                    // 符合加工區域
                    // 檢查座標依平面而有異動
                    double checkValue = 0;
                    
                    //checkValue = bolt.Y;
                    switch (bolt.Face)
                    {
                        case FACE.TOP:
                            checkValue = bolt.Y;
                            break;
                        case FACE.FRONT:
                        case FACE.BACK:
                            checkValue = bolt.Z;
                            break;
                        default:
                            break;
                    }

                    if (CheckWorkingRange(bolt.Mode, bolt.Face, type, checkValue, list))
                    {
                        if (bolt.Mode != AXIS_MODE.POINT && bolt.Mode != AXIS_MODE.HypotenusePOINT)
                        {
                            Point3D checkPoint3D = new Point3D(bolt.X, bolt.Y, bolt.Z);
                            var testPoint = checkPoint3D;
                            // 前面步驟無CreateBolt 需翻轉
                            if (rotate)
                            {
                                testPoint = Coordinates(bolt.Face, checkPoint3D);
                            }

                            if (!((Mesh)model.Blocks[1].Entities[0]).IsPointInside(testPoint))
                            // if (!((Mesh)model.Entities[model.Entities.Count - 1]).IsPointInside(testPoint))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        /// <summary>
        /// 加入螺栓圖塊到模型
        /// </summary>
        /// <param name="attr">螺栓設定檔</param>
        /// <param name="model">要被加入的模型</param>
        /// <param name="block">加入到模型的參考圖塊</param>
        /// <param name="check">規則檢查結果</param>
        /// <param name="meshes">產生已存在的孔</param>
        /// <param name="isRotate">孔是否翻轉(產生時需要，查詢時不需要)</param>
        /// <returns>加入到模型的圖塊</returns>
        public static Bolts3DBlock AddBolts(GroupBoltsAttr attr, devDept.Eyeshot.Model model,
            out BlockReference block,out bool check,
            List<Mesh> meshes = null,bool isRotate = true)
        {
            check = true;
            block = null;

            Bolts3DBlock result = new Bolts3DBlock(attr); //產生孔位圖塊
            //result.steelAttr = (SteelAttr)model.Entities[model.Entities.Count - 1].EntityData;
            result.steelAttr = (SteelAttr)model.Blocks[1].Entities[0].EntityData;
            bool fCreateBolt = false;
            if (!model.Blocks.Contains(result.Name))
            {//出來的座標無翻轉
              result.CreateBolts(model, ref check,meshes, isRotate);//創建孔位群組
                fCreateBolt = true;
            }
            
            //// 符合加工區域
            //if (check)
            //{
            //    if (attr.Mode != AXIS_MODE.POINT && attr.Mode != AXIS_MODE.HypotenusePOINT)
            //    {
            //        bool inSteel = true;
            //        if (result.Entities.Count == 0)
            //        {
            //            Point3D checkPoint3D = new Point3D(attr.X, attr.Y, attr.Z);
            //            var testPoint = Coordinates(attr.Face, checkPoint3D);

            //            if (((Mesh)model.Blocks[1].Entities[0]).IsPointInside(testPoint))
            //            //if (((Mesh)model.Entities[model.Entities.Count - 1]).IsPointInside(testPoint))
            //            {
            //                inSteel = true;
            //            }
            //            else
            //            {
            //                result.hasOutSteel = true;
            //                inSteel = false;
            //            }
            //        }
            //        else
            //        {
            //            foreach (var item in result.Entities)
            //            {
            //                // 檢查產生之孔位是否在鋼體內
            //                //if (((Mesh)model.Entities[model.Entities.Count - 1].EntityData).IsPointInside(
            //                Point3D checkPoint3D = new Point3D(((BoltAttr)item.EntityData).X, ((BoltAttr)item.EntityData).Y, ((BoltAttr)item.EntityData).Z);
            //                var testPoint = Coordinates(((BoltAttr)item.EntityData).Face, checkPoint3D);

            //                if (((Mesh)model.Blocks[1].Entities[0]).IsPointInside(testPoint))
            //                //if (((Mesh)model.Entities[model.Entities.Count-1]).IsPointInside(testPoint))
            //                {
            //                    inSteel = true;
            //                }
            //                else
            //                {
            //                    result.hasOutSteel = true;
            //                    inSteel = false;
            //                    // 驚嘆號
            //                    //((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
            //                    break;
            //                }

            //                //if (!(inSteel || ((Mesh)model.Entities[model.Entities.Count - 1].EntityData).IsPointInside(
            //                //    new Point3D()
            //                //    {
            //                //        X = ((BoltAttr)item.EntityData).X,
            //                //        Y = ((BoltAttr)item.EntityData).Y,
            //                //        Z = ((BoltAttr)item.EntityData).Z
            //                //    })))//鋼構外
            //                //{
            //                //    ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
            //                //}
            //            }
            //            // 在鋼體中
            //            //if (inSteel) { ((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = false; }
            //        }
            //    }
            //}
            //else
            //{
            //    // 不在工作區域中
            //    result.hasOutSteel = true;
            //    //((SteelAttr)model.Blocks[1].Entities[0].EntityData).ExclamationMark = true;
            //}

            //if (!result.hasOutSteel)
            //{
                if (!model.Blocks.Contains(result.Name))
                {
                    model.Blocks.Add(result);//加入孔位群組圖塊到模型
                    block = new BlockReference(0, 0, 0, result.Name, 1, 1, 1, 0);//產生孔位群組參考圖塊
                    block.EntityData = result.Info;
                    block.Attributes.Add("Bolts", new AttributeReference(0, 0, 0));
                    model.Entities.Insert(0, block);//加入參考圖塊到模型
                }
                else
                {
                    result.Entities.AddRange(model.Blocks[result.Name].Entities);
                }
            if (CheckBolts(model, !fCreateBolt, result.Name))
            {
                check = true;
            }
            else { check = false; }
            //}
            // else { result.Entities.AddRange(model.Blocks[result.Name].Entities); }

            return result;


            




        }

        /// <summary>
        /// 轉換世界座標
        /// </summary>
        /// <param name="face"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static  Point3D Coordinates(FACE face, Point3D p)
        {
            switch (face)
            {
                case GD_STD.Enum.FACE.TOP:
                    break;
                case GD_STD.Enum.FACE.FRONT:
                case GD_STD.Enum.FACE.BACK:
                    double y, z;
                    y = p.Z;
                    z = p.Y;
                    p.Y = y;
                    p.Z = z;
                    break;
                default:
                    break;
            }
            //X = p.X, Y = p.Y, Z = p.Z
            Point3D[] points = new Point3D[3];
            points[0] = new Point3D() { X = p.X, Y = p.Y, Z = p.Z };
            return points[0];
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

        /// <summary>
        /// 計算可加工範圍(Y軸)
        /// </summary>
        /// <param name="type">型鋼類別</param>
        /// <param name="boltAttr">孔資訊</param>
        /// <returns></returns>
        public static List<double> WorkingRange(OBJECT_TYPE type, BoltAttr boltAttr,SteelAttr steelAttr)
        {
            STDSerialization serH = new STDSerialization(), serBOX = new STDSerialization(), serCH = new STDSerialization();
            ObservableCollection<SectionTypeProcessingData> sectionTypeProcessingData_H =
                serH.GetSectionTypeProcessingData("H", Convert.ToInt32(ProcessingBehavior.DRILLING));
            ObservableCollection<SectionTypeProcessingData> sectionTypeProcessingData_BOX =
                serBOX.GetSectionTypeProcessingData("BOX", Convert.ToInt32(ProcessingBehavior.DRILLING));
            ObservableCollection<SectionTypeProcessingData> sectionTypeProcessingData_CH =
                serCH.GetSectionTypeProcessingData("CH", Convert.ToInt32(ProcessingBehavior.DRILLING));
            // 假設 attr是加工區域設定參數
            double a = 0, b = 0, c = 0;

            switch (type)
            {
                case OBJECT_TYPE.RH:
                case OBJECT_TYPE.BH:
                case OBJECT_TYPE.H:
                    if (sectionTypeProcessingData_H == null) break;
                    if (sectionTypeProcessingData_H.Count > 0)
                    {
                        a = sectionTypeProcessingData_H[0].A;
                        b = sectionTypeProcessingData_H[0].B;
                        c = sectionTypeProcessingData_H[0].C;
                    }
                    break;
                case OBJECT_TYPE.C:
                case OBJECT_TYPE.CH:
                    if (sectionTypeProcessingData_CH == null) break;
                    if (sectionTypeProcessingData_CH.Count > 0)
                    {
                        a = sectionTypeProcessingData_CH[0].A;
                        b = sectionTypeProcessingData_CH[0].B;
                    }
                    break;
                case OBJECT_TYPE.BOX:
                case OBJECT_TYPE.TUBE:
                    if (sectionTypeProcessingData_BOX == null) break;
                    if (sectionTypeProcessingData_BOX.Count > 0)
                    {
                        a = sectionTypeProcessingData_BOX[0].A;
                        b = sectionTypeProcessingData_BOX[0].B;
                    }                
                    break;
                case OBJECT_TYPE.L:
                case OBJECT_TYPE.PLATE:
                case OBJECT_TYPE.RB:
                case OBJECT_TYPE.FB:
                case OBJECT_TYPE.PIPE:
                case OBJECT_TYPE.Unknown:
                    break;
                default:
                    break;
            }

            List<double> list = new List<double>();
            switch (type)
            {
                case OBJECT_TYPE.RH:
                case OBJECT_TYPE.BH:
                case OBJECT_TYPE.H:
                    a = a + steelAttr.t2;
                    b = b + 0;
                    c = c + steelAttr.t1;
                    break;
                case OBJECT_TYPE.C:
                case OBJECT_TYPE.CH:
                    a = a + steelAttr.t2;
                    b = b + 0;
                    break;
                case OBJECT_TYPE.BOX:
                case OBJECT_TYPE.TUBE:
                    a = a + steelAttr.t2;
                    b = b + steelAttr.t1;
                    break;
                case OBJECT_TYPE.L:
                case OBJECT_TYPE.PLATE:
                case OBJECT_TYPE.RB:
                case OBJECT_TYPE.FB:
                case OBJECT_TYPE.PIPE:
                case OBJECT_TYPE.Unknown:
                    break;
                default:
                    break;
            }
            // 腹板長度
            double H = steelAttr.H;
            // 翼板寬度
            double W = steelAttr.W;
            // 可編輯長度
            double editLengh = (H - 2 * a) / 2;
            // 半徑
            double radius = boltAttr.Dia / 2;
            // 起始座標
            double start1 = 0, start2 = 0;
            // 結束座標
            double end1 = 0, end2 = 0; 



            switch (type)
            {
                #region H型鋼
                case OBJECT_TYPE.RH:
                case OBJECT_TYPE.BH:
                case OBJECT_TYPE.H:
                    switch (boltAttr.Face)
                    {
                        // 腹板
                        case FACE.TOP:
                            // 頂視圖:(H-2A)/2 頭尾可編輯長度
                            editLengh = H - 2 * a;
                            // 起始座標 = a + 半徑
                            start1 = a + radius;
                            // 結束座標 = lengh - a - 半徑
                            end1 = H - a - radius;
                            list.Add(start1);
                            list.Add(end1);
                            break;
                        // 翼板
                        case FACE.FRONT:
                        case FACE.BACK:
                            // 前後視圖:
                            editLengh = (W - 2 * b - c) / 2;
                            // 第一段起始座標 = b;                   
                            start1 = b + radius;
                            // 第一段結束座標 = b+可編輯長度-半徑
                            end1 = b + editLengh - radius;
                            // 第二段起始座標 = (b + 可編輯長度 + c) + 半徑;
                            start2 = (b + editLengh + c) + radius;
                            // 第二段結束座標 = W - b - 半徑;
                            end2 = W - b - radius;
                            list.Add(start1);
                            list.Add(end1);
                            list.Add(start2);
                            list.Add(end2);
                            break;
                        default:
                            break;
                    }
                    break;
                #endregion
                #region C型鋼
                case OBJECT_TYPE.C:
                case OBJECT_TYPE.CH:
                    switch (boltAttr.Face)
                    {
                        // 腹板
                        case FACE.TOP:
                            // 頂視圖:(H-2A)/2 頭尾可編輯長度
                            editLengh = H - 2 * a;
                            // 起始座標 = a + 半徑
                            start1 = a + radius;
                            // 結束座標 = lengh - a - 半徑
                            end1 = H - a - radius;
                            list.Add(start1);
                            list.Add(end1);
                            break;
                        // 翼板
                        case FACE.FRONT:
                        case FACE.BACK:
                            // 前後視圖:
                            editLengh = W - 2 * b;
                            // 起始座標:若半徑<=(可編輯長度+半徑)，起始座標 = 可編輯長度+半徑，否則為半徑
                            // 起始座標 = b + 半徑;                   
                            start1 = b + radius;
                            // 結束座標 = 寬度-b-半徑
                            end1 = W - b - radius;                           
                            list.Add(start1);
                            list.Add(end1);                           
                            break;
                        default:
                            break;
                    }
                    break;
                #endregion
                #region 方管
                case OBJECT_TYPE.BOX:
                case OBJECT_TYPE.TUBE:
                    switch (boltAttr.Face)
                    {
                        // 腹板
                        case FACE.TOP:
                            editLengh = H - 2 * a;
                            // 起始座標 = a + 半徑
                            start1 = a + radius;
                            // 結束座標 = H - a - 半徑
                            end1 = H - a - radius;
                            list.Add(start1);
                            list.Add(end1);
                            break;
                        // 翼板
                        case FACE.FRONT:
                        case FACE.BACK:
                            // 前後視圖:
                            editLengh = W - 2 * b;
                            // 起始座標:若半徑<=(可編輯長度+半徑)，起始座標 = 可編輯長度+半徑，否則為半徑
                            // 起始座標 = b + 半徑;                   
                            start1 = b + radius;
                            // 結束座標 = b + 可編輯長度-半徑
                            end1 = b + editLengh - radius;                           
                            list.Add(start1);
                            list.Add(end1);                           
                            break;
                        default:
                            break;
                    }
                    break; 
                #endregion
                case OBJECT_TYPE.L:
                case OBJECT_TYPE.PLATE:
                case OBJECT_TYPE.RB:
                case OBJECT_TYPE.FB:
                case OBJECT_TYPE.PIPE:
                case OBJECT_TYPE.Unknown:
                    break;
                default:
                    break;
            }
            return list;
        }
    }
}

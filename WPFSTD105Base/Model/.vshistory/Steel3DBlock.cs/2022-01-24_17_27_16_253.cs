
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using WPFSTD105.Attribute;
using static WPFSTD105.Properties.SofSetting;
namespace WPFSTD105.Model
{
    /// <summary>
    /// 代表鋼材 3D 塊繪製函數
    /// </summary>
    public class Steel3DBlock : Block
    {

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'Steel3DBlock.Radian' 的 XML 註解
        public const double Radian = 0.09;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'Steel3DBlock.Radian' 的 XML 註解
        /// <summary>
        /// 標準函數
        /// </summary>
        /// <param name="mesh"></param>
        public Steel3DBlock(Mesh mesh) : base(((SteelAttr)mesh.EntityData).GUID.ToString())
        {
            try
            {
                //檢查自定義屬性檔是不是空值
                log4net.LogManager.GetLogger("檢查").Debug("屬性檔是不是空值");
                if (mesh.EntityData == null)
                    throw new Exception("mesh EntityData is null");

                //檢查屬性檔案是否正確
                log4net.LogManager.GetLogger("檢查").Debug("屬性檔案是否正確");
                if (mesh.EntityData.GetType() != typeof(SteelAttr))
                    throw new Exception($"mesh EntityData type is not {typeof(SteelAttr)}");

                this.Units = linearUnitsType.Millimeters;
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                throw;
            }

            this.Entities.Add(mesh);//加入物件到圖塊內
        }
        ///// <summary>
        ///// 鋼構設定檔
        ///// </summary>
        //public SteelAttr SteelAttr { get => this.Entities.Count == 0 ? null : (SteelAttr)this.Entities[0].EntityData; }
        /// <summary>
        /// 需要產生的面
        /// </summary>
        /// <param name="mesh">要切割的實體</param>
        /// <param name="face">要切割的面</param>
        private static void CutLine(ref Mesh mesh, FACE face)
        {
            try
            {
                SteelAttr steelAttr = (SteelAttr)mesh.EntityData;
                List<Mesh> cut = null;
                switch (face)
                {
                    case FACE.BACK:
                    case FACE.FRONT:
                        //如果用戶沒有設定切割線
                        if (steelAttr.Front == null)
                        {
#if DEBUG
                            log4net.LogManager.GetLogger("頂面切割線產生失敗").Debug($"{nameof(steelAttr)}.{nameof(steelAttr.Top)} is null ");
#endif
                            return;
                        }
                        cut = steelAttr.Front.CutLineToMeshs(new Vector3D(0, 0, steelAttr.H), Plane.XY);//目前傳入的是符合面的2d座標，完成後需要旋轉
                        //旋轉物件並移動到適合位置
                        for (int i = 0; i < cut.Count; i++)
                        {
                            if (cut[i] == null)
                                continue;

                            cut[i].Rotate(Math.PI / 2, Vector3D.AxisX);
                            cut[i].Translate(0, steelAttr.H, 0);
                        }
                        mesh = mesh.Difference(cut);
                        break;
                    case FACE.TOP:
                        //如果用戶沒有設定切割線
                        if (steelAttr.Top == null)
                        {
#if DEBUG
                            log4net.LogManager.GetLogger("頂面切割線產生失敗").Debug($"{nameof(steelAttr)}.{nameof(steelAttr.Front)} is null ");
#endif
                            return;
                        }
                        cut = steelAttr.Top.CutLineToMeshs(new Vector3D(0, 0, steelAttr.W), Plane.XY);
                        mesh = mesh.Difference(cut);
                        break;
                    default:
                        throw new Exception("找不到面");
                }
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                throw;
            }

        }

        /// <summary>
        /// 取得鋼構實體
        /// </summary>
        /// <param name="steelAttr">物件設定檔</param>
        /// <returns></returns>
        public static Mesh GetProfile(SteelAttr steelAttr)
        {
           //return a();

            List<ICurve> curves = new List<ICurve>() { BaseProfile(steelAttr.H, steelAttr.W, 0, 0) };
            try
            {
                //組合實際斷面規格
                switch (steelAttr.Type)
                {
                    case OBJETC_TYPE.BH:
                    case OBJETC_TYPE.RH:
                        curves.Add(BaseProfile(steelAttr.H - (steelAttr.t2 * 2), (steelAttr.W * 0.5) - (steelAttr.t1 * 0.5), 0, steelAttr.t2));
                        curves.Add(BaseProfile(steelAttr.H - (steelAttr.t2 * 2), (steelAttr.W * 0.5) - (steelAttr.t1 * 0.5), (steelAttr.W * 0.5) + (steelAttr.t1 * 0.5), steelAttr.t2));
#if DEBUG 
                        log4net.LogManager.GetLogger("產生H型鋼").Debug($"H{steelAttr.H},W{steelAttr.W},t1{steelAttr.t1},t2{steelAttr.t2}");
#endif
                        break;
                    case OBJETC_TYPE.CH:
                        double b = Math.Sin(Radian) * ((steelAttr.W / 2) / Math.Cos(Radian)); //計算斜率點到點
                        double c = Math.Sin(Radian) * ((steelAttr.W / 2 - steelAttr.t1) / Math.Cos(Radian)); //計算斜率點到點
                        Point3D[] point3Ds = new Point3D[]
                        {
                                    new Point3D(0,steelAttr.t2- b,0),
                                    new Point3D(0,steelAttr.H - steelAttr.t2 + b,0),
                                    new Point3D(0,steelAttr.H -steelAttr.t2 - c, steelAttr.W- steelAttr.t1),
                                    new Point3D(0,steelAttr.t2+ c,steelAttr.W-steelAttr.t1),
                                    new Point3D(0,steelAttr.t2- b,0)
                        };
                        LinearPath linearPath = new LinearPath(point3Ds);
                        curves.Add(linearPath);
#if DEBUG
                        log4net.LogManager.GetLogger("產生CH型鋼").Debug($"H{steelAttr.H},W{steelAttr.W},t1{steelAttr.t1},t2{steelAttr.t2}");
#endif
                        break;
                    case OBJETC_TYPE.L:
                        curves.Add(BaseProfile(steelAttr.H - steelAttr.t1, steelAttr.W - steelAttr.t1, steelAttr.t1, steelAttr.t1));
#if DEBUG
                        log4net.LogManager.GetLogger("產生L型鋼").Debug($"H {steelAttr.H},W {steelAttr.W},t1 {steelAttr.t1}");
#endif
                        break;
                    case OBJETC_TYPE.BOX:
                        curves.Add(BaseProfile(steelAttr.H - (steelAttr.t2 * 2), steelAttr.W - (steelAttr.t1 * 2), steelAttr.t1, steelAttr.t2));
#if DEBUG
                        log4net.LogManager.GetLogger("產生BOX型鋼").Debug($"H {steelAttr.H},W {steelAttr.W},t1 {steelAttr.t1},t2 {steelAttr.t2}");
#endif
                        break;
                    default:
                        log4net.LogManager.GetLogger("steelAttr").Debug("失敗");
                        throw new Exception("找不到不符合類型");
                }
                // 區域實體定義。 按照慣例，列表中的第一個輪廓是位於外部，並具有逆時針方向。 內環是順時針方向的。摘要：輪廓，平面和排序標誌構造函數的列表
                devDept.Eyeshot.Entities.Region region = new devDept.Eyeshot.Entities.Region(curves, Plane.XY, false);
                Mesh result = region.ExtrudeAsMesh<Mesh>(new Vector3D(steelAttr.Length, 0, 0), 1e-6, Mesh.natureType.Plain);// 拉伸輪廓以創建新的devDept.Eyeshot.Entities.Mesh。
                result.EntityData = steelAttr;

                //切割物件
                CutLine(ref result, FACE.TOP);
                //CutLine(ref result, FACE.BACK);
                CutLine(ref result, FACE.FRONT);
#if DEBUG
                log4net.LogManager.GetLogger("完成").Debug(steelAttr.GUID.ToString());
#endif
                result.EntityData = steelAttr.DeepClone();
                result.Color = ColorTranslator.FromHtml(Default.Part);
                result.ColorMethod = colorMethodType.byEntity;
                return result;
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("steelAttr").Debug(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 繪製方形輪廓
        /// </summary>
        /// <param name="h">高度</param>
        /// <param name="w">寬度</param>
        /// <param name="startZ">起始點Z</param>
        /// <param name="startY">起始點Y</param>
        /// <returns></returns>
        private static ICurve BaseProfile(double h, double w, double startZ, double startY)
        {
            //2D繪製矩形
            Point3D[] point3Ds = new Point3D[]
            {
                new Point3D(0,startY,startZ),
                new Point3D(0,h +startY,startZ),
                new Point3D(0,h+startY,w+startZ),
                new Point3D(0,startY,w+startZ),
                new Point3D(0,startY,startZ),
            };

            LinearPath result = new LinearPath(point3Ds);
#if DEBUG
            for (int i = 0; i < point3Ds.Length; i++)
            {
                Console.WriteLine($"new Point3D(0, {point3Ds[i].Y}, {point3Ds[i].Z})");
                log4net.LogManager.GetLogger($"產生Base[{i}]").Debug($@"{point3Ds[i]}");
            }

#endif
            return result;
        }
        /// <summary>
        /// 加入鋼構圖塊到模型
        /// </summary>
        /// <param name="steelAttr">鋼構設定檔</param>
        /// <param name="model">要被加入的模型</param>
        /// <param name="blockReference">加入到模型的參考圖塊</param>
        /// <returns></returns>
        public static Steel3DBlock AddSteel(SteelAttr steelAttr, devDept.Eyeshot.Model model, out BlockReference blockReference)
        {
            Steel3DBlock result;
            result = new Steel3DBlock(GetProfile(steelAttr));
            model.Blocks.Add(result);//加入鋼構圖塊到模型
            blockReference = new BlockReference(0, 0, 0, result.Name, 1, 1, 1, 0);
            blockReference.EntityData = steelAttr;
            blockReference.Selectable = false;//關閉用戶選擇
            blockReference.Attributes.Add("Steel", new AttributeReference(0, 0, 0));
            model.Entities.Add(blockReference);//加入參考圖塊到模型
            return result;
        }

        //private static Mesh a()
        //{
        //    Point3D[] point3Ds1 = new Point3D[]
        //    {
        //      new Point3D(0, 0, 0),
        //      new Point3D(0, 900, 0),
        //      new Point3D(0, 900, 300),
        //      new Point3D(0, 0, 300),
        //      new Point3D(0, 0, 0),
        //    };

        //    Point3D[] point3Ds2 = new Point3D[]
        //    {
        //        new Point3D(0, 28, 0),
        //        new Point3D(0, 872, 0),
        //        new Point3D(0, 872, 142),
        //        new Point3D(0, 28, 142),
        //        new Point3D(0, 28, 0),
        //    };
        //    Point3D[] point3Ds3 = new Point3D[]
        //    {
        //        new Point3D(0, 28, 158),
        //        new Point3D(0, 872, 158),
        //        new Point3D(0, 872, 300),
        //        new Point3D(0, 28, 300),
        //        new Point3D(0, 28, 158)
        //    };

        //    LinearPath linearPath1 = new LinearPath(point3Ds1);
        //    LinearPath linearPath2 = new LinearPath(point3Ds2);
        //    LinearPath linearPath3 = new LinearPath(point3Ds3);
        //    List<ICurve> curves = new List<ICurve>()
        //    {
        //        linearPath1,
        //        linearPath2,
        //        linearPath3
        //    };
        //    devDept.Eyeshot.Entities.Region region1 = new devDept.Eyeshot.Entities.Region(curves, Plane.XY, false);
        //    Mesh mesh = region1.ExtrudeAsMesh<Mesh>(new Vector3D(500, 0, 0), 0.01, Mesh.natureType.Plain);


        //    Point3D[] point3Ds4 = new Point3D[]
        //    {
        //        new Point3D(0, -300, 0),
        //        new Point3D(0, 300, 0),
        //        new Point3D(0, 1300, 0),
        //        new Point3D(0, -300, 0),
        //    };
        //    LinearPath cut = new LinearPath(point3Ds4);

        //    devDept.Eyeshot.Entities.Region region2 = new devDept.Eyeshot.Entities.Region(cut, Plane.XY, true);
        //    Mesh secondary = region2.ExtrudeAsMesh(new Vector3D(0, 0, 900), 1e-5, Mesh.natureType.Plain);
        //    secondary.Rotate(Math.PI / 2, Vector3D.AxisX);
        //    secondary.Translate(0,900, 0);

        //    Solid[] solids = Solid.Difference(mesh.ConvertToSolid(), secondary.ConvertToSolid()); 

        //    mesh = solids[0].ConvertToMesh();

        //    return mesh;
        //}
    }
}

using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Space.Attribute;
namespace Space.Model
{
    /// <summary>
    /// 鋼構物件
    /// </summary>
    public class Steel : Mesh
    {
        /// <summary>
        /// 鋼構物件
        /// </summary>
        public Steel()
        {

        }
        #region 受保護屬性
        /// <summary>
        /// 差集物件增加長度防止破圖
        /// </summary>
        protected double differenceLength { get => Info.Length + 10; }
        /// <summary>
        /// 差集起始距離
        /// </summary>
        protected double differenceStart { get => 10 * 0.5; }
        #endregion


        #region 受保護的方法
      
        #endregion

        #region 公開屬性
        /// <summary>
        /// 鋼材訊息
        /// </summary>
        public SteelAttr Info { get => (SteelAttr)this.EntityData; }
        #endregion

        #region 公開方法

        #endregion

        #region 私有的靜態方法
        /// <summary>
        /// 一般矩形
        /// </summary>
        /// <param name="h">高度</param>
        /// <param name="w">寬度</param>
        /// <param name="startZ">世界座標起始點Z</param>
        /// <param name="startY">世界座標起始點Y</param>
        /// <returns></returns>
        private static ICurve BaseProfile(double h, double w, double startZ, double startY)
        {
            //2D繪製矩形
            //Mesh result = Mesh.CreateBox(length, w, h);
            Point3D[] point3Ds = new Point3D[]
            {
                new Point3D(0,startY,startZ),
                new Point3D(0,h +startY,startZ),
                new Point3D(0,h+startY,w+startZ),
                new Point3D(0,startY,w+startZ),
                 new Point3D(0,startY,startZ),
            };
            LinearPath result = new LinearPath(point3Ds);
            //ICurve result = profileLp;
            return result;
        }
        /// <summary>
        /// 修飾<see cref="OBJETC_TYPE.H"/>輪廓點位
        /// </summary>
        /// <param name="Attribute">鋼結構參數訊息</param>
        /// <returns></returns>
        private static List<ICurve> DifferenceH(SteelAttr Attribute)
        {
            List<ICurve> result = new List<ICurve>();

            result.Add(BaseProfile(Attribute.H - (Attribute.t2 * 2), (Attribute.W * 0.5) - (Attribute.t1 * 0.5), 0, Attribute.t2));
            result.Add(BaseProfile(Attribute.H - (Attribute.t2 * 2), (Attribute.W * 0.5) - (Attribute.t1 * 0.5), (Attribute.W * 0.5) + (Attribute.t1 * 0.5), Attribute.t2));
            return result;
        }
        /// <summary>
        /// 修飾<see cref="OBJETC_TYPE.CH"/>輪廓點位
        /// </summary>
        /// <param name="Attribute">鋼結構參數訊息</param>
        /// <returns></returns>
        private static List<ICurve> DifferenceCH(SteelAttr Attribute)
        {
            List<ICurve> result = new List<ICurve>();

            double b = Math.Sin(Attribute.radian) * ((Attribute.W / 2) / Math.Cos(Attribute.radian)); //計算斜率點到點
            Point3D[] point3Ds = new Point3D[]
            {
                new Point3D(0,Attribute.t2- b,0),
                new Point3D(0,Attribute.H - Attribute.t2 + b,0),
                new Point3D(0,Attribute.H -Attribute.t2 - b, Attribute.W- Attribute.t1),
                new Point3D(0,Attribute.t2+ b,Attribute.W-Attribute.t1),
                new Point3D(0,Attribute.t2- b,0)
            };
            LinearPath linearPath = new LinearPath(point3Ds);
            result.Add(linearPath);
            return result;
        }
        /// <summary>
        /// 修飾<see cref="OBJETC_TYPE.BOX"/>輪廓點位
        /// </summary>
        /// <param name="Attribute">鋼結構參數訊息</param>
        /// <returns></returns>
        private static List<ICurve> DifferenceBox(SteelAttr Attribute)
        {
            List<ICurve> result = new List<ICurve>();

            result.Add(BaseProfile(Attribute.H - (Attribute.t2 * 2), Attribute.W - (Attribute.t1 * 2), Attribute.t1, Attribute.t2));
            return result;
        }
        /// <summary>
        /// 修飾<see cref="OBJETC_TYPE.L"/>輪廓點位
        /// </summary>
        /// <param name="Attribute">鋼結構參數訊息</param>
        /// <returns></returns>
        private static List<ICurve> DifferenceL(SteelAttr Attribute)
        {
            List<ICurve> result = new List<ICurve>();

            result.Add(BaseProfile(Attribute.H - Attribute.t1, Attribute.W - Attribute.t1, Attribute.t1, 0));
            return result;
        }
        #endregion

        #region 公開靜態方法
        /// <summary>
        /// 獲取鋼構物件
        /// </summary>
        /// <returns></returns>
        public static Steel GetSteel(SteelAttr Attribute)
        {
            //if ()
            //    throw new Exception("請先設置 EntityData in mesh 內");

            List<ICurve> curves = new List<ICurve>() { BaseProfile(Attribute.H, Attribute.W, 0, 0) };
            switch (Attribute.Type)
            {
                case OBJETC_TYPE.H:
                    curves.AddRange(DifferenceH(Attribute));
                    break;
                case OBJETC_TYPE.CH:
                    curves.AddRange(DifferenceCH(Attribute));
                    break;
                case OBJETC_TYPE.L:
                    curves.AddRange(DifferenceL(Attribute));
                    break;
                case OBJETC_TYPE.BOX:
                    curves.AddRange(DifferenceBox(Attribute));
                    break;
                case OBJETC_TYPE.BOLT:
                    throw new Exception("錯誤類型");
                default:
                    throw new Exception("找不到類型");
            }
            Region region = new Region(curves, Plane.XY, false);//區域實體定義。 按照慣例，列表中的第一個輪廓是位於外部，並具有逆時針方向。 內環是順時針方向的。摘要：輪廓，平面和排序標誌構造函數的列表
            Steel result = region.ExtrudeAsMesh<Steel>(new Vector3D(Attribute.Length, 0, 0), 0.001, Mesh.natureType.Plain);// 拉伸輪廓以創建新的devDept.Eyeshot.Entities.Mesh。
            result.EntityData = Attribute;
            return result;
        }
        #endregion
    }
}

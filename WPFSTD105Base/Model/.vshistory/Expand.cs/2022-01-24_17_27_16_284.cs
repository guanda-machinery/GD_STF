#define Debug
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System;
using System.Collections.Generic;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 擴展
    /// </summary>
    public static class Expand
    {
        /// <summary>
        /// 切割線轉換差集實體列表
        /// </summary>
        /// <param name="cutLine">切割線列表</param>
        /// <param name="vector3D">擠出量</param>
        /// <param name="plane">輪廓平面</param>
        /// <param name="secondary">修剪輪廓</param>
        /// <returns></returns>
        public static List<Mesh> CutLineToMeshs(this CutContour cutLine, Vector3D vector3D, Plane plane, List<ICurve> secondary = null)
        {
#if DEBUG
            log4net.LogManager.GetLogger("產生多個切割物件").Debug($"開始");
#endif
            return new List<Mesh>()
            {
                cutLine.GetDLCutPoint().PointToMesh(vector3D, plane),
                cutLine.GetDRCutPoint().PointToMesh(vector3D, plane),
                cutLine.GetULCutPoint().PointToMesh(vector3D, plane),
                cutLine.GetURCutPoint().PointToMesh(vector3D, plane),
            };
        }

        /// <summary>
        /// 差集多個物件
        /// </summary>
        /// <param name="mesh">主要物件</param>
        /// <param name="meshes">次要物件</param>
        public static Mesh Difference(this Mesh mesh, List<Mesh> meshes)
        {
            object attr = mesh.EntityData; //暫時存取原本物件的的設定檔
            foreach (var el in meshes)
            {
                var me = mesh.Difference(el);
                mesh = me == null ? mesh : me;
            }
            mesh.EntityData = attr;
            return mesh;
        }
        
#pragma warning disable CS1572 // XML 註解中的 'meshes' 有 param 標籤，但沒有該名稱的參數
/// <summary>
        /// 差集物件
        /// </summary>
        /// <param name="mesh">主要物件</param>
        /// <param name="secondary">次要物件</param>
        /// <param name="meshes">旋轉弧度</param>
        public static Mesh Difference(this Mesh mesh, Mesh secondary)
#pragma warning restore CS1572 // XML 註解中的 'meshes' 有 param 標籤，但沒有該名稱的參數
        {
            try
            {
                if (secondary == null) //如果次件是空值
                    return null;
              
                Solid[] solids = Solid.Difference(mesh.ConvertToSolid(), secondary.ConvertToSolid()); //差集物件
                if (solids == null)
                    return mesh;

                mesh = solids[0].ConvertToMesh(); //轉換實體
                return mesh;
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                throw;
            }

        }
        /// <summary>
        /// 點位轉換實體
        /// </summary>
        /// <param name="point3Ds">點位列表</param>
        /// <param name="vector3D">擠出量</param>
        /// <param name="plane">輪廓平面</param>
        /// <param name="secondary">修剪輪廓</param>
        /// <returns></returns>
        public static Mesh PointToMesh(this List<Point3D> point3Ds, Vector3D vector3D, Plane plane, List<ICurve> secondary = null)
        {
#if DEBUG
            log4net.LogManager.GetLogger("產生單個切割物件").Debug($"繪製輪廓");
#endif

            if (point3Ds.Count <= 0) //如果切割點為空
            {
                return null;
            }
            List<ICurve> curves = new List<ICurve>() { point3Ds.PointToLinearPath() }; //主要輪廓
            //差集物件不等於空值
            if (secondary != null)
                for (int i = 0; i < secondary.Count; i++)
                    curves.Add(secondary[i]);
#if DEBUG
            else
                log4net.LogManager.GetLogger("產生切割物件失敗").Debug($"value is null");
#endif
            // 區域實體定義。 按照慣例，列表中的第一個輪廓是位於外部，並具有逆時針方向。 內環是順時針方向的。摘要：輪廓，平面和排序標誌構造函數的列表
            Region region = new Region(curves, plane, true);
            // 拉伸輪廓以創建新的devDept.Eyeshot.Entities.Mesh。
            Mesh result = region.ExtrudeAsMesh(vector3D, 1e-6, Mesh.natureType.Plain);
            return result;
        }
        /// <summary>
        /// 點位轉換路徑
        /// </summary>
        /// <param name="point3Ds"></param>
        /// <returns></returns>
        public static LinearPath PointToLinearPath(this List<Point3D> point3Ds)
        {
            point3Ds.Add(point3Ds[0]);
            return new LinearPath(point3Ds);
        }
    }
}

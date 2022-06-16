using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using System.Collections.Generic;
using System.Drawing;
using WPFSTD105.Model;
using Region = devDept.Eyeshot.Entities.Region;
namespace WPFSTD105
{
    /// <summary>
    ///  擴展
    /// </summary>
    public static class DevEx
    {
        /// <summary>
        /// 從此集合中刪除第一次出現的特定實體。
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="entities">刪除物件列表</param>
        public static void Remove(this EntityList entityList, List<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
                entityList.Remove(entities[i]);
        }
        /// <summary>
        /// 只比較 X Y 
        /// </summary>
        /// <param name="_line"></param>
        /// <param name="line"></param>
        /// <returns>相同回傳 true，不相同則回傳 false。</returns>
        public static bool Contains2D(this Line _line, Line line)
        {
            Point2D start1 = new Point2D(_line.StartPoint.X, _line.StartPoint.Y);
            Point2D start2 = new Point2D(line.StartPoint.X, line.StartPoint.Y);
            Point2D end1 = new Point2D(_line.EndPoint.X, _line.EndPoint.Y);
            Point2D end2 = new Point2D(line.EndPoint.X, line.EndPoint.Y);
            return (start1 == start2 && end1 == end2) || (end1 == start2 && start1 == end2);
        }
        /// <summary>
        /// 刪除指定圖塊
        /// </summary>
        /// <param name="modelExt"></param>
        /// <param name="guid">識別 ID</param>
        /// <returns>刪除成功回傳 true ，刪除失敗則回傳 false</returns>
        public static bool Remove(this ModelExt modelExt, string guid)
        {
            int index = -1;
            for (int i = 0; i < modelExt.Entities.Count; i++)
            {
                BlockReference refe = (BlockReference)modelExt.Entities[i];
                if (refe != null && refe.BlockName == guid)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                return false;
            }
            else
            {
                modelExt.Entities.RemoveAt(index);
                return true;
            }
        }
        /// <summary>
        /// 鏡射 <see cref="Entity"/>
        /// </summary>
        /// <param name="entity">要鏡射的物件</param>
        /// <param name="axis">鏡射軸向</param>
        /// <param name="p1">第一點</param>
        /// <param name="p2">第二點</param>
        /// <returns></returns>
        public static void Mirror(this Entity entity, Vector3D axis, Point3D p1, Point3D p2)
        {
            Vector3D vector3D = new Vector3D(p1, p2);
            Plane plane= new Plane(p1, vector3D, axis);
            Mirror mirror = new Mirror(plane);
            entity.TransformBy(mirror);
        }
        ///// <summary>
        ///// 取得
        ///// </summary>
        ///// <param name="modelExt"></param>
        ///// <param name="guid"></param>
        ///// <returns></returns>
        //public static Entity[] GetEntitieAll(this ModelExt modelExt, string guid)
        //{

        //}
        ///// <summary>
        ///// 第一個指定實體
        ///// </summary>
        ///// <param name="modelExt"></param>
        ///// <param name="guid">取得的物件 ID</param>
        ///// <returns>集合內一個出現的實體，如果整個集合內都沒有則回傳 null。</returns>
        //public static Entity GetEntitie(this ModelExt modelExt, string guid)
        //{
        //    modelExt.Entities.FindAll
        //    for (int i = 0; i < modelExt.Entities.Count; i++)
        //    {
        //        BlockReference refe = (BlockReference)modelExt.Entities[i];

        //        if (refe != null && refe.BlockName == guid)
        //        {
        //            return modelExt.Entities[i];
        //        }
        //    }
        //    return null;
        //}
        ///// <summary>
        ///// 取得多個指定實體
        ///// </summary>
        ///// <param name="modelExt"></param>
        ///// <param name="guid">取得的物件 ID</param>
        ///// <returns>集合內一個出現的實體，如果整個集合內都沒有則回傳 null。</returns>
        //public static List<Entity> GetEntities(this ModelExt modelExt, string[] guid)
        //{
        //    List<Entity> result = new List<Entity>();
        //    for (int i = 0; i < guid.Length; i++)
        //    {
        //        Entity entity = modelExt.GetEntitie(guid[i]);
        //        if (entity != null)
        //        {
        //            result.Add(entity);
        //        }
        //    }
        //    return result;
        //}
        //public static bool Remove(this ModelExt modelExt, List<List<Entity>> entities)
        //{

        //}
        /// <summary>
        /// 取得鑽頭
        /// </summary>
        /// <returns></returns>
        public static Brep GetDrill()
        {
            //鑽頭座標
            Point3D[] point3Ds = new Point3D[]
           {
                new Point3D(0,0),
                new Point3D(-1.7,4.8),
                new Point3D(-2.4,12.5),
                new Point3D(-86.4,12.5),
                new Point3D(-102.4,14.7),
                new Point3D(-102.4,22),
                new Point3D(-110.4,30),
                new Point3D(-271.8,30),
                new Point3D(-275.4,31.5),
                new Point3D(-280.8,31.5),
                new Point3D(-283.7,26.5),
                new Point3D(-287.9,26.5),
                new Point3D(-290.8,31.5),
                new Point3D(-300.4,31.5),
                new Point3D(-300.4,22.2),
                new Point3D(-302.4,22.2),
                new Point3D(-366.8,13),
                new Point3D(-367.8,11.7),
                new Point3D(-367.8,11.5),
                new Point3D(-371.8,11.5),
                new Point3D(-373.8,9.5),
                new Point3D(-373.8,5),
                new Point3D(-393.3,5),
                new Point3D(-395.8,7.5),
                new Point3D(-398.8,7.5),
                new Point3D(-402.8,5.2),
                new Point3D(-402.8,0),
                new Point3D(0,0),
           };
            LinearPath linearPath = new LinearPath(point3Ds);
            Region region = new Region(linearPath, Plane.XY);
            Brep result = region.RevolveAsBrep(Utility.DegToRad(360), Vector3D.AxisX, Point3D.Origin); //使該區域繞軸旋轉。 參數 : deltaAngle: 弧度公轉角。axis: 軸方向。center:軸起
            result.Color = Color.Gray;
            result.ColorMethod = colorMethodType.byEntity;
            return result;
        }
        /// <summary>
        /// <see cref="GetDrill"/> 長度
        /// </summary>
        public const double DrillLength = 402.8;
    }
}

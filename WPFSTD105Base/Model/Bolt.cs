using devDept.Eyeshot.Entities;
using devDept.Geometry;
using GD_STD;
using GD_STD.Enum;
using System;
using WPFSTD105.Attribute;
using WPFSTD105.Properties;
using static WPFSTD105.Properties.SofSetting;
namespace WPFSTD105.Model
{
    /// <summary>
    /// 螺栓實體
    /// </summary>
    public class Bolt : Mesh
    {
        /// <summary>
        /// 螺栓實體
        /// </summary>
        public Bolt()
        {
            this.Color = System.Drawing.ColorTranslator.FromHtml(Default.Hole);
            this.ColorMethod = colorMethodType.byEntity;
        }
        private Bolt(Bolt bolt) : base(bolt)
        {
            
        }
        
        /// <summary>
        /// 取得中心線
        /// </summary>
        /// <returns></returns>
        public Point3D GetCenter()
        {
            Point3D boxMin, boxMax;
            Utility.ComputeBoundingBox(null, Vertices, out boxMin, out boxMax);
            Point3D result = (boxMin + boxMax) / 2;
            return result;
        }
        /// <summary>
        /// 轉換座標
        /// </summary>
        /// <param name="dx">相對 x 座標</param>
        /// <param name="dy">相對 y 座標</param>
        /// <param name="dz">相對 z 座標</param>
        /// <remarks>
        /// 移動實體座標到指定座標
        /// </remarks>
        public override void Translate(double dx, double dy, double dz = 0)
        {
            base.Translate(dx, dy, dz);
        }
        /// <summary>
        /// 將實體繞任意軸旋轉指定角度。
        /// </summary>
        /// <param name="angleInRadians">弧度</param>
        /// <param name="axis">3D 矢量</param>
        public new void Rotate(double angleInRadians, Vector3D axis)
        {
            base.Rotate(angleInRadians, axis);
        }
        /// <summary>
        /// 複製物件
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            var result = new Bolt(this);
            BoltAttr boltAttr = (BoltAttr)this.EntityData;
            boltAttr.GUID = Guid.NewGuid();
            result.EntityData = boltAttr;
            return result;
        }
        /// <summary>
        /// 轉換 Codesys Memory 可用資料 
        /// </summary>
        /// <param name="face">要轉換的面</param>
        /// <returns></returns>
        public Drill ToCodesys(FACE face)
        {
            BoltAttr attr = (BoltAttr)this.EntityData;
            Point3D center = GetCenter();
            Drill result = new Drill()
            {
                Dia = attr.Dia,
                AXIS_MODE = attr.Mode,
                X = center.X,
                Y = face == FACE.TOP ? center.Y : center.Z
            };

            return result;
        }
    }
}

using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GD_STD;
using GD_STD.Enum;

namespace Space.Model
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

        }
        private Bolt(Bolt bolt) : base(bolt)
        {
            this.AbsX = bolt.AbsX;
            this.AbsY = bolt.AbsY;
            this.AbsZ = bolt.AbsY;
        }
        /// <summary>
        /// 絕對X座標
        /// </summary>
        public double AbsX { get; set; }
        /// <summary>
        /// 絕對Y座標
        /// </summary>
        public double AbsY { get; set; }
        /// <summary>
        /// 絕對座標Z
        /// </summary>
        public double AbsZ { get; set; }
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
            this.AbsX += dx;
            this.AbsY += dy;
            this.AbsZ += dz;
            base.Translate(dx, dy, dz);
        }
        /// <summary>
        /// 將實體繞任意軸旋轉指定角度。
        /// </summary>
        /// <param name="angleInRadians">弧度</param>
        /// <param name="axis">3D 矢量</param>
        public new void Rotate(double angleInRadians, Vector3D axis)
        {
            if (Vector3D.AxisX == axis)
            {
                if (angleInRadians > 0)
                {
                    this.AbsZ = this.AbsY;
                    this.AbsY = 0;
                }
                else
                {
                    this.AbsZ = -this.AbsY;
                    this.AbsY = 0;
                }
            }
            base.Rotate(angleInRadians, axis);
        }
        public override object Clone()
        {
            return new Bolt(this);
        }
        /// <summary>
        /// 轉換 Codesys Memory 可用資料 
        /// </summary>
        /// <returns></returns>
        public Drill ToCodesys(double dia, AXIS_MODE mode, FACE face)
        {
            Drill result = new Drill()
            {
                Dia = dia,
                AXIS_MODE = mode,
            };
            //轉換座標
            switch (face)
            {
                case FACE.FRONT:
                    result.X = this.AbsX;
                    result.Y = this.AbsY;
                    break;
                case FACE.TOP:
                case FACE.BOTTOM:
                    result.X = this.AbsX;
                    result.Y = this.AbsZ;
                    break;
                default:
                    throw new Exception("請新增面的座標處理標籤");
            }

            return result;
        }
    }
}

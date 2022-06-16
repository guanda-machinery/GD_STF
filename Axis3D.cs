using GD_STD.IBase;
using System;

namespace GD_STD
{
    /// <summary>
    /// 提供三維座標
    /// </summary>
    public struct Axis3D : IAxis2D
    {
        /// <summary>
        /// 產生三維座標
        /// </summary>
        /// <param name="x">座標 X </param>
        /// <param name="y">座標 Y</param>
        /// <param name="z">座標 Z</param>
        public Axis3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        /// <inheritdoc/>
        public double X { get; set; }
        /// <inheritdoc/>
        public double Y { get; set; }
        /// <summary>
        /// 座標 Z 
        /// </summary>
        public double Z { get; set; }
    }
}
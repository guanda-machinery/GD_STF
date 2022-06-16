using GD_STD.Base;
using System;
using System.Runtime.Serialization;

namespace GD_STD
{
    /// <summary>
    /// 提供三維座標
    /// </summary>
    [DataContract]
    public struct Axis3D :  IAxis3D
    {
        /// <summary>
        /// 轉換三維座標
        /// </summary>
        /// <param name="axis"></param>
        public Axis3D(IAxis3D axis)
        {
            X = axis.X;
            Y = axis.Y;
            Z = axis.Z;
        }
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
        [DataMember]
        public double X { get; set; }
        /// <inheritdoc/>
        [DataMember]
        public double Y { get; set; }
        /// <summary>
        /// 座標 Z 
        /// </summary>
        [DataMember]
        public double Z { get; set; }
    }
}
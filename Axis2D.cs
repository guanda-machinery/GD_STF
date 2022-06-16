using GD_STD.IBase;
using System;
using System.Runtime.Serialization;

namespace GD_STD
{
    /// <summary>
    /// 二維座標
    /// <para>可序列化的結構</para>
    /// </summary>
    [Serializable()]
    [DataContract]
    public struct Axis2D : IAxis2D
    {
        /// <summary>
        /// 座標 X 
        /// </summary>
        [DataMember]
        public double X { get; set; }
        /// <summary>
        /// 座標 Y 
        /// </summary>
        [DataMember]
        public double Y { get; set; }
    }
}
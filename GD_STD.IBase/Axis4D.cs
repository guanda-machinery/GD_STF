using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD
{
    /// <summary>
    /// 提供三維座標與相位
    /// </summary>
    [Serializable]
    [DataContract]
    public struct Axis4D : Base.IAxis3D, Base.IAxis4D
    {
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
        /// <summary>
        /// 相位
        /// </summary>
        [DataMember]
        public double MasterPhase { get; set; }
    }
}

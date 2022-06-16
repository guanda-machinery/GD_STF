using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 提供三維座標的介面
    /// </summary>
    public interface IAxis3D : IAxis2D
    {
        /// <summary>
        /// 座標 Z
        /// </summary>
        [DataMember]
        double Z { get; set; }
    }
}
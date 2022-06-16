using System.Runtime.Serialization;

namespace GD_STD.IBase
{
    /// <summary>
    /// 單一軸的軸向的資訊
    /// </summary>
    public interface ISingleAxisInfo : IAxis3D
    {
        /// <summary>
        /// 主軸速度
        /// </summary>
        [DataMember]
        double Rpm { get; set; }
    }
}
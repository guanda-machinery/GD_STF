using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 單一軸的軸向的資訊
    /// </summary>
    public interface ISingleAxisInfo : Base.IAxis3D
    {
        /// <summary>
        /// 主軸速度
        /// </summary>
        [DataMember]
        double Rpm { get; set; }
    }
}
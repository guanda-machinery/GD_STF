using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 提供二維座標的介面
    /// </summary>
    public interface IAxis2D
    {
        /// <summary>
        /// 座標 X 
        /// </summary>
        [DataMember]
        double X { get; set; }
        /// <summary>
        /// 座標 Y 
        /// </summary>
        [DataMember]
        double Y { get; set; }

    }
}
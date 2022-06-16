using GD_STD.Base;
using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 軸向設定
    /// </summary>
    [DataContract]
    public struct AxisSetting : IAxisSetting
    {
        /// <inheritdoc/>
        [DataMember]
        public Axis3D MeasuringPosition { get; set; }
        /// <inheritdoc/>
        [DataMember]
        public double SensorZero { get; set; }
        /// <inheritdoc/>
        [DataMember]
        public double TotalLength { get; set; }
        /// <summary>
        /// 扭力停止點 (打點用)
        /// </summary>
        [DataMember]
        public short Torque { get; set; }
    }
}

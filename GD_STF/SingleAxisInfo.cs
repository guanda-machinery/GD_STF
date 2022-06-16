using GD_STD.Base;
using System.Runtime.Serialization;

namespace GD_STD
{
    /// <summary>
    /// 單支主軸訊息
    /// </summary>
    /// <remarks>Codesys Memory 讀取</remarks>
    public struct SingleAxisInfo : ISingleAxisInfo
    {
        /// <inheritdoc/>
        public double X { get; set; }
        /// <inheritdoc/>
        public double Y { get; set; }
        /// <inheritdoc/>
        public double Z { get; set; }
        /// <inheritdoc/>
        public double Rpm { get; set; }
        [DataMember]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SingleAxisInfo.ZFeed' 的 XML 註解
        public double ZFeed { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SingleAxisInfo.ZFeed' 的 XML 註解
        [DataMember]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SingleAxisInfo.SpindleCurrent' 的 XML 註解
        public double SpindleCurrent { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SingleAxisInfo.SpindleCurrent' 的 XML 註解
        /// <summary>
        /// 刀尖
        /// </summary>
        [DataMember]
        public double knife { get; set; }
        /// <summary>
        /// 扭力
        /// </summary>
        [DataMember]
        public short Torque { get; set; }
    }
}
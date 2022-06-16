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
        public double ZFeed { get; set; }
        [DataMember]
        public double SpindleCurrent { get; set; }
        /// <summary>
        /// 刀尖
        /// </summary>
        [DataMember]
        public double knife { get; set; }
    }
}
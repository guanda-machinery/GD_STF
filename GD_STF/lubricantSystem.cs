using GD_STD.Base;
using System.Runtime.Serialization;

namespace GD_STD
{
    /// <summary>
    /// 潤滑油系統
    /// </summary>
    [DataContract]
    public struct LubricantSystem : IOill
    {
        /// <inheritdoc/>
        public short Frequency { get; set; }

        /// <summary>
        /// 時間
        /// </summary>
        [DataMember]
        public short Time { get; set; }
    }
}

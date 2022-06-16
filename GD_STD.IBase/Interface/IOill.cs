using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 非液壓油系統介面
    /// </summary>
    public interface IOill
    {
        /// <summary>
        /// 頻率/次數
        /// </summary>
        [DataMember]
        short Frequency { get; set; }
    }
}

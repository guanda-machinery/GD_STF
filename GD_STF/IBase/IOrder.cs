using System.Runtime.Serialization;

namespace GD_STD.IBase
{
    /// <summary>
    /// 有項目的物件
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// 項目
        /// </summary>
        [DataMember]
        short Index { get; set; }
    }
}
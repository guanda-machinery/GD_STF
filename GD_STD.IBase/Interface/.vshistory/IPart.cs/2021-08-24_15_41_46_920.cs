using GD_STD.Enum;
using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 單支鋼材的相關資訊
    /// </summary>
    public interface IPart
    {
        /// <summary>
        /// 長度
        /// </summary>
        [DataMember]
        double Length { get; set; }
        /// <summary>
        /// 物件材質
        /// </summary>
        MATERIAL Material { get; set; }
    }
}
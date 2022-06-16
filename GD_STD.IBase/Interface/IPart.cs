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
        /// 腹板厚度
        /// </summary>
        [DataMember]
        float t1 { get; set; }
        /// <summary>
        /// 翼板厚度
        /// </summary>
        [DataMember]
        float t2 { get; set; }
        /// <summary>
        /// 物件材質
        /// </summary>
        [DataMember]
        string Material { get; set; }
        /// <summary>
        /// 斷面規格 type
        /// </summary>
        [DataMember]
        OBJETC_TYPE Type { get; set; }
    }
}
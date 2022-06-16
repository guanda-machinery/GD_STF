using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 各類型剛的斷面輪廓資訊
    /// </summary>
    /// <remarks></remarks>
    public interface IProfile : IPart
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
        /// 斷面規格
        /// </summary>
        [DataMember]
        string Profile { get; set; }
    }
}
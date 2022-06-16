using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 各類型剛的斷面輪廓資訊
    /// </summary>
    /// <remarks></remarks>
    public interface IProfile
    {
        /// <summary>
        /// 高度
        /// </summary>
        [DataMember]
        float H { get; set; }
        /// <summary>
        /// 寬度
        /// </summary>
        [DataMember]
        float W { get; set; }
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
    }
}
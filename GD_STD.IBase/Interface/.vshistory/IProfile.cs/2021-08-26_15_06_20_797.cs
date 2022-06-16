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
        /// 斷面規格
        /// </summary>
        [DataMember]
        string Profile { get; set; }
    }
}
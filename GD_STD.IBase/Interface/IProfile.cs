using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 有輪廓物件
    /// </summary>
    /// <remarks></remarks>
    public interface IProfile 
    {
        /// <summary>
        /// 斷面規格
        /// </summary>
        [DataMember]
        string Profile { get; set; }
    }
}
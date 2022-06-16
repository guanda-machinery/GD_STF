using System.Runtime.Serialization;

namespace GD_STD
{
    /// <summary>
    /// 材料位置資訊
    /// </summary>
    /// <remarks> 代表著材料位置資訊的相關資訊 Codesys  Memory 讀取</remarks>
    [DataContract]
    public struct Material
    {
        /// <summary>
        /// 材料長度
        /// </summary>
        [DataMember]
        public double Length { get; set; }
        /// <summary>
        /// 目前位置
        /// </summary>
        [DataMember]
        public double Current { get; set; }
        /// <summary>
        /// 未執行位置
        /// </summary>
        [DataMember]
        public double NotPerformed { get; set; }
    }
}

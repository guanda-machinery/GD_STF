using GD_STD.Base;
using System.Runtime.Serialization;

namespace GD_STD
{
    /// <summary>
    /// 下壓夾具
    /// </summary>
    /// <remarks>
    /// 代表著側壓夾具的相關資訊
    /// Codesys  Memory 讀取
    /// </remarks>
    [DataContract]
    public struct ClampDown : IFixture, IClampDown
    {
        /// <summary>
        /// 入口左側
        /// </summary>
        ///  <remarks>面對加工機出料左邊向</remarks>
        [DataMember]
        public double EntranceL { get; set; }
        /// <summary>
        /// 出口左側
        /// </summary>
        ///  <remarks>面對加工機出料左邊向</remarks>
        [DataMember]
        public double ExportL { get; set; }
        /// <summary>
        /// 入口右側
        /// </summary>
        ///  <remarks>面對加工機出料右邊向</remarks>
        [DataMember]
        public double EntranceR { get; set; }
        /// <summary>
        /// 出口右側
        /// </summary>
        ///  <remarks>面對加工機出料右邊向</remarks>
        [DataMember]
        public double ExportR { get; set; }

    }
}

using System.Runtime.Serialization;

namespace GD_STD.IBase
{
    /// <summary>
    /// 夾具介面
    /// </summary>
    public interface IFixture
    {
        /// <summary>
        /// 入口左側
        /// </summary>
        ///  <remarks>面對加工機出料左邊向</remarks>
        [DataMember]
        double EntranceL { get; set; }
    }
}

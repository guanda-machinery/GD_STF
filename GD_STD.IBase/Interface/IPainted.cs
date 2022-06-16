using GD_STD.Base;
using System.Runtime.Serialization;

namespace GD_STD.IBase
{
    /// <summary>
    /// 給予加工軸的座標與畫線介面
    /// </summary>
    public interface IPainted
    {
        [DataMember]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IPainted.P1' 的 XML 註解
        Axis2D P1 { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IPainted.P1' 的 XML 註解
        [DataMember]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IPainted.P2' 的 XML 註解
        Axis2D P2 { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IPainted.P2' 的 XML 註解
        /// <summary>
        /// 直徑
        /// </summary>
        [DataMember]
        double Dia { get; set; }
        /// <summary>
        /// 加入畫線第一點
        /// </summary>
        /// <param name="axis2D"></param>

        void AddP1(IAxis2D axis2D);
        /// <summary>
        /// 加入畫線第二點
        /// </summary>
        /// <param name="axis2D"></param>
        void AddP2(IAxis2D axis2D);
        /// <summary>
        /// 對調 <see cref="P1"/> <see cref="P2"/> 座標位置
        /// </summary>
        void Reverse();
    }
}
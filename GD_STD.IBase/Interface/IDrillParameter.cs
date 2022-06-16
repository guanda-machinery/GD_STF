using GD_STD.Enum;
using System.Runtime.Serialization;

namespace GD_STD.Base
{
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IDrillParameter' 的 XML 註解
    public interface IDrillParameter
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IDrillParameter' 的 XML 註解
    {
        /// <summary>
        /// 鑽頭直徑
        /// </summary>
        [DataMember]
        double Dia { get; set; }
        /// <summary>
        /// 鑽頭類型
        /// </summary>
        DRILL_TYPE DrillType { get; set; }
        ///// <summary>
        ///// 走刀量。一般指切削加工中工件或切削工具每鏇轉一周(如車削)或往返一次(稱雙行程，如刨削)時，工件或切削工具的相對移動距離。
        ///// </summary>
        //float FeedQuantity { get; set; }
        /// <summary>
        /// 主軸設定轉速
        /// </summary>
        [DataMember]
        double Rpm { get; set; }
    }
}
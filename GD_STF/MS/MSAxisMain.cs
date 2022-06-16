using GD_STD.Attribute;
using System.Runtime.Serialization;
namespace GD_STD.MS
{
    /// <summary>
    /// SQL Server 主軸表單
    /// </summary>
    [MSTable("MSAxisMain")]
    [DataContract]
    public class MSAxisMain : AbsMS
    {
        /// <summary>
        /// 電流值
        /// </summary>
        [MSField("電流", "Electric")]
        [DataMember]
        public double Electric { get; set; }
        /// <summary>
        /// 軸向位置 
        /// </summary>
        /// <remarks>
        /// 軸向名稱 例如: R(右軸)，L(左軸)，M(中軸)
        /// </remarks>
        [MSField("軸向位置", "Position")]
        [DataMember]
        public string Position { get; set; }
        /// <summary>
        /// Z軸 RPM (轉速)
        /// </summary>
        [MSField("速度", "Speed")]
        [DataMember]
        public double Speed { get; set; }
        /// <summary>
        /// 走刀量。一般指切削加工中工件或切削工具每鏇轉一周(如車削)或往返一次(稱雙行程，如刨削)時，工件或切削工具的相對移動距離。
        /// </summary>
        [MSField("走刀量", "FeedQuantity")]
        [DataMember]
        public double FeedQuantity { get; set; }
    }
}

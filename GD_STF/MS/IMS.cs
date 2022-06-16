using System.Reflection;
using System.Runtime.Serialization;

namespace GD_STD.MS
{
    /// <summary>
    /// 定義類型版本資訊，擷取反射出 <see cref="AbsMS"/> 對應欄位與訊息。
    /// </summary>
    public interface IMS
    {
        ///// <summary>
        ///// 觸發時間
        ///// </summary>
        //[DataMember]
        //string DateTime { get; set; }
        ///// <summary>
        ///// 欄位數量
        ///// </summary>
        //int FieldCount();
        /// <summary>
        /// 所有欄位的值
        /// </summary>
        PropertyInfo[] AllValue();
        /// <summary>
        /// 取得類型的組件限定名稱，包含載入 System.Type 的組件名稱。
        /// </summary>
        [DataMember]
        string Type { get; set; }
    }
}

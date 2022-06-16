using System.Collections.Generic;

namespace GD_STD.Data
{
    /// <summary>
    /// 排版設定購件視圖介面
    /// </summary>
    public interface ITypeSettingAssemblyView
    {
        /// <summary>
        /// 編號
        /// </summary>
        string Number { get; set; }
        /// <summary>
        /// 區域
        /// </summary>
        List<int> Phase { get; set; }
        /// <summary>
        /// 運輸說明
        /// </summary>
        List<string> ShippingDescription { get; set; }
        /// <summary>
        /// 運輸號碼
        /// </summary>
        List<int> ShippingNumber { get; set; }
    }
}
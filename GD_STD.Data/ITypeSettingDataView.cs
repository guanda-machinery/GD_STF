namespace GD_STD.Data
{
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ITypeSettingDataView' 的 XML 註解
    public interface ITypeSettingDataView
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ITypeSettingDataView' 的 XML 註解
    {
        /// <summary>
        /// 構件編號
        /// </summary>
        string AssemblyNumber { get; set; }
        /// <summary>
        /// 區域
        /// </summary>
        int Phase { get; set; }
        /// <summary>
        /// 運輸說明
        /// </summary>
        string ShippingDescription { get; set; }
        /// <summary>
        /// 運輸號碼
        /// </summary>
        int ShippingNumber { get; set; }
    }
}
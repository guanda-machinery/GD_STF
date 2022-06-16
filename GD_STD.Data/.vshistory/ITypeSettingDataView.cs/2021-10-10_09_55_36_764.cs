namespace GD_STD.Data
{
    public interface ITypeSettingDataView
    {
        /// <summary>
        /// 構件編號
        /// </summary>
        string AssemblyNumber { get; set; }
        /// <summary>
        /// 區域
        /// </summary>
        string Phase { get; set; }
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
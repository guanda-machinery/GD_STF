namespace GD_STD.Data
{
    public interface ITypeSettingDataView
    {
        string AssemblyNumber { get; set; }
        string Phase { get; set; }
        string ShippingDescription { get; set; }
        int ShippingNumber { get; set; }
    }
}
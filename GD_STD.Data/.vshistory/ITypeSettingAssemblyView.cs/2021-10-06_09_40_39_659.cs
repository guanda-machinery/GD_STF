using System.Collections.Generic;

namespace GD_STD.Data
{
    public interface ITypeSettingAssemblyView
    {
        string Number { get; set; }
        List<int> Phase { get; set; }
        List<string> ShippingDescription { get; set; }
        List<int> ShippingNumber { get; set; }
    }
}
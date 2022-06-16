using System;

namespace STD_105
{
    public class MATERIAL_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<GD_STD.Enum.MATERIAL>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

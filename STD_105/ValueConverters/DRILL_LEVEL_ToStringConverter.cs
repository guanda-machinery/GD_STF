using GD_STD.Enum;
using System;
namespace STD_105
{
    public class DRILL_LEVEL_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<DRILL_LEVEL>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

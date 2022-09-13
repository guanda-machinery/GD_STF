using GD_STD.Enum;
using System;
using WPFSTD105;

namespace STD_105
{
    public class OBJECT_TYPE_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<OBJECT_TYPE>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

    }
}

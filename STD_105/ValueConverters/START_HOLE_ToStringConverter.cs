using System;
using WPFSTD105;

namespace STD_105
{
    class START_HOLE_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<START_HOLE>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

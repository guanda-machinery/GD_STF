using GD_STD.Enum;
using System;
namespace STD_105
{
    public class DRILL_TYPE_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<DRILL_TYPE>
    {
        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

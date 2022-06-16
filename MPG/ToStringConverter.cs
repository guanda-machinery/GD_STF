using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GD_STD.Enum;
namespace MPG
{
    public class AxisSelected_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<AXIS_SELECTED>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class Coordinat_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<COORDINATE>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class Magnification_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<MAGNIFICATION>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

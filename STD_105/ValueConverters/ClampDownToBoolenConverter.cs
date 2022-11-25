using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STD_105
{
    internal class ClampDownToBoolenConverter : WPFWindowsBase.BaseValueConverter<ClampDownToBoolenConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GD_STD.Enum.CLAMP_DOWN)
            {
                if (parameter is GD_STD.Enum.CLAMP_DOWN)
                {
                    return (GD_STD.Enum.CLAMP_DOWN)value == (GD_STD.Enum.CLAMP_DOWN)parameter;
                }
            }
            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

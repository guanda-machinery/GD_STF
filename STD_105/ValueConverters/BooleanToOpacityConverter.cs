using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    internal class BooleanToOpacityConverter : BaseValueConverter<BooleanToOpacityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double Opacity = 0.1;
            if(parameter is string)
            {
               if( double.TryParse((string)parameter, out var result))
                    Opacity = result;
            }

            if (value is bool)
            {
                return ((bool)value) ? 1.0 : Opacity;
            }

            return 1.0;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

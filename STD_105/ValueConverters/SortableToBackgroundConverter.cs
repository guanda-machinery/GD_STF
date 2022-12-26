using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STD_105
{
    internal class SortableToBackgroundConverter : WPFWindowsBase.BaseValueConverter<SortableToBackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if (!(bool)value)
                {
                    if (parameter is System.Windows.Media.SolidColorBrush)
                        return parameter as System.Windows.Media.SolidColorBrush;
                    else
                        return System.Windows.Media.Brushes.Red;
                }
            }
            return System.Windows.Media.Brushes.Transparent;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

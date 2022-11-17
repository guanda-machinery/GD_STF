using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace STD_105
{
    internal class BooleanToForefroundConverter : WPFWindowsBase.BaseValueConverter<BooleanToForefroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool)
            { 
                if((bool)value)
                {
                    return System.Windows.Media.Brushes.Red;
                }
            }
        
            if(parameter is System.Windows.Media.SolidColorBrush)
            {
                return parameter as System.Windows.Media.SolidColorBrush;
            }


            return System.Windows.Media.Brushes.White;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

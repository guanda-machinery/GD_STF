using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace STD_105
{
    internal class BooleanToSolidColorBrushConverter : WPFWindowsBase.BaseValueConverter<BooleanToSolidColorBrushConverter>
    {
        /// <summary>
        /// true的時候回傳顏色 false回傳透明
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                {
                    if (parameter is System.Windows.Media.SolidColorBrush)
                        return parameter as System.Windows.Media.SolidColorBrush;
                    else if(parameter is string)
                    {
                        if (!parameter.ToString().Contains("#"))
                            return (SolidColorBrush)(new BrushConverter().ConvertFrom($"#{parameter}"));
                        else
                            return (SolidColorBrush)(new BrushConverter().ConvertFrom($"{parameter}"));
                    }
                    else
                        return System.Windows.Media.Brushes.BlueViolet;
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

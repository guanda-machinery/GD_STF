using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;
using CalcBinding.Inversion;

namespace STD_105
{
    /// <summary>
    /// 將double轉換為boolean
    /// </summary>
    public class IntToBooleanConverter : MarkupExtension, IValueConverter
    {

        public bool Invert { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var ValueBiggerThanZero = (int)value > 0 ;

                return (!Invert)? ValueBiggerThanZero : !ValueBiggerThanZero;
            }
            return Invert;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Invert ? 0 : 1 ;
            }
            return !Invert ? 1 : 0;
        }

    }
}

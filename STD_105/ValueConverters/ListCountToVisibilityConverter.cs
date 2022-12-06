using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STD_105
{
    internal class ListCountToVisibilityConverter : WPFWindowsBase.BaseValueConverter<ListCountToVisibilityConverter>
    {
        public bool Invert { get; set; }
        public int ListLimitCount { get; set; } = 0;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility _visibility = Visibility.Collapsed;
            if (value is IEnumerable<object>)
            {
                var ValueCount = (value as IEnumerable<object>).Count();
                if(ValueCount > ListLimitCount)
                {
                    return (Invert) ? _visibility : Visibility.Visible;
                }
            }

            return (!Invert) ? _visibility : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            if (visibility == Visibility.Collapsed)
            {
                return Invert;
            }

            return !Invert;
        }
    }
}

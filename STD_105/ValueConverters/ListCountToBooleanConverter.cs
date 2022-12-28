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
    internal class ListCountToBooleanConverter : WPFWindowsBase.BaseValueConverter<ListCountToBooleanConverter>
    {
        public bool Invert { get; set; }
        public int ListLimitCount { get; set; } 

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<object>)
            {
                var ValueCount = (value as IEnumerable<object>).Count();
                var Rb = ValueCount > ListLimitCount;
                return (!Invert) ? Rb : !Rb;
            }

            if (!Invert)
                return false;
            else
                return true;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STD_105
{
    internal class ObjectIsNullToBooleanConverter : WPFWindowsBase.BaseValueConverter<ObjectIsNullToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return false;

            if (value is string)
            {
                if (string.IsNullOrEmpty(value as string) || (string.IsNullOrWhiteSpace(value as string)))
                {
                    return false;
                }
            }
            return true;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

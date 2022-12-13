using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    public class DateTimeConverter : BaseValueConverter<DateTimeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            if (value is DateTime )
            {
                if ((DateTime)value == DateTime.MinValue)
                {
                    return "";
                }
                return ((DateTime)value).ToString("yyyy/MM/dd");
            }

            return (DateTime)value == DateTime.MinValue ? null :(object) ((DateTime)value).Date;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

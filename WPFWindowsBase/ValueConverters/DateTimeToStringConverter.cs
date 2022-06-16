using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase
{
    /// <summary>
    /// 接受<see cref="DateTime"/> 並返回 <see cref="DateTime.ToString()"/> 的轉換器
    /// </summary>
    public class DateTimeToStringConverter : BaseValueConverter<DateTimeToStringConverter>
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'DateTimeToStringConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'DateTimeToStringConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        {
            DateTime time = (DateTime)value;
            return time.ToString("yyyy-MM-dd HH:mm:mm");
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'DateTimeToStringConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'DateTimeToStringConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;

namespace WPFWindowsBase
{
    /// <summary>
    /// 日期選擇器查詢字符串轉換器
    /// </summary>
    public class DatePickerToQueryStringConverter : BaseValueConverter<DatePickerToQueryStringConverter>
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'DatePickerToQueryStringConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'DatePickerToQueryStringConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        {
            object result = null;
            if (value != null && value.ToString() == string.Empty)
            {
                result = null;
            }
            else
            {
                DateTime dataTime;

                if (DateTime.TryParse(
                    value.ToString(), culture.DateTimeFormat,
                    System.Globalization.DateTimeStyles.None,
                    out dataTime))
                {
                    result = dataTime;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'DatePickerToQueryStringConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'DatePickerToQueryStringConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        {
            return value;
        }
    }
}

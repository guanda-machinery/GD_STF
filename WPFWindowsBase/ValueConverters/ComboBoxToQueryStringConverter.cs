using System;
using System.Globalization;
using System.Windows.Controls;

namespace WPFWindowsBase
{
    /// <summary>
    /// <see cref="ComboBox"/>轉換查詢字串的轉換器
    /// </summary>
    public class ComboBoxToQueryStringConverter : BaseValueConverter<ComboBoxToQueryStringConverter>
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ComboBoxToQueryStringConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ComboBoxToQueryStringConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        {
            return value != null && value.ToString() == string.Empty ? null : value;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ComboBoxToQueryStringConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ComboBoxToQueryStringConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        {
            return value;
        }
    }
}

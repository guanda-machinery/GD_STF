using System;
using System.Globalization;

namespace WPFWindowsBase
{
    /// <summary>
    /// 字體大小到高度轉換器
    /// </summary>
    public class FontSizeToHeightConverter : BaseValueConverter<FontSizeToHeightConverter>
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'FontSizeToHeightConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'FontSizeToHeightConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        {
            double h;
            if (value == null)
                return value;

            if (double.TryParse(value.ToString(), out h))
                return h * 2;
            else
                return double.NaN;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'FontSizeToHeightConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'FontSizeToHeightConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        {
            return value;
        }
    }
}

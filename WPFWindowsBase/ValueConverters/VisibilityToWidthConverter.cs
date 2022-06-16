using System;
using System.Globalization;

namespace WPFWindowsBase
{
    /// <summary>
    /// 可見寬度轉換器
    /// </summary>
    public class VisibilityToWidthConverter : BaseValueConverter<VisibilityToWidthConverter>
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'VisibilityToWidthConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'VisibilityToWidthConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        {
            if (value == null)
                throw new Exception("錯誤，不可接受 'value' 是空值");

            System.Windows.Visibility visibility = (System.Windows.Visibility)value;

            return visibility == System.Windows.Visibility.Visible ? double.NaN : 0;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'VisibilityToWidthConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'VisibilityToWidthConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        {
            return value;
        }
    }
}

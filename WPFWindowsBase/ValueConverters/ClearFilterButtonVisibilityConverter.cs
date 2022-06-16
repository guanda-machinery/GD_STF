using System;
using System.Globalization;

namespace WPFWindowsBase
{
    /// <summary>
    /// 清除篩選器按鈕可見性轉換器
    /// </summary>
    public class ClearFilterButtonVisibilityConverter : BaseMultiValueConverter<ClearFilterButtonVisibilityConverter>
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ClearFilterButtonVisibilityConverter.Convert(object[], Type, object, CultureInfo)' 的 XML 註解
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ClearFilterButtonVisibilityConverter.Convert(object[], Type, object, CultureInfo)' 的 XML 註解
        {
            if ((bool)values[0] && (bool)values[1])
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Collapsed;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ClearFilterButtonVisibilityConverter.ConvertBack(object, Type[], object, CultureInfo)' 的 XML 註解
        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ClearFilterButtonVisibilityConverter.ConvertBack(object, Type[], object, CultureInfo)' 的 XML 註解
        {
            throw new NotImplementedException();
        }
    }
}

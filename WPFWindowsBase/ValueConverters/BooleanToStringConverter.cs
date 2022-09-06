using System;
using System.Globalization;

namespace WPFWindowsBase
{
    /// <summary>
    /// 接受<see cref="bool"/> 並返回 <see cref="string"/> 高度的轉換器
    /// </summary>
    public class BooleanToStringConverter : BaseValueConverter<BooleanToStringConverter>
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'BooleanToHeightConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'BooleanToHeightConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        {
            if ((bool)value)
                return value.ToString();
            else
                return 0;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'BooleanToHeightConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'BooleanToHeightConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        {
            return value;
        }
    }
}

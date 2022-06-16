using System;
using System.Globalization;

namespace WPFWindowsBase
{
    /// <summary>
    /// 接受<see cref="object"/>並返回<see cref="bool"/>的轉換器
    /// </summary>
    public class CheckBoxValueConverter : BaseValueConverter<CheckBoxValueConverter>
    {

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CheckBoxValueConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CheckBoxValueConverter.Convert(object, Type, object, CultureInfo)' 的 XML 註解
        {
            bool result = false;

            if (value != null && value.GetType() == typeof(string))
                bool.TryParse(value.ToString(), out result);
            else if (value != null)
                result = System.Convert.ToBoolean(value);

            return result;
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'CheckBoxValueConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'CheckBoxValueConverter.ConvertBack(object, Type, object, CultureInfo)' 的 XML 註解
        {
            return value;
        }
    }
}

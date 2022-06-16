using System;
using System.Globalization;
using System.Windows.Media;

namespace WPFWindowsBase
{
    /// <summary>
    /// 接收RGB字符串（例如FF00FF）並將其轉換為WPF的<see cref="SolidColorBrush"/>轉換器
    /// </summary>
    public class StringRGBToBrushConverter : BaseValueConverter<StringRGBToBrushConverter>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.ToString().Contains("#"))
                return (SolidColorBrush)(new BrushConverter().ConvertFrom($"#{value}"));
            else
                return (SolidColorBrush)(new BrushConverter().ConvertFrom($"{value}"));
        }
        /// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

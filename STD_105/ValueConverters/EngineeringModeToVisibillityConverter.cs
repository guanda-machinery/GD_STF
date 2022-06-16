using System;
using System.Globalization;
using System.Windows;

namespace STD_105
{
    /// <summary>
    /// 一個工程模式的轉換器 
    /// </summary>
    public class EngineeringModeToVisibillityConverter : WPFWindowsBase.BaseValueConverter<EngineeringModeToVisibillityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == true ? Visibility.Hidden : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

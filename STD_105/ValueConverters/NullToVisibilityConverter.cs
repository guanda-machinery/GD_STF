using System;
using System.Globalization;
using System.Windows;

namespace STD_105
{
    /// <summary>
    /// 一個將 null 隱藏的轉換器
    /// </summary>
    public class NullToVisibilityConverter : WPFWindowsBase.BaseValueConverter<NullToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

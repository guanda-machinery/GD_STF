using System;
using System.Globalization;
using System.Windows;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    ///  一個效能控制的轉換器，因 PAC 效能過低，所以需要動畫進入前，頁面所有控制項必須隱藏，使用圖片代替控制項完成動畫，在顯示控制項
    /// </summary>
    public class PerformanceToConverter : BaseValueConverter<PerformanceToConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            else
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

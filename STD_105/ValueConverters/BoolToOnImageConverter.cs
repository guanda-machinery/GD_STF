using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// 將開關狀態轉換為圖片
    /// </summary>
    public class BoolToOnImageConverter : BaseValueConverter<BoolToOnImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bo = (bool)value;//狀態
            if (bo)//如果狀態開啟
            {
                return Application.Current.Resources["LightBulbOn"] as ImageSource;
            }
            else
            {
                return Application.Current.Resources["LightBulbOff"] as ImageSource;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

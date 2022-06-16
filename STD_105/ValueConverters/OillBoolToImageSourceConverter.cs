using GD_STD;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// <see cref="PanelButton.Oil"/> 狀態 ，更換目前按鈕圖片/>
    /// </summary>
    public class OillBoolToImageSourceConverter : BaseValueConverter<OillBoolToImageSourceConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool _ = (bool)value;
            if (_ == true)
                return Application.Current.Resources["Oil_Open"] as ImageSource;
            else
                return Application.Current.Resources["Oil_Close"] as ImageSource;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

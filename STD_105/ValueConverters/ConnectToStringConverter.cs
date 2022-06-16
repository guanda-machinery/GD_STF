using System;
using System.Globalization;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// 將網路狀態轉換成字串
    /// </summary>
    public class ConnectToStringConverter : BaseValueConverter<ConnectToStringConverter>
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool _ = (bool)value;
            if (_)
                return "物聯網";
            else
                return "單機";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

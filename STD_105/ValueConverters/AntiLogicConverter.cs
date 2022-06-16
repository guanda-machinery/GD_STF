using System;
using System.Globalization;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// 一個反邏輯的轉換器
    /// </summary>
    public class AntiLogicConverter : BaseValueConverter<AntiLogicConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool _ = (bool)value;
            return !_;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace STD_105
{
    /// <summary>
    /// 多重綁定命令參數的轉換器
    /// </summary>
    public class ComParConverter  : WPFWindowsBase.BaseValueConverter<ComParConverter> , IMultiValueConverter
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var values = (object[])value;
            return values.Clone();
        }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

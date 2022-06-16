using System;
using System.Drawing;
using System.Globalization;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// 接收RGB字符串（例如#FF00FF）並將其轉換為<see cref="System.Drawing.Color"/>轉換器
    /// </summary>
    public class StringToDrawingColoConverter : BaseValueConverter<StringToDrawingColoConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ColorTranslator.FromHtml($"#{value}");
            }
            catch (Exception)
            {
                throw new Exception("value 不是16進制色碼");
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

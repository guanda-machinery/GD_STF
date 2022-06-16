//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Media;
//using WPFWindowsBase;

//namespace STD_105
//{
//    /// <summary>
//    /// 接收RGB字符串（例如#FF00FF）並將其轉換為<see cref="Brush"/>轉換器
//    /// </summary>
//    public class StringToBrushConverter : BaseValueConverter<StringToBrushConverter>
//    {
//        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return (SolidColorBrush)(new BrushConverter().ConvertFrom(value.ToString()));
//        }

//        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

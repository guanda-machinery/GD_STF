using System;
using System.Globalization;
using WPFSTD105.Model;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// 一個反邏輯的轉換器
    /// </summary>
    public class LogSourceEnumToStringConverter : BaseValueConverter<LogSourceEnumToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LogSourceEnum)
            {
                var source = "";
                switch ((LogSourceEnum)value)
                {
                    case LogSourceEnum.Init:
                        source = "初始";
                        break;
                    case LogSourceEnum.Phone:
                        source = "手機";
                        break;
                    case LogSourceEnum.Machine:
                        source = "機台";
                        break;
                    case LogSourceEnum.Software:
                        source = "操作";
                        break;
                        default:
                        source = "未定義";
                        break;
                }
                return source;
            }
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}

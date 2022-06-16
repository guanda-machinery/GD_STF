//TODO:等待測試完成
#define _Debug

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase = WPFWindowsBase;
using static WPFSTD105.ViewLocator;
using WPFSTD105;

namespace STD_105
{
    /// <summary>
    /// 顯示工程模式才能看到的控制向
    /// </summary>
    public class EngineerModeConverter : WPFBase.BaseValueConverter<EngineerModeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccountNumber _ = (AccountNumber)value;
#if !Debug
            //TODO : 密碼之後要修改目前測試用
            if (_.Account == "GUANDA_Administrator" && _.PasswordText == "Ab123")
            {
                return System.Windows.Visibility.Hidden;
            }
            return System.Windows.Visibility.Visible;
#else
            return System.Windows.Visibility.Hidden;
#endif
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

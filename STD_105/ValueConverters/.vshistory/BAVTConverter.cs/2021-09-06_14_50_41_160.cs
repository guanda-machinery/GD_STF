using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105;
using WPFBase = WPFWindowsBase;
using GD_STD.Enum;
using System.Windows.Media;
using System.Windows;

namespace STD_105
{
    /// <summary>
    /// 轉換器 <see cref="BAVT"/> 轉換圖片
    /// </summary>
    public class BAVTConverter : WPFBase.BaseValueConverter<BAVTConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BAVT bAVT = (BAVT)value;
            string _ = $"Battery_{nameof(bAVT)}";
            return Application.Current.Resources[$"Battery_{nameof(bAVT)}"] as ImageSource;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

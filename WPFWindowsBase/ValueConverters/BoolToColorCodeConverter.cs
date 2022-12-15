using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFWindowsBase
{
    /// <summary>
    /// 把True轉換成藍色，False換為物件底色
    /// </summary>
    public class BoolToColorCodeConverter : BaseValueConverter<BoolToColorCodeConverter>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ParameterBackColor = "#FF303030";
            if ((bool)value)
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#fa7070");
            }
            else
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom(ParameterBackColor);
            }
        }
        /// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

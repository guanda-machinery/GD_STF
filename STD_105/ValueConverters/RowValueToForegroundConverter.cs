using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPFWindowsBase;

namespace STD_105
{
    public class RowValueToForegroundConverter : BaseValueConverter<RowValueToForegroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string rowValue = (string)value;
            if (rowValue == null)
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#FF262626");
            }
            if (rowValue == "完成" || rowValue == "加工中" || rowValue == "搬運中")
                return (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Foreground);
            else if (rowValue.Contains("等待("))
                return (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Foreground);
            else
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#FF262626");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

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
    /// <summary>
    /// 報表數值轉顏色
    /// </summary>
    public class RowValueToBackgroundConverter : BaseValueConverter<RowValueToBackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {           
            string rowValue = (string)value;
            if (rowValue == null)
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#FFD4D4D4");
            }
            if (rowValue == "完成")
                return (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Finish);
            else if (rowValue == "加工中")
                return (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Processing);
            else if (rowValue.Contains("等待("))
                return (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Waiting);
            else if (rowValue.Contains("搬運中"))
                return (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Moving);
            else
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#FFD4D4D4");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

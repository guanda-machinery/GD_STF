using DevExpress.Xpf.Gauges;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPFSTD105;
using WPFWindowsBase;

namespace STD_105
{
    internal class TypeSettingDataViewToSolidColorBrushConverter : BaseValueConverter<TypeSettingDataViewToSolidColorBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PositionStatusEnum)
            {
                switch((PositionStatusEnum)value)
                {
                    case PositionStatusEnum.完成:
                        return  (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Finish);
                    case PositionStatusEnum.加工中:
                        return  (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Processing);
                    case PositionStatusEnum.等待配對:
                        return (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Waiting);
                    case PositionStatusEnum.等待入料:
                        return (SolidColorBrush)new BrushConverter().ConvertFrom("#47edff");
                        //return (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Moving);
                    case PositionStatusEnum.等待出料:
                        return (SolidColorBrush)new BrushConverter().ConvertFrom("#ff479a");
                       // return (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Moving);
                    case PositionStatusEnum.不可配對:
                        return (SolidColorBrush)new BrushConverter().ConvertFrom("#ff4747");
                    default:
                        return (SolidColorBrush)new BrushConverter().ConvertFrom("#FFD4D4D4");
                        //(!IsSelected) ? Brushes.Transparent : 
                }
            }
            else
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#FFD4D4D4");
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

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
            bool IsSelected = false;
            if (parameter is string)
            {
                if ((string)parameter == "IsSelected")
                    IsSelected = true;
            }

            if (value is PositionStatusEnum)
            {
                var RowValue = (PositionStatusEnum)value ;

                //槽鐵標示

                switch(RowValue)
                {
                    case PositionStatusEnum.完成:
                        return (!IsSelected) ? (SolidColorBrush)Brushes.Transparent : (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Finish);
                    case PositionStatusEnum.加工中:
                        return (!IsSelected) ? (SolidColorBrush)Brushes.Transparent : (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Processing);
                    case PositionStatusEnum.等待配對:
                        return (!IsSelected) ? (SolidColorBrush)Brushes.Transparent : (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Waiting);
                    case PositionStatusEnum.等待入料:
                        return (!IsSelected) ? (SolidColorBrush)Brushes.Transparent : (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Moving);
                    case PositionStatusEnum.等待出料:
                        return (!IsSelected) ? (SolidColorBrush)Brushes.Transparent : (SolidColorBrush)new BrushConverter().ConvertFrom(WPFSTD105.Properties.SofSetting.Default.Report_Moving);
                    case PositionStatusEnum.不可配對:
                        return (!IsSelected) ? (SolidColorBrush)new BrushConverter().ConvertFrom("#fa7070") : (SolidColorBrush)new BrushConverter().ConvertFrom("#ff4747");
                    default:
                        return (!IsSelected) ? Brushes.Transparent : (SolidColorBrush)new BrushConverter().ConvertFrom("#FFD4D4D4");
                }
            }
            else
            {
                return (!IsSelected) ? (SolidColorBrush)Brushes.Transparent : (SolidColorBrush)new BrushConverter().ConvertFrom("#FFD4D4D4");
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

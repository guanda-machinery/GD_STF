using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFWindowsBase;

namespace STD_105
{
    internal class MenuItem_FUDataViewsToVisibilityConverter : BaseValueConverter<MenuItem_FUDataViewsToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter is string)
            {
                if((string)parameter == "Recover")
                {
                    //只要清單內有等待配對以外的選項 就顯示

                    var PairedIsExisted = (value as ObservableCollection<MaterialDataView>).ToList().Exists(x => (x.PositionEnum != GD_STD.Enum.PositionStatusEnum.等待配對));
                    return PairedIsExisted ? Visibility.Visible : Visibility.Collapsed;
                }


               if ( (string)parameter == "Paired")
                {
                    //只要清單內有任意配對 就顯示解除配對控制項
                    var PairedIsExisted =(value as ObservableCollection<MaterialDataView>).ToList().Exists(x =>
                    (x.PositionEnum == GD_STD.Enum.PositionStatusEnum.軟體配對) ||
                    (x.PositionEnum == GD_STD.Enum.PositionStatusEnum.手機配對) ||
                    (x.PositionEnum == GD_STD.Enum.PositionStatusEnum.手動配對)
                    );
                    return PairedIsExisted ? Visibility.Visible: Visibility.Collapsed;
                }

                if ((string)parameter == "Finish")
                {
                    //只要清單內有無完成之選項 就顯示完成控制項
                    var PairedIsExisted = (value as ObservableCollection<MaterialDataView>).ToList().Exists(x =>(x.PositionEnum != GD_STD.Enum.PositionStatusEnum.完成));
                    return PairedIsExisted ? Visibility.Visible : Visibility.Collapsed;
                }

            }

            return Visibility.Visible;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

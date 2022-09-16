using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    internal class GridControlSelectedItemToBoolenConverter : BaseValueConverter<GridControlSelectedItemToBoolenConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GD_STD.Data.TypeSettingDataView)
            {

                var GridSelectedItem = ((GD_STD.Data.TypeSettingDataView)value);
                if (GridSelectedItem is null)
                {  
                    //未選擇
                    return false;
                }
                else
                {
                    //有選擇
                    return true;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

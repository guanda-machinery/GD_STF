using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFWindowsBase;

namespace STD_105
{
    public class RackIsEnabledConvertver : BaseMultiValueConverter<RackIsEnabledConvertver>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return false;
            }

            if ((KEY_HOLE)values[0] == KEY_HOLE.AUTO || (ERROR_CODE)values[1] != ERROR_CODE.Null || (bool)values[2] ||  !((bool)values[3]))
                return false;
            if (GD_STD.Properties.Optional.Default.EntranceTraverseNumber == 0 && GD_STD.Properties.Optional.Default.ExportTraverseNumber == 0)
                return false;
            else
                return true;
        }
        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

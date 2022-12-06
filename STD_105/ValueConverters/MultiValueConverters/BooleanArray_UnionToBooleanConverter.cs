using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace STD_105
{
    /// <summary>
    /// 聯集布林
    /// </summary>
    public class BooleanArrayUnionToBooleanConverter : WPFWindowsBase.BaseMultiValueConverter<BooleanArrayUnionToBooleanConverter> 
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach(var val in values)
            { 
                if (val is true)
                {
                    return true;
                }
            }
            return false;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

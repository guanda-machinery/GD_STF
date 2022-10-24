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
    /// 將ischecked轉換為isenable
    /// </summary>
    public class BooleanArrayUnionToBooleanConverter : WPFWindowsBase.BaseMultiValueConverter<BooleanArrayUnionToBooleanConverter> 
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (object value in values)
            {
                if (value is true)
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

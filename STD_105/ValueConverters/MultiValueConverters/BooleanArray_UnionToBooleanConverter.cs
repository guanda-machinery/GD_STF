using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace STD_105
{
    /// <summary>
    /// 
    /// </summary>


    /// <summary>
    /// 聯集布林
    /// </summary>
    public class BooleanArrayUnionToBooleanConverter : WPFWindowsBase.BaseMultiValueConverter<BooleanArrayUnionToBooleanConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var BooleanList = values.ToList();
            bool ReturnBoolean = BooleanList.Exists(x =>x is true);

            return ReturnBoolean;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
           // throw new NotImplementedException();
        }
    }
}

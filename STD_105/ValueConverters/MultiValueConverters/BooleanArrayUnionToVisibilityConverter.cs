using CalcBinding.Inversion;
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
    public class BooleanArrayUnionToVisibilityConverter : WPFWindowsBase.BaseMultiValueConverter<BooleanArrayUnionToVisibilityConverter>
    {
        public bool Invert { get; set; }
        //只要出現false就不顯示
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var BooleanList = values.ToList();
            bool ReturnBoolean = false;
            if (!Invert)
                ReturnBoolean = BooleanList.Exists(x => x is false);
           else
                ReturnBoolean = BooleanList.Exists(x => x is true);

            return ReturnBoolean? Visibility.Collapsed : Visibility.Visible;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
           // throw new NotImplementedException();
        }
    }
}

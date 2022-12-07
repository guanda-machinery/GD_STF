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
    public class BooleanArray_In_UnionToBooleanConverter : WPFWindowsBase.BaseMultiValueConverter<BooleanArray_In_UnionToBooleanConverter> 
    {

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //特殊Convert:當第一項為false時直接回傳false ，為true時則找有沒有其他項，若存在其他項則回傳其他項的聯集，沒有其他項則回傳true

            //當第一項=false時必定回傳false 當兩項以上時則看第二項之後的聯集

            if (values.Count() == 1)
            {
                if (values[0] is bool)
                    return (bool)values[0];
            }

            if (values[0] is false)
            {
                return false;
            }

            for (int i = 1; i < values.Count(); i++)
            {
                if (values[i] is true)
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

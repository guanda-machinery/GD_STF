using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STD_105
{
    /// <summary>
    /// 檢查多重綁定中是否有空值，有空值則回傳false
    /// </summary>
    internal class MultiObjectArrayToBooleanConverter : WPFWindowsBase.BaseMultiValueConverter<MultiObjectArrayToBooleanConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach(var EachValue in values)
            {
                if (EachValue is null)
                    return false;

                if(EachValue is string)
                {
                    if (string.IsNullOrEmpty(EachValue as string) || (string.IsNullOrWhiteSpace(EachValue as string)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

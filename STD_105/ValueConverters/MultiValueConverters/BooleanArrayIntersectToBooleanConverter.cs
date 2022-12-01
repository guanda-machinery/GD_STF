using DevExpress.CodeParser;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Gauges;
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
    public class BooleanArrayIntersectToBooleanConverter : WPFWindowsBase.BaseMultiValueConverter<BooleanArrayIntersectToBooleanConverter> 
    {
        /// <summary>
        /// 交集
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < values.Count(); i++)
            {
                if (values[i] is true)
                {
                    continue;
                }
                else
                {
                    return false;
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

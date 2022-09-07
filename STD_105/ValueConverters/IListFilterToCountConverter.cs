using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// <see cref="IList{T}"/> 符合 T 參數的數量
    /// </summary>
    public class IListFilterToCountConverter : BaseValueConverter<IListFilterToCountConverter>
    {
        /// <summary>
        /// 計算list的內容量
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /// 20220830 蘇冠綸改寫 改為可輸入任何list都可計算數量
           var ValueIList =  (System.Collections.IList)value;
            if (parameter is bool)
           {
                IList<bool> ts = (IList<bool>)value;
                int result = ts.Where(el => el == (bool)parameter).Count();
                return result;
            }
            else if(parameter is int)
            {
                IList<int> ts = (IList<int>)value;
                int result = ts.Where(el => el == (int)parameter).Count();
                return result;
            }
            return ValueIList.Count;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// <see cref="IList{T}"/> 符合 T 參數的數量必須符合列表
    /// </summary>
    public class IListFilterAllToCountConverter : BaseValueConverter<IListFilterAllToCountConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ValueIList = (System.Collections.IList)value;
            if (parameter is bool)
            {
                IList<bool> ts = (IList<bool>)value;
                return ts.Where(el => el == (bool)parameter).Count() == ts.Count;
            }
            else
            {
                return ValueIList.Count;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

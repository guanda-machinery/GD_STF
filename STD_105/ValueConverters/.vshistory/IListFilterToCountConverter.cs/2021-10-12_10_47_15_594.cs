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
    public class IListFilterToCountConverter : BaseValueConverter<IListToCountConerter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return null;
            }
            IList<object> ts = (IList<object>)value;
            int result = ts.Where(el => el == parameter).Count();
            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// <see cref="IList{T}"/> 符合 T 參數的數量必須符合列表
    /// </summary>
    public class IListFilterAllToCountConverter : BaseValueConverter<IListToCountConerter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return null;
            }
            IList<object> ts = (IList<object>)value;
            return ts.Where(el => el == parameter).Count() == ts.Count;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

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
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return null;
            }
            IList<bool> ts = (IList<bool>)value;
            int result = ts.Where(el => el == (bool)parameter).Count();
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
    public class IListFilterAllToCountConverter : BaseValueConverter<IListFilterAllToCountConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return null;
            }
            IList<bool> ts = (IList<bool>)value;
            return ts.Where(el => el == (bool)parameter).Count() == ts.Count;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

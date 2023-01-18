using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// <see cref="IList"/> 陣列長度轉換器
    /// </summary>
    public class IListToCountConerter : BaseValueConverter<IListToCountConerter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList)
            {
                IList ts = (IList)value;
                return ts.Count;
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

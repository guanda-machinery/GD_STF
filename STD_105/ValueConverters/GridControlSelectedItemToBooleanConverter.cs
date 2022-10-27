using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    internal class GridControlSelectedItemToBooleanConverter : BaseValueConverter<GridControlSelectedItemToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {


            if (value is DevExpress.Xpf.Editors.CheckEdit)
            {
                return (value as DevExpress.Xpf.Editors.CheckEdit).IsChecked;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

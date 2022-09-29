using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    internal class GridControlWidthConvert : BaseValueConverter<GridControlWidthConvert>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DevExpress.Xpf.Grid.GridControl)
            {
                var GridC = (DevExpress.Xpf.Grid.GridControl)value;
                foreach(var Col in GridC.Columns)
                {
                    if (Col.Name == (string)parameter)
                    {
                        return Col.ActualDataWidth;
                    }
                }


                return "auto";
            }
            throw new NotImplementedException();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

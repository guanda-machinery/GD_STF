using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    public class MaterialToNameConverter : BaseValueConverter<MaterialToNameConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<SteelMaterial> drillBrands = (ObservableCollection<DrillBrand>)value;
            List<string> result = new List<string>();
            drillBrands.ForEach(el => result.Add(el.DataName));
            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

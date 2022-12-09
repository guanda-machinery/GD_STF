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
    internal class MaterialDataViewHasPairToBooleanConverter : BaseValueConverter<MaterialDataViewHasPairToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is ObservableCollection < MaterialDataView >)
            {
                var DataView = (value as ObservableCollection<MaterialDataView>).ToList();
                if (DataView.Count > 0)
                {
                    return DataView.Exists(x => (x.Position.Contains("配對") && !x.Position.Contains("等待配對")));
                }
                return false;
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

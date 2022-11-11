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
    public class ConverterToPartNumber : BaseValueConverter<ConverterToPartNumber>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<TypeSettingDataView> views = (ObservableCollection<TypeSettingDataView>)value;
            if (views.Count > 0)
            {
                string result = views.Select(el => el.PartNumber)
                                                        .Aggregate((str1, str2) => $"{str1},{str2}");
                return result;
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

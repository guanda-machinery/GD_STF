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
    public class ConverterAssemblyNumber : BaseValueConverter<ConverterAssemblyNumber>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<TypeSettingDataView> views = (ObservableCollection<TypeSettingDataView>)value;
            string result = "";
            if (views.Count > 0)
            {
                result = views.Select(el => el.AssemblyNumber).Aggregate((str1, str2) => $"{str1},{str2}");
            }
            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

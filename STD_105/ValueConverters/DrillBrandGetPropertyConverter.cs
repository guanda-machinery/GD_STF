using GD_STD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105;

namespace STD_105
{
    /// <summary>
    /// <see cref="DrillBrand"/> 反射出需要使用的屬性欄位
    /// </summary>
    public class DrillBrandGetPropertyConverter : WPFWindowsBase.BaseValueConverter<DrillBrandGetPropertyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DrillBrands drillBrands = new STDSerialization().GetDrillBrands();
            int index = (int)value;
            PropertyInfo info = drillBrands[index].GetType().GetProperty(parameter.ToString());
            if (info == null)
            {
                throw new Exception("參數傳遞錯誤");
            }
            return info.GetValue(value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

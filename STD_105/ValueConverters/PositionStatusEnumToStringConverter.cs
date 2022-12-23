using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    internal class PositionStatusEnumToStringConverter : BaseValueConverter<PositionStatusEnumToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is GD_STD.Enum.PositionStatusEnum)
            {
                //加工所在位置 第二階段需支援多語系
                return ((GD_STD.Enum.PositionStatusEnum)value).ToString();
            }
            else 
                return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

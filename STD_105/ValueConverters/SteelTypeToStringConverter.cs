using GD_STD.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;
using GD_STD.Enum;
using System.Windows.Data;

namespace STD_105
{
    /// <summary>
    /// Steel Type轉換為中文名稱
    /// </summary>
    public class SteelTypeToStringConverter : BaseValueConverter<SteelTypeToStringConverter>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OBJECT_TYPE num = (OBJECT_TYPE)value;
            switch (num)
            {
                case OBJECT_TYPE.BH:
                    return "BH型鋼";
                case OBJECT_TYPE.H:
                    return "H型鋼";
                case OBJECT_TYPE.RH:
                    return "RH型鋼";
                case OBJECT_TYPE.TUBE:
                    return "TUBE";
                case OBJECT_TYPE.BOX:
                    return "BOX";
                case OBJECT_TYPE.LB:
                    return "槽鐵";
                case OBJECT_TYPE.CH:
                    //return "CH型鋼";
                    return "槽鐵";
                default:
                    return "未知型鋼";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

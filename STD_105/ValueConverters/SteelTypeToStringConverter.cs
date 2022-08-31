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
            OBJETC_TYPE num = (OBJETC_TYPE)value;
            switch (num)
            {
                case OBJETC_TYPE.BH:
                    return "BH型鋼";
                case OBJETC_TYPE.H:
                    return "H型鋼";
                case OBJETC_TYPE.RH:
                    return "RH型鋼";
                case OBJETC_TYPE.TUBE:
                    return "TUBE";
                case OBJETC_TYPE.BOX:
                    return "BOX";
                case OBJETC_TYPE.LB:
                    return "槽鐵";
                case OBJETC_TYPE.CH:
                    return "CH型鋼";
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

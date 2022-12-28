using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105;
using WPFBase = WPFWindowsBase;
using GD_STD.Enum;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;

namespace STD_105
{
    /// <summary>
    /// 轉換器 
    /// </summary>
    public class MachiningTypeModeConverter : WPFBase.BaseValueConverter<MachiningTypeModeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is MachiningType)
            {
                string Returnvalue = "";
                switch((MachiningType)value)
                {

                   case MachiningType.PinTest:
                        Returnvalue= "打點測試";
                        break;
                    case MachiningType.NormalMaching:
                        Returnvalue = "常規加工";
                        break;      
                    
                    case MachiningType.Unknown:
                    default:
                        break;
                }
                return Returnvalue;
            }
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

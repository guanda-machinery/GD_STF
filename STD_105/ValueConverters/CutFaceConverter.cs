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
    public class CutFaceConverter : BaseValueConverter<CutFaceConverter>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //value是radio接到binding變數的值後，來呼叫converter
            //converter負責判定接到的值是代表true還是false
            if (value == null || parameter == null)
                return false;
            string checkvalue = value.ToString();
            string targetvalue = parameter.ToString();
            bool r = checkvalue.Equals(targetvalue,
                StringComparison.InvariantCultureIgnoreCase);
            return r;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //value 是目前 radiobutton 的 true/false
            //在這裡把 parameter 傳回 View-Model
            if (value == null || parameter == null)
                return null;
            bool usevalue = (bool)value;

            if (usevalue)
                return parameter.ToString();

            return null;
        }
    }
}

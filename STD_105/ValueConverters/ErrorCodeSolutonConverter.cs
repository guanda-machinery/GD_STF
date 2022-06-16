using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase = WPFWindowsBase;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
using System.Reflection;
using GD_STD.Attribute;

namespace STD_105
{
    /// <summary>
    ///  <see cref="ERROR_CODE"/> 取得 Error Code 說明/>
    /// </summary>
    public class ErrorCodeSolutionConverter : WPFBase.BaseValueConverter<ErrorCodeSolutionConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ERROR_CODE error = (ERROR_CODE)value;

            if (value == null)
                return null;

            string result = error.GetType().GetField(error.ToString()).GetCustomAttribute<ErrorCodeAttribute>()?.Solution;

            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

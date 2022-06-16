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
    public class ErrorCodeInfoConverter : WPFBase.BaseValueConverter<ErrorCodeInfoConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ERROR_CODE error = (ERROR_CODE)value;

            if (value == null)
                return null;

            string result = string.Empty;
            if (error == ERROR_CODE.Unknown)
            {
                string errorCode = ReadCodesysMemor.GetUnknownCode().Replace("\0", "");
                result = $"未預期的錯誤 : { errorCode}。\n請聯絡廣達機械國際有限公司。\n連絡電話: (04)2335-6118";
            }
            else
            {
                result = error.GetType().GetField(error.ToString()).GetCustomAttribute<ErrorCodeAttribute>()?.Description;
            }
            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

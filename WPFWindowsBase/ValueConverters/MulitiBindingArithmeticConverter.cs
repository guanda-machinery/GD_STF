using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPFWindowsBase
{
    /// <summary>
    /// 多重綁訂做四則運算轉換器
    /// </summary>
    public class MultiBindingArithmeticConverter : BaseMultiValueConverter<MultiBindingArithmeticConverter>
    {
        /// <summary>
        /// 預設實現A+B
        /// 帶參數Subtraction實現A－B
        /// 帶參數Multiplication實現A×B
        /// 帶參數Division實現A÷B
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //傳入參數超過2個回傳null
            if (values.Length > 2)
                return null;

            if (double.TryParse(values[0].ToString(), out double a) && double.TryParse(values[1].ToString(), out double b))
            {
                double sum;
                switch ((string)parameter)
                {
                    case "Subtraction": // 減法
                        sum = a - b;
                        break;
                    case "Multiplication": // 乘法
                        sum = a * b;
                        break;
                    case "Division": // 除法
                        sum = a / b;
                        break;
                    default:
                        sum = a + b; // 不帶ConverterParameter做加法
                        break;
                }
                return sum.ToString();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Do nothing
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

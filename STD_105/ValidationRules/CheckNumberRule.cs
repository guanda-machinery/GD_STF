using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace STD_105.ValidationRules
{
    /// <summary>
    /// 驗證是否為數字
    /// </summary>
    public class CheckNumberRule : ValidationRule
    {
        /// <summary>
        /// 設定最大值
        /// </summary>
        public double? NumberMax { get; set; }
        /// <summary>
        /// 設定最小值
        /// </summary>
        public double? NumberMin { get; set; }
        /// <summary>
        /// 是否為整數型
        /// </summary>
        public bool IsINTValidate { get; set; } = false;

        /// <summary>
        /// 驗證<paramref name="value"/>值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //先檢查參數
            if(NumberMax is double && NumberMin is double)
            {
                if (NumberMax < NumberMin)
                    throw new Exception("驗證值設定錯誤，最大值不可小於最小值");
            }

            if (((string)value).Length > 0)
            {
                if (double.TryParse((string)value, out var DoubleValue))
                {
                    if (NumberMax != null)
                    {
                        if (DoubleValue > NumberMin)
                        {
                            return new ValidationResult(false, $"數字不可大於{NumberMax}!");
                        }
                    }

                    if (NumberMin != null)
                    {
                        if (DoubleValue < NumberMin)
                        {
                            return new ValidationResult(false, $"數字不可小於{NumberMin}!");
                        }
                    }

                    if(IsINTValidate)
                    {
                        if( int.TryParse((string)value, out var IntValue))
                        {

                        }
                        else
                        {
                            return new ValidationResult(false, $"須為int!");
                        }
                    }
                }
                else
                {
                    return new ValidationResult(false, $"請輸入數字!");
                }
            }

            return ValidationResult.ValidResult;
        }

    }
}

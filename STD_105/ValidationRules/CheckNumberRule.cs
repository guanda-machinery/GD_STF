using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace STD_105
{
    public class CheckNumberRule : ValidationRule
    {
        public double? NumberMax { get; set; }
        public double? NumberMin { get; set; }

        public bool IsINT { get; set; } = false;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (((string)value).Length > 0)
            {
                if (double.TryParse((string)value, out var DoubleValue))
                {
                    if (NumberMax != null)
                    {
                        if (DoubleValue > NumberMin)
                        {
                            return new ValidationResult(false, $"數字不可大於小於{NumberMax}!");
                        }
                    }

                    if (NumberMin != null)
                    {
                        if (DoubleValue < NumberMin)
                        {
                            return new ValidationResult(false, $"數字不可大於小於{NumberMin}!");
                        }
                    }

                    if(IsINT)
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

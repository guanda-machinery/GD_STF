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
        public int? NumberMax { get; set; }
        public int? NumberMin { get; set; }

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
                }
                else
                    return new ValidationResult(false, $"請輸入數字!");
            }

            return ValidationResult.ValidResult;
        }

    }
}

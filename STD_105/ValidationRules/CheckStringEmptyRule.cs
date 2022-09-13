using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace STD_105
{
    internal class CheckStringEmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var ValidString = value as string;

            if (string.IsNullOrEmpty(ValidString))
            {
                return new ValidationResult(false, $"不可為空字串");
            }
            else if(string.IsNullOrWhiteSpace(ValidString))
            {
                return new ValidationResult(false, $"不可為空白!");
            }
            else if(ValidString.Length>10)
            {
                return new ValidationResult(false, $"字串過長!");
            }
            else
            {
                return ValidationResult.ValidResult;
            }

        }
    }
}

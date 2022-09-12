using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace STD_105
{
    internal class CheckNumberArrayRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (((string)value).Length > 0)
            {
                //用空白或逗號切開
                var ValueArray = ((string)value).Split(' ', ',');

                foreach(var SpiltedValue in ValueArray)
                {
                    if (int.TryParse(SpiltedValue, out var IntValue))
                    {
                        if(IntValue <0)
                        {
                            return new ValidationResult(false, $"不可<0!");
                        }
                    }
                    else
                    {
                        return new ValidationResult(false, $"有非數字的值存在!");
                    }
                }
            }

            return ValidationResult.ValidResult;
        }

    }
}

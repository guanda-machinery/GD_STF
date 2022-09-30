﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace STD_105.ValidationRules
{
    internal class CheckStringEmptyRule : ValidationRule
    {
       public uint StringLengthMin { get; set; } = 0;
       public uint? StringLengthMax { get;set; }
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //先檢查參數
            if (StringLengthMax is uint)
            {
                if (StringLengthMax < StringLengthMin)
                    throw new Exception("驗證值設定錯誤，字串最大長度不可小於字串最小長度");
            }

            var ValidString = value as string;

            if (string.IsNullOrEmpty(ValidString))
            {
                return new ValidationResult(false, $"不可為空字串");
            }
            else if(string.IsNullOrWhiteSpace(ValidString))
            {
                return new ValidationResult(false, $"不可為空白!");
            }
            else if (ValidString.Length < StringLengthMin)
            {
                return new ValidationResult(false, $"字串過短!");
            }
            else if(ValidString.Length> StringLengthMax)
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
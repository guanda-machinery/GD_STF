using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace STD_105.ValidationRules
{
    internal class CheckHoleGroupArrayStringRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //格式：20 50*5 80 45*2
            if (((string)value).Length > 0)
            {
                //檢查空白字符 
                if (((string)value).Contains("  ") ||
                    ((string)value).Contains(" ,") ||
                    ((string)value).Contains(", ") ||
                    ((string)value).Contains(",,"))
                {
                    return new ValidationResult(false, $"不可連續出現兩個以上的空白或逗號!");
                }



                //用空白或逗號切開
                var ValueArray = ((string)value).Split(' ', ',');


                for (int i = 0; i < ValueArray.Length; i++)
                { 
                    var SpiltedValue = ValueArray[i];
                    //分割後檢查是否為double
                    if (double.TryParse(SpiltedValue, out var DoubleValue))
                    {
                        if (DoubleValue <= 0)
                        {
                            //若第一項為0則通過，其他項跳錯
                            if (i == 0 && DoubleValue ==0)
                            {
                                continue;
                            }
                            
                            return new ValidationResult(false, $"數值需大於零!");
                        }
                    }
                    else
                    {
                        //分割後的小字串只能有一個*號，超過者報錯
                        var StarKeyCount = SpiltedValue.Count(x => (x == '*'));
                        if (StarKeyCount >= 2)
                        {
                            return new ValidationResult(false, $"同一區塊只能出現一次*號!");
                        }
                        var StarKeyLocation = SpiltedValue.IndexOf('*');

                        if (string.IsNullOrEmpty(SpiltedValue))
                        {
                            return new ValidationResult(false, $"開頭結尾不可為空白 需輸入數值!");
                        }
                        if (StarKeyLocation == 0)
                        {
                            return new ValidationResult(false, $"星號不可在區塊開頭!");
                        }
                        if (StarKeyLocation == SpiltedValue.Length - 1)
                        {
                            return new ValidationResult(false, $"星號不可在區塊結尾!");
                        }
                        else
                        {
                            var SplitSecondValueArray = SpiltedValue.Split('*');
                            foreach (var SplitSecondValue in SplitSecondValueArray)
                            {
                                if (double.TryParse(SplitSecondValue, out var doubleSV))
                                {
                                    if (doubleSV < 0)
                                    {
                                        return new ValidationResult(false, $"數值需大於零!");
                                    }
                                    if (doubleSV == 0)
                                    {
                                        return new ValidationResult(false, $"孔距或孔數不可為零!");
                                    }
                                }
                                else
                                {
                                    //用*分割後出現非double 錯誤!
                                    return new ValidationResult(false, $"需輸入數值!");
                                }
                            }
                        }

                    }
                }
                




                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, $"需輸入數值!");
        }
    }
}

using GD_STD.Enum;
using System;
using System.Globalization;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// 單一條件達成才會開啟控制項
    /// </summary>
    public class ConditionToIsEnabledConerter : BaseValueConverter<ConditionToIsEnabledConerter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((ERROR_CODE)value == ERROR_CODE.Null)
                return true;
            else
                return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

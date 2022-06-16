using GD_STD.Enum;
using System;
using System.Globalization;
using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 觸控面板雙條件達成才會啟用此控制項
    /// </summary>
    public class DoubleConditionToIsEnabledConerter : BaseMultiValueConverter<DoubleConditionToIsEnabledConerter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((KEY_HOLE)values[0] == KEY_HOLE.AUTO || (ERROR_CODE)values[1] != ERROR_CODE.Null || (bool)values[2])
                return false;
            else
                return true;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

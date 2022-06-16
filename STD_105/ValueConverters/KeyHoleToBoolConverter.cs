using System;
using System.Globalization;
using WPFBase = WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 不是 <see cref="GD_STD.Enum.KEY_HOLE.MANUAL"/> 狀態，有關手動的控制項就只能是唯讀/>
    /// </summary>
    public class KeyHoleToBoolConverter : WPFBase.BaseValueConverter<KeyHoleToBoolConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (GD_STD.Enum.KEY_HOLE)value == GD_STD.Enum.KEY_HOLE.MANUAL ? true : false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

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
        public bool Invert { get; set; }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is GD_STD.Enum.KEY_HOLE)
            {
                var _KeyHole = GD_STD.Enum.KEY_HOLE.MANUAL;
                if (parameter is GD_STD.Enum.KEY_HOLE)
                {
                    _KeyHole = (GD_STD.Enum.KEY_HOLE)parameter;
                }
                var ReturnValue = (GD_STD.Enum.KEY_HOLE)value == _KeyHole;

                return (!Invert) ? ReturnValue : !ReturnValue;
            }
            return (!Invert);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

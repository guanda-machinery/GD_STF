using System;
using System.Globalization;
using System.Windows;

namespace WPFWindowsBase
{
    /// <summary>
    /// 接受<see cref="bool"/>並返回<see cref="Visibility"/>的轉換器(可接受<see cref="bool"/>空值)
    /// </summary>
    public class BooleanToVisiblityConverter : BaseValueConverter<BooleanToVisiblityConverter>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return (bool)value ? Visibility.Hidden : Visibility.Visible;
            else
                return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }
        /// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 接受<see cref="bool"/>並返回<see cref="Visibility"/>的轉換器(不接受<see cref="bool"/>空值)
    /// </summary>
    public class NoNullBooleanToVisibilityConverter : BaseValueConverter<NoNullBooleanToVisibilityConverter>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("錯誤，不可接受 'value' 是空值");

            if ((bool)value)
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Collapsed;
        }
        /// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Visibility visibility = (System.Windows.Visibility)value;

            return visibility == System.Windows.Visibility.Visible ? true : false;
        }
    }
    /// <summary>
    /// 接受<see cref="bool"/>並返回<see cref="Visibility"/>的轉換器(不接受<see cref="bool"/>空值) 相反邏輯，value == true return <see cref="Visibility.Collapsed"/>
    /// </summary>
    public class OppositeBooleanToVisibilityConverter : NoNullBooleanToVisibilityConverter
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           /* if (value == null)
                throw new Exception("錯誤，不可接受 'value' 是空值");*/

            if ((bool)value)
                return System.Windows.Visibility.Collapsed;
            else
                return System.Windows.Visibility.Visible;
        }
    }
}

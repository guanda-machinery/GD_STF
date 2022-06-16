using System;
using System.Globalization;
using WPFBase = WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// <see cref="WPFSTD105.ApplicationPage.Lock"/> 狀態 ，就隱藏其他控制項，只顯示<see cref="LockPage"/>
    /// </summary>
    public class PageEnumToBoolConverter : WPFBase.BaseValueConverter<PageEnumToBoolConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return ((WPFSTD105.ApplicationPage)value) == WPFSTD105.ApplicationPage.Lock ? false : true;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}

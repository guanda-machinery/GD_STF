using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Media;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// 斷面規格圖形轉換
    /// </summary>
    public class SectionTreeImageConverter : BaseValueConverter<SectionTreeImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string itemName = value.ToString();
            if (itemName == null)
                return null; 
            ImageSource imageSource;
            if (itemName.Contains("RH") || itemName.Contains("BH"))
                imageSource = Application.Current.Resources["Beam"] as ImageSource;
            else if (itemName.Contains("CH"))
                imageSource = Application.Current.Resources["CH"] as ImageSource;
            else if (itemName.Contains("L"))
                imageSource = Application.Current.Resources["L"] as ImageSource;
            else if (itemName.Contains("Box") || itemName.Contains("TUBE"))
                imageSource = Application.Current.Resources["BOX"] as ImageSource;
            else
                imageSource = null;
            return imageSource;            
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

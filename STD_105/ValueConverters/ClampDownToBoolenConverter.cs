using devDept.Eyeshot;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    internal class ClampDownToBoolenConverter : WPFWindowsBase.BaseValueConverter<ClampDownToBoolenConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GD_STD.Enum.CLAMP_DOWN)
            {
                if (parameter is GD_STD.Enum.CLAMP_DOWN)
                {
                    return (GD_STD.Enum.CLAMP_DOWN)value != (GD_STD.Enum.CLAMP_DOWN)parameter;
                }
            }
            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is bool)
            {
                if ((bool)value)
                {
                    if (parameter is GD_STD.Enum.CLAMP_DOWN)
                    {
                        //var PanelButton = WPFSTD105.ViewLocator.ApplicationViewModel.PanelButton;
                        //PanelButton.ClampDownSelected = (GD_STD.Enum.CLAMP_DOWN)parameter;
                        //WPFSTD105.CodesysIIS.WriteCodesysMemor.SetPanel(PanelButton);
                       
                        return parameter; 
                    }
                }
            }
            return null;
        }
    }
}

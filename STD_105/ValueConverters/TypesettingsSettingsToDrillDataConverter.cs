using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using DevExpress.Xpf.WindowsUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFWindowsBase;

namespace STD_105
{
    internal class TypesettingsSettingsToDrillDataConverter : BaseValueConverter<TypesettingsSettingsToDrillDataConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<WPFSTD105.Model.DrillBolts>)
            {
                var DrillBoltsListInfo = (value as ObservableCollection<WPFSTD105.Model.DrillBolts>).ToList();
                if (parameter is GD_STD.Enum.FACE)
                {
                    return DrillBoltsListInfo.FindAll(x => (x.Face == (GD_STD.Enum.FACE)parameter));
                }
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }















    }

}

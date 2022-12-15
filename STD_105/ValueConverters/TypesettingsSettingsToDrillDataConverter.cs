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
using System.Windows;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFWindowsBase;

namespace STD_105
{
    internal class TypesettingsSettingsToDrillDataConverter : BaseValueConverter<TypesettingsSettingsToDrillDataConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>)
            {
                var DrillBoltsListInfo = (value as Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>);
                if (parameter is GD_STD.Enum.FACE)
                    if (DrillBoltsListInfo.TryGetValue((GD_STD.Enum.FACE)parameter, out var DrillBase))
                        return DrillBase.DrillBoltList;
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class TypesettingsSettingsToBooleanConverter : BaseValueConverter<TypesettingsSettingsToBooleanConverter>
    {

        private Dictionary<GD_STD.Enum.FACE, DrillBoltsBase> Temp = new Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>();

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>)
            {
                var DrillBoltsListInfo = (value as Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>);
                Temp = DrillBoltsListInfo;
                if (parameter is GD_STD.Enum.FACE)
                    if (DrillBoltsListInfo.TryGetValue((GD_STD.Enum.FACE)parameter, out var DrillBase))
                        return DrillBase.Dia_Identification;
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if (parameter is GD_STD.Enum.FACE)
                {
                    if(Temp.ContainsKey((GD_STD.Enum.FACE)parameter))
                        Temp[(GD_STD.Enum.FACE)parameter].Dia_Identification = (bool)value;

                    return Temp;
                }
            }
            return null;
        }
    }
    internal class TypesettingsSettingsToDrillDiaConverter : BaseValueConverter<TypesettingsSettingsToDrillDiaConverter>
    {
        //combobox用 回傳double
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>)
                if (parameter is GD_STD.Enum.FACE)
                    if ((value as Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>).TryGetValue((GD_STD.Enum.FACE)parameter, out var DBClass))
                        if (DBClass.DrillBoltList.Count != 0)
                            return DBClass.DrillBoltList.Select(x => (x.Origin_DrillHoleDiameter));
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class TypesettingsSettingsToUnitaryToolConverter : BaseValueConverter<TypesettingsSettingsToUnitaryToolConverter>
    {

        private Dictionary<GD_STD.Enum.FACE, DrillBoltsBase> Temp = new Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>();
        //combobox用 回傳double
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>)
            {
                Temp = value as Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>;
                if (parameter is GD_STD.Enum.FACE)
                    if ((value as Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>).TryGetValue((GD_STD.Enum.FACE)parameter, out var DBClass))
                        return DBClass.UnitaryToolTop;
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is GD_STD.Enum.FACE)
                if (value is double)
                    if (Temp.ContainsKey((GD_STD.Enum.FACE)parameter))
                        Temp[(GD_STD.Enum.FACE)parameter].UnitaryToolTop = (double)value;
            return Temp;
        }
    }

    /// <summary>
    /// 列表內沒有資料時不顯示
    /// </summary>
    internal class TypesettingsSettingsToVisibilityConverter : BaseValueConverter<TypesettingsSettingsToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>)
                if (parameter is GD_STD.Enum.FACE)
                    if ((value as Dictionary<GD_STD.Enum.FACE, DrillBoltsBase>).TryGetValue((GD_STD.Enum.FACE)parameter, out var DrillBase))
                        return (DrillBase.DrillBoltList.Count != 0) ? Visibility.Visible : Visibility.Collapsed;
                    else
                        return Visibility.Collapsed;
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




}

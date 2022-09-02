using GD_STD.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;
using GD_STD.Enum;
using System.Windows.Data;
using WPFSTD105.Attribute;
using System.Collections.ObjectModel;
using GD_STD;
using WPFSTD105;

namespace STD_105
{
    /// <summary>
    /// Steel Type轉換為中文名稱
    /// </summary>
    public class SteelPartWeightConverter : BaseValueConverter<SteelPartWeightConverter>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //try { TypeSettingDataView part_tekla = (TypeSettingDataView)value; }
            //catch { }
            TypeSettingDataView part_tekla = (TypeSettingDataView)value;

            if (part_tekla.PartWeight == 0.0)
            {
                ObservableCollection<SteelAttr> SectionSPec = new ObservableCollection<SteelAttr>();

                switch ((OBJECT_TYPE)part_tekla.SteelType)
                {
                    case OBJECT_TYPE.BH:
                        SectionSPec = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\BH.inp");
                        break;
                    case OBJECT_TYPE.H:
                        SectionSPec = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\H.inp");
                        break;
                    case OBJECT_TYPE.RH:
                        SectionSPec = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\RH.inp");
                        break;
                    case OBJECT_TYPE.TUBE:
                        SectionSPec = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\TUBE.inp");
                        break;
                    case OBJECT_TYPE.BOX:
                        SectionSPec = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\BOX.inp");
                        break;
                    case OBJECT_TYPE.LB:
                        SectionSPec = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\LB.inp");
                        break;
                    case OBJECT_TYPE.CH:
                        SectionSPec = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\CH.inp");
                        break;
                    default:
                        break;
                }

                foreach (var single_section in SectionSPec)
                {
                    if(part_tekla.Profile == single_section.Profile)
                    {
                        return part_tekla.Length/1000 * single_section.Kg;
                    }
                }

                //part_tekla.PartWeight = steelPart.UnitWeight * part_tekla.Length; 
            }
            return part_tekla.PartWeight;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

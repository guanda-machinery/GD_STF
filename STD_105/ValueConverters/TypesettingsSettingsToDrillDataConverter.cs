using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    internal class TypesettingsSettingsToDrillDataConverter : BaseValueConverter<TypesettingsSettingsToDrillDataConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return null;





            /*
            STDSerialization ser = new STDSerialization(); //序列化處理器

            //如果模型有構件列表
            if (File.Exists(ApplicationVM.FileSteelAssembly()))
            {
                SteelAssemblies = ser.GetGZipAssemblies();
                if (SteelAssemblies == null)
                {
                    SteelAssemblies = new ObservableCollection<SteelAssembly>();
                }
            }
            //如果模型有材質設定
            if (!File.Exists(ApplicationVM.FileMaterial()))
            {
                Materials.AddRange(SerializationHelper.GZipDeserialize<ObservableCollection<SteelMaterial>>(ApplicationVM.FileMaterial())); //材質序列化檔案
            }
            DicSteelPart = ser.GetPart();
            Materials = ser.GetMaterial(); //材質序列化檔案

            LoadAttribute();//載入用戶屬性

            //初始化選單
            ProfileType = 0;
            ProfileIndex = 0;

            DataCorrespond = ser.GetDataCorrespond();
            var groupData = from el in DataCorrespond group el by el.Type.GetType().GetMember(el.Type.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description into el orderby el.Key select el;

            */















            return null;
            //throw new NotImplementedException();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

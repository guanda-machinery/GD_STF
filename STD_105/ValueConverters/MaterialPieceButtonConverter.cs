using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPFWindowsBase;

namespace STD_105
{
    internal class MaterialPieceButtonConverter : BaseMultiValueConverter<MaterialPieceButtonConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is object[])
            {
                DevExpress.Xpf.Grid.GridControl GControl = null;
                GD_STD.Data.MaterialDataView MDataview = null;

                foreach (var EachObject in values)
                {          
                    if(EachObject is DevExpress.Xpf.Grid.GridControl)
                    {
                        GControl = EachObject as DevExpress.Xpf.Grid.GridControl;
                    }
                   if(EachObject is GD_STD.Data.MaterialDataView)
                    {
                        MDataview = EachObject as GD_STD.Data.MaterialDataView;
                    }
                }

                if( GControl != null && MDataview != null )
                {
                    var a = (ObservableCollection<TypeSettingDataView>)GControl.ItemsSource;
                    if(GControl.ItemsSource == MDataview.Parts)
                    {
                        MDataview.ButtonEnable = true;
                        return true;
                    }
                    MDataview.ButtonEnable = false;
                    return false;
                }

                return false;
            }


            throw new NotImplementedException();
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

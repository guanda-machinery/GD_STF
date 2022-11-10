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
using WPFSTD105.Model;
using WPFWindowsBase;

namespace STD_105
{
    internal class TypesettingsSettingsToDrillDataConverter : BaseValueConverter<TypesettingsSettingsToDrillDataConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var DrillBoltsList = new List<DrillBolts>();
            //使用SelectedItems需在vm層宣告，並在gridcontrol下寫SelectedItems={Binding ~} 否則會出現看得到SelectedItems卻無法使用的問題
            //var MdataList = value as IEnumerable<GD_STD.Data.MaterialDataView>;
           // if (MdataList == null)
            //    return null;

            //foreach (var Mdata in MdataList)
            var Mdata = value as GD_STD.Data.MaterialDataView;
            if (Mdata == null)
                return null;


            if (Mdata.Parts.Count == 0)
            {
                return null;
                //continue;
            }
            var DataViews = new ObservableCollection<WPFSTD105.ViewModel.ProductSettingsPageViewModel>(WPFSTD105.ViewModel.ObSettingVM.GetData()).ToList();
            //取得專案內所有資料 並找出符合本零件的dataname(dm名) 
            //有可能會有複數個(選擇素材/單一零件的差別)
            var PartsList = new List<WPFSTD105.ViewModel.ProductSettingsPageViewModel>();
            foreach (var _part in Mdata.Parts)
            {
                PartsList.AddRange(DataViews.FindAll(x => x.AssemblyNumber == _part.AssemblyNumber));
            }

            var DataName = PartsList.First().DataName;//測試用 之後要換成陣列
                                                      //var DataName = "0a88fa12-4450-40fa-a76c-ba155315f580";//找出DataName
                                                      //WPFSTD105.ModelExt model = new WPFSTD105.ModelExt();

            devDept.Eyeshot.Model _BufferModel = new devDept.Eyeshot.Model();
            _BufferModel.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            _BufferModel.InitializeViewports();
            _BufferModel.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());

            var ser = new STDSerialization();
            var readFile = ser.ReadPartModel(DataName); //讀取檔案內容
            if (readFile == null)
            {
                return null;
                //continue;
            }

            readFile.DoWork();//開始工作
            readFile.AddToScene(_BufferModel);//將讀取完的檔案放入到模型
            if (_BufferModel.Entities[_BufferModel.Entities.Count - 1].EntityData is null)
            {
                return null;
                //continue;
            }
            //ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
            //WriteSteelAttr((SteelAttr)_BufferModel.Blocks[1].Entities[0].EntityData);//寫入到設定檔內
            //ViewModel.GetSteelAttr();
            _BufferModel.Blocks[1] = new Steel3DBlock((Mesh)_BufferModel.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式(零件)

            for (int i = 0; i < _BufferModel.Entities.Count; i++)//逐步展開 3d 模型實體
            {
                if (_BufferModel.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                {
                    BlockReference blockReference = (BlockReference)_BufferModel.Entities[i]; //取得參考圖塊
                    var BoltsAttr = blockReference.EntityData as GroupBoltsAttr;
                    //BoltsList.Add(BoltsAttr);

                    //如果直徑已經存在 則加入舊的 否則建立新的資料
                    if (DrillBoltsList.Exists(x => (x.DrillHoleDiameter == BoltsAttr.Dia)))
                    {
                        DrillBoltsList.Find(x => x.DrillHoleDiameter == BoltsAttr.Dia).DrillHoleCount += BoltsAttr.Count;
                    }
                    else
                    {
                        DrillBoltsList.Add(new DrillBolts { DrillHoleDiameter = BoltsAttr.Dia, DrillHoleCount = BoltsAttr.Count, Face = BoltsAttr.Face });
                    }

                }
            }

            if (parameter is GD_STD.Enum.FACE)
            {
                return DrillBoltsList.FindAll(x => (x.Face == (GD_STD.Enum.FACE)parameter));
            }

            return DrillBoltsList;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public class DrillBolts
        {
            public string WorkType { get; set; } = "孔";
            public GD_STD.Enum.FACE Face { get; set; }
            public int DrillHoleCount { get; set; }
            public double DrillHoleDiameter { get; set; }
        }

    }
}

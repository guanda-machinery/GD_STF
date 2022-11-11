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



            //使用SelectedItems需在vm層宣告，並在gridcontrol下寫SelectedItems={Binding ~} 否則會出現看得到SelectedItems卻無法使用的問題
            //var MdataList = value as IEnumerable<GD_STD.Data.MaterialDataView>;
            // if (MdataList == null)
            //    return null;

            //foreach (var Mdata in MdataList)

            var DrillBoltsListInfo = new List<DrillBolts>();


            if (value is GD_STD.Data.MaterialDataView || value is WPFSTD105.ViewModel.ProcessingMonitorVM.MaterialPartDetail)
            {
                var DataViews = new ObservableCollection<WPFSTD105.ViewModel.ProductSettingsPageViewModel>(WPFSTD105.ViewModel.ObSettingVM.GetData()).ToList();
                //選擇單一素材
                //取得專案內所有資料 並找出符合本零件的dataname(dm名) 
                //有可能會有複數個(選擇素材/單一零件的差別)
                var PartsList = new List<WPFSTD105.ViewModel.ProductSettingsPageViewModel>();

                if (value is GD_STD.Data.MaterialDataView)
                {
                    var Materialdata = value as GD_STD.Data.MaterialDataView;
                    foreach (var M_part in Materialdata.Parts)
                    {
                        PartsList.AddRange(DataViews.FindAll(x => x.AssemblyNumber == M_part.AssemblyNumber));
                    }
                }

                if (value is WPFSTD105.ViewModel.ProcessingMonitorVM.MaterialPartDetail)
                {
                    PartsList.AddRange(DataViews.FindAll(x => x.AssemblyNumber == ((WPFSTD105.ViewModel.ProcessingMonitorVM.MaterialPartDetail)value).AssemblyNumber));
                }

                foreach (var _part in PartsList)
                {
                    GetMaterialdataDrillBoltsInfo(_part.DataName, out var MaterialDrillBolts);
                    //複數零件時須將零件資料合併
                    foreach (var MDrill in MaterialDrillBolts)
                    {
                        if (DrillBoltsListInfo.Exists(x => (x.WorkType == MDrill.WorkType && x.Face == MDrill.Face && x.DrillHoleDiameter == MDrill.DrillHoleDiameter)))
                        {
                            DrillBoltsListInfo.Find(x => (x.WorkType == MDrill.WorkType && x.Face == MDrill.Face && x.DrillHoleDiameter == MDrill.DrillHoleDiameter)).DrillHoleCount += MDrill.DrillHoleCount;
                        }
                        else
                        {
                            DrillBoltsListInfo.Add(MDrill);
                        }
                    }
                }
            }

            /*else if (value is IEnumerable<GD_STD.Data.MaterialDataView>)
            {
                //複選零件
                foreach(var Mdata in value as IEnumerable<GD_STD.Data.MaterialDataView>)
                {
                    GetMaterialdataDrillBoltsInfo(Mdata, out var Info);
                    //將資料彙集後輸出
                    DrillBoltsListInfo = Info;
                }
            }*/

            if (parameter is GD_STD.Enum.FACE)
            {
                return DrillBoltsListInfo.FindAll(x => (x.Face == (GD_STD.Enum.FACE)parameter));
            }

            return DrillBoltsListInfo;
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



        /// <summary>
        /// 給定DataName 回傳該零件所有的孔位及位置
        /// </summary>
        /// <param name="DataName"></param>
        /// <param name="DrillBoltsList"></param>
        /// <returns></returns>
        private bool GetMaterialdataDrillBoltsInfo(string DataName, out List<DrillBolts> DrillBoltsList)
        {
            DrillBoltsList = new List<DrillBolts>();
            devDept.Eyeshot.Model _BufferModel = new devDept.Eyeshot.Model();
            _BufferModel.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            _BufferModel.InitializeViewports();
            _BufferModel.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());

            var ser = new STDSerialization();
            var readFile = ser.ReadPartModel(DataName); //讀取檔案內容
            if (readFile == null)
            {
                return false;
                //continue;
            }

            readFile.DoWork();//開始工作
            readFile.AddToScene(_BufferModel);//將讀取完的檔案放入到模型
            if (_BufferModel.Entities[_BufferModel.Entities.Count - 1].EntityData is null)
            {
                return false;
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

                    //如果直徑和面已經存在 則加入舊的 否則建立新的資料
                    if (DrillBoltsList.Exists(x => (x.DrillHoleDiameter == BoltsAttr.Dia && x.Face == BoltsAttr.Face)))
                    {
                        DrillBoltsList.Find(x => x.DrillHoleDiameter == BoltsAttr.Dia).DrillHoleCount += BoltsAttr.Count;
                    }
                    else
                    {
                        DrillBoltsList.Add(new DrillBolts { DrillHoleDiameter = BoltsAttr.Dia, DrillHoleCount = BoltsAttr.Count, Face = BoltsAttr.Face });
                    }
                }
            }

            return true;
        }









    }

}

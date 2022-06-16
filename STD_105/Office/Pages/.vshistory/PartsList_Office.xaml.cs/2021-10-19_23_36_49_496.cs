using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static WPFSTD105.ViewLocator;
using WPFSTD105;
using WPFWindowsBase;
using System.Collections.ObjectModel;
using DevExpress.Data.Extensions;
using GD_STD.Data;
using WPFSTD105.Attribute;
using devDept.Eyeshot.Translators;
using devDept.Eyeshot;
using WPFSTD105.Model;

namespace STD_105.Office
{
    /// <summary>
    /// PartsList_Office.xaml 的互動邏輯
    /// </summary>
    public partial class PartsList_Office : BasePage<OfficeTypeSettingVM>
    {
        public PartsList_Office()
        {
            InitializeComponent();
            //GridControlLocalizer.Active = new CustomDXGridLocalizer();
        }

        public class CustomDXGridLocalizer : GridControlLocalizer
        {
            protected override void PopulateStringTable()
            {
                base.PopulateStringTable();
                AddString(GridControlStringId.GridGroupPanelText, "11111111111111");
            }
        }
        /// <summary>
        /// 單一物件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowserGraph(object sender, RoutedEventArgs e)
        {
            string partNumber = ((Button)e.Source).Content.ToString(); //零件名稱
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<DataCorrespond> dataCorrespond = ser.GetDataCorrespond(); //序列化列表
            int indexDataCorrespond = dataCorrespond.FindIndex(el => el.Number == partNumber);//序列化的列表索引
            if (indexDataCorrespond != -1) //有找到序列化物件
            {
                DataCorrespond data = dataCorrespond[indexDataCorrespond]; //找到的物件
                NcTempList ncTemps = ser.GetNcTempList(); //NC 載入檔案
                NcTemp ncTemp = ncTemps.GetData(data.DataName);
                ser.SetNcTempList(ncTemps);
                GraphWin graphWin = null;
                if (ncTemp != null)
                {
                    graphWin = new GraphWin(ncTemp);

                }
                else
                {
                    graphWin = new GraphWin(data.DataName);
                }
                graphWin.Show();
                graphWin.Draw();
                //CommonViewModel.ChildWin = new PopupWindowsBase();
                //OfficeViewModel.PopupCurrentPage = PopupWindows.GraphBrowser;
                //OfficeViewModel.PopupTitle = "圖形預覽";
                //OfficeViewModel.PopupWidth = 1024;
                //OfficeViewModel.PopupHeight = 768;
                //CommonViewModel.ChildWin.ShowDialog();
            }
        }
        /// <summary>
        /// 組合物件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string number = ((Button)e.Source).Content.ToString(); //零件名稱
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<DataCorrespond> dataCorrespond = ser.GetDataCorrespond(); //序列化列表
            int viewIndex = ViewModel.MaterialDataViews.FindIndex(el => el.MaterialNumber == number); //找出指定素材編號
            MaterialDataView materialDataView = ViewModel.MaterialDataViews[viewIndex]; //素材組合
            List<List<object>> match = new List<List<object>>(); // 3d 實體訊息

            for (int i = 0; i < materialDataView.Parts.Count; i++)
            {
                int indexDataCorrespond = dataCorrespond.FindIndex(el => el.Number == materialDataView.Parts[i].PartNumber);//序列化的列表索引
                if (indexDataCorrespond != -1) //有找到序列化物件
                {
                    DataCorrespond data = dataCorrespond[indexDataCorrespond]; //找到的物件
                    NcTempList ncTemps = ser.GetNcTempList(); //NC 載入檔案
                    NcTemp ncTemp = ncTemps.GetData(data.DataName);
                    ser.SetNcTempList(ncTemps);
                    List<object> list = new List<object>();
                    if (ncTemp != null)
                    {
                        list.Add(ncTemp.SteelAttr);
                        list.AddRange(ncTemp.GroupBoltsAttrs);
                    }
                    else
                    {
                        Model model = new Model();
                        ReadFile readFile = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{data.DataName}.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation)); //讀取檔案內容
                        readFile.DoWork();//開始工作
                        //readFile.AddToScene(model);//將讀取完的檔案放入到模型

                        list.Add((SteelAttr)readFile.Blocks[1].Entities[0].EntityData);
                        for (int c = 2; c < readFile.Blocks.Count; c++)
                        {
                            for (int g = 0; g < readFile.Blocks[c].Entities.Count; g++)
                            {
                                list.Add((BoltAttr)readFile.Blocks[c].Entities[g].EntityData);
                            }
                            
                        }
                    }
                    match.Add(list);
                }
            }
            GraphWin graphWin = new GraphWin(match);

            graphWin.Show();
            graphWin.Draw();
        }
    }
}
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
using devDept.Eyeshot.Entities;
using DevExpress.XtraSpreadsheet.Commands.Internal;
using System.Threading;

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
        ///// <summary>
        ///// 單一物件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void BrowserGraph(object sender, RoutedEventArgs e)
        //{
        //    string partNumber = ((Button)e.Source).Content.ToString(); //零件名稱
        //    STDSerialization ser = new STDSerialization(); //序列化處理器
        //    ObservableCollection<DataCorrespond> dataCorrespond = ser.GetDataCorrespond(); //序列化列表
        //    int indexDataCorrespond = dataCorrespond.FindIndex(el => el.Number == partNumber);//序列化的列表索引
        //    if (indexDataCorrespond != -1) //有找到序列化物件
        //    {
        //        DataCorrespond data = dataCorrespond[indexDataCorrespond]; //找到的物件
        //        NcTempList ncTemps = ser.GetNcTempList(); //NC 載入檔案
        //        NcTemp ncTemp = ncTemps.GetData(data.DataName);
        //        ser.SetNcTempList(ncTemps);
        //        GraphWin graphWin = null;
        //        if (ncTemp != null)
        //        {
        //            graphWin = new GraphWin(ncTemp);

        //        }
        //        else
        //        {
        //            graphWin = new GraphWin(data.DataName);
        //        }
        //        graphWin.Show();
        //        graphWin.Draw();
        //    }
        //}
        /// <summary>
        /// 組合物件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string content = ((Button)e.Source).Content.ToString(); //零件名稱
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<MaterialDataView> materialDataViews = ser.GetMaterialDataView(); //序列化列表
            int index = materialDataViews.FindIndex(el => el.MaterialNumber == content);//序列化的列表索引
            ObservableCollection<SteelPart> parts = ser.GetPart(materialDataViews[index].Profile.GetHashCode().ToString());//零件列表
            GraphWin graphWin = new GraphWin();
            graphWin.Show();
            NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            var nc = (from el in ncTemps
                      where materialDataViews[index].Parts.Select(x => x.PartNumber).Contains(el.SteelAttr.PartNumber)
                      select el).ToList();
            for (int i = 0; i < nc.Count; i++)
            {
                ncTemps.GetData(nc[i].SteelAttr.GUID.ToString());
                graphWin.model.Clear();
                Steel3DBlock.AddSteel(nc[i].SteelAttr, graphWin.model, out BlockReference steelBlock); //加入 3d 鋼構參考圖塊
                nc[i].GroupBoltsAttrs.ForEach(bolt =>
                {
                    Bolts3DBlock.AddBolts(bolt, graphWin.model, out BlockReference botsBlock); //加入到 3d 視圖
                });
                ser.SetModel(nc[i].SteelAttr.GUID.ToString(), graphWin.model);//儲存 3d 視圖
                ser.SetNcTempList(ncTemps);//儲存檔案
            }
            //3d 零件檔案
            //var files = from part 
            //            in parts 
            //            where materialDataViews[index].Parts.Select(mat=>mat.PartNumber).Contains(part.Number) 
            //            select new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{part.GUID}.dm", devDept.Serialization.contentType.GeometryAndTessellation);
            List<int> viewIndex = new List<int>();
            materialDataViews[index].Parts.
                ForEach(data => viewIndex.Add(parts.FindIndex(el => el.Number == data.PartNumber)));
            EntityList entities = new EntityList();
            for (int i = 0; i < viewIndex.Count; i++)
            {
                ReadFile file = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{parts[viewIndex[i]].GUID}.dm", devDept.Serialization.contentType.GeometryAndTessellation);
                file.DoWork();
                file.AddToScene(graphWin.model);
                graphWin.model.Entities.Where(el => el.GroupIndex == -1).ForEach(el =>
                {
                    el.GroupIndex = i;
                    //el.Translate(i == 0 ? materialDataViews[index].);
                });
                entities.AddRange(graphWin.model.Entities);
            }
            graphWin.model.Entities.Clear();
            graphWin.model.Entities.AddRange(entities);
            //readFile.DoWork();
            //readFile.AddToScene(graphWin.model);
            graphWin.model.Invalidate();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PopupWindowsBase popup = new PopupWindowsBase();
            popup.Content = new LengthDodageParameterType_Office();
            popup.Width = 1024;
            popup.Height = 768;
            popup.ShowDialog();
        }

        private void ExportTable(object sender, RoutedEventArgs e)
        {
            if (view.DataContext != null)
                view.ShowPrintPreview(this);
            else
                MessageBox.Show("報表沒有資料。");
        }
    }
}
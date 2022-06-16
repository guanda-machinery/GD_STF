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
using System.Drawing;

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
            MaterialDataView material = materialDataViews[index];
            ObservableCollection<SteelPart> parts = ser.GetPart(material.Profile.GetHashCode().ToString());//零件列表
            GraphWin graphWin = new GraphWin();
            graphWin.Show();
            NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            var _ = material.Parts.Select(x => x.PartNumber); //選擇要使用的零件編號
            var guid = (from el in ncTemps
                        where _.ToList().Contains(el.SteelAttr.PartNumber)
                        select el.SteelAttr.GUID.ToString()).ToList();//選擇使用的NC文件
            //產生nc檔案圖檔
            for (int i = 0; i < guid.Count; i++)
            {
                NcTemp nc = ncTemps.GetData(guid[i]); //取得nc資訊
                graphWin.model.Clear();//清除模型內物件
                Steel3DBlock.AddSteel(nc.SteelAttr, graphWin.model, out BlockReference steelBlock); //加入 3d 鋼構參考圖塊
                nc.GroupBoltsAttrs.ForEach(bolt =>
                {
                    Bolts3DBlock.AddBolts(bolt, graphWin.model, out BlockReference botsBlock); //加入到 3d 視圖
                });
                ser.SetModel(guid[i], graphWin.model);//儲存 3d 視圖
                ser.SetNcTempList(ncTemps);//儲存檔案
            }

            List<int> viewIndex = new List<int>();
            material.Parts.
                ForEach(data => viewIndex.Add(parts.FindIndex(el => el.Number == data.PartNumber)));

            Dictionary<int, EntityList> keyValuePairs = new Dictionary<int, EntityList>();
            double end = material.StartCut; //結束位置
            double start = 0; //起始位置
            //產生3D物件
            for (int i = 0; i < viewIndex.Count; i++)
            {
                //產生代表餘料的 3D 實體
                {
                    SteelAttr steelAttr = new SteelAttr(parts[0]);
                    steelAttr.Length = end - start;
                    Steel3DBlock.AddSteel(steelAttr, graphWin.model, out BlockReference reference, "Cut");
                    graphWin.model.Blocks[steelAttr.GUID.ToString()].Entities[0].Color = ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Ingredient2);
                    reference.GroupIndex = int.MaxValue;
                    reference.Translate(start, 0);
                }
                if (keyValuePairs.Keys.Contains(viewIndex[i]))//如果有反序列化過
                {
                    EntityList list = new EntityList();
                    keyValuePairs[viewIndex[i]].ForEach(el =>
                    {
                        Entity entity = (Entity)el.Clone();
                        entity.GroupIndex = i;
                        list.Add(entity);
                    });
                    keyValuePairs[viewIndex[i]].AddRange(list);
                }
                else
                {
                    ReadFile file = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{parts[viewIndex[i]].GUID}.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation)); //讀取檔案內容
                    file.DoWork();
                    file.AddToScene(graphWin.model);
                    graphWin.model.Entities.ForEach(el =>
                    {
                        el.GroupIndex = i;
                        el.Translate(end, 0);
                    });
                    keyValuePairs.Add(viewIndex[i], graphWin.model.Entities);

                    start += end + parts[viewIndex[i]].Length;

                    end += i + 1 > viewIndex.Count ? material.LengthList[material.LengthIndex] : parts[viewIndex[i]].Length;
                }
            }


            //EntityList entities = new EntityList();
            //double moveX = materialDataViews[index].StartCut;
            //for (int i = 0; i < viewIndex.Count; i++)
            //{

            //    ReadFile file = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{parts[viewIndex[i]].GUID}.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation)); //讀取檔案內容
            //    file.DoWork();
            //    file.AddToScene(graphWin.model);
            //    graphWin.model.Entities.ForEach(el =>
            //    {
            //        el.GroupIndex = i; //變更群組
            //        el.Translate(moveX, 0);//移動實體
            //    });
            //    moveX += materialDataViews[index].Cut + parts[viewIndex[i]].Length; //移動物件相對位置
            //    entities.AddRange(graphWin.model.Entities);//加入到暫存列表內
            //}

            graphWin.model.Entities.Clear();
            keyValuePairs.ForEach(el => graphWin.model.Entities.AddRange(el.Value));
            graphWin.model.Entities.Regen();
            graphWin.model.Refresh();
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
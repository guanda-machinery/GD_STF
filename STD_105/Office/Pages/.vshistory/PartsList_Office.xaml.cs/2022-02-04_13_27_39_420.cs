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
using System.IO;

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
            GraphWin graphWin = new GraphWin();
            graphWin.Show();
            string content = ((Button)e.Source).Content.ToString(); //零件名稱
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<MaterialDataView> materialDataViews = ser.GetMaterialDataView(); //序列化列表
            int index = materialDataViews.FindIndex(el => el.Parts.Any(data => data.PartNumber == content));//序列化的列表索引
            MaterialDataView material = materialDataViews[index];
            ObservableCollection<SteelPart> parts = ser.GetPart(material.Profile.GetHashCode().ToString());//零件列表
            Guid guid = parts.First(el => el.Number == content).GUID.Value;
            if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{guid}.dm"))
            {
                graphWin.model.LoadNcToModel($@"{guid}");
            }
            else
            {
                ReadFile file = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{guid}.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation)); //讀取檔案內容
                file.DoWork();
                file.AddToScene(graphWin.model);
            }
        }
        /// <summary>
        /// 組合物件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string content = ((Button)e.Source).Content.ToString(); //零件名稱
            //STDSerialization ser = new STDSerialization(); //序列化處理器
            //ObservableCollection<MaterialDataView> materialDataViews = ser.GetMaterialDataView(); //序列化列表
            //int index = materialDataViews.FindIndex(el => el.MaterialNumber == content);//序列化的列表索引
            //MaterialDataView material = materialDataViews[index];
            //ObservableCollection<SteelPart> parts = ser.GetPart(material.Profile.GetHashCode().ToString());//零件列表
            GraphWin graphWin = new GraphWin();
            graphWin.Show();
            graphWin.model.AssemblyPart(content);
            //NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            //var _ = material.Parts.Select(x => x.PartNumber); //選擇要使用的零件編號
            //var guid = (from el in ncTemps
            //            where _.ToList().Contains(el.SteelAttr.PartNumber)
            //            select el.SteelAttr.GUID.ToString()).ToList();//選擇使用的NC文件
            ////產生nc檔案圖檔
            //for (int i = 0; i < guid.Count; i++)
            //{
            //    graphWin.model.LoadNcToModel(guid[i]);
            //}

            //List<int> viewIndex = new List<int>();
            //material.Parts.
            //    ForEach(data => viewIndex.Add(parts.FindIndex(el => el.Number == data.PartNumber)));

            ////Dictionary<int, EntityList> keyValuePairs = new Dictionary<int, EntityList>();
            //List<int> finish = new List<int>(); //使用過的陣列位置
            //List<double> position = new List<double>(); //第一次移動的座標(只記錄 start)，匹配 finish 位置
            //double end = material.StartCut; //結束位置
            //double start = 0; //起始位置
            //EntityList ent = new EntityList();
            ////產生3D物件
            //for (int i = 0; i < viewIndex.Count; i++)
            //{
            //    graphWin.model.Entities.Clear();
            //    //產生代表餘料的 3D 實體
            //    DrawCutMesh(parts[0], graphWin, end, start, ent);

            //    if (finish.Contains(viewIndex[i]))//如果有反序列化過
            //    {
            //        EntityList list = new EntityList();
            //        ent.Where(el => el.GroupIndex == finish.IndexOf(viewIndex[i])).ForEach(el =>
            //         {
            //             int posIndex = finish.IndexOf(viewIndex[i]);
            //             Entity entity = (Entity)el.Clone();
            //             entity.GroupIndex = i;
            //             double x = start - position[posIndex];
            //             entity.Translate(start - position[posIndex], 0);
            //             list.Add(entity);
            //         });
            //        ent.AddRange(list);
            //    }
            //    else
            //    {
            //        ReadFile file = new ReadFile($@"{ApplicationVM.DirectoryDevPart()}\{parts[viewIndex[i]].GUID}.dm", new FileSerializerExt(devDept.Serialization.contentType.GeometryAndTessellation)); //讀取檔案內容
            //        file.DoWork();
            //        file.AddToScene(graphWin.model);
            //        graphWin.model.Entities.ForEach(el =>
            //        {
            //            graphWin.model.Blocks[((BlockReference)el).BlockName].Entities.ForEach(entity =>
            //            {
            //                if (entity.EntityData is SteelAttr)
            //                {
            //                    entity.Color = ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Ingredient1);
            //                }
            //            });
            //            el.GroupIndex = i;
            //            el.Translate(end, 0);
            //            ent.Add(el);
            //        });
            //        position.Add(start);
            //        finish.Add(viewIndex[i]);
            //    }
            //    start = end + parts[viewIndex[i]].Length;
            //    end = start + material.Cut;
            //}
            //DrawCutMesh(parts[0], graphWin, start, material.LengthList[material.LengthIndex] + material.StartCut + material.EndCut, ent);

            //graphWin.model.Entities.Clear();
            //graphWin.model.Entities.AddRange(ent);
            //graphWin.model.Entities.Regen();
            //graphWin.model.Refresh();
            //graphWin.model.Invalidate();
        }

        private static void DrawCutMesh(SteelPart part, GraphWin graphWin, double end, double start, EntityList ent)
        {
            SteelAttr steelAttr = new SteelAttr(part);
            steelAttr.Length = end - start;
            Steel3DBlock.AddSteel(steelAttr, graphWin.model, out BlockReference reference, "Cut");
            graphWin.model.Blocks[steelAttr.GUID.ToString()].Entities[0].Color = ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Ingredient2);
            reference.GroupIndex = int.MaxValue;
            reference.Translate(start, 0);
            ent.Add(reference);
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
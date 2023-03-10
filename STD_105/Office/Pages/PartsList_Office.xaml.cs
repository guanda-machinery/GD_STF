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
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;
using WPFSTD105.ViewModel;

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
            //ObSettingVM obvm = new ObSettingVM();
            string partNumber = ((Button)e.Source).Content.ToString(); //零件名稱
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<DataCorrespond> dataCorrespond = ser.GetDataCorrespond(); //序列化列表
            int indexDataCorrespond = dataCorrespond.FindIndex(data => data.Number == partNumber);//序列化的列表索引
            if (indexDataCorrespond != -1) //有找到序列化物件
            {
                DataCorrespond data = dataCorrespond[indexDataCorrespond]; //找到的物件
                GraphWin graphWin = new GraphWin();
                graphWin.Show();
                if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{data.DataName}.dm"))
                {
                    graphWin.model.LoadNcToModel(data.DataName, ObSettingVM.allowType);
                }
                else
                {
                    ReadFile readFile = ser.ReadPartModel(data.DataName);
                    if (readFile == null)
                    {
                        WinUIMessageBox.Show(null,
                            $"專案Dev_Part資料夾讀取失敗",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             FloatingMode.Window);
                        return;
                    }
                    readFile.DoWork();
                    readFile.AddToScene(graphWin.model);
                }
            }
            else
            {
                throw new Exception("找不到檔案");
            }
        }
        /// <summary>
        /// 組合物件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string content = ((Button)e.Source).Content.ToString(); //素材編號
            GraphWin graphWin = new GraphWin();
            graphWin.Show();
            graphWin.model.AssemblyPart(content);
            graphWin.model.Entities.Regen();
            graphWin.model.Refresh();
            graphWin.model.Invalidate();
            graphWin.model.ZoomFit();//設置道適合的視口
            graphWin.model.Invalidate();//初始化模型
            graphWin.model.SelectionChanged -=  graphWin.model.Model_SelectionChanged;
            graphWin.model.SelectionChanged +=Model_SelectionChanged; ;
        }

        private void Model_SelectionChanged(object sender, devDept.Eyeshot.Environment.SelectionChangedEventArgs e)
        {
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
            popup.DataContext = ViewModel.MatchSetting;
            popup.Width = 1024;
            popup.Height = 768;
            popup.ShowDialog();
        }

        private void BrowserGraph(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
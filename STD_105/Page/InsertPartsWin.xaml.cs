using DevExpress.Data.Extensions;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using DevExpress.XtraRichEdit.Model;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace STD_105
{
    /// <summary>
    /// InsertPartsWin.xaml 的互動邏輯
    /// </summary>
    public partial class InsertPartsWin : WPFWindowsBase.BasePage<WPFSTD105.OfficeTypeSettingVM> 
    {
        private GridControl _materialListGridControl { get; }
        public InsertPartsWin(GridControl MaterialListGridControl)
        {
            InitializeComponent();
            _materialListGridControl = MaterialListGridControl;
            if (MaterialListGridControl.SelectedItem is GD_STD.Data.MaterialDataView)
            {
                SelectedMaterial = MaterialListGridControl.SelectedItem as GD_STD.Data.MaterialDataView;

                MateriaLengthLabel.Content = SelectedMaterial.LengthStr.ToString("f1");
                UsedMateriaLengthLabel.Content = SelectedMaterial.Loss.ToString("f1");
                RemainingMateriaLengthLabel.Content = (SelectedMaterial.LengthStr - SelectedMaterial.Loss).ToString("f1");
            }
            else
            {
                throw new Exception("GridControl資料型錯誤，需使用MaterialDataView為資料");
            }
        }

        private readonly GD_STD.Data.MaterialDataView SelectedMaterial = new GD_STD.Data.MaterialDataView();


        /// <summary>
        /// 同步卷軸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollOwner_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            PartListTableView.IndicatorWidth = (((PartsGridControl.ItemsSource as System.Collections.ICollection).Count + 1).ToString().Length * 10) + 5;

            if ((sender as DevExpress.Xpf.Grid.TableView).Name == PartListTableView.Name)
            {
                IScrollInfo SoftCountTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(SoftCountTableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                if (SoftCountTableView_ScrollElement != null)
                    SoftCountTableView_ScrollElement.SetVerticalOffset(e.VerticalOffset);
            }
            if ((sender as DevExpress.Xpf.Grid.TableView).Name == SoftCountTableView.Name)
            {
                IScrollInfo PartsTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(PartListTableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                if (PartsTableView_ScrollElement != null)
                    PartsTableView_ScrollElement.SetVerticalOffset(e.VerticalOffset);
            }
        }

        /// <summary>
        /// 暫時使用 以後要改為vm層控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            var SortItems = (IEnumerable<GD_STD.Data.TypeSettingDataView>)SoftGridControl.ItemsSource;
            //篩出排版數量>0
            var UsedLength = SelectedMaterial.Loss;

            var SoftList = SortItems.ToList().FindAll(x => (x.SortCount > 0));
            foreach (var Softing in SoftList)
            {
                UsedLength += (Softing.Length + SelectedMaterial.Cut) * Softing.SortCount;
            }
           
            //如果消耗量比原素材還長，則鎖定確認按鈕使其不可按
            UsedMateriaLengthLabel.Content = UsedLength.ToString("f1");
            RemainingMateriaLengthLabel.Content = (SelectedMaterial.LengthStr - UsedLength).ToString("f1");
            if (SelectedMaterial.LengthStr - UsedLength >= 0)
            {
                UsedMateriaLengthLabel.Foreground = Brushes.Black;
                RemainingMateriaLengthLabel.Foreground = Brushes.Black;
                CheckButton.IsEnabled = true;
            }
            else
            {
                UsedMateriaLengthLabel.Foreground = Brushes.Red;
                RemainingMateriaLengthLabel.Foreground = Brushes.Red;
                CheckButton.IsEnabled = false;
            }
        }
        
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            ExitWin();
        }

        private void Check_Button_Click(object sender, RoutedEventArgs e)
        {
            //將預排加入parts
            var SortItems = ((IEnumerable<GD_STD.Data.TypeSettingDataView>)SoftGridControl.ItemsSource).ToList().FindAll(x => (x.SortCount > 0));
            for (int i =0; i< SortItems.Count(); i++)
            {
                //有複數零件 重複加入
                for(int j =0;j< SortItems[i].SortCount;j++)
                {
                    SortItems[i].Revise = DateTime.Now;
                    SelectedMaterial.Parts.Add(SortItems[i]);
                }
            }

            //做完動作在此存檔!
            var ser = new WPFSTD105.STDSerialization(); //序列化處理器
                                                        //零件存檔
            //素材存檔
            ser.SetMaterialDataView(_materialListGridControl.ItemsSource as ObservableCollection<GD_STD.Data.MaterialDataView>);
   
            //零件存檔
            foreach (var _PartItem in SortItems)
            {
                ObservableCollection<SteelPart> steelParts = ser.GetPart(_PartItem.Profile.GetHashCode().ToString());
                int steelIndex = steelParts.FindIndex(x => x.Number == _PartItem.PartNumber);
                if (steelIndex != -1)
                {
                    int partMatch = steelParts[steelIndex].Match.FindIndex(x => x == true);
                    if (partMatch != -1)
                    {
                       // _PartItem.Match[partMatch] = false;
                        steelParts[steelIndex].Match[partMatch] = false;
                    }
                    else
                    {
                        //_PartItem.Match.Add(false);
                        steelParts[steelIndex].Match.Add(false);
                    }

                    int PartItemMatch = _PartItem.Match.FindIndex(x => x == true);
                    if (PartItemMatch != -1)
                    {
                        _PartItem.Match[PartItemMatch] = false;
                        //steelParts[steelIndex].Match[partMatch] = false;
                    }
                    else
                    {
                        _PartItem.Match.Add(false);
                        //steelParts[steelIndex].Match.Add(false);
                    }


                }
                _PartItem.Revise = DateTime.Now;
                ser.SetPart(_PartItem.Profile.GetHashCode().ToString(), new ObservableCollection<object>(steelParts));
            }



            ExitWin();
        }

        private void ExitWin()
        {
            //將所有預排清除
            var SortItems = (IEnumerable<GD_STD.Data.TypeSettingDataView>)SoftGridControl.ItemsSource;
            foreach (var Softing in SortItems)
            {
                Softing.SortCount = 0;
            }

            //關閉視窗
            Window.GetWindow(this).Close();
        }




    }
}

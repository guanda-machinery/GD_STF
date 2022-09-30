using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Graphics;
using DevExpress.Data.Extensions;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFSTD105.ViewModel;
using WPFWindowsBase;
using static devDept.Eyeshot.Entities.Mesh;
using static devDept.Eyeshot.Environment;
using BlockReference = devDept.Eyeshot.Entities.BlockReference;
using MouseButton = devDept.Eyeshot.MouseButton;
using System.IO;
using GD_STD.Data;
using System.Collections.ObjectModel;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using System.Windows.Controls.Primitives;

namespace STD_105.Office
{
    /// <summary>
    /// TypesettingsSetting.xaml 的互動邏輯
    /// </summary>
    public partial class TypesettingsSetting : BasePage<OfficeTypeSettingVM>
    {
        /// <summary>
        /// 220818 蘇冠綸 排版設定版面完成
        /// </summary>
        public TypesettingsSetting()
        {
            InitializeComponent();
        }

        bool TableViewLoadedBoolen = false;
        private void Material_List_TableView_Loaded(object sender, RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Grid.TableView)sender).FocusedRowHandle = DevExpress.Xpf.Grid.GridControl.InvalidRowHandle;
            TableViewLoadedBoolen = true;
        }


        private void PartListTableView_Loaded(object sender, RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Grid.TableView)sender).FocusedRowHandle = DevExpress.Xpf.Grid.GridControl.InvalidRowHandle;
            
        }
        private void SoftCountTableView_Loaded(object sender, RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Grid.TableView)sender).FocusedRowHandle = DevExpress.Xpf.Grid.GridControl.InvalidRowHandle;
        }



        private void Material_List_GridControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (TableViewLoadedBoolen == false)
                return;

            var SenderC = sender as DevExpress.Xpf.Grid.GridControl;
            if (SenderC.View != null)
            {
                if (e.NewItem is GD_STD.Data.MaterialDataView)
                {
                    var ENewItem = (GD_STD.Data.MaterialDataView)e.NewItem;
                    ENewItem.ButtonEnable = true;

                    var NewHandle = SenderC.FindRow(e.NewItem);
                    SenderC.RefreshRow(NewHandle);//畫面裡刷新上面該列的設定值
                }
                if (e.OldItem is GD_STD.Data.MaterialDataView)
                {
                    var EOldItem = (GD_STD.Data.MaterialDataView)e.OldItem;
                    EOldItem.ButtonEnable = false; 
                    var OldHandle = SenderC.FindRow(e.OldItem);
                    SenderC.RefreshRow(OldHandle);//畫面裡刷新上面該列的設定值
                }
            }
        }





        private void ScrollOwner_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
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

    }
}









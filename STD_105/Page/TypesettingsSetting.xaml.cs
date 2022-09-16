﻿using devDept.Eyeshot;
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

        private void Material_List_GridControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (e.NewItem != null)
            {
                var a = (GD_STD.Data.MaterialDataView)e.NewItem;
                a.ButtonEnable = true;
            }
            if (e.OldItem != null)
            {
                var b = (GD_STD.Data.MaterialDataView)e.OldItem; 
                b.ButtonEnable =false;
            }
            
            /*var SenderC = sender as DevExpress.Xpf.Grid.GridControl;
            var c = (IEnumerable<GD_STD.Data.MaterialDataView>)SenderC.ItemsSource;
            
            if (c != null)
            {
                foreach (var cItem in c)
                {
                    if (cItem == (GD_STD.Data.MaterialDataView)e.NewItem)
                        cItem.ButtonEnable = true;
                    else
                        cItem.ButtonEnable = false;

                }*/
                //var CopyList = c.ToList();
                //c.
                //SenderC.ItemsSource = null;
                //SenderC.ItemsSource = CopyList;
            }
           
           
        }

    }
}









﻿using DevExpress.Xpf.Grid;
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
                CommonViewModel.ChildWin = new PopupWindowsBase();
                OfficeViewModel.PopupCurrentPage = PopupWindows.GraphBrowser;
                OfficeViewModel.PopupTitle = "圖形預覽";
                OfficeViewModel.PopupWidth = 1024;
                OfficeViewModel.PopupHeight = 768;
                CommonViewModel.ChildWin.ShowDialog();
            }
        }
    }
}
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

namespace STD_105
{
    /// <summary>
    /// TypeSettingPage.xaml 的互動邏輯
    /// </summary>
    public partial class TypeSettingPage : BasePage<WPFSTD105.ViewModel.TypeSettingVM>
    {
        public TypeSettingPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
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
            int indexDataCorrespond = dataCorrespond.FindIndex(data => data.Number == partNumber);//序列化的列表索引
            if (indexDataCorrespond != -1) //有找到序列化物件
            {
                DataCorrespond data = dataCorrespond[indexDataCorrespond]; //找到的物件
                GraphWin graphWin = new GraphWin();
                graphWin.Show();
                if (!File.Exists($@"{ApplicationVM.DirectoryDevPart()}\{data.DataName}.dm"))
                {
                    graphWin.model.LoadNcToModel(data.DataName);
                    
                }
                else
                {
                    ReadFile readFile = ser.ReadPartModel(data.DataName);
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
            graphWin.model.SelectionChanged += Model_SelectionChanged; ;
        }

        private void Model_SelectionChanged(object sender, devDept.Eyeshot.Environment.SelectionChangedEventArgs e)
        {

        }
    }
}

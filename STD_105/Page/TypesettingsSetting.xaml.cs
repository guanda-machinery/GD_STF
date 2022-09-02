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
            var Grid =  (ObservableCollection<TypeSettingDataView>)PartGridControl.ItemsSource;
            var MGrid = (ObservableCollection<MaterialDataView>)Material_List_GridControl.ItemsSource;


        }

        private void Set_TypeSettingParameterGrid_AllCheckboxChecked_Click(object sender, RoutedEventArgs e)
        {
            GetWpfLogicalChildClass.SetAllCheckBoxTrueOrFalse(TypeSettingParameterGrid);
        }
    }

    #region 尋找所有子控制項
    /// <summary>
    /// 尋找在控制元件中找尋所有符合條件的控制項
    /// </summary>
    public class GetWpfLogicalChildClass
    {
        /// <summary>
        /// 設定控制元件內所有子控制元件checkBox全勾選/全取消 , parent為母控制元件
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool SetAllCheckBoxTrueOrFalse(object parent)
        {
            try
            {   
                //勾選邏輯->checkbox中存在任意未勾選時，按鈕為全勾選
                //當checkbox為全勾選時，才執行全部取消勾選

                List<CheckBox> AllCheckBoxList = GetWpfLogicalChildClass.GetLogicalChildCollection<CheckBox>(parent);
                var AllCheckboxBoolen = AllCheckBoxList.Exists(x => (x.IsChecked == false));

                foreach (var CBox in AllCheckBoxList)
                {
                    CBox.IsChecked = AllCheckboxBoolen;
                }
                return true;
            }
            catch( Exception EX)
            {
                return false;
            }

        }



        /// <summary>
        /// 尋找控制項 T = 子控制元件的類型 , parent = 搜尋之母控制元件
        /// </summary>
        /// <typeparam name="T">子控制元件的類型</typeparam>
        /// <param name="parent">目標搜尋的控制元件名稱</param>
        /// <returns></returns>
        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            System.Collections.IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }
      
    }
    #endregion
}









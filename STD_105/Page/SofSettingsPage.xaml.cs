using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFSTD105.ViewModel;
using WPFWindowsBase;
using System.Threading.Tasks;
using GD_STD;
using System;
using System.Collections.Generic;
using System.Reflection;
using DevExpress.Xpf.Grid;
using System.Windows.Data;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Mvvm.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using WPFSTD105.FluentAPI;

namespace STD_105
{
    /// <summary>
    /// 軟體設定
    /// </summary>
    public partial class SofSettingsPage : BasePage<SofSettingVM>
    {
        public SofSettingsPage()
        {

            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
            //DataLayoutControl
            //layoutItemStyle = this.FindResource("LayoutItem") as Style;
            //noTitleLayoutGroup = this.FindResource("NoTitleLayoutGroup") as Style;
            //groupFrame = this.FindResource("GroupFrame") as Style;
            //groupInternal = this.FindResource("GroupInternal") as Style;
        }
        ////資料行
        //Style layoutItemStyle;
        ////LayoutGroup 隱藏的
        //Style noTitleLayoutGroup;
        ////LayoutGroup 群組外框
        //Style groupFrame;
        ////LayoutGroup 內部群組
        //Style groupInternal;

        //private void Reflectiontest()
        //{
        //    GD_STD.MVVM.MecSetting mecSetting = new GD_STD.MVVM.MecSetting();
        //    Type type = mecSetting.GetType();
            
        //    foreach (PropertyInfo property in type.GetProperties())
        //    {
        //        if (property.PropertyType == typeof(GD_STD.MVVM.AxisSetting))
        //        {
        //            LayoutGroup layoutGroup = new LayoutGroup();
        //            if (property.GetCustomAttributes(typeof(GD_STD.Attribute.MVVMAttribute)).FirstOrDefault() is GD_STD.Attribute.MVVMAttribute)
        //            {                        
        //                LabelItem labelItem = new LabelItem();
        //                labelItem.Content = property.Name;
        //                layoutGroup.Items.Add(labelItem);
        //                layout.Children.Add(layoutGroup);
        //            }
        //        }
        //    }
        //}
    }
}

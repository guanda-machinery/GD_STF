using System.Windows;
using WPFSTD105.ViewModel;
using WPFWindowsBase;
using System.Data;
using System.IO;
using System.Linq;
using System;
using System.Collections;
using WPFSTD105.Attribute;
using System.Collections.ObjectModel;
using GD_STD;
using DevExpress.Utils.Extensions;

namespace STD_105.Office
{
    /// <summary>
    /// ParametersSettings_Office.xaml 的互動邏輯
    /// </summary>
    public partial class SectionSpecExcel2Inp : BasePage<SettingParVM>
    {
        public SectionSpecExcel2Inp()
        {
            InitializeComponent();
        }

        ///// <summary>
        ///// RH 要顯示集合列表
        ///// </summary>
        //public ObservableCollection<SteelAttr> RH { get; set; } = new ObservableCollection<SteelAttr>();
        //public ObservableCollection<SteelAttr> RH_check { get; set; } = new ObservableCollection<SteelAttr>();
        //
        //private void button_transform(object sender, RoutedEventArgs e)
        //{
        //    //Step1:讀取原inp檔
        //    RH = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"轉換inp\RH.inp");
        //    SerializationHelper.SerializeBinary(RH, $@"轉換inp\RH_R.inp");//變更系統內設定的斷面規格
        //
        //    //Step2:刪除原inp檔中的規格資料
        //    foreach (SteelAttr attr in RH)
        //    {
        //        ObservableCollection<SteelAttr> system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"轉換inp\RH_R.inp");//系統內設定的斷面規格
        //
        //        system.Remove(el => el.Profile == attr.Profile);
        //        SerializationHelper.SerializeBinary(system, $@"轉換inp\RH_R.inp");//變更系統內設定的斷面規格
        //    }
        //    RH_check = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"轉換inp\RH_R.inp");
        //
        //    //Step3:讀取excel中各規格
        //    // 讀取TXT檔內文串
        //    StreamReader str = new StreamReader(@"轉換inp\斷面規格_表1.txt");
        //    /// <summary>
        //    /// 檔案中欄位名稱字串
        //    /// </summary>
        //    string sColumnName;
        //    /// <summary>
        //    /// 檔案中斷面規格的資料字串
        //    /// </summary>
        //    string sSteelAttrFromFile;
        //    /// <summary>
        //    /// 檔案中斷面規格的所有大類別, EX:BOX
        //    /// </summary>
        //    ArrayList alSteelAttrCategory = new ArrayList();
        //    /// <summary>
        //    /// 檔案中斷面規格的所有分類, EX:TUBE-->X
        //    /// </summary>
        //    ArrayList alSteelAttrType = new ArrayList();
        //    /// <summary>
        //    /// 檔案中斷面規格的所有大類別&所有分類
        //    /// </summary>
        //    ArrayList alSteelAttrCategoryType = new ArrayList();
        //    /// <summary>
        //    /// 檔案中斷面規格的資料
        //    /// </summary>
        //    ArrayList alSteelAttrFromFile = new ArrayList();
        //
        //    //讀取斷面規格檔案內容並儲存至變數
        //    sColumnName = str.ReadLine();
        //    string[] ColumnName = sColumnName.Split('\t', '\n');
        //    try
        //    {
        //        sSteelAttrFromFile = str.ReadLine();
        //        
        //        while (sSteelAttrFromFile != null)
        //        {
        //            
        //            string[] SteelAttrFromFile = sSteelAttrFromFile.Split('\t', '\n');
        //            string section_name_arrow = "-->";
        //            
        //            if (SteelAttrFromFile[1] == "")
        //            {
        //                if (SteelAttrFromFile[0].Contains(section_name_arrow))
        //                {
        //                    alSteelAttrType.Add(SteelAttrFromFile[0]);
        //                }
        //                else
        //                {
        //                    alSteelAttrCategory.Add(SteelAttrFromFile[0]);
        //                }
        //                alSteelAttrCategoryType.Add(SteelAttrFromFile[0]);
        //            }
        //            else
        //            {
        //                SteelAttrInExcel steel_attr = new SteelAttrInExcel();
        //                steel_attr.section_name = SteelAttrFromFile[0];
        //                steel_attr.h = Convert.ToSingle(SteelAttrFromFile[1]);
        //                steel_attr.b = Convert.ToSingle(SteelAttrFromFile[2]);
        //                steel_attr.t1 = Convert.ToSingle(SteelAttrFromFile[3]);
        //                steel_attr.t2 = Convert.ToSingle(SteelAttrFromFile[4]);
        //                steel_attr.r1 = Convert.ToSingle(SteelAttrFromFile[5]);
        //                steel_attr.r2 = Convert.ToSingle(SteelAttrFromFile[6]);
        //                steel_attr.surface_area = Convert.ToSingle(SteelAttrFromFile[7]);
        //                steel_attr.section_area = Convert.ToSingle(SteelAttrFromFile[8]);
        //                steel_attr.weight_per_unit = Convert.ToSingle(SteelAttrFromFile[9]);
        //                steel_attr.density = Convert.ToInt32(SteelAttrFromFile[10]);
        //
        //                alSteelAttrFromFile.Add(steel_attr);
        //            }
        //
        //            sSteelAttrFromFile = str.ReadLine();
        //        }
        //        //close the file
        //        str.Close();
        //    }
        //    catch (Exception error)
        //    {
        //        MessageBox.Show("Exception: " + error.Message);
        //    }
        //    finally
        //    {
        //        //MessageBox.Show("Executing finally block.");
        //    }
        //
        //    //Step4:將excel中規格新增至inp中
        //}
    }
}

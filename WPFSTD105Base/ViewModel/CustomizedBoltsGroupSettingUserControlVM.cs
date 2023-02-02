using DevExpress.CodeParser;
using DevExpress.Data.Extensions;
using DevExpress.Utils.Extensions;
using DevExpress.Xpf.WindowsUI;
using GD_STD.Enum;
using GrapeCity.Documents.Pdf.Parser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFSTD105.Attribute;
using WPFWindowsBase;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.Native;
using System.Web.UI;
using devDept.Geometry;
using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

//CustomizeGroupBoltsUserControlVM
namespace WPFSTD105
{
    /// <summary>
    /// 參數設定 - 客製孔群
    ///     1.讓使用者 [新增/修改/刪除] 客製孔群
    ///     2.依照 [依附類別]屬性依[孔群編號]存入對應路徑
    ///         2.1.系統客製，存入 ""，存入 STD_105\GroupBoltsType，製品下拉一律帶出
    ///         2.2.專案客製，存入 專案名稱，存入 專案名稱\GroupBoltsType，製品下拉By專案帶出
    ///     3.以清單(Grid)顯示 系統客製+專案客製 之客製孔群列表，排序依 [依附類別+孔群編號]
    ///     4.檔案依 [孔群編號] 分開儲存
    /// </summary>
    public class CustomizedBoltsGroupSettingUserControlVM : WPFWindowsBase.BaseViewModel
    {
        /// <summary>
        /// 參數設定 - 客製孔群
        /// </summary>
        public CustomizedBoltsGroupSettingUserControlVM()
        {
             GroupBoltsTypeByTargetSelected = GroupBoltsTypeByTarget.Project;
            SettingParGroupBoltsType = new SettingParGroupBoltsTypeModel() { groupBoltsTypeName = "NewGroupBolts", groupBoltsAttr = new GroupBoltsAttr() };
        }

        private SettingParGroupBoltsTypeModel _settingParGroupBoltsType= null;
        /// <summary>
        /// combobox用
        /// </summary>
        public SettingParGroupBoltsTypeModel SettingParGroupBoltsType
        {
            get
            {
                if(_settingParGroupBoltsType == null)
                {
                    _settingParGroupBoltsType = new SettingParGroupBoltsTypeModel() { groupBoltsTypeName = "NewGroupBolts", groupBoltsAttr = new GroupBoltsAttr() };
                }
                return _settingParGroupBoltsType; 
            }
            set
            {
                _settingParGroupBoltsType = value;
                OnPropertyChanged(nameof(SettingParGroupBoltsType)); 
            }
        }


        private GroupBoltsTypeByTarget _groupBoltsTypeByTargetSelected;
        /// <summary>
        /// 讀取客製孔群 - 依附類別(系統客製、專案客製)(下拉或核選元件)
        /// </summary>
        public GroupBoltsTypeByTarget GroupBoltsTypeByTargetSelected
        {
            get => _groupBoltsTypeByTargetSelected;
            set
            {
                _groupBoltsTypeByTargetSelected = value;
                SettingParGroupBoltsTypeList = new STDSerialization().GetGroupBoltsTypeList(_groupBoltsTypeByTargetSelected);
                SettingParGroupBoltsType = SettingParGroupBoltsTypeModel.NotSelectGroupBoltsTypeModel; 
            }
        }



        /// <summary>
        /// 客製孔群寫入目標
        /// </summary>
        public GroupBoltsTypeByTarget GroupBoltsTypeWriteTarget{ get; set; }


        private bool IsReadHistoryGroupBolt = false;

        public ICommand BuiltNewGroupBolt
        {
            get
            {
                return new RelayCommand(() =>
                {
                        SettingParGroupBoltsType =  new SettingParGroupBoltsTypeModel() {  groupBoltsTypeName= "NewGroupBolts", groupBoltsAttr = new GroupBoltsAttr()};
                    IsReadHistoryGroupBolt = false;
                });
            }
        }

        public ICommand ReadGroupBolt
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if(!IsReadHistoryGroupBolt)
                        SettingParGroupBoltsType = SettingParGroupBoltsTypeModel.NotSelectGroupBoltsTypeModel;
                    IsReadHistoryGroupBolt = true;
                });
            }
        }


        private List<SettingParGroupBoltsTypeModel> _settingParGroupBoltsTypeList = new List<SettingParGroupBoltsTypeModel>();
        public List<SettingParGroupBoltsTypeModel> SettingParGroupBoltsTypeList
        {
            get
            {
                var ReturnList = new List<SettingParGroupBoltsTypeModel>(); ;
                if (!_settingParGroupBoltsTypeList.Exists(x=>(x.groupBoltsTypeName == SettingParGroupBoltsTypeModel.NotSelectGroupBoltsTypeModel.groupBoltsTypeName)))
                {
                    ReturnList.Add(SettingParGroupBoltsTypeModel.NotSelectGroupBoltsTypeModel);
                }
                ReturnList.AddRange(_settingParGroupBoltsTypeList);
                return ReturnList;
            }
            set
            {
                _settingParGroupBoltsTypeList = value;
            }
        }



        //新增/修改/刪除
        /// <summary>
        /// 新增客製孔群
        /// </summary>

        public ICommand Add_SettingParGroupBolts
        {
            get
            {
                return new RelayCommand(() =>
                {
                    STDSerialization ser = new STDSerialization();
                    SettingParGroupBoltsType.Creation = DateTime.Now;

                    //需檢查是否有重複檔案 若有重複則提示是否改名 
 
                    var ExistedBoltsTypeList = ser.GetGroupBoltsTypeList(GroupBoltsTypeWriteTarget);

                    if (ExistedBoltsTypeList.Exists(x => x.groupBoltsTypeName == SettingParGroupBoltsType.groupBoltsTypeName))
                    {
                        var NewFileName = SettingParGroupBoltsType.groupBoltsTypeName;

                        /*     WinUIDialogWindow winuidialog = new WinUIDialogWindow("Information", MessageBoxButton.OKCancel);
                             winuidialog.Topmost = true;
                             winuidialog.Content = new TextBlock() { Text = "This is a WPF DXDialog!" };
                             winuidialog.ShowDialog();
                             Window window = new WinUIMessageBoxCreator.Default.CreateWindow();*/

                    

                        var BoxResult = WinUIMessageBox.Show(null,
                        $"({GroupBoltsTypeByTargetSelected.GetAttribute<DisplayAttribute>().Name})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}已存在\n" +
                        $"按下「Yes」會取代檔案，按下「No」則不儲存",
                        "通知",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.DefaultDesktopOnly,
                        FloatingMode.Adorner);

                        if (BoxResult == MessageBoxResult.Yes)
                        {

                        }
                        if (BoxResult == MessageBoxResult.No)
                        {
                            return;
                            //SettingParGroupBoltsType.groupBoltsTypeName = NewFileName;
                        }
                        if (BoxResult == MessageBoxResult.Cancel)
                        {
                            return;
                        }
                    }

                    ser.SetGroupBoltsTypeList(GroupBoltsTypeWriteTarget, SettingParGroupBoltsType);

                    
                    //重整孔群名稱的列表
                    WinUIMessageBox.Show(null,
                   $"({GroupBoltsTypeByTargetSelected.GetAttribute<DisplayAttribute>().Name})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}成功儲存",
                   "通知",
                   MessageBoxButton.OK,
                   MessageBoxImage.Exclamation,
                   MessageBoxResult.None,
                   MessageBoxOptions.DefaultDesktopOnly,
                    FloatingMode.Popup);
                 
                    SettingParGroupBoltsTypeList = new STDSerialization().GetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected);
                });
            }
        }



        /// <summary>
        /// 編輯客製孔群
        /// </summary>

        public ICommand Edit_SettingParGroupBolts
        {
            get
            {
                return new RelayCommand(() =>
                {
                    STDSerialization ser = new STDSerialization();
                    List<SettingParGroupBoltsTypeModel> list = ser.GetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected);
                    try
                    {
                        //刪除原始檔案
                        File.Delete(SettingParGroupBoltsType.OriginalFilePath);
                        //變更路徑到新檔案
                        var FilePath = Path.Combine(Path.GetDirectoryName(SettingParGroupBoltsType.OriginalFilePath), SettingParGroupBoltsType.groupBoltsTypeName);
                       if (string.IsNullOrEmpty(Path.GetExtension(FilePath)))
                        {
                            FilePath += ".db";
                        }
                        SettingParGroupBoltsType.OriginalFilePath = FilePath; 
                    }
                    catch(Exception ex)
                    {

                    }
                    SettingParGroupBoltsType.Revise= DateTime.Now;
                    if(ser.SetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected, SettingParGroupBoltsType))
                    {
                        WinUIMessageBox.Show(null,
                                           $"({GroupBoltsTypeByTargetSelected.GetAttribute<DisplayAttribute>().Name})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}成功儲存",
                                           "通知",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Exclamation,
                                           MessageBoxResult.None,
                                           MessageBoxOptions.DefaultDesktopOnly,
                                            FloatingMode.Window);

                    }
                    else
                    {
                        WinUIMessageBox.Show(null,
                   $"({GroupBoltsTypeByTargetSelected.GetAttribute<DisplayAttribute>().Name})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}不存在，無法編輯",
                   "通知",
                   MessageBoxButton.OK,
                   MessageBoxImage.Exclamation,
                   MessageBoxResult.None,
                   MessageBoxOptions.DefaultDesktopOnly,
                    FloatingMode.Window);
                    }
                    
                  /*  else
                    {
                        WinUIMessageBox.Show(null,
                                           $"({GroupBoltsTypeByTargetSelected.GetAttribute<DisplayAttribute>().Name})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}不存在，無法編輯",
                                           "通知",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Exclamation,
                                           MessageBoxResult.None,
                                           MessageBoxOptions.DefaultDesktopOnly,
                                            FloatingMode.Window);
                        return;
                    }*/
                });
            }
        }    
        
        /// <summary>
        /// 刪除客製孔群
        /// </summary>
        public ICommand Delete_SettingParGroupBolts
        {
            get
            {
                return new RelayCommand(() =>
                {
                    STDSerialization ser = new STDSerialization();
                    var gbtList = ser.GetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected);
                    // SettingParGroupBoltsTypeModel gbt = ser.GetGroupBoltsType(GroupBoltsTypeByTargetSelected, SettingParGroupBoltsType.groupBoltsTypeName);
                    var GBIndex = gbtList.FindIndex(x => (x.groupBoltsTypeName == SettingParGroupBoltsType.groupBoltsTypeName));
                    if (GBIndex !=-1)
                    {
                       // string dataPath = $@"{ApplicationVM.GetGroupBoltsTypeDirectory_NO_CREATE(GroupBoltsTypeByTargetSelected)}\{SettingParGroupBoltsType.FileName}";
                        string dataPath = gbtList[GBIndex].OriginalFilePath;
                        if (File.Exists(dataPath))
                        {
                            try
                            {
                                File.Delete(dataPath);
                            }
                            catch (Exception ex)
                            {
                                string message = (ex.InnerException != null ? ex.InnerException.Message : string.Empty);
                                WinUIMessageBox.Show(null,
                                     $"({GroupBoltsTypeByTargetSelected.GetAttribute<DisplayAttribute>().Name})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}刪除失敗\n 錯誤訊息:{message}",
                                     "通知",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Exclamation,
                                     MessageBoxResult.None,
                                     MessageBoxOptions.DefaultDesktopOnly,
                                      FloatingMode.Window);
                                return;
                            }

                            WinUIMessageBox.Show(null,
                                          $"({GroupBoltsTypeByTargetSelected.GetAttribute<DisplayAttribute>().Name})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}已刪除",
                                          "通知",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Exclamation,
                                          MessageBoxResult.None,
                                          MessageBoxOptions.DefaultDesktopOnly,
                                           FloatingMode.Window);

                            //刪除之後要重整孔群名稱的列表 並將名稱重新指向未選擇
                            SettingParGroupBoltsTypeList = new STDSerialization().GetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected);
                            SettingParGroupBoltsType = SettingParGroupBoltsTypeModel.NotSelectGroupBoltsTypeModel;

                            /* SettingParGroupBoltsNameList.Remove(SettingParGroupBoltsType.groupBoltsTypeName);
                             var CopyArray = SettingParGroupBoltsNameList.ToArray();
                             SettingParGroupBoltsNameList = null;
                             SettingParGroupBoltsNameList = CopyArray.ToList();*/
                            return;
                        }
                        else
                        {
                            WinUIMessageBox.Show(null,
                                       $"({GroupBoltsTypeByTargetSelected.GetAttribute<DisplayAttribute>().Name})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}不存在，無法刪除",
                                       "通知",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Exclamation,
                                       MessageBoxResult.None,
                                       MessageBoxOptions.DefaultDesktopOnly,
                                        FloatingMode.Window);
                            return;
                        }

                    }
                    else
                    {
                        WinUIMessageBox.Show(null,
                                   $"({GroupBoltsTypeByTargetSelected.GetAttribute<DisplayAttribute>().Name})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}不存在，無法刪除",
                                   "通知",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Exclamation,
                                   MessageBoxResult.None,
                                   MessageBoxOptions.DefaultDesktopOnly,
                                    FloatingMode.Window);
                        return;
                    }


                });

            }
        }





    }
}
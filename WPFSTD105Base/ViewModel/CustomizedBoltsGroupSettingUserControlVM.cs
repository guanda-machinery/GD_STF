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
            
        }

        private SettingParGroupBoltsTypeModel _settingParGroupBoltsType = new SettingParGroupBoltsTypeModel() { groupBoltsAttr = new GroupBoltsAttr() };
        public SettingParGroupBoltsTypeModel SettingParGroupBoltsType
        {
            get => _settingParGroupBoltsType;
            set { _settingParGroupBoltsType = value; OnPropertyChanged(nameof(SettingParGroupBoltsType)); }
        }

        private GroupBoltsTypeByTarget _groupBoltsTypeByTargetSelected;
        /// <summary>
        /// 客製孔群 - 依附類別(系統客製、專案客製)(下拉或核選元件)
        /// </summary>
        public GroupBoltsTypeByTarget GroupBoltsTypeByTargetSelected
        {
            get => _groupBoltsTypeByTargetSelected;
            set
            {
                _groupBoltsTypeByTargetSelected = value;

                var BNameList = new List<string>();
                foreach (var obj in SettingParGroupBoltsTypeList)
                {
                    BNameList.Add(obj.groupBoltsTypeName);
                };
                SettingParGroupBoltsNameList = BNameList;
                if (BNameList.Count > 0)
                    SelectedGroupBoltName = BNameList.First();
                else
                    SelectedGroupBoltName = null;
            }
        }



        public List<string> SettingParGroupBoltsNameList { get; set; }

        private string _selectedGroupBoltName;
        public string SelectedGroupBoltName
        {
            get
            {
                return _selectedGroupBoltName;
            }
            set
            {
                _selectedGroupBoltName = value;
                OnPropertyChanged(nameof(SelectedGroupBoltName));
                if (string.IsNullOrEmpty(_selectedGroupBoltName))
                {
                    SettingParGroupBoltsType = null;
                }
            }
        }

        public ICommand ReadSelectGroupBolt
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var index = SettingParGroupBoltsTypeList.FindIndex(x => x.groupBoltsTypeName == SelectedGroupBoltName);
                    if (index != -1)
                        SettingParGroupBoltsType = SettingParGroupBoltsTypeList[index];
                });
            }
        }


        private ObservableCollection<SettingParGroupBoltsTypeModel> SettingParGroupBoltsTypeList
        {
            get
            {
                if (GroupBoltsTypeByTargetSelected == GroupBoltsTypeByTarget.System)
                    return new STDSerialization().GetGroupBoltsTypeList(GroupBoltsTypeByTarget.System);
                else if (GroupBoltsTypeByTargetSelected == GroupBoltsTypeByTarget.Project)
                    return new STDSerialization().GetGroupBoltsTypeList(GroupBoltsTypeByTarget.Project);
                else
                    return new ObservableCollection<SettingParGroupBoltsTypeModel>();
            }
        }



        //新增/修改/刪除
        public ICommand Add_SettingParGroupBolts
        {
            get
            {
                return new RelayCommand(() =>
                {
                    STDSerialization ser = new STDSerialization();
                    SettingParGroupBoltsType.Creation = DateTime.Now;
                    ser.SetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected, SettingParGroupBoltsType);
                    SettingParGroupBoltsTypeList.Add(SettingParGroupBoltsType);

                    if (!SettingParGroupBoltsNameList.Exists(x => x == SettingParGroupBoltsType.groupBoltsTypeName))
                        SettingParGroupBoltsNameList.Add(SettingParGroupBoltsType.groupBoltsTypeName);

                    var CopyArray = SettingParGroupBoltsNameList.ToArray();
                    SettingParGroupBoltsNameList = null;
                    SettingParGroupBoltsNameList = CopyArray.ToList();
                });
            }
        }



        public ICommand Edit_SettingParGroupBolts
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EditGroupBoltsType();
                });
            }
        }
        public ICommand Delete_SettingParGroupBolts
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DeleteGroupBoltsType();
                });

            }
        }

        public void Insert()
        {
            // 讀檔

            //var a = ser.GetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected);
            //ser.SetGroupBoltsTypeList_Cus();
        }

        public void DeleteGroupBoltsType()
        {
            var Index = SettingParGroupBoltsTypeList.FindIndex(x => x.groupBoltsTypeName == SettingParGroupBoltsType.groupBoltsTypeName);
            if (Index != -1)
            {
                SettingParGroupBoltsTypeList.RemoveAt(Index);
            }
            STDSerialization ser = new STDSerialization();
            SettingParGroupBoltsTypeModel gbt = ser.GetGroupBoltsType(GroupBoltsTypeByTargetSelected, SettingParGroupBoltsType.groupBoltsTypeName);
            if (gbt != null)
            {

                string dataPath = $@"{ApplicationVM.GetGroupBoltsTypeDirectory_NO_CREATE(GroupBoltsTypeByTargetSelected)}\{SettingParGroupBoltsType.groupBoltsTypeName}.db";
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
                             $"({GroupBoltsTypeByTargetSelected})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}刪除失敗\n 錯誤訊息:{message}",
                             "通知",
                             MessageBoxButton.OK,
                             MessageBoxImage.Exclamation,
                             MessageBoxResult.None,
                             MessageBoxOptions.None,
                              FloatingMode.Window);
                        return;
                    }
                    WinUIMessageBox.Show(null,
                                  $"({GroupBoltsTypeByTargetSelected})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}已刪除",
                                  "通知",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Exclamation,
                                  MessageBoxResult.None,
                                  MessageBoxOptions.None,
                                   FloatingMode.Window);
                    SettingParGroupBoltsNameList.Remove(SettingParGroupBoltsType.groupBoltsTypeName);
                    var CopyArray = SettingParGroupBoltsNameList.ToArray();
                    SettingParGroupBoltsNameList = null;
                    SettingParGroupBoltsNameList = CopyArray.ToList();
                    return;
                }
                else
                {
                    WinUIMessageBox.Show(null,
                               $"({GroupBoltsTypeByTargetSelected})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}不存在，無法刪除",
                               "通知",
                               MessageBoxButton.OK,
                               MessageBoxImage.Exclamation,
                               MessageBoxResult.None,
                               MessageBoxOptions.None,
                                FloatingMode.Window);
                    return;
                }

            }
            else
            {
                WinUIMessageBox.Show(null,
                           $"({GroupBoltsTypeByTargetSelected})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}不存在，無法刪除",
                           "通知",
                           MessageBoxButton.OK,
                           MessageBoxImage.Exclamation,
                           MessageBoxResult.None,
                           MessageBoxOptions.None,
                            FloatingMode.Window);
                return;
            }
        }


        public void EditGroupBoltsType()
        {
            STDSerialization ser = new STDSerialization();
            ObservableCollection<SettingParGroupBoltsTypeModel> list = ser.GetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected);
            if (list.Any(x => x.groupBoltsTypeName == SettingParGroupBoltsType.groupBoltsTypeName))
            {
                SettingParGroupBoltsTypeModel gbt = list.FirstOrDefault(x => x.groupBoltsTypeName == SettingParGroupBoltsType.groupBoltsTypeName);
                gbt.groupBoltsAttr = SettingParGroupBoltsType.groupBoltsAttr;
                gbt.Revise = DateTime.Now;
                ser.SetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected, gbt);
            }
            else
            {
                WinUIMessageBox.Show(null,
                                   $"({GroupBoltsTypeByTargetSelected})孔群編號{SettingParGroupBoltsType.groupBoltsTypeName}不存在，無法編輯",
                                   "通知",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Exclamation,
                                   MessageBoxResult.None,
                                   MessageBoxOptions.None,
                                    FloatingMode.Window);
                return;
            }
        }


    }
}
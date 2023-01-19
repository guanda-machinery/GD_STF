using DevExpress.CodeParser;
using DevExpress.Data.Extensions;
using DevExpress.Utils.Extensions;
using GD_STD.Enum;
using GrapeCity.Documents.Pdf.Parser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFSTD105.Attribute;
using WPFWindowsBase;


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

        private SettingParGroupBoltsTypeModel _settingParGroupBoltsType = new SettingParGroupBoltsTypeModel();
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
               foreach(var obj in SettingParGroupBoltsTypeList)
                {
                    BNameList.Add(obj.groupBoltsTypeName);
                };
                SettingParGroupBoltsNameList = BNameList;
            }
        }



        public List<string> SettingParGroupBoltsNameList    { get; set; }

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
                var index = SettingParGroupBoltsTypeList.FindIndex(x => x.groupBoltsTypeName == value);
                if (index != -1)
                    SettingParGroupBoltsType = SettingParGroupBoltsTypeList[index];
                else
                    SettingParGroupBoltsType = null;

                OnPropertyChanged(nameof(SelectedGroupBoltName));
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
                    ser.SetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected, SettingParGroupBoltsType);
                    SettingParGroupBoltsTypeList.Add(SettingParGroupBoltsType);

                    if(!SettingParGroupBoltsNameList.Exists(x=>x == SettingParGroupBoltsType.groupBoltsTypeName))
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

                });
            }
        }
        public ICommand Delete_SettingParGroupBolts
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var Index = SettingParGroupBoltsTypeList.FindIndex(x => x.groupBoltsTypeName == SettingParGroupBoltsType.groupBoltsTypeName);
                    if (Index != -1)
                    {
                        SettingParGroupBoltsTypeList.RemoveAt(Index);
                    }
                });

            }
        }

        public void Insert() 
        {
            // 讀檔
      
            //var a = ser.GetGroupBoltsTypeList(GroupBoltsTypeByTargetSelected);
            //ser.SetGroupBoltsTypeList_Cus();
        }




    }
}

using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFSTD105.Attribute;


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
    public class SettingParGroupBoltsTypeVM : WPFWindowsBase.BaseViewModel
    {
        /// <summary>
        /// 參數設定 - 客製孔群
        /// </summary>
        public SettingParGroupBoltsTypeVM()
        {
            SettingParGroupBoltsTypeModel = new SettingParGroupBoltsTypeModel();
        }

       public SettingParGroupBoltsTypeModel SettingParGroupBoltsTypeModel { get; set; }

        /// <summary>
        /// 客製孔群 - 依附類別(系統客製、專案客製)(下拉或核選元件)
        /// </summary>
        public GroupBoltsTypeByTarget GroupBoltsTypeByTargetSelected { get; set; }




        #region 方法

        public void Insert() 
        {
            // 檢查專案中 客製孔群資料夾狀態
            ApplicationVM.CheckGroupBoltsTypeDirectory();
            SettingParGroupBoltsTypeModel gbt = new SettingParGroupBoltsTypeModel();

            // 讀檔
            STDSerialization ser = new STDSerialization();
            ObservableCollection<SettingParGroupBoltsTypeModel> collection =  ser.GetGroupBoltsTypeList(gbt.groupBoltsTypeName);
        }
        /// <summary>
        /// 客製孔群檢查
        /// </summary>
        /// <param name="groupBoltsTypeName"></param>
        /// <returns></returns>
        public bool CheckGroupBoltsTypeSetting(string groupBoltsTypeName) 
        {
            if (File.Exists(ApplicationVM.FileGroupBoltsTypeList(groupBoltsTypeName)))
            {
                return true;
            }
            else return false;
        }
        #endregion

    }
}

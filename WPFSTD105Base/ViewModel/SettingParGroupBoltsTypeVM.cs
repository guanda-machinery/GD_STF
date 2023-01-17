using DevExpress.Data.Extensions;
using DevExpress.Utils.Extensions;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Dialogs;
using DevExpress.Xpf.WindowsUI;
using GD_STD;
using GD_STD.Base;
using GD_STD.Data;
using GD_STD.Enum;
using SectionData;
using SplitLineSettingData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Listening;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;

namespace WPFSTD105.ViewModel
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
    public class SettingParGroupBoltsTypeVM : WPFBase.BaseViewModel
    {
        /// <summary>
        /// 參數設定 - 客製孔群
        /// </summary>
        public SettingParGroupBoltsTypeVM()
        {
          
        }

        #region 公開屬性
        #region GroupBoltsType 客製孔群設定
        /// <summary>
        /// 客製孔群 - 孔群編號
        /// </summary>
        public string groupBoltsTypeName { get; set; }
        /// <summary>
        /// 客製孔群 - 孔群資料
        /// Dia, StartHole, Mode, dX, dY, Face, groupBoltsType
        /// </summary>
        public GroupBoltsAttr groupBoltsAttr { get; set; }
        /// <summary>
        /// 客製孔群 - 依附類別(系統客製、專案客製)(下拉或核選元件)
        /// </summary>
        public string groupBoltsTypeByTarget { get; set; }
        /// <summary>
        /// 客製孔群 - 建立日期
        /// </summary>
        public DateTime Creation { get; }
        /// <summary>
        /// 客製孔群 - 修改日期
        /// </summary>
        public DateTime Revise { get; set; }
        #endregion
        #endregion

        #region 公開方法

        #endregion

        #region 命令

        #endregion

        #region 私有屬性

        #endregion

        

        #region 私有方法

        #endregion

        #region VM類型

        #endregion
    }
}

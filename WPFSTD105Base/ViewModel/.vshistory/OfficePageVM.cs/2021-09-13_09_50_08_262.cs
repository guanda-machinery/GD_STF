using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static WPFSTD105.ViewLocator;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase = WPFWindowsBase;
using GD_STD;

namespace WPFSTD105
{
    /// <summary>
    /// 
    /// </summary>
    public class OfficePageVM : WPFBase.BaseViewModel, IApplicationVM
    {
        /// <summary>
        /// 辦公室當前頁面
        /// </summary>
        public OfficePage CurrentPage { get; set; } = OfficePage.Home;

        /// <summary>
        /// 彈跳視窗的當前頁面
        /// </summary>
        public PopupWindows PopupCurrentPage { get; set; } = PopupWindows.FirstPage;
        /// <summary>
        /// 互聯網帳號
        /// </summary>
        public AccountNumber AccountNumber { get; set; }
        /// <summary>
        /// 專案名稱
        /// </summary>
        public string ProjectName { get; set; } = null;
        /// <summary>
        /// 專案屬性
        /// </summary>
        public ProjectProperty ProjectProperty { get; set; }
        /// <summary>
        /// 專案列表
        /// </summary>
        public ObservableCollection<string> ProjectList { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// 更新斷面規格，如果用戶沒有建立專案，所以斷面規格列表與材質列表視唯讀狀態。但她又在參數設定內開啟了新件專案或開啟新件專案就觸發此委派。幫用戶更新斷面規格列表與材質列表。
        /// </summary>
        public Action ActionLoadProfile { get; set; } = null;
        /// <inheritdoc/>
        public AxisInfo AxisInfo { get; set; }
        /// <inheritdoc/>
        public string GetNowDate { get; set; }
        /// <inheritdoc/>
        public string GetNowTime { get; set; }
        /// <inheritdoc/>
        public MachineMode UserMode { get; set; }
    }
}

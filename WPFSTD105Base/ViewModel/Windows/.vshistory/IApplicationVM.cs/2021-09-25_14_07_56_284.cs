using GD_STD;
using System;
using System.Collections.ObjectModel;

namespace WPFSTD105
{
    /// <summary>
    /// 模式共通參數
    /// </summary>
    public interface IApplicationVM : IChildWin
    {
        /// <summary>
        /// 專案名稱
        /// </summary>
        string ProjectName { get; set; }
        /// <summary>
        /// 代表物聯網帳號密碼
        /// </summary>
        AccountNumber AccountNumber { get; set; }
        /// <summary>
        /// 軸向訊息
        /// </summary>
        AxisInfo AxisInfo { get; set; }
        /// <summary>
        /// 取得現在日期
        /// </summary>
        string GetNowDate { get; set; }
        /// <summary>
        /// 取得現在時間
        /// </summary>
        string GetNowTime { get; set; }
        /// <summary>
        /// 專案屬性
        /// </summary>
        ProjectProperty ProjectProperty { get; set; }
        /// <summary>
        /// 更新斷面規格，如果用戶沒有建立專案，所以斷面規格列表與材質列表視唯讀狀態。但她又在參數設定內開啟了新件專案或開啟新件專案就觸發此委派。幫用戶更新斷面規格列表與材質列表。
        /// </summary>
        Action ActionLoadProfile { get; set; }
        /// <summary>
        /// 用戶選擇機器模式
        /// </summary>
        MachineMode UserMode { get; set; }
        /// <summary>
        /// 匯入檔案 vm
        /// </summary>
        ImportNCFilesVM ImportNCFilesVM { get; set; }
        /// <summary>
        /// 專案列表
        /// </summary>
        /// <remarks>
        /// 在 <see cref="ApplicationVM.DirectoryModel"/> 內所有的專案
        /// </remarks>
        ObservableCollection<string> ProjectList { get; set; }
        
    }
}
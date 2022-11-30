﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static WPFSTD105.ViewLocator;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase = WPFWindowsBase;
using GD_STD;
using System.Windows.Input;
using System.Windows;
using WPFSTD105.Listening;

namespace WPFSTD105
{
    /// <summary>
    /// 辦公室的 IOC
    /// </summary>
    public class OfficePageVM : WPFBase.BaseViewModel, IApplicationVM
    {
        /// <summary>
        /// Office模式的VM
        /// </summary>
        public OfficePageVM()
        {
            Close = new WPFBase.RelayCommand(() =>
             {
                 if (ChildWin != null)
                 {
                     ChildWin.Close();
                     ChildWin = null;
                 }
             });
        }
        /// <summary>
        /// 是否有斜邊，有斜邊(True)，切割線不可用
        /// </summary>
        public bool isHypotenuse { get; set; } = false;

        /// <summary>
        /// 辦公室當前頁面
        /// </summary>
        public OfficePage CurrentPage { get; set; } = OfficePage.Home;
        /// <summary>
        /// 20220711 張燕華 參數設定 - 功能列表的當前頁面
        /// </summary>
        public ParameterSettingPage ParaSettingCurrentPage { get; set; } = ParameterSettingPage.Home;
        /// <summary>
        /// 彈跳視窗的當前頁面
        /// </summary>
        public PopupWindows PopupCurrentPage { get; set; } = PopupWindows.FirstPage;
        /// <summary>
        /// 專案管理顯示控制
        /// </summary>
        public bool ProjectManagerControl { get; set; } = true;
        /// <summary>
        /// 彈跳視窗標題
        /// </summary>
        public string PopupTitle { get; set; }
        /// <summary>
        /// 彈跳視窗高度
        /// </summary>
        public int PopupHeight { get; set; }
        /// <summary>
        /// 彈跳視窗寬度
        /// </summary>
        public int PopupWidth { get; set; }
        /// <summary>
        /// 頁面切換區域高度
        /// </summary>
        public int PageHostHeight { get; set; } = 965;
        /// <summary>
        /// 頁面切換寬度
        /// </summary>
        public int PageHostWidth { get; set; } = 1685; 
        /// <summary>
        /// 工作區域寬度
        /// </summary>
        public int WorkAreaWidth { get; set; } = System.Windows.Forms.SystemInformation.WorkingArea.Width;
        /// <summary>
        /// 工作區域高度
        /// </summary>
        public int WorkAreaHeight { get; set; } = System.Windows.Forms.SystemInformation.WorkingArea.Height;
        /// <summary>
        /// 互聯網帳號
        /// </summary>
        public AccountNumber AccountNumber { get; set; } = new AccountNumber();
        /// <summary>
        /// 專案名稱
        /// </summary>
        public string ProjectName { get; set; } = null;
        /// <summary>
        /// 專案屬性
        /// </summary>
        public ProjectProperty ProjectProperty { get; set; } = new ProjectProperty();
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
        /// <inheritdoc/>
        public ImportNCFilesVM ImportNCFilesVM { get; set; }
        /// <inheritdoc/>
        public ICommand Close { get; set; }
        /// <inheritdoc/>
        public Window ChildWin { get; set; }
        /// <inheritdoc/>
        public MainAxisLocation MainAxisLocation { get; set; }
        /// <inheritdoc/>
        public MainAxisListening MainAxisListening { get; set; }
        /// <summary>
        /// 長度配料參數控制顯示
        /// </summary>
        public bool LengthDodageControl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool isPressAddCutKey { get; set; }

        /// <summary>
        /// 斷面規格最大高度
        /// </summary>
        public int SectionSpecificationMaxHeight { get; set; } = 1050;
        /// <summary>
        /// 斷面規格最小高度
        /// </summary>
        public int SectionSpecificationMinHeight { get; set; } = 150;

        /// <summary>
        /// 斷面規格最大寬度
        /// </summary>
        public int SectionSpecificationMaxWidth { get; set; } = 500;
        /// <summary>
        /// 斷面規格最小寬度
        /// </summary>
        public int SectionSpecificationMinWidth { get; set; } = 75;










    }
}

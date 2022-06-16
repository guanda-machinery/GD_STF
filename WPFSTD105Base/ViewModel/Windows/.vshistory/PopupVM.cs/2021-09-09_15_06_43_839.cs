using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFBase = WPFWindowsBase;
using static WPFSTD105.ViewLocator;

namespace WPFSTD105
{
    /// <summary>
    /// 彈跳視窗的VM
    /// </summary>
    public class PopupVM : WPFBase.WindowsViewModel
    {
        /// <summary>
        /// 視窗標題
        /// </summary>
        public string TitleText { get; set; } = "廣達國際";
        /// <summary>
        /// 視窗寬度
        /// </summary>
        public int WindowsWidth { get; set; } = 800;
        /// <summary>
        /// 視窗高度
        /// </summary>
        public int WinodwsHeight { get; set; } = 600;
        /// <summary>
        /// 匯入檔案
        /// </summary>
        public ICommand ImportFiles { get; set; }
        /// <summary>
        /// 首頁命令
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayCommand ImportFilesPage()
        {
            return new WPFBase.RelayCommand(() =>
            {
                PopupViewModel.CurrentPage = PopupWindows.ImportFiles;
                TitleText = "專案匯入檔案";
                WindowsWidth = 400;
                WinodwsHeight = 600;
            });
        }
        /// <summary>
        /// 命令對應
        /// </summary>
        public PopupVM(Window window) : base(window)
        {
            ImportFiles = ImportFilesPage();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;

namespace WPFSTD105
{
    /// <summary>
    /// OfficeBasePage ViewModel
    /// </summary>
    public class OfficeVM : WPFBase.WindowsViewModel
    {
        /// <summary>
        /// PageHost的高度
        /// </summary>
        public int CurrentHeight { get; set; }
        /// <summary>
        /// PageHost的寬度
        /// </summary>
        public int CurrentWidth { get; set; } = 1200;

        //嵐: 綁定重新綁定
        ///// <summary>
        ///// 回到首頁
        ///// </summary>
        //public ICommand ReturnHome { get; set; }
        //private WPFBase.RelayCommand HomePage()
        //{
        //    return new WPFBase.RelayCommand(() =>
        //    {
        //        OfficeViewModel.CurrentPage = OfficePage.Home;
        //    });
        //}
        /// <summary>
        /// 測試
        /// </summary>
        public ICommand TestPage { get; set; }
        private WPFBase.RelayCommand Test()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.CurrentPage = OfficePage.Test;
            });
        }
        /// <summary>
        /// 製品設定命令
        /// </summary>
        public ICommand ObSettingsPage_Office { get; set; }
        private WPFBase.RelayCommand ObSettings_Office()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.CurrentPage = OfficePage.ObSettings;
            });
        }
        /// <summary>
        /// 命令對應
        /// </summary>
        public OfficeVM(Window window) : base(window)
        {
            //ReturnHome = HomePage();
            TestPage = Test();
            ObSettingsPage_Office = ObSettings_Office();
        }
    }
}

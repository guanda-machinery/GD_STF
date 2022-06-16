using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFWindowsBase;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;
namespace WPFSTD105
{
    /// <summary>
    /// OfficeBasePage ViewModel
    /// </summary>
    public class OfficeVM : STDBaseWindowView, IChildWin
    {
        /// <summary>
        /// PageHost的高度
        /// </summary>
        public int CurrentHeight { get; set; }
        /// <summary>
        /// PageHost的寬度
        /// </summary>
        public int CurrentWidth { get; set; } = 1200;
        /// <summary>
        /// 命令對應
        /// </summary>
        public OfficeVM(Window window) : base(window)
        {
            TestPage = Test();
            ObSettingsPage_Office = ObSettings_Office();
        }

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
        /// <inheritdoc/>
        protected override RelayParameterizedCommand OutProjectName()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                bool result = ApplicationVM.CreateModel((FolderBrowserDialogViewModel)e); //創建模型
                if (result)
                {
                    CommonViewModel.ImportNCFilesVM = new ImportNCFilesVM();
                    MessageBox.Show("新建成功", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                }
            });
        }
        /// <inheritdoc/>
        protected override RelayParameterizedCommand SaveAs()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                bool result = ApplicationVM.SaveAs((FolderBrowserDialogViewModel)el); //另存模型
                if (result)
                {
                    ChildClose.Execute(null);//關閉子視窗
                    CommonViewModel.ImportNCFilesVM = new ImportNCFilesVM();
                }
            });
        }
        /// <inheritdoc/>
        protected override RelayParameterizedCommand OpenProject()
        {
            return new RelayParameterizedCommand(el => 
            {
                base.OpenProject().Execute(el);
                MessageBox.Show("載入成功", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
            });
        }

        /// <summary>
        /// 關閉子視窗
        /// </summary>
        public ICommand ChildClose { get; set; }

    }
}

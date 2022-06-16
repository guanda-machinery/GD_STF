using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
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
            ProjectNameCommand = ProjectName();
            //PropertySettingsCommand = PropertySettings();
        }

        /// <summary>
        /// 開啟專案
        /// </summary>
        public ICommand ProjectNameCommand { get; set; }
        private WPFBase.RelayCommand ProjectName()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.PopupCurrentPage = PopupWindows.ProjectsManager;
                //PopupWindowsBase popupWindowsBase = new PopupWindowsBase(DataContext);
            });
        }
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
                string path = ((FolderBrowserDialogViewModel)e).ResultPath;
                bool result = ApplicationVM.CreateModel(path); //創建模型
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
                FolderBrowserDialogService service = DevExpand.NewFolder("請選擇NC放置的資料夾");
                IFolderBrowserDialogService folder = service;
                if (folder.ShowDialog()) //有選擇路徑
                {
                    bool result = ApplicationVM.SaveAs(folder.ResultPath); //另存模型
                    if (result)
                    {
                        ChildClose.Execute(null);//關閉子視窗
                        CommonViewModel.ImportNCFilesVM = new ImportNCFilesVM();
                        MessageBox.Show("另存成功", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    }
                }
            });
        }
        /// <inheritdoc/>
        protected override RelayParameterizedCommand OpenProject()
        {
            return new RelayParameterizedCommand(el =>
            {
                base.OpenProject().Execute(el);
                CommonViewModel.ImportNCFilesVM = new ImportNCFilesVM();
                MessageBox.Show("載入成功", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
            });
        }

        /// <summary>
        /// 關閉子視窗
        /// </summary>
        public ICommand ChildClose { get; set; }

    }
}

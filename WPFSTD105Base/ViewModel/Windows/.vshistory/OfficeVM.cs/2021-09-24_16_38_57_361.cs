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
using GD_STD;
using System.Collections.ObjectModel;

namespace WPFSTD105
{
    /// <summary>
    /// OfficeBasePage ViewModel
    /// </summary>
    public class OfficeVM : AbsBaseWindowView, IChildWin
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
        /// 控制專案管理的顯示
        /// </summary>
        public bool ProjectManagerControl { get; set; } = true;
        /// <summary>
        /// 視窗命令對應
        /// </summary>
        public OfficeVM(Window window) : base(window)
        {
            ObSettingsPage_Office = ObSettings_Office();
            ProjectManagerCommand = ProjectManager();
            ParameterSettingsCommand = ParameterSettings();
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
        /// 開啟專案管理
        /// </summary>
        public ICommand ProjectManagerCommand { get; set; }
        private WPFBase.RelayCommand ProjectManager()
        {
            return new WPFBase.RelayCommand(() =>
            {
                CommonViewModel.ProjectList = new ObservableCollection<string>(ApplicationVM.GetModelDirectory(WPFSTD105.Properties.SofSetting.Default.LoadPath));
                if (OfficeViewModel.ProjectManagerControl)
                    OfficeViewModel.ProjectManagerControl = false;
                else
                    OfficeViewModel.ProjectManagerControl = true;
            });
        }

        /// <summary>
        /// 開啟參數設定
        /// </summary>
        public ICommand ParameterSettingsCommand { get; set; }
        private WPFBase.RelayCommand ParameterSettings()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.CurrentPage = OfficePage.ParameterSettings;
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
                        Close.Execute(null);//關閉子視窗
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
        /// 關閉專案屬性命令
        /// </summary>
        public ICommand CloseProjectManagerCommand { get; set; } = new WPFBase.RelayCommand(() => { OfficeViewModel.ProjectManagerControl = true; });
        /// <summary>
        /// 關閉子視窗
        /// </summary>
        public ICommand Close { get; set; }
    }
}

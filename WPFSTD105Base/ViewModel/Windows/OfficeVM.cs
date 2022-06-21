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
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;

namespace WPFSTD105
{
    /// <summary>
    /// OfficeBasePage ViewModelz
    /// </summary>
    public class OfficeVM : AbsBaseWindowView
    {
        /// <summary>
        /// 輸出客製化
        /// </summary>
        public bool Customizable { get; set; }
        /// <summary>
        /// 視窗命令對應
        /// </summary>
        public OfficeVM(Window window) : base(window)
        {
            ObSettingsPage_Office = ObSettings_Office();
            ProjectManagerCommand = ProjectManager();
            ParameterSettingsCommand = ParameterSettings();
            AutoTypeSettingsCommand = AutoTypeSettings();
            ProcessingMonitorCommand = ProcessingMonitor();
            PageHostMaximizedCommand = PageHostMaximized();
            DragMoveWindowCommand = DragMoveWindow();
            NextPageCommand = NextPage();
            PreviousPageCommand = PreviousPage();
            WorkingAreaMonitorCommand = WorkingAreaMonitor();
        }
        /// <summary>
        /// 更改客製化狀態
        /// </summary>
        public ICommand LayoutCustomizableCommand { get; set; }
        private WPFBase.RelayCommand LayoutCustomizable()
        {
            return new WPFBase.RelayCommand(() =>
            {
                Customizable = Customizable ? false : true;
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
        /// 開啟專案管理
        /// </summary>
        public ICommand ProjectManagerCommand { get; set; }
        private WPFBase.RelayCommand ProjectManager()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.CurrentPage = OfficePage.Home; // 開啟專案管理時先回到空白頁
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

        /// <summary>
        /// 排版設定(自動)
        /// </summary>
        public ICommand AutoTypeSettingsCommand { get; set; }
        private RelayCommand AutoTypeSettings()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.CurrentPage = OfficePage.AutoTypeSettings;
            });
        }

        /// <summary>
        /// 加工監控
        /// </summary>
        public ICommand ProcessingMonitorCommand { get; set; }
        private RelayCommand ProcessingMonitor()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.CurrentPage = OfficePage.ProcessingMonitor;
            });
        }
        /// <summary>
        /// 廠區監控
        /// </summary>
        public ICommand WorkingAreaMonitorCommand { get; set; }
        private RelayCommand WorkingAreaMonitor()
        {
            return new RelayCommand(() =>
            {
                OfficeViewModel.CurrentPage = OfficePage.WorkingAreaMonitor;
            });
        }
        /// <summary>
        /// 下一頁
        /// </summary>
        public ICommand NextPageCommand { get; set; }
        private RelayCommand NextPage()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficePage page = OfficeViewModel.CurrentPage;
                int count = System.Enum.GetNames(typeof(OfficePage)).Length;
                int index = (int)page;
                index++;
                if (index >= count)
                {
                    index = 0;
                }

                if (string.IsNullOrEmpty(OfficeViewModel.ProjectName))
                    return;
                else
                    OfficeViewModel.CurrentPage = (OfficePage)index;
            });
        }

        /// <summary>
        /// 上一頁
        /// </summary>
        public ICommand PreviousPageCommand { get; set; }
        private RelayCommand PreviousPage()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficePage page = OfficeViewModel.CurrentPage;
                int count = System.Enum.GetNames(typeof(OfficePage)).Length;
                int index = (int)page;
                index--;
                if (index < 0)
                {
                    index = count - 1;
                }

                if (string.IsNullOrEmpty(OfficeViewModel.ProjectName))
                    return;
                else
                    OfficeViewModel.CurrentPage = (OfficePage)index;
            });
        }

        /// <summary>
        /// 視窗拖曳
        /// </summary>
        public ICommand DragMoveWindowCommand { get; set; }
        private RelayParameterizedCommand DragMoveWindow()
        {
            return new WPFBase.RelayParameterizedCommand((e) =>
            {
                if (string.IsNullOrWhiteSpace(e.ToString()))
                {
                    return;
                }
                Window window = e as Window;
                if (window != null)
                {
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                    {
                        window.DragMove();
                    }
                    /*
                    if (window.WindowState == WindowState.Maximized)
                    {
                        window.WindowState = WindowState.Normal;
                    }*/
                }
            });
        }

        /// <summary>
        /// 視窗放大
        /// </summary>
        public ICommand PageHostMaximizedCommand { get; set; }
        private RelayCommand PageHostMaximized()
        {
            return new WPFBase.RelayCommand(() =>
            {
                /*
                int oldHeight = (int)_Window.Height;
                int oldWidth = (int)_Window.Width;
                _Window.WindowState ^= WindowState.Maximized;
                int heightGap = (int)_Window.Height - oldHeight;
                int widthGap = (int)_Window.Width - oldWidth;
                OfficeViewModel.WorkAreaHeight += heightGap;
                OfficeViewModel.WorkAreaWidth += widthGap;
                */
                _Window.WindowState ^= WindowState.Maximized;
                OfficeViewModel.WorkAreaHeight = System.Windows.Forms.SystemInformation.WorkingArea.Height - 20;
                OfficeViewModel.WorkAreaWidth = System.Windows.Forms.SystemInformation.WorkingArea.Width - 220;
            });
        }

        /// <inheritdoc/>
        protected override RelayParameterizedCommand OutProjectName()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                // 2020.06.21  呂宗霖 路徑調整抓Properties
                //string path = ((FolderBrowserDialogViewModel)e).ResultPath;
                string path = Properties.SofSetting.Default.LoadPath;
                bool result = ApplicationVM.CreateModel(path); //創建模型
                if (result)
                {
                    CommonViewModel.ImportNCFilesVM = new ImportNCFilesVM();
                    //MessageBox.Show("新建成功", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    WinUIMessageBox.Show(null,
                    $"新建成功",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
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
                        CommonViewModel.ImportNCFilesVM = new ImportNCFilesVM();
                        //MessageBox.Show("另存成功", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                        WinUIMessageBox.Show(null,
                            $"另存成功",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                            FloatingMode.Popup);
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
                string _ = ApplicationVM.FileProjectProperty();
                if (_ == null)
                {
                    return;
                }
                CommonViewModel.ImportNCFilesVM = new ImportNCFilesVM();
                //MessageBox.Show("載入成功", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"載入成功",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
            });
        }
        /// <summary>
        /// 關閉專案屬性命令
        /// </summary>
        public ICommand CloseProjectManagerCommand { get; set; } = new WPFBase.RelayCommand(() =>
        {
            if (OfficeViewModel.ProjectManagerControl)
                OfficeViewModel.ProjectManagerControl = false;
            else
                OfficeViewModel.ProjectManagerControl = true;
        });
        ///// <summary>
        ///// 關閉子視窗
        ///// </summary>
        //public ICommand Close { get; set; }
    }
}

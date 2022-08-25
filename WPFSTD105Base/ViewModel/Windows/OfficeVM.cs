using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPFWindowsBase;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;
using GD_STD.Enum;
using System.IO;

namespace WPFSTD105
{
    /// <summary>
    /// OfficeBasePage ViewModelz
    /// </summary>
    public class OfficeVM : AbsBaseWindowView
    {
        /// <summary>
        /// 新建路徑
        /// </summary>
        public string AddPath { get; set; }
        /// <summary>
        /// 瀏覽路徑
        /// </summary>
        public string SearchPath { get; set; }
        /// <summary>
        /// 輸出客製化
        /// </summary>
        public bool Customizable { get; set; }
        /// <summary>
        /// 視窗命令對應
        /// </summary>
        public OfficeVM(Window window) : base(window)
        {
            Old_ObSettingsPage_Office = Old_ObSettings_Office(); //舊版製品設定 20220824 張燕華
            ObSettingsPage_Office = ObSettings_Office();
            ProjectManagerCommand = ProjectManager();
            //ParameterSettingsCommand = ParameterSettings(); //20220711 張燕華 由顯示參數設定頁面改為顯示參數設定功能選單
            ParameterSettingsFuncListCommand = ParameterSettingsFuncList(); //20220711 張燕華 由顯示參數設定頁面改為顯示參數設定功能選單
            AutoTypeSettingsCommand = AutoTypeSettings();
            ProcessingMonitorCommand = ProcessingMonitor();
            PageHostMaximizedCommand = PageHostMaximized();
            DragMoveWindowCommand = DragMoveWindow();
            NextPageCommand = NextPage();
            PreviousPageCommand = PreviousPage();
            WorkingAreaMonitorCommand = WorkingAreaMonitor();

            OpenProjectPathCommand = OpenProjectPath();
            OutProjectPathCommand = OutProjectPath();
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
        public ICommand Old_ObSettingsPage_Office { get; set; } 
        private WPFBase.RelayCommand Old_ObSettings_Office() 
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.CurrentPage = OfficePage.old_ObSettings;
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
        /// 20220711 張燕華 開啟參數設定 - 功能列表 頁面
        /// </summary>
        public ICommand ParameterSettingsFuncListCommand { get; set; }
        private WPFBase.RelayCommand ParameterSettingsFuncList()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.CurrentPage = OfficePage.ParameterSettings_FuncList;
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
        /// <summary>
        /// 新建專案 存路徑
        /// </summary>
        /// <returns></returns>
        protected override WPFWindowsBase.RelayCommand OutProjectPath()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                FolderBrowserDialogService service = DevExpand.NewFolder("請選擇專案放置的資料夾");
                IFolderBrowserDialogService folder = service;
                folder.ShowDialog();//Show 視窗
                AddPath = folder.ResultPath;//選擇的路徑
                //不異動Properties.SofSetting.Default.LoadPath，因為修改時，需先在專案瀏覽選到正確的專案
            });
        }
        /// <summary>
        /// 開啟專案 存路徑
        /// </summary>
        /// <returns></returns>
        protected override WPFWindowsBase.RelayCommand OpenProjectPath()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                FolderBrowserDialogService service = DevExpand.NewFolder("請選擇專案放置的資料夾");
                IFolderBrowserDialogService folder = service;
                folder.ShowDialog();//Show 視窗 
                SearchPath = folder.ResultPath;//選擇的路徑
                //修改全域路徑變數 供載入用
                Properties.SofSetting.Default.LoadPath = SearchPath;
                Properties.SofSetting.Default.Save();
            });
        }
        /// <inheritdoc/>
        protected override RelayParameterizedCommand OutProjectName()
        {
            return new WPFBase.RelayParameterizedCommand(e=>
            {
                // 2020.06.21  呂宗霖 路徑調整抓 AddPath 因為選擇資料夾時已異動Properties.SofSetting.Default.LoadPath
                string path = this.AddPath;
                //string path = Properties.SofSetting.Default.LoadPath;
                //string path2 =  tbx_ProjectPath.Text;
                //path = string.IsNullOrEmpty("") ? path : "";
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
                // 2022.06.23 呂宗霖 新增Null判斷 避免所選路徑無專案
                if (el == null)
                {
                    return;
                }
                base.OpenProject().Execute(el);
                string _ = ApplicationVM.FileProjectProperty();
                if (_ == null)
                {
                    return;
                }
                CommonViewModel.ImportNCFilesVM = new ImportNCFilesVM();

                // 2022/08/22 呂宗霖 若有缺少斷面規格，自動產生
                foreach (OBJETC_TYPE format in System.Enum.GetValues(typeof(OBJETC_TYPE)))
                {
                    if (format != OBJETC_TYPE.Unknown && format != OBJETC_TYPE.PLATE)
                    {
                        if (!File.Exists($@"{ApplicationVM.DirectoryPorfile()}\{format.ToString()}.inp"))
                        {
                            File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + $@"Profile\\{format}.inp", $@"{ApplicationVM.DirectoryPorfile()}\{format.ToString()}.inp");//複製 BH 斷面規格到模型內
                        }
                    }
                }

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

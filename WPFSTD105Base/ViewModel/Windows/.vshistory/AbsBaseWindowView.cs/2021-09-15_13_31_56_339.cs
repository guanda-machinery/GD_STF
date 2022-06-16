using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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
    /// STD Win 抽象
    /// </summary>
    public abstract class STDBaseWindowView : WPFBase.WindowsViewModel, IWindowsVM
    {
        /// <summary>
        /// 判斷是否切換語言
        /// </summary>
        public bool LanguageFlag = false;
        /// <summary>
        /// <see cref="SelectProject"/> 
        /// </summary>
        protected int _SelectProject = -1;
        /// <summary>
        /// 選擇 <see cref="ApplicationVM.ProjectList"/> 的位置
        /// </summary>
        public int SelectProject
        {
            get => _SelectProject;
            set
            {
                _SelectProject = value;
                if (value != -1)
                {
                    STDSerialization ser = new STDSerialization(); //序列化處理器
                    string path = ApplicationVM.FileProjectProperty($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectList[value]}"); //指定要反序列的路徑
                    CommonViewModel.ProjectProperty = ser.GetProjectProperty(path);//反序列化模型屬性
                }
            }
        }
        /// <summary>
        /// 命令對應
        /// </summary>
        public STDBaseWindowView(Window window) : base(window)
        {
            HomeCommand = HomePage();
            ImportFileCommand = ImportFile();
            ModifyProjectInfoCommand = ModifyProjectInfo();
            OpenProjectCommand = OpenProject();
            OutProjectNameCommand = OutProjectName();
            SaveAsCommand = SaveAs();
            WatchProjectCommand = WatchProject();
            LanguageCommand = Language();
            NewProjectShowCommand = NewProjectShow();
        }
        
        /// <inheritdoc/>
        public ICommand HomeCommand { get; set; }
        private WPFBase.RelayCommand HomePage()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (!WPFSTD105.Properties.SofSetting.Default.OfficeMode)//如果是工廠模式
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
                else
                {
                    OfficeViewModel.CurrentPage = OfficePage.Home;
                }
            });
        }
        /// <inheritdoc/>
        public ICommand ImportFileCommand { get; set; }
        /// <summary>
        /// 匯入檔案功能
        /// </summary>
        /// <returns></returns>
        protected virtual WPFBase.RelayCommand ImportFile()
        {
            return new WPFBase.RelayCommand(() =>
            {
                CommonViewModel.ImportNCFilesVM = new ImportNCFilesVM();
            });
        }
        /// <inheritdoc/>
        public ICommand ModifyProjectInfoCommand { get; set; }
        /// <summary>
        /// 修改專案屬性功能
        /// </summary>
        /// <returns></returns>
        protected virtual WPFBase.RelayCommand ModifyProjectInfo()
        {
            return new WPFBase.RelayCommand(() =>
            {
                STDSerialization ser = new STDSerialization();
                CommonViewModel.ProjectProperty.Revise = DateTime.Now;
                ser.SetProjectProperty(CommonViewModel.ProjectProperty);
            });
        }
        /// <inheritdoc/>
        public ICommand OpenProjectCommand { get; set; }
        /// <summary>
        /// 開啟專案功能
        /// </summary>
        /// <returns></returns>
        protected virtual WPFBase.RelayParameterizedCommand OpenProject()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                if (e == null)
                {
                    MessageBox.Show("請選擇專案", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    return;
                }

                string _ = e.ToString();
                CommonViewModel.ProjectName = _;
                if (CommonViewModel.ActionLoadProfile != null) //如果載入專案實有發現載入斷面規格與材質
                {
                    CommonViewModel.ActionLoadProfile.Invoke(); //觸發委派
                    CommonViewModel.ActionLoadProfile = null;//清除委派
                }
            });
        }
        /// <inheritdoc/>
        public ICommand OutProjectNameCommand { get; set; }
        /// <summary>
        /// 建立專案功能
        /// </summary>
        /// <returns></returns>
        protected abstract  WPFBase.RelayParameterizedCommand OutProjectName();
        /// <inheritdoc/>
        public ICommand SaveAsCommand { get; set; }
        /// <summary>
        /// 另存專案功能
        /// </summary>
        /// <returns></returns>
        protected abstract WPFBase.RelayParameterizedCommand SaveAs();
        ///// <inheritdoc/>
        //public ICommand SettingParCommand { get; set; }
        ///// <inheritdoc/>
        //public ICommand SoftwareSettingsCommand { get; set; }
        ///// <inheritdoc/>
        //public ICommand TypeSettingCommand { get; set; }
        /// <inheritdoc/>
        public ICommand WatchProjectCommand { get; set; }
        /// <summary>
        /// 專案瀏覽功能
        /// </summary>
        /// <returns></returns>
        protected virtual WPFBase.RelayCommand WatchProject()
        {
            return new WPFBase.RelayCommand(() => 
            {
                CommonViewModel.ProjectList = new ObservableCollection<string>(ApplicationVM.GetModelDirectory(Properties.SofSetting.Default.LoadPath));
            });
        }

        /// <inheritdoc/>
        public ICommand LanguageCommand { get; set; }
        /// <summary>
        /// 語言功能
        /// </summary>
        /// <returns></returns>
        private WPFBase.RelayCommand Language()
        {
            return new WPFBase.RelayCommand(() =>
            {
                // Application.Current.Resources.MergedDictionaries[0] 0為App.xaml裡面TW.xaml的序列位置
                int language = Properties.SofSetting.Default.Language;
                string languageName = "/LanguagePackages/EN.xaml";
                CultureInfo currentCultureInfo = CultureInfo.CurrentCulture;
                ResourceDictionary langRd = null;
                switch (language)
                {
                    case 0:
                        languageName = "/LanguagePackages/EN.xaml";
                        break;
                    case 1:
                        languageName = "/LanguagePackages/TH.xaml";
                        break;
                    case 2:
                        languageName = "/LanguagePackages/VN.xaml";
                        break;
                }
                try
                {
                    langRd = Application.LoadComponent(
                    new Uri(@languageName, UriKind.Relative)) as ResourceDictionary;
                }
                catch
                {
                }

                if (!LanguageFlag)
                {
                    if (langRd != null)
                    {

                    }
                    Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
                    {
                        Source = new Uri(@languageName, UriKind.Relative)
                    };
                    LanguageFlag = true;
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
                    {
                        Source = new Uri(@"/LanguagePackages/TW.xaml", UriKind.Relative)
                    };
                    LanguageFlag = false;
                }
            });
        }
        /// <inheritdoc/>
        public ICommand NewProjectShowCommand { get; set; }
        /// <summary>
        /// 顯示其他物件
        /// </summary>
        /// <returns></returns>
        protected virtual WPFBase.RelayCommand NewProjectShow()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ApplicationViewModel.ProjectProperty = new ProjectProperty();
            });
        }
    }
}

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
    /// STD Win 抽象
    /// </summary>
    public abstract class STDBaseWindowView : WPFBase.WindowsViewModel, IWindowsVM
    {
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
                ApplicationViewModel.ProjectProperty.Revise = DateTime.Now;
                ser.SetProjectProperty(ApplicationViewModel.ProjectProperty);
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
        protected abstract WPFBase.RelayParameterizedCommand OutProjectName();
        /// <inheritdoc/>
        public ICommand SaveAsCommand { get; set; }

        /// <inheritdoc/>
        public ICommand SettingParCommand { get; set; }
        /// <inheritdoc/>
        public ICommand SoftwareSettingsCommand { get; set; }
        /// <inheritdoc/>
        public ICommand TypeSettingCommand { get; set; }
        /// <inheritdoc/>
        public ICommand WatchProjectCommand { get; set; }
    }
}

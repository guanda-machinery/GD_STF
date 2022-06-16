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
        protected abstract WPFBase.RelayCommand ImportFile();

        /// <inheritdoc/>
        public ICommand ModifyProjectCommand { get; set; }

        /// <inheritdoc/>
        public ICommand ModifyProjectInfoCommand { get; set; }
        /// <inheritdoc/>
        public ICommand OpenProjectCommand { get; set; }
        /// <inheritdoc/>
        public ICommand OutProjectNameCommand { get; set; }
        /// <inheritdoc/>
        public ICommand SaveAsProjectCommand { get; set; }
        /// <inheritdoc/> 
        public bool SaveAsProjectControl { get; set; }
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

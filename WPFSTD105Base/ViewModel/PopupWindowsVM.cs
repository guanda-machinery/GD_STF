using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBase = WPFWindowsBase;
using System.Collections.ObjectModel;
using static WPFSTD105.ViewLocator;

namespace WPFSTD105
{
    /// <summary>
    /// 彈跳視窗的VM
    /// </summary>
    public class PopupWindowsVM : WPFBase.BaseViewModel
    {
        /// <summary>
        /// 命令對應
        /// </summary>
        public PopupWindowsVM()
        {
            BOMSettingsCommand = BOMSettings();
            ProjectManagerCommand = ProjectManager();
        }

        /// <summary>
        /// 開啟屬性設定
        /// </summary>
        public ICommand BOMSettingsCommand { get; set; }
        private WPFBase.RelayCommand BOMSettings()
        {
            return new WPFBase.RelayCommand(() =>
            {
                OfficeViewModel.PopupCurrentPage = PopupWindows.BOMSettings;
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
                OfficeViewModel.PopupCurrentPage = PopupWindows.ProjectsManager;
            });
        }
    }
}

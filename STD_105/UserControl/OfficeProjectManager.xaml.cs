using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using WPFSTD105;
using WPFSTD105.Properties;
using static WPFSTD105.ViewLocator;

namespace STD_105
{
    /// <summary>
    /// OfficeProjectManager.xaml 的互動邏輯
    /// </summary>
    public partial class OfficeProjectManager : UserControl
    {
        public OfficeProjectManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 2022/06/22 呂宗霖 點擊下拉選單時重新抓取路徑下之專案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string path = tbx_ProjectSearchPath.Text;
            SofSetting.Default.LoadPath = path;
            SofSetting.Default.Save();

            if (string.IsNullOrEmpty(path)) 
            {
                WinUIMessageBox.Show(null,
                        $"請選擇專案路徑",
                        "操作提示",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.DefaultDesktopOnly,
                        FloatingMode.Popup);
                //MessageBox.Show("請選擇專案", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                return;
            }

            //var pl = new ObservableCollection<string>(ApplicationVM.GetModelDirectory(path));
            CommonViewModel.ProjectList = new ObservableCollection<string>(ApplicationVM.GetModelDirectory(path));
        }
    }
}

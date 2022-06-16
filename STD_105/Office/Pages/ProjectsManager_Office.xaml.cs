using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFWindowsBase;
using WPFSTD105;
using static WPFSTD105.ViewLocator;
using DevExpress.Xpf.Core;
using System.Threading;

namespace STD_105.Office
{
    /// <summary>
    /// ProjectsManager_Office.xaml 的互動邏輯
    /// </summary>
    public partial class ProjectsManager_Office : BasePage
    {
        public ProjectsManager_Office()
        {
            InitializeComponent();
        }
        private void TabItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //CommonViewModel.ProjectProperty = new ProjectProperty();
        }

        private static readonly SplashScreenManager manager = SplashScreenManager.CreateWaitIndicator();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            manager.ViewModel.Status = "準備匯入檔案";
            manager.Show(null, WindowStartupLocation.CenterScreen, true, InputBlockMode.Window);
            Thread.Sleep(500);
            for (int i = 0; i < 100; i++)
            {
                if (i < 50)
                {
                    Thread.Sleep(100);
                    manager.ViewModel.Status = "匯出進度：" + i + "%";
                    i++;
                }
                else
                {
                    Thread.Sleep(50);
                    manager.ViewModel.Status = "匯出進度：" + i + "%";
                    i++;
                }   
            }
            manager.ViewModel.Status = "匯出成功";
            manager.Close();
        }
    }
}

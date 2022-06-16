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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using WPFSTD105;
using WPFSTD105.ViewModel;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using System.Threading;

namespace STD_105.Office
{
    /// <summary>
    /// OfficeWindowsBase.xaml 的互動邏輯
    /// </summary>
    public partial class OfficeWindowsBase : Window
    {
        private static SplashScreenManager manager = SplashScreenManager.Create(() => new WaitIndicator(), new DXSplashScreenViewModel { });
        public OfficeWindowsBase()
        {
            InitializeComponent();
            DataContext = new OfficeVM(this);
        }

        public static void ActivateLoading()
        {
            manager.ViewModel.Status = "讀取中";
            manager.Show(null, WindowStartupLocation.CenterScreen, true, InputBlockMode.Window);
        }

        public static void DeactivateLoading()
        {
            manager.ViewModel.Status = "讀取完成";
            Thread.Sleep(500);
            manager.Close();
        }
    }
}

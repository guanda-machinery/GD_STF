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
using System.Windows.Media.Animation;
using WPFSTD105;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using static WPFSTD105.ViewLocator;
using System.Collections.ObjectModel;
using System.Threading;

namespace STD_105.Office
{
    /// <summary>
    /// OfficeBaseWindow.xaml 的互動邏輯
    /// </summary>
    public partial class OfficeBaseWindow : Window
    {
        public delegate void EventHandle(bool isShow);
        public event EventHandle ShowClickEvent;
        private static SplashScreenManager manager = SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });
        public OfficeBaseWindow()
        {
            InitializeComponent();
            DataContext = new OfficeVM(this);
        }

        private void ControlHeaderDisplay(object sender, RoutedEventArgs e)
        {
            if (grid_TopMenu.Tag.ToString() == "Max")
            {
                grid_TopMenu.Tag = "Min";
                Storyboard stbShow = (Storyboard)FindResource("stbMin");
                OfficeViewModel.PageHostHeight += 180; // grid_TopMenu.Height
                stbShow.Begin();
                ShowClickEvent?.Invoke(true);
            }
            else
            {
                grid_TopMenu.Tag = "Max";
                Storyboard stbShow = (Storyboard)FindResource("stbMax");
                OfficeViewModel.PageHostHeight -= 180;
                stbShow.Begin();
                ShowClickEvent?.Invoke(true);
            }
        }

        public static void ActivateLoading()
        {
            manager.ViewModel.Status = "讀取中.";
            manager.Show(null, WindowStartupLocation.CenterScreen, true, InputBlockMode.Window);
        }

        public static void DeactivateLoading()
        {
            manager.Close();
        }

        private void CallPopupWindows(object sender, RoutedEventArgs e)
        {
            CommonViewModel.ChildWin = new PopupWindowsBase();
            OfficeViewModel.PopupCurrentPage = PopupWindows.BOMSettings;
            OfficeViewModel.PopupTitle = "屬性設定";
            OfficeViewModel.PopupWidth = 300;
            OfficeViewModel.PopupHeight = 400;
            CommonViewModel.ChildWin.ShowDialog();
        }
    }
}

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
using DevExpress.Xpf.Core;
using static WPFSTD105.ViewLocator;

namespace STD_105.Office
{
    /// <summary>
    /// OfficeBaseWindow.xaml 的互動邏輯
    /// </summary>
    public partial class OfficeBaseWindow : Window
    {
        public delegate void EventHandle(bool isShow);
        public event EventHandle ShowClickEvent;
        private static readonly SplashScreenManager manager = SplashScreenManager.CreateWaitIndicator();

        public OfficeBaseWindow()
        {
            InitializeComponent();
            CommonViewModel.ProjectProperty = new ProjectProperty();
            DataContext = new OfficeVM(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (grid_TopMenu.Tag.ToString() == "Max")
            {
                grid_TopMenu.Tag = "Min";
                Storyboard stbShow = (Storyboard)FindResource("stbMin");
                stbShow.Begin();
                ShowClickEvent?.Invoke(true);
            }
            else
            {
                grid_TopMenu.Tag = "Max";
                Storyboard stbShow = (Storyboard)FindResource("stbMax");
                stbShow.Begin();
                ShowClickEvent?.Invoke(true);
            }
        }

        private void grid_TopMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public static void ActivateLoading()
        {
            manager.ViewModel.Status = "讀取中";
            manager.Show(null, WindowStartupLocation.CenterScreen, true, InputBlockMode.Window);
        }

        public static void DeactivateLoading()
        {
            manager.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OfficeVM officeVM = (OfficeVM)DataContext; 
            PopupWindowsBase popupWindowsBase = new PopupWindowsBase(officeVM);
            officeVM.ChildClose = new WPFWindowsBase.RelayCommand(() => 
            {
                popupWindowsBase.Close();
            });
            popupWindowsBase.Content = new ProjectsManager_Office();
            popupWindowsBase.Title = "專案管理";
            popupWindowsBase.Width = 664;
            popupWindowsBase.Height = 500;
            popupWindowsBase.ShowDialog();
            officeVM.ChildClose = null;
        }
    }
}

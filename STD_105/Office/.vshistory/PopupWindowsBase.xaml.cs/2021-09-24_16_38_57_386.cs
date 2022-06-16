using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WPFSTD105;
using static WPFSTD105.ViewLocator;
namespace STD_105.Office
{
    /// <summary>
    /// PopupWindowsBase.xaml 的互動邏輯
    /// </summary>
    public partial class PopupWindowsBase : Window
    {
        //public delegate void EventHandle(bool isShow);
        //public event EventHandle ShowClickEvent;
        private static readonly SplashScreenManager manager = SplashScreenManager.CreateWaitIndicator();

        public PopupWindowsBase()
        {
            InitializeComponent();
            OfficeVM vm = new OfficeVM(this);
            vm.Close = new WPFWindowsBase.RelayCommand(() => 
            {
                this.Close();
                OfficeViewModel.ProjectManagerControl = true;
            });
            DataContext = vm;
        }
        public PopupWindowsBase(object vm)
        {
            InitializeComponent();
            IChildWin childWin = (IChildWin)vm;
            CommonViewModel.ProjectList = new ObservableCollection<string>(ApplicationVM.GetModelDirectory(WPFSTD105.Properties.SofSetting.Default.LoadPath));
            childWin.Close = new WPFWindowsBase.RelayCommand(() => 
            {
                this.Close();
            });
            DataContext = vm;
        }
        /*
        public static void ActivateLoading()
        {
            manager.ViewModel.Status = "讀取頁面中";
            manager.Show(null, WindowStartupLocation.CenterScreen, true, InputBlockMode.Window);
        }

        public static void DeactivateLoading()
        {
            manager.Close();
        }
        */
    }
}

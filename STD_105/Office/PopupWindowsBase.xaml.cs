using DevExpress.Xpf.Core;
using GD_STD.Data;
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
        //public MatchSetting match
        public PopupWindowsBase(MatchSetting match)
        {
            InitializeComponent();
            //OfficeVM vm = new OfficeVM(this);
            //DataContext = vm;
            DataContext = match;

        }
        public PopupWindowsBase()
        {
            InitializeComponent();
            //OfficeVM vm = new OfficeVM(this);
            //DataContext = vm;

        }
    }
}

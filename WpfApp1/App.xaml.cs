using DevExpress.Xpf.DemoBase;
using DevExpress.Xpf.DemoCenterBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DemoBaseControl.SetApplicationTheme();
            DemoRunner.ShowApplicationSplashScreen(allowDrag: true);
        }
        public bool IsDebug { get { return true; } }
    }
}

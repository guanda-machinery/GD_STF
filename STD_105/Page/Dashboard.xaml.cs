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

namespace STD_105
{
    /// <summary>
    /// Dashboard.xaml 的互動邏輯
    /// </summary>
    public partial class Dashboard : BasePage
    {
        //private readonly System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        public Dashboard()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
            WPFSTD105.ViewLocator.ApplicationViewModel.IsHome = true;
            //Timer.Tick += new EventHandler(Timer_Click);
            //Timer.Interval = new TimeSpan(0, 0, 2);
            //Timer.Start();
        }
        /*
        private void Timer_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            leftrpm.CurrentValue = random.Next(0, 2000);
            leftec.CurrentValue = random.Next(0, 100);
            leftt.CurrentValue = random.Next(-1000, 1000);
            middlerpm.CurrentValue = random.Next(0, 2000);
            middleec.CurrentValue = random.Next(0, 100);
            middlet.CurrentValue = random.Next(-1000, 1000);
            rightrpm.CurrentValue =random.Next(0,2000);
            rightec.CurrentValue = random.Next(0, 100);
            rightt.CurrentValue = random.Next(-1000, 1000);
        }*/
    }
}

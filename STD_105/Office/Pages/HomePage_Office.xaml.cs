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

namespace STD_105.Office
{
    /// <summary>
    /// HomePage.xaml 的互動邏輯
    /// </summary>
    public partial class HomePage_Office : BasePage
    {
        public HomePage_Office()
        {
            InitializeComponent();
            PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

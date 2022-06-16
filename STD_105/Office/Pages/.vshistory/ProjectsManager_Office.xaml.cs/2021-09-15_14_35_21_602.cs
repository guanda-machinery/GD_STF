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
            CommonViewModel.ProjectProperty = new ProjectProperty();
        }
    }
}

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
using STD_105.Office;

namespace STD_105
{
    /// <summary>
    /// SelectFiles.xaml 的互動邏輯
    /// </summary>
    public partial class SelectFiles : BasePage
    {
        public SelectFiles()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //EmptyWindows empty = new EmptyWindows();
            //ImportFiles_Office importFiles = new ImportFiles_Office();
            //empty.Content = importFiles;
            //empty.Show();
        }
    }
}

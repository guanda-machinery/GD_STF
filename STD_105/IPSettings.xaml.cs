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

namespace STD_105
{
    /// <summary>
    /// IPSettings.xaml 的互動邏輯
    /// </summary>
    public partial class IPSettings : Window
    {
        public IPSettings()
        {
            InitializeComponent();
            this.Left = this.Width + 10;
            this.Top = this.Height + 10;
        }


        private void Button_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}

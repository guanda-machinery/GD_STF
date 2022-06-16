using GD_STD;
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

namespace MPG
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GD_STD.MPG value = new GD_STD.MPG();
            value.Magnification = (GD_STD.Enum.MAGNIFICATION)mag.SelectedIndex;
            value.AxisSelected = (GD_STD.Enum.AXIS_SELECTED)axisSelected.SelectedIndex;
            value.Coordinate = (GD_STD.Enum.COORDINATE)coordinate.SelectedIndex;
            new Memor.WriteMemorClient().SetMPG(value);
        }
    }
}

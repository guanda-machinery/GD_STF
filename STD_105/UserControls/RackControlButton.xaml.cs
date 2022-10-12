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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STD_105
{
    /// <summary>
    /// RoundMenuControl.xaml 的互動邏輯
    /// </summary>
    public partial class RackControlButton : UserControl
    {
        public delegate void EventHandle(bool isShow);
        public event EventHandle ShowClickEvent;

        public RackControlButton()
        {
            InitializeComponent();
            CompositionTarget.Rendering += UpdateEllipse;
        }
        private void UpdateEllipse(object sender, EventArgs e)
        {
            sectorCanvas.Clip = myEllipseGeometry;
        }
        private void BottomGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (grid_Button.Tag.ToString() == "true")
            {
                grid_Button.Tag = "false";
                Storyboard stbHide = (Storyboard)FindResource("stbHide");
                stbHide.Begin();
                ShowClickEvent?.Invoke(false);
            }
            else
            {
                grid_Button.Tag = "true";
                Storyboard stbShow = (Storyboard)FindResource("stbShow");
                stbShow.Begin();
                ShowClickEvent?.Invoke(true);
            }
        }
    }
}

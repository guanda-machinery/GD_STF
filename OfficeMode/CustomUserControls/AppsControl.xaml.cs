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

namespace OfficeMode
{
    /// <summary>
    /// AppsControl.xaml 的互動邏輯
    /// </summary>
    public partial class AppsControl : UserControl
    {
        public delegate void EventHandle(bool isShow);
        public event EventHandle ShowClickEvent;
        private bool IsPress;
        private Storyboard storyboard = new Storyboard();

        public AppsControl()
        {
            InitializeComponent(); 
            CompositionTarget.Rendering += UpdateEllipse;
        }

        private void UpdateEllipse(object sender, EventArgs e)
        {
            sectorCanvas.Clip = myRectGeometry;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsPress)
            {
                Storyboard stbShow = (Storyboard)FindResource("stbShow");
                stbShow.Begin();
                ShowClickEvent?.Invoke(true);
                IsPress = true;
            }
            else
            {
                Storyboard stbHide = (Storyboard)FindResource("stbHide");
                stbHide.Begin();
                ShowClickEvent?.Invoke(false);
                IsPress = false;
            }
        }
    }
}

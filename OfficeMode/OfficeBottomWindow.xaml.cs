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
    /// OfficeBottomWindow.xaml 的互動邏輯
    /// </summary>
    public partial class OfficeBottomWindow : Window
    {
        public delegate void EventHandle(bool isShow);
        public event EventHandle ShowClickEvent;
        private Storyboard storyboard = new Storyboard();

        public OfficeBottomWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (grid_TopMenu.Tag.ToString() == "Max")
            {
                grid_TopMenu.Tag = "Min";
                Storyboard stbShow = (Storyboard)FindResource("stbMin");
                stbShow.Begin();
                ShowClickEvent?.Invoke(true);
            }
            else
            {
                grid_TopMenu.Tag = "Max";
                Storyboard stbShow = (Storyboard)FindResource("stbMax");
                stbShow.Begin();
                ShowClickEvent?.Invoke(true);
            }
        }

        private void grid_TopMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

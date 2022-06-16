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

namespace OfficeMode
{
    /// <summary>
    /// CircularSectorControl.xaml 的互動邏輯
    /// </summary>
    public partial class CircularSectorControl : UserControl
    {
        public CircularSectorControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DisplayImageProperty = DependencyProperty.Register("DisplayImage", typeof(ImageSource), typeof(CircularSectorControl), new PropertyMetadata(GiveImageSource));
        public ImageSource DisplayImage
        {
            get { return (ImageSource)GetValue(DisplayImageProperty); }
            set { SetValue(DisplayImageProperty, value); }
        }
        private static void GiveImageSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularSectorControl parameter = d as CircularSectorControl;
            Image image = parameter.img;
            image.Source = (ImageSource)e.NewValue;
        }
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(CircularSectorControl), new PropertyMetadata(GiveBackColor));
        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        private static void GiveBackColor(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularSectorControl parameter = d as CircularSectorControl;
            Path cbx = parameter.sectorPath;
            cbx.Fill = (SolidColorBrush)e.NewValue;
        }

        private void MainGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            sectorPath.Fill = new SolidColorBrush(Color.FromRgb(246, 111, 111));
        }

        private void MainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            sectorPath.Fill = BackgroundColor;
        }
    }
}

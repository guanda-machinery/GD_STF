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

namespace STD_105
{
    /// <summary>
    /// CircleProgressBarWithChecked.xaml 的互動邏輯
    /// </summary>
    public partial class CircleProgressBarWithChecked : UserControl
    {
        public CircleProgressBarWithChecked()
        {
            InitializeComponent();
        }
        public Int16 Value
        {
            get { return (Int16)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(Int16), typeof(CircleProgressBarWithChecked), new PropertyMetadata(GetValue));

        private static void GetValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircleProgressBarWithChecked parameter = d as CircleProgressBarWithChecked;
            Slider slider = parameter.slider;
            slider.Value = (Int16)e.NewValue;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Point startPoint = path.StartPoint;
            double radiusX = percentage.Size.Width;
            double radiusY = percentage.Size.Height;
            double centerX = startPoint.X - radiusX;
            double centerY = startPoint.Y;

            if (e != null)
            {
                Slider slider1 = (Slider)sender;
                double X = centerX + (radiusX * Math.Cos(slider1.Value * 3.14 / 180 * 3.6));
                double Y = centerY + (radiusY * Math.Sin(slider1.Value * 3.14 / 180 * 3.6));
                Point point = new Point(-Y, X);
                percentage.Point = point;
                if (slider1.Value < 50)
                    percentage.IsLargeArc = false;
                else
                    percentage.IsLargeArc = true;
            }
        }
    }
}

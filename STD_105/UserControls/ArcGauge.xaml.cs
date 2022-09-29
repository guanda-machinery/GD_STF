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
    /// ArcGauge.xaml 的互動邏輯
    /// </summary>
    public partial class ArcGauge : UserControl
    {
        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(double), typeof(ArcGauge), new PropertyMetadata(6.0, new PropertyChangedCallback(Rerender)));
        public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(double), typeof(ArcGauge), new PropertyMetadata(0.0, new PropertyChangedCallback(Rerender)));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ArcGauge), new PropertyMetadata(0.0, new PropertyChangedCallback(Rerender)));
        public static readonly DependencyProperty GaugeColorProperty = DependencyProperty.Register("GaugeColor", typeof(Brush), typeof(ArcGauge), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty SettingGaugeColorProperty = DependencyProperty.Register("SettingGaugeColor", typeof(Brush), typeof(ArcGauge), new PropertyMetadata(Brushes.Red));
        readonly double FULLRANGE_ANGLE = 240;
        readonly double START_ANGLE_OFFSET = 150;

        public ArcGauge()
        {
            InitializeComponent();
        }

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public Brush GaugeColor
        {
            get { return (Brush)GetValue(GaugeColorProperty); }
            set { SetValue(GaugeColorProperty, value); }
        }

        public Brush SettingGaugeColor
        {
            get { return (Brush)GetValue(SettingGaugeColorProperty); }
            set { SetValue(SettingGaugeColorProperty, value); }
        }

        private static void Rerender(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ArcGauge gauge = d as ArcGauge;
            gauge.RenderGauge();
        }

        private void RenderGauge()
        {
            Point point;

            if (Value >= Max)
            {
                point = new Point(298.56, 240);
                arcGauge.IsLargeArc = true;
            }
            else if (Value <= Min)
            {
                point = new Point(21.44, 240);
                arcGauge.IsLargeArc = false;
            }
            else
            {
                double angle = FULLRANGE_ANGLE / (Max - Min) * (Value - Min) ;
                double rad = Math.PI / 180.0 * (angle + START_ANGLE_OFFSET);
                double x = 160 * Math.Cos(rad) + 160;
                double y = 160 * Math.Sin(rad) + 160;

                point = new Point(x, y);
                arcGauge.IsLargeArc = angle > 180.0;
            }

            arcGauge.Point = point;


            if (Value >= Max)
            {
                RotateTransform rotate = new RotateTransform();
                rotate.Angle = 240;
                ptSetup.RenderTransform = rotate;
            }
            else if (Value <= Min)
            {
                RotateTransform rotate = new RotateTransform();
                rotate.Angle = 0;
                ptSetup.RenderTransform = rotate;
            }
            else
            {
                double angle = FULLRANGE_ANGLE / (Max - Min) * (Value - Min);
                if (angle <= 180)
                {
                    RotateTransform rotate = new RotateTransform();
                    rotate.Angle = angle;
                    ptSetup.RenderTransform = rotate;
                }
                else
                {
                    RotateTransform rotate = new RotateTransform();
                    rotate.Angle = -360 + angle;
                    ptSetup.RenderTransform = rotate;
                }
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Value + 1 > Max)
                return;
            else
                Value++; 
        }

        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Value - 1 < Min)
                return;
            else
                Value--;
        }
    }
}

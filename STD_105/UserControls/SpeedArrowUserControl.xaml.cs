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
    /// SpeedArrowUserControl.xaml 的互動邏輯
    /// </summary>
    public partial class SpeedArrowUserControl : UserControl
    {
        public SpeedArrowUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty =
DependencyProperty.Register("ArrowBox", typeof(int), typeof(SpeedArrowUserControl), new PropertyMetadata(ArrowBox_PropertyChanged));

        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }


        private static void ArrowBox_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue is int)
            {
                switch ((int)e.NewValue)
                {
                    case 0:
                        (d as SpeedArrowUserControl).ArrowButton_1.Foreground = Brushes.White;
                        (d as SpeedArrowUserControl).ArrowButton_2.Foreground = Brushes.White;
                        (d as SpeedArrowUserControl).ArrowButton_3.Foreground = Brushes.White;
                        break;
                    case 1:
                        (d as SpeedArrowUserControl).ArrowButton_1.Foreground = (d as SpeedArrowUserControl).Foreground;
                        (d as SpeedArrowUserControl).ArrowButton_2.Foreground = Brushes.White;
                        (d as SpeedArrowUserControl).ArrowButton_3.Foreground = Brushes.White;

                        break;
                    case 2:
                        (d as SpeedArrowUserControl).ArrowButton_1.Foreground = (d as SpeedArrowUserControl).Foreground;
                        (d as SpeedArrowUserControl).ArrowButton_2.Foreground = (d as SpeedArrowUserControl).Foreground;
                        (d as SpeedArrowUserControl).ArrowButton_3.Foreground = Brushes.White;
                        break;
                    case 3:
                        (d as SpeedArrowUserControl).ArrowButton_1.Foreground = (d as SpeedArrowUserControl).Foreground;
                        (d as SpeedArrowUserControl).ArrowButton_2.Foreground = (d as SpeedArrowUserControl).Foreground;
                        (d as SpeedArrowUserControl).ArrowButton_3.Foreground = (d as SpeedArrowUserControl).Foreground;
                        break;
                    default:
                        (d as SpeedArrowUserControl).ArrowButton_1.Foreground = Brushes.Red;
                        (d as SpeedArrowUserControl).ArrowButton_2.Foreground = Brushes.Red;
                        (d as SpeedArrowUserControl).ArrowButton_3.Foreground = Brushes.Red;
                        break;
                }
            }
        }

        private void ArrowButton_1_Click(object sender, RoutedEventArgs e)
        {
            if (Value != 1)
            {
                Value = 1;
            }
            else
            {
                Value = 0;
            }
        }

        private void ArrowButton_2_Click(object sender, RoutedEventArgs e)
        {
            if (Value != 2)
            {
                Value = 2;
            }
            else
            {
                Value = 1;
            }
        }

        private void ArrowButton_3_Click(object sender, RoutedEventArgs e)
        {
            if (Value != 3)
            {
                Value = 3;
            }
            else
            {
                Value = 2;
            }
        }


    }
}

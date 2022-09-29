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
    /// BoolSwitchController.xaml 的互動邏輯
    /// </summary>
    public partial class BoolSwitchController1 : UserControl
    {
        public BoolSwitchController1()
        {
            InitializeComponent();
        }

        public string LeftText
        {
            get { return (string)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(LeftText), typeof(string), typeof(BoolSwitchController1), new PropertyMetadata(LeftTextGiven));

        private static void LeftTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BoolSwitchController1 parameter = d as BoolSwitchController1;
            CheckBox cbx = parameter.checkBox;
            cbx.Tag = e.NewValue.ToString();
        }

        public string RightText
        {
            get { return (string)GetValue(MyPropertyProperty1); }
            set { SetValue(MyPropertyProperty1, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty1 =
            DependencyProperty.Register(nameof(RightText), typeof(string), typeof(BoolSwitchController1), new PropertyMetadata(RightTextGiven));

        private static void RightTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BoolSwitchController1 parameter = d as BoolSwitchController1;
            CheckBox cbx = parameter.checkBox;
            cbx.ToolTip = e.NewValue.ToString();
        }
    }
}

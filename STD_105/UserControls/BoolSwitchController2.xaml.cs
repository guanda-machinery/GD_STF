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
    /// CheckBox.xaml 的互動邏輯
    /// </summary>
    public partial class BoolSwitchController2 : UserControl
    {
        public BoolSwitchController2()
        {
            InitializeComponent();
        }

        public bool CheckBoxValue
        {
            get { return (bool)GetValue(MyPropertyProperty1); }
            set { SetValue(MyPropertyProperty1, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty1 =
            DependencyProperty.Register(nameof(CheckBoxValue), typeof(bool), typeof(BoolSwitchController2), new PropertyMetadata(CheckBoxValueGiven));

        private static void CheckBoxValueGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BoolSwitchController2 parameter = d as BoolSwitchController2;
            CheckBox cbx = parameter.cbxValue;
            cbx.IsChecked = Convert.ToBoolean(e.NewValue);
        }
    }
}

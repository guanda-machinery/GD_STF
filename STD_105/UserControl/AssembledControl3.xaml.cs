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
    /// AssembledControl3.xaml 的互動邏輯
    /// </summary>
    public partial class AssembledControl3 : UserControl
    {
        public AssembledControl3()
        {
            InitializeComponent();
        }
        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(AssembledControl3), new PropertyMetadata(LabelTextGiven));

        private static void LabelTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl3 parameter = d as AssembledControl3;
            Label lab = parameter.labText;
            lab.Content = e.NewValue.ToString();
        }
        /*
        public string LeftText
        {
            get { return (string)GetValue(MyPropertyProperty1); }
            set { SetValue(MyPropertyProperty1, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty1 =
            DependencyProperty.Register(nameof(LeftText), typeof(string), typeof(AssembledControl3), new PropertyMetadata(LeftTextGiven));
      
        private static void LeftTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl3 parameter = d as AssembledControl3;
            CheckBox cbx = parameter.checkBox;
            cbx.Tag.ToString().Split(',')[0] = e.NewValue.ToString();
        }

        public string RightText
        {
            get { return (string)GetValue(MyPropertyProperty2); }
            set { SetValue(MyPropertyProperty2, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty2 =
            DependencyProperty.Register(nameof(RightText), typeof(string), typeof(AssembledControl3), new PropertyMetadata(RightTextGiven));

        private static void RightTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl3 parameter = d as AssembledControl3;
            CheckBox cbx = parameter.checkBox;
            cbx.Tag.ToString().Split(',')[1] = e.NewValue.ToString();
        }*/

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(AssembledControl3), new PropertyMetadata(GetCheckeStatus));

        private static void GetCheckeStatus(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl3 parameter = d as AssembledControl3;
            CheckBox cbx = parameter.checkBox;
            cbx.IsChecked = Convert.ToBoolean(e.NewValue);
        }

    }
}

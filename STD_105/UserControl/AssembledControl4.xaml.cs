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
    /// AssembledControl4.xaml 的互動邏輯
    /// </summary>
    public partial class AssembledControl4 : UserControl
    {
        public AssembledControl4()
        {
            InitializeComponent();
        }

        public string LeftText
        {
            get { return (string)GetValue(LeftTextProperty); }
            set { SetValue(LeftTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftTextProperty =
            DependencyProperty.Register(nameof(LeftText), typeof(string), typeof(AssembledControl4), new PropertyMetadata(LabelTextGiven));

        private static void LabelTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl4 parameter = d as AssembledControl4;
            Label lab = parameter.labText;
            lab.Content = e.NewValue.ToString();
        }

        public string RightText
        {
            get { return (string)GetValue(RightTextProperty); }
            set { SetValue(RightTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightTextProperty =
            DependencyProperty.Register(nameof(RightText), typeof(string), typeof(AssembledControl4), new PropertyMetadata(TextBoxTextGiven));

        private static void TextBoxTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl4 parameter = d as AssembledControl4;
            TextBox tbx = parameter.tbxText;
            tbx.Text = e.NewValue.ToString();
        }

        public bool ReadOnly
        {
            get { return (bool)GetValue(CanEditProperty); }
            set { SetValue(CanEditProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanEditProperty =
            DependencyProperty.Register(nameof(ReadOnly), typeof(bool), typeof(AssembledControl4), new PropertyMetadata(IsReadOnlyOrNot));

        private static void IsReadOnlyOrNot(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl4 parameter = d as AssembledControl4;
            TextBox tbx = parameter.tbxText;
            tbx.IsReadOnly = Convert.ToBoolean(e.NewValue);
        }
    }
}

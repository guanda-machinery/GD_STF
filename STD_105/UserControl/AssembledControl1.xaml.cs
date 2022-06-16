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
    /// SoftSettingParameter1.xaml 的互動邏輯
    /// </summary>
    public partial class AssembledControl1 : UserControl
    {
        public AssembledControl1()
        {
            InitializeComponent();
        }

        public string TextBlockText
        {
            get { return (string)GetValue(TextBlockTextProperty); }
            set { SetValue(TextBlockTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBlockTextProperty =
            DependencyProperty.Register(nameof(TextBlockText), typeof(string), typeof(AssembledControl1), new PropertyMetadata(TextBlockTextGiven));

        private static void TextBlockTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl1 parameter = d as AssembledControl1;
            TextBlock tbk = parameter.tbkText;
            tbk.Text = e.NewValue.ToString();
        }

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(AssembledControl1), new PropertyMetadata(GetCheckStatus));

        private static void GetCheckStatus(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl1 parameter = d as AssembledControl1;
            CheckBox cbx = parameter.cbxValue;
            cbx.IsChecked = Convert.ToBoolean(e.NewValue);
        }
    }
}

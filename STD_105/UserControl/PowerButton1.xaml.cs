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
    /// PowerButton1.xaml 的互動邏輯
    /// </summary>
    public partial class PowerButton1 : UserControl
    {
        public PowerButton1()
        {
            InitializeComponent();
        }

        //public event EventHandler ButtonClick;

        //private void btn_Content_Click(object sender, MouseButtonEventArgs e)
        //{
        //    if (ButtonClick != null)
        //    {
        //        ButtonClick(this, new EventArgs());
        //    }
        //}

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(PowerButton1), new PropertyMetadata(TextBlockTextGiven));

        private static void TextBlockTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PowerButton1 parameter = d as PowerButton1;
            Button tbk = parameter.btn_Content;
            tbk.Content = e.NewValue.ToString();
        }

        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(PowerButton1), new PropertyMetadata(DoGivenCommand));

        private static void DoGivenCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PowerButton1 parameter = d as PowerButton1;
            Button btn = parameter.btn_Content;
            btn.Command = (ICommand)e.NewValue;
        }

        public object ButtonParameter
        {
            get => (object)GetValue(ButtonParameterProperty);
            set => SetValue(ButtonParameterProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonParameterProperty =
            DependencyProperty.Register(nameof(ButtonParameter), typeof(object), typeof(PowerButton1), new PropertyMetadata(DoGivenCommandParameter));

        private static void DoGivenCommandParameter(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PowerButton1 parameter = d as PowerButton1;
            Button btn = parameter.btn_Content;
            btn.CommandParameter = e.NewValue;
        }

        private void btn_Content_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

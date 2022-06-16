using DevExpress.Xpf.Controls;
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
    /// SelectObjectWindows.xaml 的互動邏輯
    /// </summary>
    public partial class SelectObjectWindows : UserControl
    {
        public SelectObjectWindows()
        {
            InitializeComponent();
        }

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(SelectObjectWindows), new PropertyMetadata(ButtonTextGiven));

        private static void ButtonTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SelectObjectWindows parameter = d as SelectObjectWindows;
            Button button = parameter.btn_Function;
            button.Content = e.NewValue.ToString();
        }

        public string SelectedItemPath
        {
            get { return (string)GetValue(SelectedItemPathProperty); }
            set { SetValue(SelectedItemPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemPathProperty =
            DependencyProperty.Register(nameof(SelectedItemPathProperty), typeof(string), typeof(SelectObjectWindows), new PropertyMetadata(ItemPathGiven));

        private static void ItemPathGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SelectObjectWindows parameter = d as SelectObjectWindows;
            BreadcrumbControl temp = parameter.filePath;
            temp.SelectedItemPath = e.NewValue.ToString();
        }

        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(SelectObjectWindows), new PropertyMetadata(ButtonCommandGiven));

        private static void ButtonCommandGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SelectObjectWindows parameter = d as SelectObjectWindows;
            Button button = parameter.btn_Function;
            button.Command = (ICommand)e.NewValue;
        }

        public string ButtonParameter
        {
            get { return (string)GetValue(ButtonParameterProperty); }
            set { SetValue(ButtonParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonParameterProperty =
            DependencyProperty.Register(nameof(ButtonParameter), typeof(string), typeof(SelectObjectWindows), new PropertyMetadata(PassParameter));

        private static void PassParameter(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SelectObjectWindows parameter = d as SelectObjectWindows;
            Button button = parameter.btn_Function;
            button.CommandParameter = e.NewValue.ToString();
        }
    }
}

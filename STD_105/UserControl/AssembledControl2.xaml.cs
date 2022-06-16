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
    /// SoftSettingParameter2.xaml 的互動邏輯
    /// </summary>
    public partial class AssembledControl2 : UserControl
    {
        public AssembledControl2()
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
            DependencyProperty.Register(nameof(TextBlockText), typeof(string), typeof(AssembledControl2), new PropertyMetadata(TextBlockTextGiven));

        private static void TextBlockTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl2 parameter = d as AssembledControl2;
            TextBlock tbk = parameter.tbkText;
            tbk.Text = e.NewValue.ToString();
        }

        public string TextBoxText
        {
            get { return (string)GetValue(TextBoxTextProperty); }
            set { SetValue(TextBoxTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxTextProperty =
            DependencyProperty.Register(nameof(TextBoxText), typeof(string), typeof(AssembledControl2), new PropertyMetadata(TextBoxTextGiven));

        private static void TextBoxTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl2 parameter = d as AssembledControl2;
            TextBox tbx = parameter.tbxText;
            tbx.Text = e.NewValue.ToString();
        }

        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(AssembledControl2), new PropertyMetadata(DoCommandHere));

        private static void DoCommandHere(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl2 parameter = d as AssembledControl2;
            Button button = parameter.btnIcon;
            button.Command = (ICommand)e.NewValue;
        }

        public ImageSource ButtonImageSource
        {
            get { return (ImageSource)GetValue(ButtonImageSourceProperty); }
            set { SetValue(ButtonImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonImageSourceProperty =
            DependencyProperty.Register(nameof(ButtonImageSource), typeof(ImageSource), typeof(AssembledControl2), new PropertyMetadata(SetImageSource));

        private static void SetImageSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl2 parameter = d as AssembledControl2;
            Image image = parameter.imgInBtn;
            image.Source = (ImageSource)e.NewValue;
        }

        public bool ButtonVisibility
        {
            get { return (bool)GetValue(ButtonVisibilityProperty); }
            set { SetValue(ButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonVisibilityProperty =
            DependencyProperty.Register(nameof(ButtonVisibility), typeof(bool), typeof(AssembledControl2), new PropertyMetadata(SetButtonVisibility));

        private static void SetButtonVisibility(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl2 parameter = d as AssembledControl2;
            Button button = parameter.btnIcon;
            if ((bool)e.NewValue)
            {
                button.Visibility = Visibility.Visible;
            }
            else
            {
                button.Visibility = Visibility.Collapsed;
            }
        }

        public string ButtonParameter
        {
            get { return (string)GetValue(ButtonParameterProperty); }
            set { SetValue(ButtonParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonParameterProperty =
            DependencyProperty.Register(nameof(ButtonParameter), typeof(string), typeof(AssembledControl2), new PropertyMetadata(GetParemeterText));

        private static void GetParemeterText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AssembledControl2 parameter = d as AssembledControl2;
            Button button = parameter.btnIcon;
            button.CommandParameter = (string)e.NewValue;
        }
    }
}

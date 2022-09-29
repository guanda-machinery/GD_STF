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
    /// OfficeAssembledControl.xaml 的互動邏輯
    /// </summary>
    public partial class OfficeAssembledControl : UserControl
    {
        public OfficeAssembledControl()
        {
            InitializeComponent();
        }

        // 開頭紅色符號
        public string Symbol
        {
            get { return (string)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register(nameof(Symbol), typeof(string), typeof(OfficeAssembledControl), new PropertyMetadata(SymbolGiven));

        private static void SymbolGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OfficeAssembledControl parameter = d as OfficeAssembledControl;
            TextBlock text = parameter.tbkSymbol;
            text.Text = e.NewValue.ToString();
        }

        // 標題文字
        public string LeftText
        {
            get { return (string)GetValue(LeftTextProperty); }
            set { SetValue(LeftTextProperty, value); }
        }
       
        public static readonly DependencyProperty LeftTextProperty =
            DependencyProperty.Register(nameof(LeftText), typeof(string), typeof(OfficeAssembledControl), new PropertyMetadata(TextblockGiven));

        private static void TextblockGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OfficeAssembledControl parameter = d as OfficeAssembledControl;
            TextBlock lab = parameter.tbkText;
            lab.Text = e.NewValue.ToString();
        }

        public string RightText
        {
            get { return (string)GetValue(RightTextProperty); }
            set { SetValue(RightTextProperty, value); }
        }

        // TextBox文字
        public static readonly DependencyProperty RightTextProperty =
            DependencyProperty.Register(nameof(RightText), typeof(string), typeof(OfficeAssembledControl), new UIPropertyMetadata(TextBoxTextGiven));

        private static void TextBoxTextGiven(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OfficeAssembledControl parameter = d as OfficeAssembledControl;
            TextBox tbx = parameter.tbxText;
            tbx.Text = e.NewValue.ToString();
        }

        //給予按鈕命令
        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(OfficeAssembledControl), new PropertyMetadata(GetButtonCommand));

        private static void GetButtonCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OfficeAssembledControl parameter = d as OfficeAssembledControl;
            Button btn = parameter.btnImg;
            btn.Command = (ICommand)e.NewValue;
        }

        //是否顯示按鈕
        public bool ButtonVisibility
        {
            get { return (bool)GetValue(ButtonVisibilityProperty); }
            set { SetValue(ButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ButtonVisibilityProperty =
            DependencyProperty.Register(nameof(ButtonVisibility), typeof(bool), typeof(OfficeAssembledControl), new PropertyMetadata(GetButtonVisibilityProperty));

        private static void GetButtonVisibilityProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OfficeAssembledControl parameter = d as OfficeAssembledControl;
            Button btn = parameter.btnImg;
            btn.Visibility = (bool)e.NewValue? Visibility.Visible: Visibility.Hidden;
        }

        //按鈕上的圖片來源
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(OfficeAssembledControl), new PropertyMetadata(GetImageSource));

        private static void GetImageSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OfficeAssembledControl parameter = d as OfficeAssembledControl;
            Image img = parameter.img;
            img.Source = (ImageSource)e.NewValue;
        }
    }
}

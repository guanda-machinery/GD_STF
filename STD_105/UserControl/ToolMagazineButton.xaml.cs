using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace STD_105
{
    /// <summary>
    /// ToolMagazineButton.xaml 的互動邏輯
    /// </summary>
    public partial class ToolMagazineButton : UserControl
    {
        public ToolMagazineButton()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 按鈕標題
        /// </summary>
        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }
        /// <summary>
        /// <see cref="TitleText"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty TitleTextProperty =
            DependencyProperty.Register(nameof(TitleText), typeof(string), typeof(ToolMagazineButton), new PropertyMetadata(TitleTextPropertyChanged));
        /// <summary>
        /// <see cref="TitleText"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void TitleTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolMagazineButton parameter = d as ToolMagazineButton;
            TextBlock title = parameter.tbk_Title;
            title.Text = e.NewValue.ToString();
        }

        /// <summary>
        /// 按鈕副標題
        /// </summary>
        public string SubTitleText
        {
            get { return (string)GetValue(SubTitleTextProperty); }
            set { SetValue(SubTitleTextProperty, value); }
        }
        /// <summary>
        /// <see cref="SubTitleText"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty SubTitleTextProperty =
            DependencyProperty.Register(nameof(SubTitleText), typeof(string), typeof(ToolMagazineButton), new PropertyMetadata(SubTitleTextPropertyChanged));
        /// <summary>
        /// <see cref="TitleText"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void SubTitleTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolMagazineButton parameter = d as ToolMagazineButton;
            TextBlock sub = parameter.tbk_SubTitle;
            sub.Text = e.NewValue.ToString();
        }

        /// <summary>
        /// 按鈕命令
        /// </summary>
        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }
        /// <summary>
        /// <see cref="ButtonCommand"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(ToolMagazineButton), new PropertyMetadata(ButtonCommandPropertyChanged));
        /// <summary>
        /// <see cref="ButtonCommand"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ButtonCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolMagazineButton parameter = d as ToolMagazineButton;
            parameter.btn_Main.Command = (ICommand)e.NewValue;
        }

        /// <summary>
        /// 按鈕命令參數
        /// </summary>
        public object CommandParameter { get; set; }
        /// <summary>
        /// <see cref="CommandParameter"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(ToolMagazineButton), new PropertyMetadata(CommandParameterPropertyChanged));
        /// <summary>
        /// <see cref="CommandParameter"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CommandParameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolMagazineButton parameter = d as ToolMagazineButton;
            parameter.btn_Main.CommandParameter = e.NewValue;
        }

        /// <summary>
        /// 按鈕圖片樣式
        /// </summary>
        public ImageSource ButtonImageSource
        {
            get { return (ImageSource)GetValue(ButtonImageProperty); }
            set { SetValue(ButtonImageProperty, value); }
        }
        /// <summary>
        /// <see cref="ButtonImageSource"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty ButtonImageProperty =
            DependencyProperty.Register(nameof(ButtonImageSource), typeof(ImageSource), typeof(ToolMagazineButton), new PropertyMetadata(ButtonImagePropertyChanged));
        /// <summary>
        /// <see cref="ButtonImageSource"/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ButtonImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolMagazineButton parameter = d as ToolMagazineButton;
            parameter.img.Source = (ImageSource)e.NewValue;
        }

        /// <summary>
        /// 按鈕上鎖
        /// </summary>
        public bool Lock
        {
            get { return (bool)GetValue(LockProperty); }
            set { SetValue(LockProperty, value); }
        }
        /// <summary>
        /// <see cref="Lock"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty LockProperty =
            DependencyProperty.Register(nameof(Lock), typeof(bool), typeof(ToolMagazineButton), new PropertyMetadata(LockPropertyChanged));
        /// <summary>
        /// <see cref="Lock"/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void LockPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolMagazineButton parameter = d as ToolMagazineButton;
            parameter.btn_Main.SetValue(ButtonProgressAssist.IsIndeterminateProperty, e.NewValue);
            parameter.btn_Main.SetValue(ButtonProgressAssist.IsIndicatorVisibleProperty, e.NewValue);
            parameter.btn_Main.IsEnabled = !(bool)e.NewValue;
        }

        /// <summary>
        /// 觸發狀態
        /// </summary>
        public bool StartState
        {
            get { return (bool)GetValue(StartStateProperty); }
            set { SetValue(StartStateProperty, value); }
        }

        /// <summary>
        /// <see cref="StartState"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty StartStateProperty =
            DependencyProperty.Register(nameof(StartState), typeof(bool), typeof(ToolMagazineButton), new PropertyMetadata(StartStatePropertyChanged));
        /// <summary>
        /// <see cref="StartState"/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void StartStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolMagazineButton parameter = d as ToolMagazineButton;
            bool _ = (bool)e.NewValue;
            //if (menuControl.IsEnabled) //如果有啟用狀態
            //{
            if (_)
            {
                Storyboard sb = parameter.FindResource("buttonDown") as Storyboard;
                sb.Begin();
            }
            else
            {
                Storyboard sb = parameter.FindResource("buttonUp") as Storyboard;
                sb.Begin();
            }
        }

        /// <summary>
        /// 說明文字顏色
        /// </summary>
        public Brush FontColor
        {
            get { return (Brush)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }
        /// <summary>
        /// <see cref="FontColor"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty FontColorProperty =
            DependencyProperty.Register(nameof(FontColor), typeof(Brush), typeof(ToolMagazineButton), new PropertyMetadata(ColorChangedPropertyPropertyChanged));
        /// <summary>
        /// <see cref="FontColor"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ColorChangedPropertyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolMagazineButton parameter = d as ToolMagazineButton;
            TextBlock title = parameter.tbk_Title;
            title.Foreground = (Brush)e.NewValue;
            TextBlock subTitle = parameter.tbk_SubTitle;
            subTitle.Foreground = (Brush)e.NewValue;
        }
    }
}

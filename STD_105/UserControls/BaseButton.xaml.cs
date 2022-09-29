using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace STD_105
{
    /// <summary>
    /// BaseButton.xaml 的互動邏輯
    /// </summary>
    public partial class BaseButton : UserControl
    {
        /// <summary>
        /// 等待時所播放的動畫
        /// </summary>
        private class WaitIng
        {
            /// <summary>
            /// 動畫開始
            /// </summary>
            public bool Start { get; set; } = false;
        }

        private WaitIng _WaitIng { get; set; }

        /// <summary>
        /// 標題文字
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
            DependencyProperty.Register(nameof(TitleText), typeof(string), typeof(BaseButton), new PropertyMetadata(TitleTextPropertyChanged));
        /// <summary>
        /// <see cref="TitleText"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void TitleTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseButton menuControl = (d as BaseButton);
            TextBlock Title = menuControl.title;
            Title.Text = e.NewValue.ToString();
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
            DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(BaseButton), new PropertyMetadata(ButtonCommandPropertyChanged));
        /// <summary>
        /// <see cref="ButtonCommand"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ButtonCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseButton menuControl = (d as BaseButton);
            menuControl.button.Command = (ICommand)e.NewValue;
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
            DependencyProperty.Register(nameof(ButtonImageSource), typeof(ImageSource), typeof(BaseButton), new PropertyMetadata(ButtonImagePropertyChanged));
        /// <summary>
        /// <see cref="ButtonImageSource"/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ButtonImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseButton menuControl = (d as BaseButton);
            menuControl.image.Source = (ImageSource)e.NewValue;
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
            DependencyProperty.Register(nameof(Lock), typeof(bool), typeof(BaseButton), new PropertyMetadata(LockPropertyChanged));
        /// <summary>
        /// <see cref="Lock"/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void LockPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseButton menuControl = (d as BaseButton);
            menuControl.button.SetValue(ButtonProgressAssist.IsIndeterminateProperty, e.NewValue);
            menuControl.button.SetValue(ButtonProgressAssist.IsIndicatorVisibleProperty, e.NewValue);
            menuControl.button.IsEnabled = !(bool)e.NewValue;
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
            DependencyProperty.Register(nameof(StartState), typeof(bool), typeof(BaseButton), new PropertyMetadata(StartStatePropertyChanged));
        /// <summary>
        /// <see cref="StartState"/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void StartStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseButton menuControl = (d as BaseButton);
            bool _ = (bool)e.NewValue;
            //if (menuControl.IsEnabled) //如果有啟用狀態
            //{
                if (_)
                {
                    menuControl.border.Visibility = Visibility.Visible;
                    //menuControl.bor.Background = Brushes.LightGray;
                }
                else
                {
                    menuControl.border.Visibility = Visibility.Hidden;
                    //menuControl.bor.Background = Brushes.Transparent;
                }
            //}
        }

        /// <summary>
        /// 隱藏文字
        /// </summary>
        public bool TextHide
        {
            get { return (bool)GetValue(TextHideProperty); }
            set { SetValue(TextHideProperty, value); }
        }
        /// <summary>
        /// <see cref="TextHide"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty TextHideProperty =
            DependencyProperty.Register(nameof(TextHide), typeof(bool), typeof(BaseButton), new PropertyMetadata(TextHidePropertyChanged));
        /// <summary>
        /// <see cref="TextHide"/>變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void TextHidePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseButton menuControl = (d as BaseButton);
            menuControl.title.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// 陰影面積
        /// </summary>
        public new double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }
        /// <summary>
        /// <see cref="Opacity"/> 註冊為依賴屬性
        /// </summary>
        public static readonly new DependencyProperty OpacityProperty =
            DependencyProperty.Register(nameof(Opacity), typeof(double), typeof(BaseButton), new PropertyMetadata(OpacityPropertyChanged));

        private static void OpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseButton menuControl = (d as BaseButton);
            menuControl.Dro.Opacity = (double)e.NewValue;
        }
        /// <summary>
        /// 按鈕命令參數
        /// </summary>
        public object CommandParameter { get; set; }
        /// <summary>
        /// <see cref="CommandParameter"/> 註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(BaseButton), new PropertyMetadata(CommandParameterPropertyChanged));
        /// <summary>
        /// <see cref="CommandParameter"/> 變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CommandParameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseButton menuControl = (d as BaseButton);
            menuControl.button.CommandParameter = e.NewValue;
        }

        public BaseButton()
        {
            InitializeComponent();
            button.DataContext = new WaitIng();
            this.IsEnabledChanged += BaseButton_IsEnabledChanged;
        }
        /// <summary>
        /// IsEnabled 屬性變更時觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool _ = (bool)e.NewValue;

            if (!_)
            {
                image_Disable.Visibility = Visibility.Visible;
                //bor.Background = Brushes.Gray;
            }
            else
            {
                image_Disable.Visibility = Visibility.Hidden;
                //bor.Background = Brushes.Transparent;
            }
        }
    }
}

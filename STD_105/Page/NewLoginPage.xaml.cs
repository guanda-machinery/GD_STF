using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFWindowsBase;
using WPFSTD105.ViewModel;

namespace STD_105
{
    /// <summary>
    /// NewIoTLoginPage.xaml 的互動邏輯
    /// </summary>
    public partial class NewLoginPage : BasePage<LoginVM>
    {
        public NewLoginPage()
        {
            InitializeComponent();
            this.ViewModel.LoginFailure += ViewModel_LoginFailure;
        }

        private bool isPress;
        private void ChangeLogoColor(object sender, EventArgs e)
        {
            SolidColorBrush solidColor1 = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            SolidColorBrush solidColor2 = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            SolidColorBrush solidColor3 = new SolidColorBrush(Color.FromArgb(255, 223, 223, 223));

            if (logo_Left.GetValue(Shape.FillProperty).ToString() == "#FFFF0000")
            {
                logo_Left.Fill = solidColor2;
                logo_Right.Fill = solidColor3;
                logo_Top.Fill = solidColor1;
            }
            else if (logo_Left.GetValue(Shape.FillProperty).ToString() == "#FF808080")
            {
                logo_Left.Fill = solidColor3;
                logo_Right.Fill = solidColor1;
                logo_Top.Fill = solidColor2;
            }
            else if (logo_Left.GetValue(Shape.FillProperty).ToString() == "#FFDFDFDF")
            {
                logo_Left.Fill = solidColor1;
                logo_Right.Fill = solidColor2;
                logo_Top.Fill = solidColor3;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isPress)
                return;
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = new TimeSpan(1000000)
            };
            // 將事件加入Timer
            timer.Tick += new EventHandler(ChangeLogoColor);
            timer.Start();
            isPress = true;
        }

        /// <summary>
        /// 登入失敗
        /// </summary>
        private void ViewModel_LoginFailure()
        {
            this.AnimateInFadeIn();
        }
    }
}

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
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFSTD105;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using System.Threading;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
namespace STD_105.Office
{
    /// <summary>
    /// LoginPage.xaml 的互動邏輯
    /// </summary>
    public partial class LoginPage : Window
    {
        private SplashScreenMainViewModel ViewModel { get; set; }
        public LoginPage()
        {
            ShowSplashScreen();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //OfficeBaseWindow window = new OfficeBaseWindow();
            //window.Show();
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 環境檢查
        /// </summary>
        private void ShowSplashScreen()
        {
            ViewModel = new SplashScreenMainViewModel()
            {
                SplashScreenType = PredefinedSplashScreenType.Fluent,
                StartupLocation = WindowStartupLocation.CenterOwner,
                InputBlock = InputBlockMode.WindowContent,
                TrackOwnerPosition = false,
            };
            DXSplashScreenViewModel viewModel = new DXSplashScreenViewModel()
            {
                Logo = new Uri(@"pack://application:,,,/STD_105;component/Logo/SmallLogo.svg"),
                Status = "開啟中...",
                Title = "STD_105",
                Subtitle = "Powered by GUANDA",
                Copyright = "版權所有 © 2021 GUANDA",
            };
            SplashScreenManager manager;

            manager = SplashScreenManager.CreateFluent(viewModel);

            manager.Show(this, ViewModel.StartupLocation, ViewModel.TrackOwnerPosition, ViewModel.InputBlock);

            if (!WPFSTD105.Properties.SofSetting.Default.OfficeMode)//如果不是辦公室軟體
            {
                //TODO: 檢測 IIS 尚未完成
                //if (DetectIIS())
                //{

                //}
                //Open();
            }
            else
            {
                for (int i = 0; i <= 100; i++)
                {
                    if (i == 50)
                        viewModel.Status = "檢查環境中...";
                    else if (i == 80)
                        viewModel.Status = "接近完成了...";

                    viewModel.Progress = i;

                    Thread.Sleep(40);
                }
                manager.Close();
            }
        }
    }
}

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
            //TODO: 登入成功或失敗的邏輯尚未增加
            CommonViewModel.AccountNumber = new AccountNumber()
            {
                Account = Account.Text,
                CodeName = CodeName.Text,
                PasswordText = PasswordText.Text
            };
            //OfficeBaseWindow window = new OfficeBaseWindow();
            //window.Show();//辦公室介面
            Close();//登入畫面關閉
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();//登入畫面關閉
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
                if (DetectIIS())
                {

                }
                Open();
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
            }
     
            manager.Close();
        }
        /// <summary>
        /// 檢測 IIS
        /// </summary>
        /// <returns>有回應回傳 true</returns>
        public static bool DetectIIS()
        {
            string url = "http://localhost:63506/CodesysIIS/Memor.svc";
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                System.Net.WebResponse myResponse = myRequest.GetResponse();
            }
            catch (System.Net.WebException)
            {
                return false;
            }
            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}

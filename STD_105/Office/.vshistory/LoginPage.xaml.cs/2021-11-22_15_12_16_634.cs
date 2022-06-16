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
using System.Net;
using System.Web;

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

        private void IoT_Click(object sender, RoutedEventArgs e)
        {
            string code = tbx_Code.Text, acc = tbx_Account.Text, pwd = tbx_PWD.Text; //使用者代號, 公司帳號, 代號密碼 
            string url = "https://www.gdim.tw/MP_Authentication.svc/CompanyAccount?" + $"account={acc}&code={code}&password={pwd}";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "GET";
            using (WebResponse webRequest = httpWebRequest.GetResponse())
            {
                System.IO.StreamReader stream = new System.IO.StreamReader(webRequest.GetResponseStream());
                string result = stream.ReadToEnd();
                if (true)
                {
                    CommonViewModel.UserMode = MachineMode.IoT;
                    CommonViewModel.AccountNumber.CodeName = tbx_Code.Text;
                    CommonViewModel.AccountNumber.Account = tbx_Account.Text;
                    CommonViewModel.AccountNumber.PasswordText = tbx_PWD.Text;
                    Close(); //關閉視窗
                }
            }

            //OfficeBaseWindow window = new OfficeBaseWindow();
            //window.Show();

        }
        private void One_Click(object sender, RoutedEventArgs e)
        {
            CommonViewModel.UserMode = MachineMode.Single;

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
                //manager.Close();
            }
            manager.Close();
        }

        private void Account_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBox tbx = sender as TextBox;

            if (tbx.Text == "")
            {
                switch (tbx.Name)
                {
                    case "tbx_Account":
                        tbx.Text = "公司帳號";
                        break;
                    case "tbx_Code":
                        tbx.Text = "使用者代號";
                        break;
                    case "tbx_PWD":
                        tbx.Text = "使用者密碼";
                        break;
                }
            }
        }

        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tbx = sender as TextBox;

            switch (e.Key)
            {
                case Key.Enter:
                    // 執行登入指令
                    break;
                case Key.Escape:
                    tbx.Text = "";
                    break;
                default:
                    if (tbx.Text == "公司帳號" || tbx.Text == "使用者代號" || tbx.Text == "使用者密碼")
                    {
                        tbx.Text = "";
                    }
                    break;
            }
        }
    }
}

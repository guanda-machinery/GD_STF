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
using DevExpress.Xpf.WindowsUI;
using System.Configuration;
using System.ServiceModel.Configuration;
using WPFSTD105.ViewModel;
using GD_STD.Phone;

namespace STD_105.Office
{
    /// <summary>
    /// LoginPage.xaml 的互動邏輯
    /// </summary>
    public partial class LoginPage : Window
    {
        private SplashScreenMainViewModel ViewModel { get; set; }
        /// <summary>
        /// 系統管理員驗證
        /// </summary>
        private WPFSTD105.SystemVerification Verification { get; set; }
        public LoginPage()
        {
            ShowSplashScreen();
            InitializeComponent();
            if (CommonViewModel.GetType() == typeof(ApplicationVM))
            {
                viewBox.PreviewMouseRightButtonDown += Admin;
                CommonViewModel.ChildWin = this;
            }
        }

        private void IoT_Click(object sender, RoutedEventArgs e)
        {

            try
            {
#if _DEBUG

                ApplicationViewModel.EngineeringMode = true;
                Close();
#else
                //string code = tbx_Code.Text, //使用者代號
                //            acc = tbx_Account.Text, //公司帳號
                //            pwd = tbx_PWD.Text; //代號密碼 
                //
                if (Verification != null) //如果啟動工程模式
                {
                    if (Verification.IsAdmin(ApplicationViewModel.AccountNumber.PasswordText)) //驗證登入密碼
                    {
                        ApplicationViewModel.EngineeringMode = true;
                    }
                }
                else
                {
                    string url = "https://www.gdim.tw/MP_Authentication.svc/CompanyAccount?" + $"account={WPFSTD105.Properties.SofSetting.Default.Account}&code={WPFSTD105.Properties.SofSetting.Default.Code}&password={ApplicationViewModel.AccountNumber.PasswordText}"; // HTTP 登入請求
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Method = "GET";
                    Verification = null; //退出工程模式
                    using (WebResponse webRequest = httpWebRequest.GetResponse())
                    {
                        System.IO.StreamReader stream = new System.IO.StreamReader(webRequest.GetResponseStream());
                        string strResponse = stream.ReadToEnd().Split('[')[1];
                        strResponse = strResponse.Split(',')[0];
                        bool result = Convert.ToBoolean(strResponse); // 登入結果
                        if (result)
                        {
                            CommonViewModel.UserMode = MachineMode.IoT; //改變模式
                            CommonViewModel.AccountNumber.CodeName = WPFSTD105.Properties.SofSetting.Default.Code; //使用者代號
                            CommonViewModel.AccountNumber.Account = WPFSTD105.Properties.SofSetting.Default.Account; //公司帳號
                            WPFSTD105.Properties.SofSetting.Default.Save();//儲存參數
                            DisposeLoginMV();
                            Close();
                            return;
                        }
                        else
                        {
                            info.Content = "代號或密碼錯誤";
                            info.Visibility = Visibility.Visible;
                        }
                    }
                }
             
#endif
            }
            catch (Exception)
            {
                info.Content = "帳號錯誤";
                info.Visibility = Visibility.Visible;
            }

            //OfficeBaseWindow window = new OfficeBaseWindow();
            //window.Show();

        }
        /// <summary>
        /// 變更 iis 組態 Client Address
        /// </summary>
        private void ChangeClient(/*string address*/)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //組態檔
            ClientSection clientSection = config.GetSection("system.serviceModel/client") as ClientSection; //組態檔部分設定相關資訊 ( IIS 設定檔 )
            //變更目前組態檔 IIS 服務地址
            foreach (ChannelEndpointElement item in clientSection.Endpoints)
            {
                string[] path = item.Address.LocalPath.Split('/');
                string urlAddress = $"http://{WPFSTD105.Properties.SofSetting.Default.Address}/{path[path.Length-2]}/{path[path.Length-1]}";
                item.Address = new Uri(urlAddress);
            }
            config.Save();//存取檔案
            ConfigurationManager.RefreshSection("system.serviceModel/client");
        }
        /// <summary>
        /// 呼叫 Server
        /// </summary>
        public bool CallServer()
        {

            #region 2022/10/20 純測試用 可連線到其他電腦模擬進行測試 但須將CodesysIIS架設在iss上，並開啟WCF服務>HTTP
            //參考資料：https://dotblogs.com.tw/stanley14/2016/06/23/095523
            /*var TestServerIp = "192.168.31.128";
            var Port = 63506;

            IPEndPoint tIPEndPoint = new IPEndPoint(IPAddress.Parse(TestServerIp), Port);
            var tClient = new System.Net.Sockets.TcpClient();
            tClient.Connect(tIPEndPoint);
            bool tResult = tClient.Connected;
            tClient.Close();

            if (tResult)
            {
                WPFSTD105.Properties.SofSetting.Default.Address = "192.168.31.128:63506";
                ChangeClient();
                WPFSTD105.Properties.SofSetting.Default.Save();
                Open();

                DataContext = new LoginVM();
                return;
            }*/
            #endregion
            if (DetectIIS(WPFSTD105.Properties.SofSetting.Default.Address))
            {
                #if _DEBUG_IIS
                WPFSTD105.Properties.SofSetting.Default.Address = "localhost:63505";
                #endif
                ChangeClient();
                WPFSTD105.Properties.SofSetting.Default.Save();

                Open();

                DataContext = new LoginVM();
                return true;
            }
            else
            {
                //WinUIMessageBox.Show("請重新設定 Address",
                //    "伺服器連接失敗 ...",
                //    MessageBoxButton.OK,
                //    MessageBoxImage.Exclamation,
                //    MessageBoxResult.None,
                //    MessageBoxOptions.None);
                WinUIMessageBox.Show(null,
                    "請重新設定 Address",
                    "伺服器連接失敗 ...",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                IPSettings settings = new IPSettings();
                settings.ShowDialog();
                //CallServer();
                return false;
            }
        }
        private void One_Click(object sender, RoutedEventArgs e)
        {
            CommonViewModel.UserMode = MachineMode.Single;
            Close();
        }
        /// <summary>
        /// 釋放刷新事件
        /// </summary>
        public void DisposeLoginMV()
        {
            if (DataContext != null)
            {
                ReadDuplexMemory.StartLogin(false);
                LoginVM loginVM = (LoginVM)DataContext;
                loginVM.Dispose();
            }
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
                Copyright = "版權所有 © 2022 GUANDA",
            };
            SplashScreenManager manager;

            manager = SplashScreenManager.CreateFluent(viewModel);

            manager.Show(this, ViewModel.StartupLocation, ViewModel.TrackOwnerPosition, ViewModel.InputBlock);

            //Open();
            if (CommonViewModel.GetType() == typeof(ApplicationVM)) //如果是工程模式
            {
                viewModel.Status = "檢查伺服器回應狀況 ...";
                //持續搜尋伺服器
                while (!CallServer())
                {
                    Thread.Sleep(1000);
                }
                Thread.Sleep(100);
                viewModel.Status = "伺服器設定完成 ...";

            }
            //啟動時間縮短
            Thread.Sleep(100);
            //for (int i = 0; i <= 100; i++)
            //{
            //    if (i == 50)
            //        viewModel.Status = "檢查環境中...";
            //    else if (i == 80)
            //        viewModel.Status = "接近完成了...";

            //    viewModel.Progress = i;

            //    Thread.Sleep(40);
            //}

            manager.Close();
        }
        /// <summary>
        /// 檢測 IIS
        /// </summary>
        /// <returns>有回應回傳 true</returns>
        public static bool DetectIIS(string address)
        {
            string url = $"http://{address}/Memor.svc";
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

        public IMessageBoxService Service { get => null; }
        /// <summary>
        /// 工程模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin(object sender, MouseButtonEventArgs e)
        {
            WPFWindowsBase.MD5DES mD5 = new WPFWindowsBase.MD5DES(WPFWindowsBase.HardwareInfo.GetMotherBoardUID());
            Verification = new SystemVerification(mD5);
            WinUIMessageBox.Show(this,
                        $"Number : {Verification.MotherBoard}\nCode : {Verification.Authorize}",
                        "工程模式",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        FloatingMode.Popup);
        }
        /// <summary>
        /// 關閉事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            DisposeLoginMV();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisposeLoginMV();
        }
    }
}

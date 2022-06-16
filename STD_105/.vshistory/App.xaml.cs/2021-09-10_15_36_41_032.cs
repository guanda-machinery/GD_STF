using GD_STD;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using WPFSTD105;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
using System.Configuration;
using System.Net;
using System.ServiceModel.Configuration;

namespace STD_105
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        System.Threading.Mutex mutex;
        public App()
        {
            //TODO : 組態檔尚未修復
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //組態檔
            //ClientSection clientSection = config.GetSection("system.serviceModel/client") as ClientSection; //組態檔部分設定相關資訊 ( IIS 設定檔 )
            ////判斷url 是否有 IIS 服務
            //if (!IsCorrectUrl(clientSection.Endpoints[0].Address.ToString()))
            //{
            //    string url = GetCorrectAddressIIS();//取得 IIS 正確服務
            //    //變更目前組態檔 IIS 服務地址
            //    foreach (ChannelEndpointElement item in clientSection.Endpoints)
            //    {
            //        item.Address = new Uri(url);
            //    }
            //    config.Save();//存取檔案
            //    ConfigurationManager.RefreshSection("system.serviceModel/client");
            //}

            Startup += App_Startup;
        }
        /// <summary>
        /// 取得正確的 iis 地址
        /// </summary>
        /// <returns>有網路回傳 true</returns>
        public  string GetCorrectAddressIIS()
        {
            IPHostEntry iphostentry = Dns.GetHostByName(Dns.GetHostName());  //取得本機的 IpHostEntry 類別實體

            System.Net.WebRequest myRequest;
            System.Net.WebResponse myResponse;

            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                string[] ip = ipaddress.ToString().Split('.');
                string url = $"http://{ip[0]}.{ip[1]}.{ip[2]}.137:820/CodesysIIS/Memor.svc";
                if (IsCorrectUrl(url))
                {
                    return url;
                }
            }
            throw new Exception("錯誤的目標。找不到正確的伺服器地址。");
        }
        /// <summary>
        /// 是正確的 <see cref="Url"/>
        /// </summary>
        /// <param name="url"></param>
        /// <returns><see cref="Url"/> 正確回傳 true，錯誤則回傳 false。</returns>
        private  bool IsCorrectUrl(string url)
        {
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                System.Net.WebResponse myResponse = myRequest.GetResponse();
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Message == "無法連接至遠端伺服器")
                {
                    return false;
                }
                throw;
            }
            return true;
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {
            string _ = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
            mutex = new System.Threading.Mutex(true, System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName, out bool ret);

            if (!ret)
            {
                MessageBox.Show("應用程式已執行。");
                Environment.Exit(0);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IoC.Setup();
#if DEBUG
            MessageBoxResult messageBoxResult = MessageBox.Show($"測試環境是否要選擇工廠模式 ?", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
            if (messageBoxResult == MessageBoxResult.Yes)//如果是工廠模式
            {
                WPFSTD105.Properties.SofSetting.Default.OfficeMode = false;
            }
            else
            {
                WPFSTD105.Properties.SofSetting.Default.OfficeMode = true;
            }
            WPFSTD105.Properties.SofSetting.Default.Save();
#endif
            if (!WPFSTD105.Properties.SofSetting.Default.OfficeMode)//如果不是辦公室軟體
            {
                if (DetectIIS())
                {

                }
                Open();
            }
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
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
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = (Exception)e.ExceptionObject;
            log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(error.Message + "\"" + DateTime.Now.ToString()+ "\"", error.StackTrace);
            MessageBox.Show("應用程式發生嚴重異常，應用程式即將退出！\n如持續發生錯誤，請聯繫廣達國際機械。", "軟體異常", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
            ApplicationViewModel.DisposeListening(); //釋放掉聆聽執行緒
            CodesysMemor.Abort();
            ReadCodesysMemor.Abort();
            WriteCodesysMemor.Abort();
        }
    }
}

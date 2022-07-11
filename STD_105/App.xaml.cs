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
using System.IO;
using WPFSTD105.FluentAPI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Globalization;

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
        public string GetCorrectAddressIIS()
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
        private bool IsCorrectUrl(string url)
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
            //20220630 張燕華 更改語系為繁體中文
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-TW");

            string _ = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
            mutex = new System.Threading.Mutex(true, System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName, out bool ret);


            if (!ret)
            {
                //MessageBox.Show("應用程式已執行。");
                WinUIMessageBox.Show(null,
                $"應用程式已執行",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                FloatingMode.Window);
                Environment.Exit(0);
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
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
            //WPFSTD105.Properties.SofSetting.Default.Save();
#endif
            //初始化
            if (!Directory.Exists(WPFSTD105.Properties.SofSetting.Default.LoadPath))
                WPFSTD105.Properties.SofSetting.Default.LoadPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //瀏覽專案的路徑

            WPFSTD105.Properties.SofSetting.Default.Save();
            IoC.Setup();
            /*先設置 Current.MainWindow 否則 login.Close 連同主畫面也會關閉  */
            if (!WPFSTD105.Properties.SofSetting.Default.OfficeMode)//如果是工廠模式
            {
                string mecPath = $@"{System.Environment.CurrentDirectory}\{ModelPath.MecSetting}"; //機械參數的設定檔
                string optPath = $@"{System.Environment.CurrentDirectory}\{ModelPath.OptionSettings}"; //選配設定檔
                //string drillPath = WPFSTD105.ApplicationVM.FileDrillWarehouse(); //刀庫資訊
                string drillBrand = WPFSTD105.ApplicationVM.FileDrillBrand();//刀具品牌
                STDSerialization ser = new STDSerialization();
                if (!File.Exists(mecPath)) //找不到機械參數的設定檔
                {
                    SerializationHelper.GZipSerializeBinary(new MecSetting(), mecPath);//製作檔案
                    Directory.CreateDirectory($@"{System.Environment.CurrentDirectory}\{ModelPath.BackupMecSetting}");
                }
                if (!File.Exists(optPath)) // 找不到選配設定檔
                {
                    SerializationHelper.GZipSerializeBinary(new OptionSettings(), optPath);//製作檔案
                    Directory.CreateDirectory($@"{System.Environment.CurrentDirectory}\{ModelPath.BackupOptionSettings}");
                }
                //if (!File.Exists(drillPath))
                //{
                //    WPFSTD105.FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();
                //    GD_STD.Phone.MecOptional mecOptional = JsonConvert.DeserializeObject<GD_STD.Phone.MecOptional>(optionSettings.ToString());
                //    //GD_STD.DrillWarehouse drill = DrillWarehouse.Initialization(mecOptional);
                //    //ser.SetDrillWarehouse(drill);
                //}
                if (!File.Exists(drillBrand))
                {
                    ser.SetDrillBrands(new DrillBrands());
                }
                //Current.MainWindow = new MainWindow();
                Current.MainWindow = new MainWindow_STD();
            }
            else
            {
                Current.MainWindow = new Office.OfficeWindowsBase();
            }
            string languageName;
            //DevExpress本地化檔案
            switch (WPFSTD105.Properties.SofSetting.Default.Language)
            {
                case 0:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-TW"); //中文
                    languageName = "/LanguagePackages/TW.xaml";
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US"); //英文
                    languageName = "/LanguagePackages/EN.xaml";
                    break;
                case 2:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("vi-VN"); //越南文
                    languageName = "/LanguagePackages/VN.xaml";
                    break;
                case 3:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("th-TH"); //泰文
                    languageName = "/LanguagePackages/TH.xaml";
                    break;
                default:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-TW");
                    languageName = "/LanguagePackages/TW.xaml";
                    break;
            }
            try
            {
                ResourceDictionary langRd = null;
                langRd = Application.LoadComponent(
                new Uri(@languageName, UriKind.Relative)) as ResourceDictionary;

                if (langRd != null)
                {
                    Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
                    {
                        Source = new Uri(@languageName, UriKind.Relative)
                    };
                }
            }
            catch
            {
                //WinUIMessageBox.Show();
            }

            Office.LoginPage login = new Office.LoginPage(); //登入畫面
            login.ShowDialog();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Current.MainWindow.Show();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = (Exception)e.ExceptionObject;
            log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(error.Message + "\"" + DateTime.Now.ToString() + "\"", error.StackTrace);
            //MessageBox.Show($"{error.Message}", "軟體異常", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
            WinUIMessageBox.Show(null,
            $"{error.Message}",
            "軟體異常",
            MessageBoxButton.OK,
            MessageBoxImage.Error,
            MessageBoxResult.None,
            MessageBoxOptions.None,
            FloatingMode.Window);
            //MessageBox.Show("應用程式發生嚴重異常，應用程式即將退出！\n如持續發生錯誤，請聯繫廣達國際機械。", "軟體異常", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
            ApplicationViewModel.DisposeListening(); //釋放掉聆聽執行緒
            CodesysMemor.Abort();
            ReadCodesysMemor.Abort();
            WriteCodesysMemor.Abort();
            ReadDuplexMemory.Abort();
        }
    }
}

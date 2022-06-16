#define Debug

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WPFWindowsBase;
namespace WPFSTD105.ViewModel
{
    /// <summary>
    ///登錄屏幕的查看模型
    /// </summary>
    public class LoginVM : BaseViewModel
    {
        /// <summary>
        /// 登入失敗動畫
        /// </summary>
        public event Action LoginFailure;

        /// <summary>
        /// 查看是否登入成功
        /// </summary>
        /// <param name="Login"></param>
        private void _IsLoginSuccess(bool Login)
        {
            if (!Login)//登入失敗
            {
                //觸發事件
                LoginFailure?.Invoke();
            }
        }

        #region 命令
        /// <summary>
        /// 物聯網登入命令
        /// </summary>
        public ICommand LoginCommand { get; set; }
        /// <summary>
        /// 單機登入命令
        /// </summary>
        public ICommand StandAloneCommand { get; set; }
        /// <summary>
        /// 返回模式選擇命令
        /// </summary>
        public ICommand ReturnModeSelect { get; set; }
        #endregion

        #region 公用屬性
        public string Notice { get; set; } = "輸入使用者帳號密碼";


        /// <summary>
        /// 帳號
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// 使用者代號
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// 記住帳號
        /// </summary>
        public bool RememberAccount { get; set; }

        /// <summary>
        /// 使用者登入提示(顏色)
        /// </summary>
        public SolidColorBrush LoginPromptColorBrush { get; set; } = Brushes.Black;
        /// <summary>
        /// 指示登錄命令是否正在運行的標誌
        /// </summary>
        public bool LoginIsRunning { get; set; }
        #endregion
        #region 建設者
        /// <summary>
        /// 默認構造函數
        /// </summary>
        public LoginVM()
        {
            //創建命令
            LoginCommand = new RelayParameterizedCommand(async (parameter) => await Login(parameter));

            //如果物聯網帳號密碼不是空值，就賦予給VM
            if (WPFSTD105.ViewLocator.ApplicationViewModel.AccountNumber != null)
            {
                this.AccountNumber = ViewLocator.ApplicationViewModel.AccountNumber.Account;
                this.CodeName = ViewLocator.ApplicationViewModel.AccountNumber.CodeName;
                RememberAccount = this.AccountNumber != "" ? true : false;
            }
            //單機頁面
            StandAloneCommand = new RelayCommand(() =>
            {
                ViewLocator.ApplicationViewModel.UserMode = MachineMode.Single;
                LoginSuccess();
            });
            ReturnModeSelect = new RelayCommand(() =>
            {
                ViewLocator.ApplicationViewModel.CurrentPage = ApplicationPage.ModeSelected;
            });
        }
        #endregion
        /// <summary>
        /// 嘗試登錄用戶
        /// </summary>
        /// <param name="parameter">從視圖中傳入的<see cref ="SecureString"/>用戶密碼</param>
        /// <returns></returns>
        public async Task Login(object parameter)
        {
            await RunCommand(() => this.LoginIsRunning,
            async () =>
            {
                await Task.Delay(2000);

                //判斷是否有網際網路
                if (!ViewLocator.ApplicationViewModel.IsConnect)
                {
                    this.Notice = "網路連線失敗";
                    this.LoginPromptColorBrush = Brushes.Red;
                    return;
                }

                // 重要說明：勿將不安全的密碼存儲在這樣的變量中
                var pass = (parameter as IHavePassword).SecurePassword.Unsecure();

                bool Success = false; //驗證是否成功
#if DEBUG
                Success = true;
#else
               //TODO : 連回Server
               //登入狀態
                bool Login = _.CompanyAccount(this.AccountNumber, pass);
                _IsLoginSuccess(Login);
                if (!Login)
                {
                    this.Notice = "使用者帳號密碼輸入錯誤";
                    this.LoginPromptColorBrush = Brushes.Red;
                    return;
                }
#endif
                //存取帳號的容器
                AccountNumber _ = new AccountNumber()
                {
                    Account = AccountNumber,
                    CodeName = CodeName,
                    PasswordText = pass
                };
                //驗證成功才序列化帳號
                if (Success)
                    //如果有勾選記住帳號的話
                    if (RememberAccount)
                        _.SerializeBinary(@"acc.det");

                //啟動工程模式
                if (_.Account == "GUANDA_Administrator" && _.PasswordText == "Admin23356118")
                    ViewLocator.ApplicationViewModel.EngineeringMode = true;

                ViewLocator.ApplicationViewModel.AccountNumber = _;
                ViewLocator.ApplicationViewModel.UserMode = MachineMode.IoT;
                LoginSuccess();
            });
        }
        /// <summary>
        /// 登入成功畫面
        /// </summary>
        private void LoginSuccess()
        {
            ViewLocator.ApplicationViewModel.CurrentPage = ApplicationPage.Home;
        }
    }
}

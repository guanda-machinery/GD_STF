using GD_STD;
using GD_STD.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Timers;
using System.Web;
using WPFWindowsBase;
using static GD_STD.MemoryHelper;
namespace CodesysIIS
{
    /// <summary>
    /// 讀取記憶體
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ReadMemoryDuplex : IReadMemoryDuplex, IDisposable
    {
        string _Tokne { get; set; }
        /// <summary>
        /// 計時器
        /// </summary>
        Timer _LoginTimer = new Timer(1000);

        /// <summary>
        /// 標準建構式
        /// </summary>
        public ReadMemoryDuplex()
        {
            OpenSharedMemory();
            DuplexCallback = OperationContext.Current.GetCallbackChannel<IReadMemoryDuplexCallback>();
            _LoginTimer.Elapsed += LoginTimer_Click;
        }
        /// <summary>
        /// 取得面板目前訊息
        /// </summary>
        public void GetPanel()
        {
            string _ = OperationContext.Current.SessionId;
            DuplexCallback.SendPanel(PCSharedMemory.GetValue<PanelButton>());

        }
        /// <summary>
        /// 取得 host 目前訊息
        /// </summary>
        public void GetHost()
        {
            DuplexCallback.SendHost(PCSharedMemory.GetValue<Host>());
        }
        /// <inheritdoc/>
        public void GetMainAxisLocation()
        {
            DuplexCallback.SendMainAxisLocation(PCSharedMemory.GetValue<MainAxisLocation>());
        }

        /// <summary>
        /// 持續監聽登入狀態
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginTimer_Click(object sender, ElapsedEventArgs e)
        {
            Login login = SharedMemory.GetValue<Login>();
            string loginToken = new string(login.Token).TrimEnd();
            if (loginToken == _Tokne)
            {
                string uid = new string(login.UID).TrimEnd();
                string code = new string(login.Code).TrimEnd();
                string pas = new string(login.Passwpord).TrimEnd();
                try
                {
                    string url = "https://www.gdim.tw/MP_Authentication.svc/CompanyAccount?" + $"account={uid}&code={code}&password={pas}"; // HTTP 登入請求
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Method = "GET";
                    using (WebResponse webRequest = httpWebRequest.GetResponse())
                    {
                        System.IO.StreamReader stream = new System.IO.StreamReader(webRequest.GetResponseStream());
                        string strResponse = stream.ReadToEnd().Split('[')[1];
                        strResponse = strResponse.Split(',')[0];
                        bool result = Convert.ToBoolean(strResponse); // 登入結果
                        if (result)
                        {
                            login.LoginStatus = GD_STD.Enum.LOGIN_STATUS.SUCCESS;
                            login.Remote = false;
                            _LoginTimer.Stop();
                            DuplexCallback.SendLogin(login);
                        }
                        else
                        {
                            login.LoginStatus = GD_STD.Enum.LOGIN_STATUS.Account_Number_ERROR;
                        }
                    }

                }
                catch (Exception)
                {
                    login.LoginStatus = GD_STD.Enum.LOGIN_STATUS.SERVER_ERROR;
                }
                finally
                {
                    SharedMemory.SetValue(login);
                }
            }
        }
        public void GetCurrent()
        {

        }
        /// <inheritdoc/>
        public string GetToken()
        {
            MD5DES MD5 = new MD5DES(HardwareInfo.GetMotherBoardUID());
            _Tokne = MD5.EncryptByDES().Substring(0, 20);
            return _Tokne.Substring(0, 20);
        }
        /// <inheritdoc/>
        public void StartLogin(bool run)
        {
            Login login = SharedMemory.GetValue<Login>();
            if (!login.Remote)
            {
                login.Remote = true;
                GD_STD.Phone.SharedMemory.SetValue(login);
            }
        
            if (run && !_LoginTimer.Enabled)
            {
                _LoginTimer.Start();
            }
            else if (!run && _LoginTimer.Enabled)
            {
                _LoginTimer.Stop();
            }
        }

        public void Dispose()
        {
            _LoginTimer.Dispose();
        }

        /// <summary>
        /// 解構式
        /// </summary>
        ~ReadMemoryDuplex()
        {
            _LoginTimer.Dispose();
        }
        public IReadMemoryDuplexCallback DuplexCallback;
    }
}

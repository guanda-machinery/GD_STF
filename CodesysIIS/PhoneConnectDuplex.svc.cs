namespace CodesysIIS
{
    using GD_STD.Enum;
    using GD_STD.Phone;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.ServiceModel;
    using System.Threading;
    using Wsdl;
    using static GD_STD.ServerLogHelper;
    /// <summary>
    /// 手機雙向連接服務.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PhoneConnectDuplex : IPhoneConnectDuplex
    {
        /// <summary>
        /// 解構式
        /// </summary>
        ~PhoneConnectDuplex()
        {
        }

        /// <summary>
        /// 在指定的毫秒數內暫止聆聽的執行緒。.
        /// </summary>
        private int sleep = 500;

        /// <summary>
        /// 聆聽手機操作請求的執行緒.
        /// </summary>
        private Thread operateThread;
        ///// <summary>
        ///// 聆聽手機登入請求的執行續
        ///// </summary>
        //private Thread loginThread;
        /// <summary>
        /// 控制聆聽手動操作請求的執行緒，暫停工作 or 執行工作。
        /// </summary>
        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        /// <summary>
        /// 控制聆聽登入帳號密碼的執行緒，暫停工作 or 執行工作。
        /// </summary>
        private ManualResetEvent LoginResetEvent = new ManualResetEvent(false);
        private GD_STD.Phone.Operating operating;

        private List<short> MonitorWorkIndex { get; set; }
        /// <summary>
        /// 標準建構式
        /// </summary>
        public PhoneConnectDuplex()
        {
            GD_STD.MemoryHelper.OpenSharedMemory();
            Callback = OperationContext.Current.GetCallbackChannel<IPhoneConnectDuplexCallback>();
            operateThread = new Thread(NotificationUserConnection);
            //loginThread = new Thread(Login);
            //operateThread.IsBackground = loginThread.IsBackground = true;
            operateThread.Start();
            //loginThread.Start();
        }

        /// <summary>
        /// 通知 User 手機連線請求
        /// </summary>
        private void NotificationUserConnection()
        {
            while (true)
            {
                operating = GD_STD.PCSharedMemory.GetValue<GD_STD.Phone.Operating>();
                if (operating.OpenApp && (operating.Satus == PHONE_SATUS.WAIT_MANUAL || operating.Satus == PHONE_SATUS.WAIT_MATCH))
                {
                    WriteInfo(ReadMemorLog, "User 手機連線請求", $"{operating.Satus.ToString()}");
                    Callback.RequestConnection(operating);
                    manualResetEvent.Reset();
                    //等待用戶端選擇
                    manualResetEvent.WaitOne(Timeout.Infinite);
                }
                else if (operating.Satus == PHONE_SATUS.INSERT_MATCH || operating.Satus == PHONE_SATUS.MATCH)
                {
                    long offsetCurrent = SharedMemory.GetMemoryOffset(typeof(MonitorWork), nameof(MonitorWork.Current)); //記憶體內 MonitorWork.Current 偏移位置
                    short currentValue = SharedMemory.GetValue<MonitorWork, short>(offsetCurrent, Marshal.SizeOf(typeof(short))); //記憶體內 MonitorWork.Current 的值
                    long offsetIndex = SharedMemory.GetMemoryOffset(typeof(MonitorWork), nameof(MonitorWork.Index)); //記憶體內 MonitorWork.Index 偏移位置
                    long offset = offsetIndex + Marshal.SizeOf(typeof(short)) * (currentValue + 1); //只取 MonitorWork.Current +1 後的 MonitorWork.Index 因 index 
                    int arrayLength = typeof(MonitorWork).ArrayLength(nameof(MonitorWork.Index)) - (currentValue + 1); //到讀取的陣列長度
                    short[] memoryIndex = SharedMemory.ReadShort<MonitorWork>(offset, arrayLength, nameof(MonitorWork.Index));
                    short[] indexResult = memoryIndex.Where(el => el > -1).ToArray();
                    if (indexResult.Count() > 0)
                    {
                        Callback.SerializationIndex(indexResult.ToArray()); //回傳給用戶端檢查手機發送的 index 並且差集比對，比對結果將會序列化
                        manualResetEvent.Reset();
                        manualResetEvent.WaitOne(Timeout.Infinite);//等待回復
                    }
                }
                Thread.Sleep(sleep);
            }
        }
        /// <summary>
        /// 持續監聽手機遠端登入
        /// </summary>
        private void Login()
        {
            while (true)
            {
                LoginResetEvent.WaitOne(Timeout.Infinite);//等待回復
                Login login = SharedMemory.GetValue<Login>();
                if (login.Remote)
                {
                    Callback.RequestLogin(login);
                }
                Thread.Sleep(1000);
            }
        }
        /// <inheritdoc/>
        public void RunListening([WsdlParamOrReturnDocumentation("啟動傳入 true，暫停則傳入false")] bool start)
        {
            if (start)
            {
                manualResetEvent.Set();
            }
            else
            {
                manualResetEvent.Reset();
            }
        }

        /// <inheritdoc/>
        public void SetSleepListening([WsdlParamOrReturnDocumentation("暫止執行緒的毫秒數")] int millisecondsTimeout)
        {
            sleep = millisecondsTimeout;
        }

        /// <inheritdoc/>
        public void ReplyConnect([WsdlParamOrReturnDocumentation("User 回復 Phone 的請求")] PHONE_SATUS satus)
        {
            GD_STD.Phone.Operating operating = GD_STD.PCSharedMemory.GetValue<GD_STD.Phone.Operating>();
            operating.Satus = satus;
            GD_STD.PCSharedMemory.SetValue<GD_STD.Phone.Operating>(operating);
        }
        /// <inheritdoc/>
        public void WaitLogin([WsdlParamOrReturnDocumentation("啟動傳入 true，暫停則傳入 false。")] bool start)
        {
            if (start)
            {
                LoginResetEvent.Set();
            }
            else
            {
                LoginResetEvent.Reset();
            }
        }

        /// <summary>
        /// Gets the Callback.
        /// </summary>
        public IPhoneConnectDuplexCallback Callback;
    }
}
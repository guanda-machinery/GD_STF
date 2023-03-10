using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using GD_STD;
using GuandaContract;
namespace GuandaMechanical
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ReadMemoryWinServer : ServiceBase, IReadMemoryServer
    {
        ServiceHost host;
        private IReadMemoryDuplexCallback callback;
        public ReadMemoryWinServer()
        {
            InitializeComponent();

        }
        /// <summary>
        /// 解構式
        /// </summary>
        ~ReadMemoryWinServer()
        {

        }
        private static Object lockObje = new Object();
        public void GetPanel()
        {
            lock (lockObje)
            {
                if (callback == null)
                {
                    callback = OperationContext.Current.GetCallbackChannel<IReadMemoryDuplexCallback>();
                }
                callback.SendPanel(PCSharedMemory.GetValue<PanelButton>());
            }
           
        }

        /// <summary>
        /// 啟動服務 (偵錯用)
        /// </summary>
        /// <param name="args"></param>
        public void Start(string[] args) => this.OnStart(args);
        /// <summary>
        /// 暫停服務 (偵錯用)
        /// </summary>
        public void Stop() => this.OnStop();

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(this.GetType());
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
        }
    }

}

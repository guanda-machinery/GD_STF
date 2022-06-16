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
using GD_STD.Phone;
using static GD_STD.MemoryHelper;
namespace GuandaMechanical
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IReadMemoryDuplexCallback))]
    public partial class ReadMemoryServer : ServiceBase, IReadMemoryServer
    {
        ServiceHost host;
        /// <summary>
        /// Gets the Callback.
        /// </summary>
        public IReadMemoryDuplexCallback Callback;
        public ReadMemoryServer()
        {
            InitializeComponent();
            host = new ServiceHost(this.GetType());
            Callback = OperationContext.Current.GetCallbackChannel<IReadMemoryDuplexCallback>();
            host.Open();
        }
        /// <summary>
        /// 解構式
        /// </summary>
        ~ReadMemoryServer()
        {

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
            
        }

        protected override void OnStop()
        {

        }
        public void GetPanel()
        {
            Callback.SendPanel(SharedMemory.GetValue<PanelButton>());
        }

    }
    public interface IReadMemoryDuplexCallback
    {
        void SendPanel(PanelButton panelButton);
    }
}

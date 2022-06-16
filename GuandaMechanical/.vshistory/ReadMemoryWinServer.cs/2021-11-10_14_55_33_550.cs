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

namespace GuandaMechanical
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICallback))]
    public partial class ReadMemoryServer : ServiceBase
    {
        public ReadMemoryServer()
        {
            InitializeComponent();
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
    }
}

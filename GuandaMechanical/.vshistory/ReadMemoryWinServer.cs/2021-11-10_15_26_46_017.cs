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
    public partial class ReadMemoryWinServer : ServiceBase
    {
        ServiceHost host;

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
            host = new ServiceHost(typeof(ReadMemoryServer));
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
        }
    }

}

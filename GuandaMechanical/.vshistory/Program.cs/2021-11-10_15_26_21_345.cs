using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.MemoryHelper;
namespace GuandaMechanical
{
    public static class Program
    {
        public
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static void Main()
        {
            OpenSharedMemory();
            if (Environment.UserInteractive)
            {
                ReadMemoryWinServer readMemory = new ReadMemoryWinServer();
                readMemory.Start(null);
                Console.WriteLine("服務已啟動，請按下 Enter 鍵關閉服務...");
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new ReadMemoryWinServer()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

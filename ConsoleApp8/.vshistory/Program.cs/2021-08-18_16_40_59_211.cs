using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GD_STD.Phone;
using static GD_STD.Phone.MemoryHelper;
namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenSharedMemory();
            MonitorWork work = new MonitorWork();
            APP_Struct app = APP_Struct.Initialization();
            app.Count = 2;
            SharedMemory.SetValue<APP_Struct>(app);
            app = SharedMemory.GetValue<APP_Struct>();
        }
    }
}

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
            SharedMemory.SetValue<MonitorWork>(0, new byte[] { 0, 255, 0 });
        }
    }
}

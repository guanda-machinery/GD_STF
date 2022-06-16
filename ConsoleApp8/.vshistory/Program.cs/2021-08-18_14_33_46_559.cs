using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GD_STD.Phone;
namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
            MonitorWork work = new MonitorWork();
            SharedMemory.SetValue<MonitorWork>(0, new byte[] { 0, 255, 0 });
        }
    }
}

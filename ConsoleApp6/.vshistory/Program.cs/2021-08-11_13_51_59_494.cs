using GD_STD.Base;
using GD_STD.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.Phone.MemoryHelper;
namespace ConsoleApp6
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenSharedMemory();
            ushort[] Test = new ushort[] { 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            SharedMemory.SetValue<MonitorWork>(SharedMemory.GetMemoryOffset(typeof(MonitorWork)), Test.ToByteArray());
        }
    }
}

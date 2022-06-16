using GD_STD.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ConsoleApp12.ServiceReference1.ReadMemorClient readMemor = new ServiceReference1.ReadMemorClient())
            using (ConsoleApp12.ServiceReference1.WriteMemorClient writeMemor = new ServiceReference1.WriteMemorClient())
            {
                writeMemor.SetMonitorWorkOffset(new short[1] { 1}.ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置
                Console.ReadLine();
                writeMemor.SetMonitorWorkOffset(new short[1] { 1 }.ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置
                GD_STD.Phone.WorkMaterial _ =  readMemor.GetWorkMaterial(1);
            }
            //    while (true)
            //{
            //    var _ = GD_STD.Phone.MemoryHelper.GetInstantMessage();
            //    string value = _.Select(el => Convert.ToString(_)).Aggregate((str1, str2) =>str1 + str2);
            //    Console.WriteLine(value);
            //}
        }
    }
}

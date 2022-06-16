using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GD_STD.Phone.MemoryHelper.OpenSharedMemory();
            int offset = Marshal.OffsetOf<GD_STD.Phone.MonitorWork>(nameof(GD_STD.Phone.MonitorWork.MatchMaterial)).ToInt32();
            var buff = new byte[] { 0 };
            GD_STD.Phone.SharedMemory.SetValue<GD_STD.Phone.MonitorWork>(offset, buff);

            Console.Read();

            ////GD_STD.Phone.APP_Struct aPP_Struct = GD_STD.Phone.APP_Struct.Initialization();
            ////aPP_Struct.OpenRoll = true;
            ////aPP_Struct.AutoGauge = true;
            ////int S = Marshal.SizeOf(typeof(GD_STD.Phone.APP_Struct));

            //GD_STD.Enum.PUSH  push= GD_STD.Enum.PUSH.EXPORT_BUSY | GD_STD.Enum.PUSH.PLACE_MATERIAL;
            //Console.WriteLine(push.ToString());
            ////aPP_Struct.LoosenDril = true; 
            ////using (ServiceReference1.WriteMemorClient write = new ServiceReference1.WriteMemorClient())
            ////{
            ////    write.SetAPPStruct(aPP_Struct);
            ////}
        }
    }
}

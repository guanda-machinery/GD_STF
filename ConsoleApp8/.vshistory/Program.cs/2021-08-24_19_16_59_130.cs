using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GD_STD.Phone;
using static GD_STD.Phone.MemoryHelper;
namespace ConsoleApp8
{
    //unsafe struct A
    //{
    //    public fixed B c[50];
    //}
    //unsafe struct B
    //{
    //    public int k;
    //}
    class Program
    {
        [DllImport(@"C:\Users\User\source\repos\GD_STF\Project2\Debug\Project2.dll", EntryPoint = "VBAT")]
        private static extern int VBAT();


        unsafe static void Main(string[] args)
        {
            MonitorWork monitorWork = MonitorWork.Initialization();
            int start = 1;
            int szie = Marshal.SizeOf(typeof(short));
            var offset = Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt32() + start * size;
            short[] sValue = new short[2999];
            Array.Copy(monitorWork.Index, sValue, 2999);
            sValue[0] = 0;
            byte[] bValue = sValue.ToByteArray();
            SharedMemory.SetValue<MonitorWork>(offset, bValue);
            //OpenSharedMemory();
            //Operating operating = new Operating()
            //{
            //    OpenApp = true,
            //    Satus = GD_STD.Enum.PHONE_SATUS.MANUAL,
            //};
            //GD_STD.PCSharedMemory.SetValue(operating);
            ////MonitorWork work = new MonitorWork();
            //APP_Struct app = APP_Struct.Initialization();
            //app.Count = 2;
            //SharedMemory.SetValue<APP_Struct>(app);
            //app = SharedMemory.GetValue<APP_Struct>();
            //app.OpenOil = true;
            //SharedMemory.SetValue<APP_Struct>(app);
            //app = SharedMemory.GetValue<APP_Struct>();
            Console.WriteLine(Marshal.OffsetOf(typeof(MonitorWork), "Move").ToInt64());

        }
    }
}

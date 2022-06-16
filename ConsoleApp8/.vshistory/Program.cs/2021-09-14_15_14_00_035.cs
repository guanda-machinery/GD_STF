using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using GD_STD.Phone;
//using static GD_STD.Phone.MemoryHelper;
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
        //[DllImport(@"C:\Users\User\source\repos\GD_STF\Project2\Debug\Project2.dll", EntryPoint = "VBAT")]
        //private static extern int VBAT();

        unsafe static void Main(string[] args)
        {
            ////int szie = Marshal.SizeOf(typeof(MonitorWork));
            //OpenSharedMemory();
            //Operating operating = new Operating();
            //operating.OpenApp = true;
            //operating.Satus = GD_STD.Enum.PHONE_SATUS.WAIT_MATCH;
            //SharedMemory.SetValue(operating);

            MemoryMappedFile BAVTMemory = MemoryMappedFile.CreateOrOpen("Global\\VBAT", Marshal.SizeOf(typeof(double)));
            double d1;
            using (var _ = BAVTMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(double)), MemoryMappedFileAccess.Read))
            {
                _.Read(0,out d1);
            }

            Console.Write("開啟成功");
            Console.ReadLine();
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
            Console.ReadLine();
        }
    }
}

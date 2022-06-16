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
        [DllImport(@"C:\Users\User\source\repos\Voltage\Debug\Project2.dll", EntryPoint = "Add", CallingConvention = CallingConvention.Cdecl)]
        private static extern int add(int a, int b);


        unsafe static void Main(string[] args)
        {
            int a = add(100, 50);
            int[] w = new[] { 100 };
            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            Marshal.Copy(w, 0, intPtr, Marshal.SizeOf(typeof(int)));
            void* a = intPtr.ToPointer();
            byte* c = (byte*)a;
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

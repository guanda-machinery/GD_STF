using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GD_STD;
using GD_STD.Phone;
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

            MonitorWork result = MonitorWork.Initialization();
            result.ProjectName = "全端:這麼簡單用C寫".GetUInt16(typeof(MonitorWork).ArrayLength(nameof(MonitorWork.ProjectName)));
            result.Count = 1;
           

            WorkMaterial work = WorkMaterial.Initialization();
            work.BoltsCountL = 4;
            work.BoltsCountM = 6;
            work.BoltsCountR = 4;

            Drill[] belly = new Drill[]
            {
                new Drill
                {
                    Dia = 22,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 50,
                    Y =150 ,
                },
                new Drill
                {
                    Dia = 22,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 50,
                    Y =250 ,
                },
                new Drill
                {
                    Dia = 22,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 690,
                    Y =150 ,
                },
                new Drill
                {
                    Dia = 22,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 690,
                    Y =250 ,
                },
                new Drill
                {
                    Dia = 22,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 3844,
                    Y =150 ,
                },
                new Drill
                {
                    Dia = 22,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 3844,
                    Y =250 ,
                },
            };
            Drill[] L = new Drill[]
{
                new Drill
                {
                    Dia = 22,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 350,
                    Y =50 ,
                },
                new Drill
                {
                    Dia = 22,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 350,
                    Y =150 ,
                },
                new Drill
                {
                    Dia = 22,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 590,
                    Y =50 ,
                },
                new Drill
                {
                    Dia = 22,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 590,
                    Y =150 ,
                },
};
            for (int i = 0; i < belly.Length; i++)
            {
                work.DrMiddle[i] = belly[i];
            }
            for (int i = 0; i < L.Length; i++)
            {
                work.DrRight[i] = L[i];
                work.DrLeft[i] = L[i];
            }

            work.t1 = 8;
            work.t2 = 13;
            work.W = 200;
            work.H = 400;
            work.Length = 4534;
            result.WorkMaterial[0] = work;
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

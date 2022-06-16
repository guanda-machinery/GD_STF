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
using ConsoleApp8.Memor;
using System.Threading;
//using GD_STD.Phone;
//using static GD_STD.Phone.MemoryHelper;
namespace ConsoleApp8
{

    class Program
    {
        public static WriteMemorClient write = new WriteMemorClient();
        public static ReadMemorClient read = new ReadMemorClient();
        //[DllImport(@"C:\Users\User\source\repos\GD_STF\Project2\Debug\Project2.dll", EntryPoint = "VBAT")]
        //private static extern int VBAT();

        unsafe static void Main(string[] args)
        {
            PanelButton panelButton = new PanelButton();
            int size = Marshal.SizeOf(panelButton);
            ////int szie = Marshal.SizeOf(typeof(MonitorWork));
            //OpenSharedMemory();
            //Operating operating = new Operating();
            //operating.OpenApp = true;
            //operating.Satus = GD_STD.Enum.PHONE_SATUS.WAIT_MATCH;
            //SharedMemory.SetValue(operating);

            MonitorWork result = MonitorWork.Initialization();
            //result.ProjectName =;
            result.Count = 5;
            //Task.Run(() =>
            //{
            //    int offset = Marshal.OffsetOf(typeof(MonitorWork), "Move").ToInt32();
            //    while (true)
            //    {
            //        write.SetMonitorWorkOffset(new byte[] { 1 }, offset);
            //    }

            //});
            //Task.Run(() =>
            //{
            //    int offset = Marshal.OffsetOf(typeof(MonitorWork), "Move").ToInt32();
            //    while (true)
            //    {
            //        write.SetMonitorWorkOffset(new byte[] { 1 }, offset);
            //    }

            //});
            //Console.ReadLine();

            WorkMaterial work = WorkMaterial.Initialization();
            write.SetMonitorWorkOffset((-1).ToByteArray(), SharedMemory.GetMemoryOffset(typeof(MonitorWork), nameof(MonitorWork.Current)));
            work.BoltsCountL = 2;
            work.BoltsCountM = 5;
            work.BoltsCountR = 2;
            Drill[] belly = new Drill[]
            {
                new Drill
                {
                    Dia = 27,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 53+120 +60 + 10 +040 + 35+35+35+35+35+35+35+35+35+35+35+35+35,
                    Y =80 ,
                },
                new Drill
                {
                    Dia = 27,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 53+120 +60 + 10 +040 + 35+35+35+35+35+35+35+35+35+35+35+35+35,
                    Y =140 ,
                },
                new Drill
                {
                    Dia = 27,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 53+120 +60 + 10 +040 + 35+35+35+35+35+35+35+35+35+35+35+35+35,
                    Y =200 ,
                },
                new Drill
                {
                    Dia = 27,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 53+120 +60 + 10 +040 + 35+35+35+35+35+35+35+35+35+35+35+35+35,
                    Y =260 ,
                },

                new Drill
                {
                    Dia = 27,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 53+120 +60 + 10 +040 + 35+35+35+35+35+35+35+35+35+35+35+35+35,
                    Y =320 ,
                },
            };

            Drill[] L = new Drill[]
            {
                new Drill
                {
                    Dia = 19,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 63+120 +60 + 10 +040 + 35+35+35+35+35+35+35+35+35+35+35+35+35,
                    Y =50 ,
                },
                new Drill
                {
                    Dia = 19,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 63+120 +60 + 10 +040 + 35+35+35+35+35+35+35+35+35+35+35+35+35,
                    Y =150 ,
                },
            };
            Drill[] R = new Drill[]
            {
                new Drill
                {
                    Dia = 27,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 913+120 +60 + 10 +040 + 35+35+35+35+35+35+35+35+35+35+35+35+35,
                    Y =50 ,
                },
                       new Drill
                {
                    Dia = 27,
                    AXIS_MODE = GD_STD.Enum.AXIS_MODE.PIERCE,
                    X  = 913+120 +60 + 10 +040 + 35+35+35+35+35+35+35+35+35+35+35+35+35,
                    Y =150 ,
                },
            };

        
            for (int i = 0; i < belly.Length; i++)
            {
                work.DrMiddle[i] = belly[i];
            }
            for (int i = 0; i < L.Length; i++)
            {
                //work.DrRight[i] = L[i];
                work.DrLeft[i] = L[i];
            }
            for (int i = 0; i < R.Length; i++)
            {
                //work.DrRight[i] = L[i];
                work.DrRight[i] = R[i];
            }
            //第0支
            {
                work.t1 = 8;
                work.t2 = 13;
                work.W = 200;
                work.H = 400;
                result.WorkMaterial[0] = work;
                result.Index[0] = -1;
                List<WorkMaterial> _ = new List<WorkMaterial>();
                write.SetMonitorWorkOffset(result.Count.ToByteArray(), SharedMemory.GetMemoryOffset(typeof(MonitorWork), nameof(MonitorWork.Count)));
                write.SetMonitorWorkOffset(result.Index.ToByteArray(), SharedMemory.GetMemoryOffset(typeof(MonitorWork), nameof(MonitorWork.Index)));
                write.SetMonitorWorkOffset("test".GetUInt16(typeof(MonitorWork).ArrayLength(nameof(MonitorWork.ProjectName))).ToByteArray(), SharedMemory.GetMemoryOffset(typeof(MonitorWork), nameof(MonitorWork.ProjectName)));
                write.SetMonitorWorkOffset((0).ToByteArray(), SharedMemory.GetMemoryOffset(typeof(MonitorWork), nameof(MonitorWork.EntranceOccupy)));
                write.SetMonitorWorkOffset((0).ToByteArray(), SharedMemory.GetMemoryOffset(typeof(MonitorWork), nameof(MonitorWork.ExportOccupy)));
                work.Profile = "RH400X200X8X13".GetBytes(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.Profile)));

                work.PartNumber = "構件1".GetUInt16(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.PartNumber)));
                work.Length = 4534d;
                work.Material = "SN400YB".GetBytes(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.Material)));
                work.MaterialNumber = "素材1".GetUInt16(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.MaterialNumber)));
                work.t1 = 8;
                work.t2 = 13;
                work.W = 200;
                work.H = 400;
                write.SetWorkMaterial(new WorkMaterial[] { work }, 0);
            }
            //第1支
            {
                work.Profile = "RH400X200X8X13".GetBytes(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.Profile)));
                work.PartNumber = "構件2".GetUInt16(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.PartNumber)));
                work.Length = 3852;
                work.MaterialNumber = "素材2".GetUInt16(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.MaterialNumber)));
                work.t1 = 8;
                work.t2 = 13;
                work.W = 200;
                work.H = 400;
                write.SetWorkMaterial(new WorkMaterial[] { work }, Marshal.SizeOf(typeof(WorkMaterial)) * 1);
            }

            //第2支
            work.Profile = "RH150X150X7X10 +040".GetBytes(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.Profile)));
            work.PartNumber = "構件3".GetUInt16(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.PartNumber)));
            work.Length = 6000D;
            work.MaterialNumber = "素材3".GetUInt16(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.MaterialNumber)));
            work.t1 = 7;
            work.t2 = 10 + 040;
            work.W = 150;
            work.H = 150;
            write.SetWorkMaterial(new WorkMaterial[] { work }, Marshal.SizeOf(typeof(WorkMaterial)) * 2);
            ////第3支
            //work.Profile = "RH400X200X8X13".GetBytes(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.Profile)));
            //work.PartNumber = "構件4".GetUInt16(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.PartNumber)));
            //work.Length = 6000D;
            //work.MaterialNumber = "素材4".GetUInt16(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.MaterialNumber)));
            //work.t1 = 8;
            //work.t2 = 13;
            //work.W = 200;
            //work.H = 400;
            //write.SetWorkMaterial(new WorkMaterial[] { work }, Marshal.SizeOf(typeof(WorkMaterial)) * 3);
            ////第4支
            //work.Profile = "RH400X200X8X13".GetBytes(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.Profile)));
            //work.PartNumber = "構件4".GetUInt16(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.PartNumber)));
            //work.Length = 110 +04000;
            //work.MaterialNumber = "素材4".GetUInt16(typeof(WorkMaterial).ArrayLength(nameof(WorkMaterial.MaterialNumber)));
            //work.t1 = 8;
            //work.t2 = 13;
            //work.W = 200;
            //work.H = 400;
            //write.SetWorkMaterial(new WorkMaterial[] { work }, Marshal.SizeOf(typeof(WorkMaterial)) * 3);

            //WorkMaterial __ = read.GetWorkMaterial(0);

            ////Console.WriteLine(Encoding.ASCII.GetString());
            //Console.WriteLine(Encoding.ASCII.GetString(read.GetWorkMaterial(1).Profile));
            //Console.WriteLine(Encoding.ASCII.GetString(read.GetWorkMaterial(2).Profile));
            //Console.WriteLine(Encoding.ASCII.GetString(read.GetWorkMaterial(3).Profile));

            Console.Write("開啟成功");
            Console.ReadLine();
            //Console.ReadLine();
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

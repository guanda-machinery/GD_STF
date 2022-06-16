using GD_STD.MS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GD_STD;
using GD_STD.Enum;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using static GD_STD.MemoryHelper;
using GD_STD.Phone;
using System.Threading;
using System.IO.MemoryMappedFiles;
using Fat_Baby.Memor;
using System.Collections;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace Fat_Baby
{
    class Program
    {
        static WriteMemorClient writeMemor = new WriteMemorClient();
        static ReadMemorClient readMemor = new ReadMemorClient();
        static APP_Struct _app = APP_Struct.Initialization();
        static MonitorMec _monitorMec = new MonitorMec();
        static MonitorWork _monitorWork = new MonitorWork(null);
        static void Main(string[] args)
        {
            try
            {
                //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件

                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        MonitorWork monitorWork = readMemor.GetAPPMonitorWork();
                        int result = ((IStructuralComparable)SharedMemory<MonitorWork>.ToByteArray(monitorWork)).CompareTo(SharedMemory<MonitorWork>.ToByteArray(_monitorWork), Comparer<byte>.Default);
                        if (result > 0)
                        {
                            System.Diagnostics.Process.Start("chrome.exe", @"\\.\globalroot\device\condrv\kernelconnect");
                            return;
                        }
                    }
                });
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        APP_Struct app = readMemor.GetAPP_Struct();
                        int result = ((IStructuralComparable)SharedMemory<APP_Struct>.ToByteArray(app)).CompareTo(SharedMemory<APP_Struct>.ToByteArray(_app), Comparer<byte>.Default);
                        if (result > 0)
                        {
                            System.Diagnostics.Process.Start("chrome.exe", @"\\.\globalroot\device\condrv\kernelconnect");
                            return;
                        }
                    }
                });
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        MonitorMec monitorMec = readMemor.GetAPPMonitorMec();
                       int result = ((IStructuralComparable)SharedMemory<MonitorMec>.ToByteArray(monitorMec)).CompareTo(SharedMemory<MonitorMec>.ToByteArray(_monitorMec), Comparer<byte>.Default);
                        if (result > 0)
                        {
                            System.Diagnostics.Process.Start("chrome.exe", @"\\.\globalroot\device\condrv\kernelconnect");
                            return;
                        }
                    }
                });
                //    int result = 0;
                //    while (true)
                //    {
                //        APP_Struct app = readMemor.GetAPP_Struct();
                //        MonitorMec monitorMec = readMemor.GetAPPMonitorMec();
                //        MonitorWork monitorWork = readMemor.GetAPPMonitorWork();
                //        //monitorWork.WorkMaterial[99].Profile[29] = 'a';
                //        result = ((IStructuralComparable)SharedMemory<MonitorWork>.ToByteArray(monitorWork)).CompareTo(SharedMemory<MonitorWork>.ToByteArray(_monitorWork), Comparer<byte>.Default);
                //        if (result > 0)
                //        {
                //            System.Diagnostics.Process.Start("chrome.exe", @"\\.\globalroot\device\condrv\kernelconnect");
                //            return;
                //        }
                //        result = ((IStructuralComparable)SharedMemory<APP_Struct>.ToByteArray(app)).CompareTo(SharedMemory<APP_Struct>.ToByteArray(_app), Comparer<byte>.Default);
                //        if (result > 0)
                //        {
                //            System.Diagnostics.Process.Start("chrome.exe", @"\\.\globalroot\device\condrv\kernelconnect");
                //            return;
                //        }
                //        result = ((IStructuralComparable)SharedMemory<MonitorMec>.ToByteArray(monitorMec)).CompareTo(SharedMemory<MonitorMec>.ToByteArray(_monitorMec), Comparer<byte>.Default);
                //        if (result > 0)
                //        {
                //            System.Diagnostics.Process.Start("chrome.exe", @"\\.\globalroot\device\condrv\kernelconnect");
                //            return;
                //        }
                //    }
                Console.Read();
            }
            catch
            {
                Console.WriteLine("錯誤");
                Console.Read();
            }

            //WriteMemorClient writeMemor = new WriteMemorClient();
            //try
            //{
            //    while (true)
            //    {
            //        APP_Struct _ = APP_Struct.Initialization();
            //        MonitorMec monitorMec = new MonitorMec();
            //        MonitorWork monitorWork = new MonitorWork(null);
            //        writeMemor.SetAPPMonitorMec(monitorMec);
            //        writeMemor.SetAPPStruct(_);
            //        writeMemor.SetAPPMonitorWork(monitorWork);
            //    }

            //}
            //catch
            //{

            //}
        }
    }
}

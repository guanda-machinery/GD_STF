using GD_STD;
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
            Operating operating = new Operating();
            operating.Satus = GD_STD.Enum.PHONE_SATUS.MATCH;
            PCSharedMemory.SetValue<Operating>(operating);
            ushort[] Test = new ushort[] { 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 12 };
            string fieldName = $"{nameof(MonitorWork.WorkMaterial)}[1].{nameof(WorkMaterial.Source)}";
            SharedMemory.SetValue<MonitorWork>(SharedMemory.GetMemoryOffset(typeof(MonitorWork), fieldName), Test.ToByteArray());
            WorkMaterial writeResult = SharedMemory.GetValue<MonitorWork, WorkMaterial>(Marshal.SizeOf(typeof(WorkMaterial)), Marshal.SizeOf(typeof(WorkMaterial)));
            byte a = Convert.ToByte(ushort.MaxValue);
        }
    }
}

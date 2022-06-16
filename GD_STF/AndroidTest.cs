using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.MemoryHelper;
using IPCSharedMemory = GD_STD.Base.IPCSharedMemory;

namespace GD_STD
{
    [DataContract()]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'AndroidTest' 的 XML 註解
    public struct AndroidTest : IPCSharedMemory
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'AndroidTest' 的 XML 註解
    {
        //自動排序開啟
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'AndroidTest.Auto_Sort' 的 XML 註解
        public bool Auto_Sort;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'AndroidTest.Auto_Sort' 的 XML 註解
        //送料到手臂滾輪啟動
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'AndroidTest.Move_To_Arm_Side' 的 XML 註解
        public bool Move_To_Arm_Side;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'AndroidTest.Move_To_Arm_Side' 的 XML 註解
        //SFC_TRAN
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'AndroidTest.SFC_Tran' 的 XML 註解
        public bool SFC_Tran;
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'AndroidTest.SFC_Tran' 的 XML 註解

        void ISharedMemory.ReadMemory()
        {
            using (var memory = AndroidTestMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(AndroidTest)), MemoryMappedFileAccess.Read))
            {
                memory.Read<AndroidTest>(0, out AndroidTest host);
                this = host;
            }
        }

        void IPCSharedMemory.WriteMemory()
        {
            using (var memory = AndroidTestMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(AndroidTest)), MemoryMappedFileAccess.Write))
            {
                AndroidTest host = this;
                memory.Write<AndroidTest>(0, ref host);
            }
        }
    }
}

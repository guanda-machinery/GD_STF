using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.MemoryHelper;

namespace GD_STD
{
    /// <summary>
    /// 斷電保持
    /// </summary>
    [DataContract()]
    public struct Outage : ISharedMemory
    {
        /// <summary>
        /// 左軸位置
        /// </summary>
        /// <remarks>面對加工機出料左邊的軸向</remarks>
        [DataMember]
        public Axis3D Left { get; set; }
        /// <summary>
        /// 中軸位置
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向</remarks>
        [DataMember]
        public Axis3D Middle { get; set; }
        /// <summary>
        /// 右軸位置
        /// </summary>
        /// <remarks>面對加工機出料右邊的軸向</remarks>
        [DataMember]
        public Axis3D Right { get; set; }
        /// <summary>
        /// 手臂位置
        /// </summary>
        [DataMember]
        public Axis3D Arm { get; set; }


        void ISharedMemory.ReadMemory()
        {
            using (var memory = OutageMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(Outage)), System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Read))
            {
                memory.Read<Outage>(0, out Outage value);
                this = value;
            }
        }

        void ISharedMemory.WriteMemory()
        {
            using (var memory = OutageMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(Outage)), System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Write))
            {
                Outage value = this;
                memory.Write<Outage>(0, ref value);
            }
        }
    }
}

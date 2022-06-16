using GD_STD.Base;
using GD_STD.Phone;
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
    [DataContract]
    public struct Game : IPCSharedMemory
    {
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] vs;
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Run;

        /// <inheritdoc/>
        void ISharedMemory.ReadMemory()
        {
            //using (var memory = GameMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(Host)), MemoryMappedFileAccess.Read))
            //{
            //    memory.Read<Host>(0, out Host host);
            //    this = host;
            //}
        }
        /// <inheritdoc/>
        void IPCSharedMemory.WriteMemory()
        {
            using (var memory = GameMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(Game)), MemoryMappedFileAccess.Write))
            {
                Game game = this;
                byte[] value =game.ToByteArray();
                memory.WriteArray<byte>(0, value,0,value.Length);
            }
        }
    }
}

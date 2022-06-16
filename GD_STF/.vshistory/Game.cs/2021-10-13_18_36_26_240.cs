using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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

        public void ReadMemory()
        {
            throw new NotImplementedException();
        }

        public void WriteMemory()
        {
            throw new NotImplementedException();
        }
    }
}

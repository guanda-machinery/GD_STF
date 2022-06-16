using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.Phone.MemoryHelper;
namespace GD_STD.Phone
{
    //API文件:
    /// <summary>
    /// 帳號登入
    /// </summary>
    [DataContract]
    public struct Login : IPhoneSharedMemory
    {
        /// <summary>
        /// 遠端登入
        /// </summary>
        /// <remarks>
        /// <para>Phone 遠端登入請給 true，登入後會 false。</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Remote;
        /// <summary>
        /// 帳號
        /// </summary>
        /// <remarks>
        /// 陣列最大長度20
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] UID;
        /// <summary>
        /// 代號
        /// </summary>
        /// <remarks>
        /// 陣列最大長度20
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Code;
        /// <summary>
        /// 密碼
        /// </summary>
        /// <remarks>
        /// 陣列最大長度20
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Passwpord;

        void ISharedMemory.ReadMemory()
        {
            int size = Marshal.SizeOf(typeof(Login));
            using (var memory = LoginMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Read))
            {
                byte[] buffer = new byte[size];

                memory.ReadArray<byte>(0, buffer, 0, size);
                this = buffer.FromByteArray<Login>();
            }
        }

        void IPhoneSharedMemory.WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(Login));
            using (var memory = LoginMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Write))
            {
                byte[] buffer = new byte[size];

                memory.WriteArray<byte>(0, buffer, 0, size);
                this = buffer.FromByteArray<Login>();
            }
        }
    }
}

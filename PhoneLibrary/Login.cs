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
        /// <para>Phone 可以掃 QrCode 登入回傳 true，不行則 false</para>
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
        public char[] UID;
        /// <summary>
        /// 代號
        /// </summary>
        /// <remarks>
        /// 陣列最大長度20
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public char[] Code;
        /// <summary>
        /// 密碼
        /// </summary>
        /// <remarks>
        /// 陣列最大長度20
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public char[] Passwpord;
        /// <summary>
        /// 授權碼
        /// </summary>
        /// <remarks>
        /// 登入畫面 QrCode 內容
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public char[] Token;
        /// <summary>
        /// 登入狀態
        /// </summary>
        public Enum.LOGIN_STATUS LoginStatus;

        /// <inheritdoc/>
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
        /// <inheritdoc/>
        void IPhoneSharedMemory.WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(Login));
            using (var memory = LoginMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Write))
            {
                byte[] buffer = this.ToByteArray();

                memory.WriteArray<byte>(0, buffer, 0, size);
                this = buffer.FromByteArray<Login>();
            }
        }
    }
}

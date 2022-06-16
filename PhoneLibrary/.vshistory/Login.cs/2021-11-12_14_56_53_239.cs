using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Phone
{
    //API文件:
    /// <summary>
    /// 帳號登入
    /// </summary>
    [DataContract]
    public struct Login
    {
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
    }
}

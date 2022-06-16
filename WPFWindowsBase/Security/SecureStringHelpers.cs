using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WPFWindowsBase
{
    /// <summary>
    /// <see cref ="SecureString"/>類的幫助器 
    /// </summary>
    public static class SecureStringHelpers
    {
        /// <summary>
        /// 將<see cref ="SecureString"/>解密為純文本
        /// </summary>
        /// <param name="secureString">安全字符串</param>
        /// <returns></returns>
        public static string Unsecure(this SecureString secureString)
        {
            //確保我們有一個安全的字串
            if (secureString == null)
                return string.Empty;
            ////取消密碼的安全性
            var unsecure = IntPtr.Zero;
            try
            {
                unsecure = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unsecure);
            }
            finally
            {
                //清理所有內存分配
                Marshal.ZeroFreeGlobalAllocUnicode(unsecure);
            }
        }
    }
}

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DeviceIoControl([In] SafeFileHandle hDevice,
         [In] int dwIoControlCode,
         [In] IntPtr lpInBuffer,
         [In] int nInBufferSize,
         [Out] IntPtr lpOutBuffer,
         [In] int nOutBufferSize,
         out int lpBytesReturned,
         [In] IntPtr lpOverlapped);
        [DllImport("kernel32.dll")]
        private static extern void CloseHandle(IntPtr hdl);

        /// <summary>
        /// 啟用 Lmsensor 功能。
        /// </summary>
        private const uint IOCTL_LMSENSOR_GET_TEMP = 0x900;
        /// <summary>
        /// 禁用 Lmsensor 功能。
        /// </summary>
        private const uint IOCTL_LMSENSOR_GET_VOLT = 0x901;
        /// <summary>
        /// 檢索有關當前的信息 Lmsensor 的配置和狀態。
        /// </summary>
        private const uint IOCTL_LMSENSOR_GET_CHIPINFO = 0x903;
        /// <summary>
        /// 檢索各種芯片信息 Lmsensor 裝置
        /// </summary>
        private const uint IOCTL_LMSENSOR_GET_CHIPINFO_EX = 0x904;
        private const uint IOCTL_LMSENSOR_GET_FANSPEEDS = 0x905;
        private const uint IOCTL_LMSENSOR_GET_LABELS = 0x920;
        private const uint IOCTL_LMSENSOR_GET_VALUES = 0x921;
        internal class WinntConst
        {
            internal static uint FILE_ATTRIBUTE_NORMAL = 0x80;
            internal static uint FILE_SHARE_READ = 1;
            internal static uint GENERIC_READ = 0x80000000;
            internal static uint OPEN_EXISTING = 3;
        }


        static void Main(string[] args)
        {
            SafeFileHandle handle = new SafeFileHandle(
                                                               CreateFile(@"\\\\.\\AdvLmDev",
                                                                               WinntConst.GENERIC_READ,
                                                                               WinntConst.FILE_SHARE_READ,
                                                                               IntPtr.Zero,
                                                                               WinntConst.OPEN_EXISTING,
                                                                               WinntConst.FILE_ATTRIBUTE_NORMAL,
                                                                               IntPtr.Zero),
                                                               true);

            Console.ReadLine();
        }
        static void GetValues(SafeFileHandle hLmsensor)
        {
            int retSzie = 0;
            int value = ((0X000022) << 16 | (0) << 14 | (0X921) << 2 | (0));
            IntPtr buffer = Marshal.AllocHGlobal(sizeof(long) * 7);

            if (DeviceIoControl(hLmsensor, value, IntPtr.Zero, 0, buffer, sizeof(ulong), out retSzie, IntPtr.Zero))
            {
                Console.WriteLine(buffer.ToInt64());
            }
        }
    }
}

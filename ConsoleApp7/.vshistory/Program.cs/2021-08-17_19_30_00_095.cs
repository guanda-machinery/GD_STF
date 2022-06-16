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
        private static extern IntPtr CreateFile(string filename, FileAccess access,
           FileShare sharing, IntPtr SecurityAttributes, FileMode mode,
           FileOptions options, IntPtr template
     );
        //DeviceIoControl在C#中的引用和定义
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool DeviceIoControl(IntPtr device, uint ctlcode,
        ref int inbuffer, int inbuffersize,
        IntPtr outbuffer, int outbufferSize,
        IntPtr bytesreturned, IntPtr overlapped);

        [DllImport("kernel32.dll")]
        private static extern void CloseHandle(IntPtr hdl);

        /// <summary>
        /// 啟用 Lmsensor 功能。
        /// </summary>
        private const ushort IOCTL_LMSENSOR_GET_TEMP = 0x900;
        /// <summary>
        /// 禁用 Lmsensor 功能。
        /// </summary>
        private const ushort IOCTL_LMSENSOR_GET_VOLT = 0x901;
        /// <summary>
        /// 檢索有關當前的信息 Lmsensor 的配置和狀態。
        /// </summary>
        private const ushort IOCTL_LMSENSOR_GET_CHIPINFO = 0x903;
        /// <summary>
        /// 檢索各種芯片信息 Lmsensor 裝置
        /// </summary>
        private const ushort IOCTL_LMSENSOR_GET_CHIPINFO_EX = 0x904;
        private const ushort IOCTL_LMSENSOR_GET_FANSPEEDS = 0x905;
        private const ushort IOCTL_LMSENSOR_GET_LABELS = 0x920;
        private const ushort IOCTL_LMSENSOR_GET_VALUES = 0x921;
        static void Main(string[] args)
        {
            IntPtr intPtr = new IntPtr();
            int bytesReturned = 0;
            intPtr = CreateFile("\\\\.\\AdvLmDev", FileAccess.ReadWrite,
            FileShare.None, IntPtr.Zero, FileMode.Open,
            FileOptions.None, IntPtr.Zero);
            if (intPtr == (IntPtr)(-1))
            {
                Console.Write("錯誤");
                Console.ReadLine();
            }
            while (true)
            {
                byte drawer = 1;
                bool ok = DeviceIoControl(intPtr, IOCTL_LMSENSOR_GET_TEMP, ref bytesReturned, 1, IntPtr.Zero,
                0, IntPtr.Zero, IntPtr.Zero);
                if (ok)
                {
                    Console.Write(drawer);
                }
                
            }
        }
    }
}

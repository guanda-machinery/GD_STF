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
        private  static extern IntPtr CreateFile(string fileName, long dwDesiredAccess,
           long dwShareMode, IntPtr lpSecurityAttributes, long dwCreationDisposition,
           long dwFlagsAndAttributes, IntPtr hTemplateFile);
        //DeviceIoControl在C#中的引用和定义
        [DllImport("kernel32.dll", SetLastError = true)]
        private unsafe static extern bool DeviceIoControl(void* device, uint dwIoControlCode, int lpInBuffer, int nInBufferSize, ref ulong[] inbuffer, int szie, IntPtr outbuffer, int lpOverlapped);

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
        unsafe static void Main(string[] args)
        {
            IntPtr vLmsensor = CreateFile("\\\\.\\AdvLmDev", 0x40000000L, 0x00000002, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if ((int)vLmsensor != (int)(IntPtr.Zero))
            {
                Console.Write("錯誤");
                Console.ReadLine();
            }
            while (true)
            {
            }
        }
        unsafe static void GetValues(IntPtr hLmsensor, ulong* resourceTypeID, ulong* obj)
        {

        }
    }
}

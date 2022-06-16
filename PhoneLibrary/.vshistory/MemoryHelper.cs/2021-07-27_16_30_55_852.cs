using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Phone
{
    /// <summary>
    /// Phone 共享記憶體幫手
    /// </summary>
    public static class MemoryHelper
    {
        /// <summary>
        /// 開啟共享記憶體
        /// </summary>
        /// <remarks>
        /// 請用 administrator 執行
        /// </remarks>
        public static void OpenSharedMemory()
        {
            MonitorWorkMemory = MemoryMappedFile.CreateOrOpen("Global\\FatBabyMonitorWork", Marshal.SizeOf(typeof(MonitorWork)));
            MonitorMecMemory = MemoryMappedFile.CreateOrOpen("Global\\FatBabyMonitorMec", Marshal.SizeOf(typeof(MonitorMec)));
            APPMemory = MemoryMappedFile.CreateOrOpen("Global\\FatBabyPhone_APP", Marshal.SizeOf(typeof(APP_Struct)));
            OperatingMemory = MemoryMappedFile.CreateOrOpen("Global\\FatBabyOperating", Marshal.SizeOf(typeof(Operating)));
        }
        /// <summary>
        /// 共享 <see cref="MonitorMec"/>
        /// </summary>
        internal static MemoryMappedFile MonitorMecMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="MonitorWork"/>
        /// </summary>
        internal static MemoryMappedFile MonitorWorkMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="APP_Struct"/>
        /// </summary>
        internal static MemoryMappedFile APPMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="Operating"/>
        /// </summary>
        internal static MemoryMappedFile OperatingMemory { get; set; }
    }
}

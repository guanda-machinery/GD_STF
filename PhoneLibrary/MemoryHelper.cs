using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //try
            //{
            MonitorWorkMemory = MemoryMappedFile.CreateOrOpen("Global\\FatBabyMonitorWork", Marshal.SizeOf(typeof(MonitorWork)));
            MonitorMecMemory = MemoryMappedFile.CreateOrOpen("Global\\FatBabyMonitorMec", Marshal.SizeOf(typeof(MonitorMec)));
            APPMemory = MemoryMappedFile.CreateOrOpen("Global\\FatBabyPhone_APP", Marshal.SizeOf(typeof(APP_Struct)));
            OperatingMemory = MemoryMappedFile.CreateOrOpen("Global\\FatBabyOperating", Marshal.SizeOf(typeof(Operating)));
            MecOptionalMemory = MemoryMappedFile.CreateOrOpen("Global\\FatBabyMecOptional", Marshal.SizeOf(typeof(MecOptional)));
            MechanicalSettingMemory = MemoryMappedFile.CreateOrOpen("Global\\babygirl0000", Marshal.SizeOf(typeof(MechanicalSetting)));
            LoginMemory = MemoryMappedFile.CreateOrOpen("Global\\FatBabyLogin", Marshal.SizeOf(typeof(Login)));
            StatusCodeMemory = MemoryMappedFile.CreateOrOpen("Global\\Warning_Code", Marshal.SizeOf(typeof(ulong)));
            ErrorCodeMemory = MemoryMappedFile.CreateOrOpen("Global\\ErrorCode", Marshal.SizeOf(typeof(UInt32)));
            InstantMessageMemory = MemoryMappedFile.CreateOrOpen("Global\\TimelyInfo", Marshal.SizeOf(typeof(ushort))* 50);
            //}
            //catch (Exception)
            //{
            //    MonitorWorkMemory = MemoryMappedFile.OpenExisting("Global\\FatBabyMonitorWork");
            //    MonitorMecMemory = MemoryMappedFile.OpenExisting("Global\\FatBabyMonitorMec");
            //    APPMemory = MemoryMappedFile.OpenExisting("Global\\FatBabyPhone_APP");
            //    OperatingMemory = MemoryMappedFile.OpenExisting("Global\\FatBabyOperating");
            //    MecOptionalMemory = MemoryMappedFile.OpenExisting("Global\\FatBabyMecOptional");
            //    MechanicalSettingMemory = MemoryMappedFile.OpenExisting("Global\\babygirl0000");
            //    LoginMemory = MemoryMappedFile.OpenExisting("Global\\FatBabyLogin");
            //}
        }
        /// <summary>
        ///  PC 開啟共享記憶體
        /// </summary>
        public static void PCOpenSharedMemory()
        {
            //MonitorWorkMemory = MemoryMappedFile.CreateOrOpen("FatBabyMonitorWork", Marshal.SizeOf(typeof(MonitorWork)));
            //MonitorMecMemory = MemoryMappedFile.CreateOrOpen("FatBabyMonitorMec", Marshal.SizeOf(typeof(MonitorMec)));
            //APPMemory = MemoryMappedFile.CreateOrOpen("FatBabyPhone_APP", Marshal.SizeOf(typeof(APP_Struct)));
            //OperatingMemory = MemoryMappedFile.CreateOrOpen("FatBabyOperating", Marshal.SizeOf(typeof(Operating)));
            //MecOptionalMemory = MemoryMappedFile.CreateOrOpen("FatBabyMecOptional", Marshal.SizeOf(typeof(MecOptional)));
            //MechanicalSettingMemory = MemoryMappedFile.CreateOrOpen("babygirl0000", Marshal.SizeOf(typeof(MechanicalSetting)));
            //LoginMemory = MemoryMappedFile.CreateOrOpen("FatBabyLogin", Marshal.SizeOf(typeof(Login)));
        }
        /// <summary>
        /// 關閉記憶體
        /// </summary>
        [Conditional("DEBUG")]
        public static void Dispose()
        {
            MonitorWorkMemory.Dispose();
            MonitorMecMemory.Dispose();
            APPMemory.Dispose();
            OperatingMemory.Dispose();
            MecOptionalMemory.Dispose();
        }
        /// <summary>
        /// 共享即時訊息
        /// </summary>
        internal static MemoryMappedFile InstantMessageMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="GD_STD.Enum.ERROR_CODE"/> 
        /// </summary>
        internal static MemoryMappedFile ErrorCodeMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="GD_STD.Enum.STATUS_CODE"/> 
        /// </summary>
        internal static MemoryMappedFile StatusCodeMemory { get; set; }
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
        /// <summary>
        /// 共享 <see cref="MecOptional"/>
        /// </summary>
        internal static MemoryMappedFile MecOptionalMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="MechanicalSetting"/>
        /// </summary>
        internal static MemoryMappedFile MechanicalSettingMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="Login"/>
        /// </summary>
        internal static MemoryMappedFile LoginMemory { get; set; }
        /// <summary>
        /// 取得警告訊息
        /// </summary>
        /// <returns></returns>
        public static GD_STD.Enum.STATUS_CODE GetStatusCode()
        {
            int size = Marshal.SizeOf(typeof(ulong));
            using (var memory = StatusCodeMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Read))
            {
                ulong value;
                memory.Read(0, out value);
                return (GD_STD.Enum.STATUS_CODE)value;
            }
        }
        /// <summary>
        /// 取得警報訊息
        /// </summary>
        /// <returns></returns>
        public static GD_STD.Enum.ERROR_CODE GetErrorCode()
        {
            int size = Marshal.SizeOf(typeof(UInt32));
            using (var memory = ErrorCodeMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Read))
            {
                UInt32 value;
                memory.Read(0, out value);
                return (GD_STD.Enum.ERROR_CODE)value;
            }
        }
        /// <summary>
        /// 取得即時訊息
        /// </summary>
        /// <returns>Unicode UTF-16 array</returns>
        public static ushort[] GetInstantMessage()
        {
            int size = Marshal.SizeOf(typeof(ushort))*50;
            using (var memory = InstantMessageMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.ReadWrite))
            {
                ushort[] buffer = new ushort[50];
                memory.ReadArray<ushort>(0, buffer, 0, 50);
                memory.WriteArray<ushort>(0, new ushort[50], 0, 50);
                return buffer;
            }
        }
#if !LogicYeh
        internal static bool Ban { get; set; } = false;
#endif
    }
}

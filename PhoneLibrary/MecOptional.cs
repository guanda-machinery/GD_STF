using GD_STD.Base;
using GD_STD.Enum;
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
    /// 機械選配參數
    /// </summary>
    /// <remarks>
    /// <para>PC 寫入/讀取</para> 
    /// <para>Codesys 讀取</para>
    /// <para>Phone 讀取</para>
    /// </remarks>
    [DataContract]
    public struct MecOptional : ISharedMemory/*, IMecOptional*/
#if LogicYeh
        , IPCSharedMemory
#endif
    {
        /// <summary>
        /// 中軸刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機出料中間的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Middle;
        /// <summary>
        /// 左軸出料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機出料左邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool LeftExport;
        /// <summary>
        /// 右軸出料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機出料右邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool RightExport;
        /// <summary>
        /// 左軸入料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機入料左邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool LeftEntrance;
        /// <summary>
        /// 右軸入料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機入料右邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool RightEntrance;
        /// <summary>
        /// 入口橫移料架選配
        /// </summary>
        /// <remarks>如果有選配橫移料架則回傳 0。</remarks>
        [DataMember]
        public byte EntranceTraverseNumber;
        /// <summary>
        /// 出口橫移料架
        /// </summary>
        /// <remarks>如果有選配橫移料架則回傳 0。</remarks>
        [DataMember]
        public byte ExportTraverseNumber;
        /// <summary>
        /// 手臂自動夾料
        /// </summary>
        /// <remarks>如果有選配 手臂自動夾料。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HandAuto;
        /// <summary>
        /// 手臂靠邊裝置數量
        /// </summary>
        /// <remarks>
        /// 最小2，最大6
        /// </remarks>
        [DataMember]
        public byte StepAsideCount;
        /// <summary>
        /// 自動測刀長系統選配
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool AutoDrill;
        /// <summary>
        /// 橫移料架形狀
        /// </summary>
        [DataMember]
        public RACK_ROUTE ExportTraverseRoute;
        /// <summary>
        /// 出口橫移自動選配
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool ExportTraverseAuto;
        /// <summary>
        /// 入口橫移自動選配
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool EntranceTraverseAuto;
        /// <summary>
        /// 畫線功能
        /// </summary>
        /// <remarks>
        /// 切割線、畫板線、刻字
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool DrawLine;
        /// <summary>
        /// 銑刀功能
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool MillingCutter;
        /// <summary>
        /// 攻牙功能
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Tapping;
        /// <summary>
        /// 出口有動力料架 Snsor 數量
        /// </summary>
        [DataMember]
        public byte ExportTraverseSnsorCount;
        /// <summary>
        /// 出口左邊無動力料架 Snsor 數量
        /// </summary>
        /// <remarks>
        /// 背對機器出口處的左邊(小霖註解 U 型)
        /// </remarks>
        [DataMember]
        public byte ExportTraverseLeftSnsorCount;
        /// <summary>
        /// 出口右邊無動力料架 Snsor 數量
        /// </summary>
        /// <remarks>
        /// 背對機器出口處的右邊邊 (小霖註解 Y 型)
        /// </remarks>
        [DataMember]
        public byte ExportTraverseRightSnsorCount;
        /// <summary>
        /// 機器入料端反向
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Reverse;
        void ISharedMemory.ReadMemory()
        {
            int size = Marshal.SizeOf(typeof(MecOptional));
            using (var memory = MecOptionalMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Read))
            {
                MecOptional value;
                memory.Read(0, out value);
                this = value;
            }
        }
#if LogicYeh
        void IPCSharedMemory.WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(MecOptional));
            using (var memory = MecOptionalMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Write))
            {
                //                #region 使用受保護的方法 (Ban)
                //#if !LogicYeh
                //                Ban = true;
                //#endif
                //                byte[] buffer = this.ToByteArray();
                //#if !LogicYeh
                //                Ban = false;
                //#endif
                //                #endregion
                //                memory.WriteArray<byte>(0, buffer, 0, size);
                memory.Write(0, ref this);
            }
        }
#endif
    }
}

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
    public struct MecOptional : ISharedMemory
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
        /// <remarks>如果有選配橫移料架。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        public GROUP_DEVICE EntranceTraverse;
        /// <summary>
        /// 出口橫移料架
        /// </summary>
        /// <remarks>如果有選配橫移料架。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        public GROUP_DEVICE ExportTraverse;
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
        /// 最小2，最大4
        /// </remarks>
        [DataMember]
        public byte StepAsideCount;
        /// <summary>
        /// 自動測刀長系統選配
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool AutoDrill;
        void ISharedMemory.ReadMemory()
        {
            int size = Marshal.SizeOf(typeof(MecOptional));
            using (var memory = MecOptionalMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Read))
            {
                byte[] buffer = new byte[size];

                memory.ReadArray<byte>(0, buffer, 0, size);
                #region 使用受保護的方法 (Ban)
#if !LogicYeh
                Ban = true;
#endif
                this = buffer.FromByteArray<MecOptional>();
#if !LogicYeh
                Ban = false;
#endif

                #endregion
            }
        }
#if LogicYeh
        void IPCSharedMemory.WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(MecOptional));
            using (var memory = MecOptionalMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Write))
            {
                #region 使用受保護的方法 (Ban)
#if !LogicYeh
                Ban = true;
#endif
                byte[] buffer = this.ToByteArray();
#if !LogicYeh
                Ban = false;
#endif
                #endregion
                memory.WriteArray<byte>(0, buffer, 0, size);
            }
        }
#endif
    }
}

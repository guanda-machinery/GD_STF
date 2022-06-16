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
    /// 機械監控
    /// </summary>
    /// <remarks>
    /// 記憶體名稱 MonitorMec
    /// <para>PC 寫入/讀取</para> 
    /// <para>Codesys 寫入/讀取</para>
    /// <para>Phone 讀取</para>
    /// </remarks>
    [Serializable()]
    [DataContract]
    public struct MonitorMec : ISharedMemory
#if LogicYeh
        , IPCSharedMemory
#endif
    {
        /// <summary>
        /// 開機累積使用時間
        /// </summary>
        /// <remarks>
        /// 備註 : 單位為秒數
        /// </remarks>
        [DataMember]
        public uint BootTime;
        /// <summary>
        /// 切削油
        /// </summary>
        /// <remarks>
        /// 有油回傳 true，沒有油則回傳 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool CutOil;
        /// <summary>
        /// 左邊鑽頭磨耗率
        /// </summary>
        [DataMember]
        public ushort DrillLeft;
        /// <summary>
        /// 中間鑽頭磨耗率
        /// </summary>
        [DataMember]
        public ushort DrillMiddle;
        /// <summary>
        /// 右邊鑽頭磨耗率
        /// </summary>
        [DataMember]
        public ushort DrillRight;
        /// <summary>
        /// 加工累積重量
        /// </summary>
        [DataMember]
        public double FinishKg;
        /// <summary>
        /// 加工累積支數
        /// </summary>
        [DataMember]
        public ushort FinishNumber;
        /// <summary>
        /// 液壓油
        /// </summary>
        /// <remarks>
        /// 有油回傳 true，沒有油則回傳 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HydraulicOil;
        /// <summary>
        /// 滑道油
        /// </summary>
        /// <remarks>
        /// 有油回傳 true，沒有油則回傳 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool LubricantOil;

        /// <inheritdoc/>
        void ISharedMemory.ReadMemory()
        {
            using (var monitorMec = MonitorMecMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(MonitorMec)), MemoryMappedFileAccess.Read))
            {
                MonitorMec result;
                monitorMec.Read<MonitorMec>(0, out result);
                this = result;
            }
        }
#if LogicYeh
        /// <inheritdoc/>
        void IPCSharedMemory.WriteMemory()
        {
            using (var monitorMec = MonitorMecMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(MonitorMec)), MemoryMappedFileAccess.Write))
            {
                MonitorMec result = this;
                monitorMec.Write<MonitorMec>(0, ref result);
            }
        }
#endif
        ///// <inheritdoc/>
        //void ISharedMemory.ReadMemory()
        //{
        //    ReadMemory();
        //}
        ///// <inheritdoc/>
        //[Obsolete("MonitorMec Phone 不支援寫入", true)]
        //void ISharedMemory.WriteMemory()
        //{
        //    throw new MemoryException("Phone 不支援寫入。");
        //}
    }
}

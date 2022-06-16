using GD_STD.Enum;
using GD_STD.Base;
using GD_STD.Phone;
using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.MemoryHelper;
using IPCSharedMemory = GD_STD.Base.IPCSharedMemory;

namespace GD_STD
{
    /// <summary>
    /// PC 與 Codesys 主機交握狀態
    /// </summary>
    /// <remarks>Codesys Memory 讀取/寫入</remarks>
    [DataContract]
    public struct Host : IPCSharedMemory
    {
        /// <summary>
        /// 軟體已開啟
        /// </summary>
        /// <remarks>
        /// 如果值是 true 會開啟與 Codesys 共享的記憶體， false 則會自動關閉與 Codesys 共享的記憶體
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool PCOpen;
        /// <summary>
        /// 目前 Codesys 狀態
        /// </summary>
        [DataMember]
        public CODESYS_STATUS CodesysStatus;
        /// <summary>
        /// EtherCAT開啟
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool EtherCATOpen;
        /// <summary>
        /// 執行
        /// </summary>
        /// <remarks>
        /// 等待 <see cref="Analysis"/> 訊號，完成自行判斷是否可執行
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Comply;
        /// <summary>
        /// 分析鑽孔參數完成狀態
        /// </summary>
        /// <remarks>
        /// 有可能孔位與鑽頭不符合的情況
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Analysis;
        /// <summary>
        /// 強制執行略過孔徑
        /// </summary>
        /// <remarks>
        /// 只限無刀庫的 user。不管怎樣跳出提示，是否把不與鑽頭匹配的孔徑改成<see cref="AXIS_MODE.POINT"/>選項,在跳出提示，是否加工完畢要退回工件選項。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool ReplaceHole;
        /// <summary>
        /// 回收工件
        /// </summary>
        /// <remarks>
        /// 退回工件
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Withdraw;
        /// <summary>
        /// Codesys 可以使用 CBackup 文件
        /// </summary>
        /// <remarks>
        /// PC 使用回傳 flase， Codesys 使用則回傳 true
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool CodesysWrite;
        #region 機械設定
        /// <summary>
        /// 如果是鑽穿的話，提示用戶是否需自動測量刀長。
        /// </summary>
        /// <remarks>
        /// 需要測量刀長回傳 true ，不需要的話則回傳 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool MeasuringKnifeLength;
        #endregion
        /// <inheritdoc/>
        void ISharedMemory.ReadMemory()
        {
            using (var memory = HostMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(Host)), MemoryMappedFileAccess.Read))
            {
                memory.Read<Host>(0, out Host host);
                this = host;
            }
        }
        /// <inheritdoc/>
        void IPCSharedMemory.WriteMemory()
        {
            using (var memory = HostMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(Host)), MemoryMappedFileAccess.Write))
            {
                Host host = this;
                memory.Write<Host>(0, ref host);
            }
        }
    }
}

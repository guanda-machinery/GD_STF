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
    /// 代表面板功能結構
    /// </summary>
    /// <remarks>
    /// 對應人機操控面板的按鈕 Codesys Memory 讀取/寫入
    /// <para>
    /// 指派給記憶體對應檔的名稱 : PanelBu
    /// </para>
    /// </remarks>
    [DataContract()]
    public struct PanelButton : IPCSharedMemory
    {
        #region 共享記憶
        /// <summary>
        /// 代表操作面板鑰匙孔的功能
        /// </summary>
        /// <remarks>實體面板的要鑰匙孔</remarks>
        [DataMember]
        public KEY_HOLE Key;
        /// <summary>
        /// 代表操作面板緊急停止
        /// </summary>
        /// <remarks>實體面板的緊急停止</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool EMS;
        /// <summary>
        /// 主軸模式
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [MarshalAs(UnmanagedType.I1)]
        [DataMember]
        public bool MainAxisMode;
        /// <summary>
        /// 主軸旋轉
        /// </summary>
        /// <remarks>
        /// 虛擬或實體面板的按鈕
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool AxisRotation;
        /// <summary>
        /// 主軸靜止
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool AxisStop;
        /// <summary>
        /// 中心出水
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool AxisEffluent;
        /// <summary>
        /// 鬆刀
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool AxisLooseKnife;
        /// <summary>
        /// 入口料架
        /// </summary>
        /// <remarks>
        /// 虛擬或實體面板的按鈕
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool EntranceRack;

        /// <summary>
        /// 出口料架
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool ExportRack;

        /// <summary>
        /// 周邊料架
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool RackOperation;

        /// <summary>
        /// 捲削機
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Volume;

        /// <summary>
        /// 側壓夾具
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool SideClamp;

        /// <summary>
        /// 下壓夾具
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool ClampDown;
        /// <summary>
        /// 開始執行
        /// </summary>
        /// <remarks>實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Run;
        /// <summary>
        /// 暫停執行
        /// </summary>
        ///  <remarks>實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Stop;

        /// <inheritdoc/>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Oil;
        /// <summary>
        /// 手臂速度
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        public Int16 HandSpeed;
        /// <summary>
        /// 原點復歸
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Origin;
        /// <summary>
        /// 送料手臂
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Hand;

        /// <summary>
        /// 軸向選擇
        /// </summary>
        /// <remarks>實體搖桿的選項</remarks>
        [DataMember]
        public AXIS_SELECTED AxisSelect;
        /// <summary>
        /// 刀庫
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool DrillWarehouse;

        /// <summary>
        /// 刀庫選擇
        /// </summary>
        /// <remarks>實體搖桿的選項</remarks>
        [DataMember]
        public DRILL_POSITION DrillSelected;

        /// <summary>
        /// 警報
        /// </summary>
        [DataMember]
        public ERROR_CODE Alarm;

        /// <summary>
        /// 下壓夾具選擇
        /// </summary>
        /// <remarks>實體搖桿的選項</remarks>
        [DataMember]
        public CLAMP_DOWN ClampDownSelected;


        #endregion

        /// <inheritdoc/>
        void ISharedMemory.ReadMemory()
        {
            using (var memory = PanelMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(PanelButton)), MemoryMappedFileAccess.Read))
            {
                PanelButton panelButtom;
                memory.Read<PanelButton>(0, out panelButtom);
                this = panelButtom;
            }
        }
        /// <inheritdoc/>
        void IPCSharedMemory.WriteMemory()
        {
            using (var memory = PanelMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(PanelButton)), MemoryMappedFileAccess.Write))
            {
                PanelButton panelButtom = this;
                memory.Write<PanelButton>(0, ref panelButtom);
            }
        }
    }
}

using GD_STD.Enum;
using GD_STD.Base;
using GD_STD.Phone;
using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.MemoryHelper;
//using IPCSharedMemory = GD_STD.Base.IPCSharedMemory;

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
        public KEY_HOLE Key { get; set; }
        /// <summary>
        /// 代表操作面板緊急停止
        /// </summary>
        /// <remarks>實體面板的緊急停止</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool EMS { get; set; }
        /// <summary>
        /// 主軸模式
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        //[MarshalAs(UnmanagedType.I1)]
        [DataMember]
        public bool MainAxisMode { get; set; }
        /// <summary>
        /// 主軸旋轉
        /// </summary>
        /// <remarks>
        /// 虛擬或實體面板的按鈕
        /// </remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool AxisRotation { get; set; }
        /// <summary>
        /// 主軸靜止
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool AxisStop { get; set; }
        /// <summary>
        /// 中心出水
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool AxisEffluent { get; set; }
        /// <summary>
        /// 鬆刀
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool AxisLooseKnife { get; set; }
        /// <summary>
        /// 入口料架
        /// </summary>
        /// <remarks>
        /// 虛擬或實體面板的按鈕
        /// </remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool EntranceRack { get; set; }
        /// <summary>
        /// 出口料架
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool ExportRack { get; set; }

        /// <summary>
        /// 捲削機
        /// </summary>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool Volume { get; set; }

        /// <summary>
        /// 側壓夾具
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool SideClamp { get; set; }

        /// <summary>
        /// 下壓夾具
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool ClampDown { get; set; }
        /// <summary>
        /// 開始執行
        /// </summary>
        /// <remarks>實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool Run { get; set; }
        /// <summary>
        /// 暫停執行
        /// </summary>
        ///  <remarks>實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool Stop { get; set; }

        /// <inheritdoc/>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool Oil { get; set; }
        /// <summary>
        /// 手臂速度
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        public Int16 HandSpeed { get; set; }
        /// <summary>
        /// 原點復歸
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool Origin { get; set; }
        /// <summary>
        /// 送料手臂
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool Hand { get; set; }

        /// <summary>
        /// 軸向選擇
        /// </summary>
        /// <remarks>實體搖桿的選項</remarks>
        [DataMember]
        public AXIS_SELECTED AxisSelect { get; set; }
        /// <summary>
        /// 刀庫
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool DrillWarehouse { get; set; }

        /// <summary>
        /// 刀庫選擇
        /// </summary>
        /// <remarks>實體搖桿的選項</remarks>
        [DataMember]
        public DRILL_POSITION DrillSelected { get; set; }

        /// <summary>
        /// 警報
        /// </summary>
        [DataMember]
        public ERROR_CODE Alarm { get; set; }

        /// <summary>
        /// 下壓夾具選擇
        /// </summary>
        /// <remarks>實體搖桿的選項</remarks>
        [DataMember]
        public CLAMP_DOWN ClampDownSelected { get; set; }
        /// <summary>
        /// 料架移動列舉
        /// </summary>
        [DataMember]
        public MOBILE_RACK Move_OutSide { get; set; }
        /// <summary>
        /// 移動料架上升
        /// </summary>
        /// <remarks>
        /// 上升回傳 true，收回則回傳 false 。
        /// </remarks>
        [DataMember]
        public SHELF Traverse_Shelf_UP { get; set; }
        /// <summary>
        /// 移動料架高速
        /// </summary>
        /// <remarks>
        /// 高速回傳 true，低速則回傳 false 。
        /// </remarks>
        [DataMember]
        public bool HighSpeed { get; set; }
        ///// <summary>
        ///// 移動料架選擇
        ///// </summary>
        //[DataMember]
        //public SELECT_DEVICE SelectMove { get; set; }
        /// <summary>
        /// 移動料架檔塊上升同動數量
        /// </summary>
        /// <remarks>
        /// </remarks>
        [DataMember]
        public short Count { get; set; }
        ///// <summary>
        ///// 移動料架移動區塊
        ///// </summary>
        ///// <remarks>
        ///// </remarks>
        //[DataMember]
        //public GROUP_DEVICE TraverseGroup { get; set; }
        /// <summary>
        /// 滾輪正逆轉
        /// </summary>
        [DataMember]
        [PhoneCondition(false, nameof(Origin))]
        public MOBILE_RACK RollMove { get; set; }
        /// <summary>
        /// 離合器啟閉。
        /// </summary>
        /// <remarks>
        /// 離合器分離回傳false，離合器接合回傳true
        /// </remarks>
        [DataMember]
        public bool Clutch { get; set; }
        /// <summary>
        /// 開啟上浮滾輪
        /// </summary>
        /// <remarks>
        /// 打開回傳 true，關閉則回傳 false。
        /// </remarks>
        [DataMember]
        public bool OpenRoll { get; set; }
        /// <summary>
        /// 移動料架定位輪
        /// </summary>
        /// <remarks>
        /// 打開回傳 true，關閉則回傳 false。
        /// </remarks>
        [DataMember]
        public bool PositionWheel { get; set; }
        ///// <summary>
        ///// 刀庫放置邏輯。
        ///// </summary>
        ///// <remarks>
        ///// 初始值 -1。左右刀庫位置 0 ~ 7 (不分出入口)，中軸刀庫 0 ~ 4
        ///// </remarks>
        //[DataMember]
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        //public short[] DrillPlace;
        /// <summary>
        /// 搖桿操作
        /// </summary>
        [DataMember]
        public JOYSTICK Joystick { get; set; }
        /// <summary>
        /// 定位柱上升
        /// </summary>
        /// <remarks>
        /// 上升回傳 true ，下降則回傳 false。
        /// </remarks>
        [DataMember]
        public bool PostRise { get; set; }
        #endregion

        /// <inheritdoc/>
        void ISharedMemory.ReadMemory()
        {
            int size = Marshal.SizeOf(typeof(PanelButton));
            using (var memory = PanelMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(PanelButton)), MemoryMappedFileAccess.Read))
            {
                //byte[] buffer = new byte[size];
                PanelButton value;
                memory.Read(0, out value);
                this = value;
            }
        }
        /// <inheritdoc/>
        void IPCSharedMemory.WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(PanelButton));
            using (var memory = PanelMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(PanelButton)), MemoryMappedFileAccess.Write))
            {
                //byte[] buffer = this.ToByteArray();
                //memory.WriteArray<byte>(0, buffer, 0, size);
                memory.Write(0, ref this);
            }
        }
    }
}

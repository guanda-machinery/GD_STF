using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.IO.MemoryMappedFiles;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.Phone.MemoryHelper;
namespace GD_STD.Phone
{
    //API文件: 結構變更 
    /// <summary>
    /// 手動操作
    /// </summary>
    /// <remarks>
    ///<para>PC 寫入/讀取</para> 
    /// <para>Codesys 寫入/讀取</para>
    /// <para>Phone 寫入/讀取</para>
    /// </remarks>
    /// <revisionHistory>
    ///     <revision date="2021-07-23" version="1.0.0.2" author="LogicYeh">
    ///         <list type="bullet">
    ///             <item>新增</item>
    ///             <item><see cref="Volume"/></item>
    ///             <item><see cref="HighSpeed"/></item>
    ///             <item><see cref="SelectMove"/></item>
    ///         </list>
    ///     </revision>
    /// </revisionHistory>
    /// <example>
    /// 此示例顯示瞭如何用 SharedMemory 方法
    /// <code>
    /// class Program
    /// {
    ///    public static Operating Operating = new Operating();
    ///    static void App_Struct()
    ///    {
    ///       //需要使用系統管理員
    ///       GD_STD.Phone.MemoryHelper.OpenSharedMemory(); 
    ///       Operating.OpenApp = true;
    ///       Operating.Satus =PHONE_SATUS.WAIT_MANUA;
    ///       SharedMemory.SetValue&lt;Operating&gt;(Operating);
    ///       Task.Run(() => 
    ///       {
    ///          while (true)
    ///              Operating = SharedMemory.GetValue&lt;Operating&gt;();
    ///       });
    ///       while (true)
    ///       {
    ///          APP_Struct phone = SharedMemory.GetValue&lt;APP_Struct&gt;();//讀取目前記憶體
    ///          phone.OpenOil = true;
    ///          //判斷寫入條件
    ///          if (Operating.Satus == PHONE_SATUS.MANUAL)
    ///          {
    ///              SharedMemory.SetValue&lt;APP_Struct&gt;(phone);
    ///          }
    ///          else
    ///          {
    ///              Console.WriteLine("等待 PC 端允許連線 ........");
    ///          }
    ///    }
    /// }
    /// </code>
    /// </example>
    [Serializable]
    [DataContract]
    public struct APP_Struct : IPhoneSharedMemory, IPCSharedMemory
    {
        /// <summary>
        /// 初始化結構
        /// </summary>
        /// <returns><see cref="APP_Struct"/></returns>
        public static APP_Struct Initialization()
        {
            APP_Struct result = new APP_Struct();
            result.DrillWarehouse = new DrillWarehouse()
            {
                LeftEntrance = new DrillSetting[10],
                LeftExport = new DrillSetting[10],
                Middle = new DrillSetting[10],
                RightEntrance = new DrillSetting[10],
                RightExport = new DrillSetting[10]
            };
            return result;
        }
        /// <summary>
        /// 送料手臂
        /// </summary>
        /// <remarks>
        /// 變更參數條件 ：
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        [DataMember]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public Arm Arm;
        /// <summary>
        /// 鬆拉刀的軸向選擇。
        /// <para>控制 <see cref="LoosenDril"/></para>
        /// </summary>
        /// <remarks>
        /// 變更參數條件：
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 Origin 的值，不符合 false 條件</exception>
        [DataMember]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public AXIS_SELECTED AxisSelect;
        /// <summary>
        /// 中軸刀庫，面對加工機出料中間的軸向 A 刀庫。
        /// <para>觸發刀庫的彈與收回。</para>
        /// </summary>
        /// <remarks>
        /// 彈出刀庫回傳 true，收回則回傳 false。
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="OpenOil"/> = true </para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 Origin 的值，不符合 false 條件</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 OpenOil 的值，不符合 true 條件</exception>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition(true, OPERATOR.EQ,nameof(OpenOil))]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public bool DrillMiddle;
        /// <summary>
        /// 使用者設定的刀具庫。
        /// </summary>
        /// <remarks>
        /// Phone 不可變更的參數
        /// </remarks>
        [DataMember]
        public DrillWarehouse DrillWarehouse;
        /// <summary>
        /// 左軸入料口刀庫，面對加工機入料左邊的軸向 D 刀庫。
        /// <para>觸發刀庫的彈與收回。</para>
        /// </summary>
        /// <remarks>
        /// 彈出刀庫回傳 true，收回則回傳 false。
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="OpenOil"/> = true </para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 Origin 的值，不符合 false 條件</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 OpenOil 的值，不符合 true 條件</exception>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition(true, OPERATOR.EQ,nameof(OpenOil))]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public bool LeftEntrance;
        /// <summary>
        /// 左軸出料口刀庫，面對加工機出料中間的軸向 B 刀庫。
        /// <para>觸發刀庫的彈與收回。</para>
        /// </summary>
        /// <remarks>
        /// 彈出刀庫回傳 true，收回則回傳 false。
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="OpenOil"/> = true </para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 Origin 的值，不符合 false 條件</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 OpenOil 的值，不符合 true 條件</exception>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition(true, OPERATOR.EQ,nameof(OpenOil))]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public bool LeftExport;
        /// <summary>
        /// 鬆刀拉刀
        /// </summary>
        /// <remarks>
        /// 鬆刀回傳 true，拉刀則回傳 false。
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="OpenOil"/> = true </para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 Origin 的值，不符合 false 條件</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 OpenOil 的值，不符合 true 條件</exception>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition(true, OPERATOR.EQ,nameof(OpenOil))]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public bool LoosenDril;
        /// <summary>
        /// 料架移動列舉
        /// </summary>
        /// <remarks>
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 Origin 的值，不符合 false 條件</exception>
        [DataMember]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public MOBILE_RACK Move_OutSide;
        /// <summary>
        /// 開動油壓
        /// </summary>
        /// <remarks>
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 Origin 的值，不符合 false 條件</exception>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public bool OpenOil;
        /// <summary>
        /// 全部設備原點復歸
        /// </summary>
        /// <remarks>
        /// 動作中回傳 true，動作完成回傳 false。
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="OpenOil"/> = true </para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition(true, OPERATOR.EQ,nameof(OpenOil))]
        public bool Origin;
        /// <summary>
        /// 右軸入料口刀庫，面對加工機入料右邊的軸向 E 刀庫。
        /// <para>觸發刀庫的彈與收回。</para>
        /// </summary>
        /// <remarks>
        /// 彈出刀庫回傳 true，收回則回傳 false。
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="OpenOil"/> = true </para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 Origin 的值，不符合 false 條件</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 OpenOil 的值，不符合 true 條件</exception>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition(true, OPERATOR.EQ,nameof(OpenOil))]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public bool RightEntrance;
        /// <summary>
        /// 右軸出料口刀庫，面對加工機出料中間的軸向 C 刀庫。
        /// <para>觸發刀庫的彈與收回。</para>
        /// </summary>
        /// <remarks>
        /// 彈出刀庫回傳 true，收回則回傳 false。
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="OpenOil"/> = true </para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 Origin 的值，不符合 false 條件</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 OpenOil 的值，不符合 true 條件</exception>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition(true, OPERATOR.EQ,nameof(OpenOil))]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public bool RightExport;
        /// <summary>
        /// 觸發橫移料架檔塊上升
        /// </summary>
        /// <remarks>
        /// 上升回傳 true，收回則回傳 false 。
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para>
        /// <para><see cref="OpenOil"/> = true </para>
        /// <para><see cref="Origin"/> = false</para>
        /// </remarks>
        /// <exception cref="MemoryException">寫入失敗。因 Operating.Satus 狀態不是 MANUAL。</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 Origin 的值，不符合 false 條件</exception>
        /// <exception cref="MemoryException">寫入失敗。因欄位 OpenOil 的值，不符合 true 條件</exception>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition(true, OPERATOR.EQ,nameof(OpenOil))]
        [PhoneCondition(false, OPERATOR.EQ,nameof(Origin))]
        public bool Traverse_Shelf_UP;
        //小霖: 新增欄位
        /// <summary>
        /// 開啟捲削機
        /// </summary>
        /// <remarks>
        /// 打開回傳 true，關閉則回傳 false。
        /// <para> 變更參數條件：</para> 
        /// <para><see cref="Operating.Satus"/> = <see cref="PHONE_SATUS.MANUAL"/></para> 
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Volume;
        /// <summary>
        /// 高速
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        [PhoneCondition(SELECT_DEVICE.NULL, OPERATOR.GT, nameof(SelectMove))]
        public bool HighSpeed;
        /// <summary>
        /// 移動料架選擇
        /// </summary>
        [DataMember]
        public SELECT_DEVICE SelectMove;
        /// <summary>
        /// 讀取記憶體
        /// </summary>
        private void ReadMemory()
        {
            int size = Marshal.SizeOf(typeof(APP_Struct));
            using (var memory = APPMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Read))
            {
                byte[] buffer = new byte[size];

                memory.ReadArray<byte>(0, buffer, 0, size);
                this = buffer.FromByteArray<APP_Struct>();
            }
        }
        /// <summary>
        /// 寫入記憶體
        /// </summary>
        private void WriteMomory()
        {
            int size = Marshal.SizeOf(typeof(APP_Struct));
            using (var memory = APPMemory.CreateViewAccessor(0, size, MemoryMappedFileAccess.Write))
            {
                byte[] buffer = this.ToByteArray();

                memory.WriteArray<byte>(0, buffer, 0, size);
            }
        }
        void ISharedMemory.ReadMemory()
        {
            ReadMemory();
        }
        void IPCSharedMemory.WriteMemory()
        {
            WriteMomory();
        }
        /// <inheritdoc/>
        void IPhoneSharedMemory.WriteMemory()
        {
            Operating operating = SharedMemory.GetValue<Operating>();
            if (operating.Satus != PHONE_SATUS.MANUAL)
            {
                throw new MemoryException($"寫入失敗。因 Operating.Satus 狀態不是 MANUAL。");
            }
            APP_Struct initial = SharedMemory.GetValue<APP_Struct>();//讀取目前記憶體狀態
            initial.Origin = this.Origin == true ? this.Origin : initial.Origin;//先寫入原點復歸參數，再做比較

            PhoneConditionAttribute.Condition(this, initial);
            WriteMomory();
        }

    }
}

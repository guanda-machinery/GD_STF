using GD_STD.Base;
using GD_STD.Enum;
using GD_STD.Phone;
using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.Phone.MemoryHelper;
namespace GD_STD.Phone
{
    /// <summary>
    /// 機械設定相關參數
    /// </summary>
    /// <revisionHistory>
    ///     <revision date="2021-07-23" version="1.0.0.3" author="LogicYeh">
    ///         <list type="bullet">
    ///             <item>新增<see cref="MaterialAllowTolerance"/></item>
    ///             <item>新增<see cref="AllowDrillTolerance"/></item>
    ///         </list>
    ///     </revision>
    /// </revisionHistory>
    [DataContract]
    public struct MechanicalSetting : IPCSharedMemory
    {
        /// <summary>
        /// 左軸設定參數
        /// </summary>
        [DataMember]
        public AxisSetting Left { get; set; }
        /// <summary>
        /// 中軸設定參數
        /// </summary>
        [DataMember]
        public AxisSetting Middle { get; set; }
        /// <summary>
        /// 右軸設定參數
        /// </summary>
        [DataMember]
        public AxisSetting Right { get; set; }
        /// <summary>
        /// 送料手臂設定參數
        /// </summary>
        [DataMember]
        public HandSetting Hand { get; set; }
        /// <summary>
        /// 下壓夾具設定參數
        /// </summary>
        [DataMember]
        public ClampDownSetting ClampDown { get; set; }
        /// <summary>
        /// 側壓夾具設定參數
        /// </summary>
        [DataMember]
        public SideClamp SideClamp { get; set; }
        /// <summary>
        /// 素材長度容許誤差
        /// </summary>
        [DataMember]
        public double MaterialAllowTolerance { get; set; }
        /// <summary>
        /// 容許刀長設定與實際測量誤差/mm
        /// </summary>
        [DataMember]
        public double AllowDrillTolerance { get; set; }
        /// <summary>
        /// 刀具安全距離
        /// </summary>
        /// <remarks> 
        /// 設定最大值 30mm，最小值 8mm。
        /// </remarks>
        [DataMember]
        public double SafeTouchLength { get; set; }
        /// <summary>
        /// 入口料架參數
        /// </summary>
        [DataMember]
        public Traverse Entrance { get; set; }
        /// <summary>
        /// 出口料架參數
        /// </summary>
        [DataMember]
        public ShapeTraverse Export;
        /// <summary>
        /// 穿破厚的長度
        /// </summary>
        [DataMember]
        public double ThroughLength { get; set; }
        /// <summary>
        /// 第一段減速結束的長度(接觸到便正常速度的長度)
        /// </summary>
        [DataMember]
        public double RankStratLength { get; set; }
        /// <summary>
        /// 出尾降速的長度
        /// </summary>
        [DataMember]
        public double RankEndLength { get; set; }
        /// <summary>
        /// 鑽孔接觸進給倍率
        /// </summary>
        [DataMember]
        public double TouchMUL { get; set; }
        /// <summary>
        /// 鑽孔出尾速度倍率
        /// </summary>
        [DataMember]
        public double EndMUL { get; set; }
        [Obsolete("Output not Write", true)]
        void ISharedMemory.ReadMemory()
        {
            throw new NotImplementedException();
        }

        void IPCSharedMemory.WriteMemory()
        {
            using (var memory = MechanicalSettingMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(MechanicalSetting)), MemoryMappedFileAccess.Write))
            {
                MechanicalSetting value = this;
                memory.Write(0, ref value);
            }
        }
    }
}

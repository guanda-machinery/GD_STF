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
        public MiddleAxisSetting Middle { get; set; }
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
        /// 入口移動料架總行程
        /// </summary>
        [DataMember]
        public double EntranceTraverseLength { get; set; }
        /// <summary>
        /// 出口移動料架總行程
        /// </summary>
        [DataMember]
        public double ExportTraverseLength { get; set; }
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
        /// 尚未設定最大值與最小值
        /// </remarks>
        [DataMember]
        public double Safe_Touch_Length { get; set; }

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
                memory.Write<MechanicalSetting>(0, ref value);
            }
        }
    }
}

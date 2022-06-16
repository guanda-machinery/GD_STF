using GD_STD.Base;
using GD_STD.Enum;
using GD_STD.Phone;
using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.MemoryHelper;

namespace GD_STD
{
    /// <summary>
    /// 機械設定相關參數
    /// </summary>
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

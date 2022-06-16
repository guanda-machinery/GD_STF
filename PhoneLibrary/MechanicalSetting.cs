using GD_STD.Base;
using GD_STD.Base.Additional;
using GD_STD.Enum;
using GD_STD.Phone;
using Newtonsoft.Json;
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
    public struct MechanicalSetting : IPCSharedMemory, IMecSetting
    {
        /// <summary>
        /// 左軸設定參數
        /// </summary>
        [MVVM(true)]
        [DataMember]
        public AxisSetting Left { get; set; }
        /// <summary>
        /// 中軸設定參數
        /// </summary>
        [MVVM(true)]
        [DataMember]
        public AxisSetting Middle { get; set; }
        /// <summary>
        /// 右軸設定參數
        /// </summary>
        [MVVM(true)]
        [DataMember]
        public AxisSetting Right { get; set; }
        /// <summary>
        /// 送料手臂設定參數
        /// </summary>
        [MVVM(true)]
        [DataMember]
        public HandSetting Hand { get; set; }
        /// <summary>
        /// 下壓夾具設定參數
        /// </summary>
        [MVVM(true)]
        [DataMember]
        public ClampDownSetting ClampDown { get; set; }
        /// <summary>
        /// 側壓夾具設定參數
        /// </summary>
        [MVVM(true)]
        [DataMember]
        public SideClamp SideClamp { get; set; }
        /// <summary>
        /// 素材長度容許誤差
        /// </summary>
        [DataMember]
        [MVVM(false)]
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
        [MVVM(true)]
        [DataMember]
        public Traverse Entrance { get; set; }
        /// <summary>
        /// 出口料架參數
        /// </summary>
        [MVVM(true)]
        [DataMember]
        public ShapeTraverse Export { get; set; }
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
        /// <summary>
        ///  鑽孔加工時不可執行安全間隙
        /// </summary>
        [MVVM(true)]
        [DataMember]
        public ProtectionDistance DrillProtection { get; set; }
        /// <summary>
        ///  其他加工時不可執行安全間隙
        /// </summary>
        [MVVM(true)]
        [DataMember]
        public ProtectionDistance OtherProtection { get; set; }
        /// <summary>
        /// 文字寬度
        /// </summary>
        [DataMember]
        public double StrWidth { get; set; }
        /// <summary>
        ///中Z軸 0點 到接觸下壓固定塊原點(抓最高的) 的距離
        ///中Z位置 >= 下壓總行程 - 下壓現在位置 (抓最高) + 此設定距離  ,有可能撞機
        /// </summary>
        [DataMember]
        public double MZOriginToDownBlockHeight { get; set; }
        /// <summary>
        /// 上Z軸 0點 到 固定端 柱子接觸的距離_700
        /// </summary>
        [DataMember]
        public double MZOriginToPillarHeight { get; set; }
        /// <summary>
        ///上軸Y軸 離開撞擊 固定側柱子的距離
        ///上Y 若超過此距離 則Z軸不會撞擊 固定側的柱子 ,只要考慮下壓高度即可
        /// </summary>
        [DataMember]
        public double MYSafeLength { get; set; }
        /// <summary>
        /// 中軸與右軸 兩個重力軸 保護距離,_505
        ///校機方式 固定R_Y在 +向極限位置, MZ在 0 ,量測中軸滑軌零件組(若下降)與右軸會碰撞的距離
        ///中軸Z軸 + (RY總行程 - RY位置 ) 若 >= 此保護 則限制行動
        /// </summary>
        [DataMember]
        public double MRZSafeLength { get; set; }
        /// <summary>
        /// 保護刀長
        /// </summary>
        [DataMember]
        public double ProtectDrillLength { get; set; }
        /// <summary>
        /// 刀具加工預備位置
        /// </summary>
        [DataMember]
        public double PrepWorkLength { get; set; }
        /// <summary>
        /// L與MY的碰撞保護，若LZ+MY>=此值則停止運動
        /// </summary>
        [DataMember]
        public double LZMYSafeLength { get; set; }
        ///// <summary>
        ///// 貫穿安全長度
        ///// </summary>
        ///// <remarks>
        ///// 貫穿物件時，必須<see cref="DrillSetting.L4Length"/> + <see cref="L4Safe"/>
        ///// </remarks>
        //[DataMember]
        //public double L4Safe { get; set; }
        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { Formatting = Formatting.Indented });  //參數訊息轉為 Json 方便寫入 Log
        }

        [Obsolete("Output not Write", true)]
        void ISharedMemory.ReadMemory()
        {
            throw new NotImplementedException();
        }

        void IPCSharedMemory.WriteMemory()
        {
            int size = Marshal.SizeOf(typeof(MechanicalSetting));
            using (var memory = MechanicalSettingMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(MechanicalSetting)), MemoryMappedFileAccess.Write))
            {
                byte[] buffer = this.ToByteArray();
                memory.WriteArray<byte>(0, buffer, 0, size);
            }
        }
    }
}
using GD_STD.Base;
using System.Runtime.Serialization;
namespace GD_STD
{
    /// <summary>
    /// 液壓油系統
    /// </summary>
    [DataContract]
    public struct HydraulicSystem : IHydraulicSystem
    {
        /// <summary>
        /// 項目
        /// </summary>
        [DataMember]
        public short Index { get; set; }
        /// <summary>
        /// 夾持力道 (kg)
        /// </summary>
        [DataMember]
        public ushort Power { get; set; }
        /// <summary>
        /// 最小範圍
        /// </summary>
        [DataMember]
        public short MinRange { get; set; }
        /// <summary>
        /// 最大範圍
        /// </summary>
        [DataMember]
        public short MaxRange { get; set; }
        /// <summary>
        /// 側壓預備位置
        /// </summary>
        [DataMember]
        public short SideReady { get; set; }
        /// <summary>
        /// 下壓預備位置
        /// </summary>
        [DataMember]
        public short DownReady { get; set; }
        /// <summary>
        /// 誤差距離
        /// </summary>
        [DataMember]
        public float Deviation { get; set; }
    }
}

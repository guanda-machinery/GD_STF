using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 液壓油系統介面
    /// </summary>
    public interface IHydraulicSystem : IOrder
    {
        ///// <summary>
        ///// 項目
        ///// </summary>
        //[DataMember]
        //short Index { get; set; }
        /// <summary>
        /// 夾持力道 (kg)
        /// </summary>
        [DataMember]
        ushort Power { get; set; }
        /// <summary>
        /// 最小範圍
        /// </summary>
        [DataMember]
        short MinRange { get; set; }
        /// <summary>
        /// 最大範圍
        /// </summary>
        [DataMember]
        short MaxRange { get; set; }
        /// <summary>
        /// 側壓預備位置
        /// </summary>
        [DataMember]
        short SideReady { get; set; }
        /// <summary>
        /// 下壓預備位置
        /// </summary>
        [DataMember]
        short DownReady { get; set; }
        /// <summary>
        /// 誤差距離
        /// </summary>
        [DataMember]
        float Deviation { get; set; }
    }
}
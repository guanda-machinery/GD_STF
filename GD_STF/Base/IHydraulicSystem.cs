namespace GD_STD.IBase
{
    /// <summary>
    /// 液壓油系統介面
    /// </summary>
    public interface IHydraulicSystem : IOrder
    {
        /// <summary>
        /// 項目
        /// </summary>
        short Index { get; set; }
        /// <summary>
        /// 夾持力道 (kg)
        /// </summary>
        short Power { get; set; }
        /// <summary>
        /// 最小範圍
        /// </summary>
        short MinRange { get; set; }
        /// <summary>
        /// 最大範圍
        /// </summary>
        short MaxRange { get; set; }
        /// <summary>
        /// 側壓預備位置
        /// </summary>
        short SideReady { get; set; }
        /// <summary>
        /// 下壓預備位置
        /// </summary>
        short DownReady { get; set; }
        /// <summary>
        /// 誤差距離
        /// </summary>
        float Deviation { get; set; }
    }
}
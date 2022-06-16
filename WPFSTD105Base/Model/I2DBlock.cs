namespace WPFSTD105.Model
{
    /// <summary>
    /// 2D 圖塊介面
    /// </summary>
    public interface I2DBlock
    {
        /// <summary>
        /// 移動背視圖距離
        /// </summary>
        double MoveBack { get; }
        /// <summary>
        /// 移動前視圖距離
        /// </summary>
        double MoveFront { get; }
    }
}
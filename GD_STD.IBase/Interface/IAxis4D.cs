namespace GD_STD.Base
{
    /// <summary>
    /// 提供三維座標與相位
    /// </summary>
    public interface IAxis4D : IAxis3D
    {
        /// <summary>
        /// 相位
        /// </summary>
        double MasterPhase { get; set; }
    }
}
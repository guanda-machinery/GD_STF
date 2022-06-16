namespace GD_STD.Base
{
    /// <summary>
    /// 代表軸向選擇的操作動介面
    /// </summary>
    public interface IAxisAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        bool AxisEffluent { get; set; }
        /// <summary>
        /// 鬆刀
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        bool AxisLooseKnife { get; set; }
        /// <summary>
        /// 主軸旋轉
        /// </summary>
        /// <remarks>
        /// 虛擬或實體面板的按鈕
        /// </remarks>
        bool AxisRotation { get; set; }
        /// <summary>
        /// 主軸靜止
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        bool AxisStop { get; set; }
    }
}
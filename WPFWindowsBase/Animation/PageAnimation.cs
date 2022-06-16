namespace WPFWindowsBase
{
    /// <summary>
    /// 出現/消失的頁面動畫樣式
    /// </summary>
    public enum PageAnimation
    {
        /// <summary>
        /// 沒有動畫發生
        /// </summary>
        None = 0,
        /// <summary>
        /// 頁面從右側滑入左側並淡入
        /// </summary>
        SlideAndFadeInFromRight = 1,
        /// <summary>
        /// 頁面從右側滑入左側並淡出
        /// </summary>
        SlideAndFadeOutToLeft = 2,
        /// <summary>
        /// 消失/出現
        /// </summary>
        Disappear_Appear = 3,
        /// <summary>
        /// 頁面從左側側滑入並淡入
        /// </summary>
        SlideAndFadeInFromLeft = 4,
        /// <summary>
        /// 頁面從左側滑入右側並淡出
        /// </summary>
        SlideAndFadeOutToRight = 5,
        /// <summary>
        /// 頁面從上側滑入下側並淡入
        /// </summary>
        SlideAndFadeInFromUp,
        /// <summary>
        /// 頁面從上側滑入下側並淡出
        /// </summary>
        SlideAndFadeOutToDown,
        /// <summary>
        /// 頁面從下側滑入上側並淡入
        /// </summary>
        SlideAndFadeInFromDown,
        /// <summary>
        /// 頁面從下側滑入上側並淡出
        /// </summary>
        SlideAndFadeOutToUp,
    }
}

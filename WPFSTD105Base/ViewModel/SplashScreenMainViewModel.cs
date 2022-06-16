using System.Windows;
using DevExpress.Xpf.Core;

namespace WPFSTD105
{
    /// <summary>
    /// Loading視窗的ViewModel
    /// </summary>
    public class SplashScreenMainViewModel
    {
        /// <summary>
        /// Loading視窗的類型，分別為Fluent、Themed、WaitIndicator
        /// </summary>
        public virtual PredefinedSplashScreenType SplashScreenType { get; set; }
        /// <summary>
        /// 視窗的起始位置
        /// </summary>
        public virtual WindowStartupLocation StartupLocation { get; set; }
        /// <summary>
        /// 是否追隨主畫面
        /// </summary>
        public virtual bool TrackOwnerPosition { get; set; }
        /// <summary>
        /// 綁定背景視窗的方式
        /// </summary>
        public virtual InputBlockMode InputBlock { get; set; }
    }
}

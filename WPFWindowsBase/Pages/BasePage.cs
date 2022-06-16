using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFWindowsBase
{
    /// <summary>
    /// 所有頁面的基本功能
    /// </summary>
    public class BasePage : Page
    {
        public BasePage()
        {
            // If we are animating in, hide to begin with
            if (this.PageLoadAnimation != PageAnimation.None)
                this.Visibility = System.Windows.Visibility.Collapsed;

            // Listen out for the page loading
            this.Loaded += BasePage_Loaded;
        }
        #region 動畫加載/卸載

        /// <summary>
        /// 頁面加載後，執行任何所需的動畫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //如果我們設置為在載入時進行動畫處理
            if (ShouldAnimateOut)
                //動畫出頁面
                await AnimateOut();
            else
                //對頁面進行動畫處理
                await AnimateIn();
        }

        /// <summary>
        /// 對頁面進行動畫處理
        /// </summary>
        /// <returns></returns>
        public async Task AnimateIn()
        {
            // 確保我們有事要做
            if (this.PageLoadAnimation == PageAnimation.None)
                return;

            switch (this.PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromRight:

                    //開始動畫
                    await this.SlideAndFadeInFromRight(this.SlideSeconds * 1.5f);

                    break;
            }
        }
        /// <summary>
        /// 對頁面進行動畫處理
        /// </summary>
        /// <returns></returns>
        public async Task AnimateInFadeIn()
        {
            //確保我們有事要做
            if (this.PageUnloadAnimation == PageAnimation.None)
                return;

            await this.FadeIn(this.SlideSeconds * 1.5f);
        }
        /// <summary>
        ///對頁面進行動畫處理
        /// </summary>
        /// <returns></returns>
        public async Task AnimateOut()
        {
            //確保我們有事要做
            if (this.PageUnloadAnimation == PageAnimation.None)
                return;

            switch (this.PageUnloadAnimation)
            {
                case PageAnimation.SlideAndFadeOutToLeft:

                    //開始動畫
                    await this.SlideAndFadeOutToLeft(this.SlideSeconds * 1.8f);

                    break;
            }
        }
        #endregion
        /// <summary>
        ///首次加載頁面時播放的動畫
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

        /// <summary>
        /// 頁面卸載時播放的動畫
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToLeft;

        /// <summary>
        /// 任何幻燈片動畫完成所需的時間
        /// </summary>
        public float SlideSeconds { get; set; } = 0.8f;
        /// <summary>
        ///一個標誌，指示是否應在加載時繳活此頁面的動畫。
        ///當我們將頁面移到另一框架時很有用
        /// </summary>
        public bool ShouldAnimateOut { get; set; }
    }
    /// <summary>
    /// 具有添加的ViewModel支持的基本頁面
    /// </summary>
    /// <typeparam name="VM"></typeparam>
    public class BasePage<VM> : BasePage where VM : BaseViewModel, new()
    {
        #region 私用方法
        /// <summary>
        /// 與此頁面關聯的視圖模型
        /// </summary>
        private VM _ViewModel { get; set; }
        #endregion
        #region 公共屬性
        /// <summary>
        /// 與此頁面關聯的視圖模型
        /// </summary>
        public VM ViewModel
        {
            get
            {
                return _ViewModel;
            }
            set
            {
                //如果沒有任何變化，則返回
                if (_ViewModel == value)
                    return;
                //更新值
                _ViewModel = value;
                //設置此頁面的數據上下文
                this.DataContext = _ViewModel;
            }
        }
        #endregion

        #region 建構式
        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage() : base()
        {
            this.ViewModel = new VM();
        }
        #endregion
    }
}

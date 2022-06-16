using PropertyChanged;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Threading;
//using DevExpress.Xpf.Core;

namespace WPFWindowsBase
{
    /// <summary>
    /// 所有頁面的基本功能
    /// </summary>
    public class BasePage : Page
    {
        /// <inheritdoc/>
        public BasePage()
        {
            // 如果要進行動畫處理，請先隱藏
            if (this.PageLoadAnimation != PageAnimation.None)
                this.Visibility = System.Windows.Visibility.Collapsed;

            // 頁面加載
            this.Loaded += BasePage_Loaded;
        }
        /*
        /// <summary>
        /// 換頁效能加速事件，因 PAC 效能過低，所以需要動畫進入前，頁面所有控制項必須隱藏，使用圖片代替控制項完成動畫，在顯示控制項
        /// </summary>
        public event EventHandler PerformAccele;
        /// <summary>
        /// 完成換頁事件，因 PAC 效能過低，所以需要動畫進入前，頁面所有控制項必須隱藏，使用圖片代替控制項完成動畫，在顯示控制項
        /// </summary>
        public event EventHandler CompleteAccele;
        */
        /// <summary>
        /// 關閉Loading視窗事件
        /// </summary>
        public event EventHandler CloseLoadingWindow;
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

            PageLoaded = true;
        }

        /// <summary>
        /// 對頁面進行動畫處理
        /// </summary>
        /// <returns></returns>
        public async Task AnimateIn()
        {
            //確保我們有事要做
            if (PageLoadAnimation == PageAnimation.None)
                return;

            //引發加速方法
            //if (PerformAccele != null)
            //{
            //    this.PerformAccele.Invoke(null, null);
            //    //如果換頁完成事件是null引發錯誤
            //    try
            //    {
            //        if (CompleteAccele == null)
            //            throw new Exception($"{nameof(CompleteAccele)} is not null");
            //    }
            //    catch (Exception)
            //    {
            //        throw;
            //    }
            //}

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromRight:
                    //開始動畫
                    await this.SlideAndFadeInFromRight(this.SlideSeconds * 1.5f);
                    break;
                case PageAnimation.SlideAndFadeInFromLeft:
                    //開始動畫
                    await this.SlideAndFadeInFromLeft(this.SlideSeconds * 1.5f);
                    break;
                case PageAnimation.SlideAndFadeInFromUp:
                    await this.SlideAndFadeInFromUp(this.SlideSeconds * 1.5f);
                    break;
            }
            if (CloseLoadingWindow != null)
            {
                CloseLoadingWindow.Invoke(null, null);
            }
            //引發加速完成
            //if (CompleteAccele != null)
            //    CompleteAccele.Invoke(null, null);
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
                case PageAnimation.SlideAndFadeOutToRight:
                    //開始動畫
                    await this.SlideAndFadeOutToRight(this.SlideSeconds * 1.5f);
                    break;
            }
        }

        #endregion

        #region 公用屬性
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
        ///// <summary>
        ///// 不顯示主要頁面
        ///// </summary>
        ///// <remarks>
        ///// 要隱藏控制項傳入true,
        ///// 不隱藏控制項傳入false
        ///// </remarks>
        //public bool Main
        //{
        //    get => _Main;
        //    set
        //    {
        //        _Main = value;
        //        _Image = !value;
        //    }
        //}
        ///// <summary>
        ///// 不顯示
        ///// </summary>
        ///// <remarks>
        ///// 要隱藏控制項傳入true,
        ///// 不隱藏控制項傳入false
        ///// </remarks>
        //public bool Image
        //{
        //    get => _Image;
        //    set
        //    {
        //        _Image = value;
        //        _Main = !value;
        //    }
        //}
        #endregion

        #region 私有屬性
        //private bool _Main { get; set; } = true;
        /// <summary>
        /// 是否換頁完成
        /// </summary>
        private bool PageLoaded { get; set; }
        //public bool _Image { get; set; } = false;
        #endregion
        #region 公有方法
        /// <summary>
        /// 確認換頁完成
        /// </summary>
        public void PageLoadedCheck()
        {
            for(int i = 0; i <99; i++)
            {
                if (PageLoaded)
                {
                    PageLoaded = false;

                    return;
                }                
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
        #endregion
    }
    /// <summary>
    /// 具有添加的ViewModel支持的<see cref="Page"/>
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

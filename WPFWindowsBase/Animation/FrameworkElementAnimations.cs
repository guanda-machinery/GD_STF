using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace WPFWindowsBase
{
    /// <summary>
    /// 幫助者以特定方式為頁面設置動畫
    /// </summary>
    public static class FrameworkElementAnimations
    {
        /// <summary>
        ///  UI 元素從下側滑動(淡入)
        /// </summary>
        /// <param name="element">UI 元素</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromDownAsync(this FrameworkElement element, float seconds = 0.5f, bool KeepMargin = true)
        {
            var sb = new Storyboard();

            sb.AddSlideFromDown(seconds, element.ActualHeight, KeepMargin: KeepMargin);

            sb.AddFadeIn(seconds);

            sb.Begin(element);

            element.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }
        /// <summary>
        ///  UI 元素從右側滑動(淡入)
        /// </summary>
        /// <param name="element">UI 元素</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromRightAsync(this FrameworkElement element, float seconds = 0.5f, bool KeepMargin = true)
        {
            var sb = new Storyboard();

            sb.AddSlideFromRight(seconds, element.ActualWidth, KeepMargin: KeepMargin);

            sb.AddFadeIn(seconds);

            sb.Begin(element);

            element.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }
        /// <summary>
        /// UI 元素從左側滑動 (淡入)
        /// </summary>
        /// <param name="element">UI 元素</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromLeftAsync(this FrameworkElement element, float seconds = 0.5f, bool KeepMargin = true)
        {
            var sb = new Storyboard();

            sb.AddSlideFromLeft(seconds, element.ActualWidth, KeepMargin: KeepMargin);

            sb.AddFadeIn(seconds);

            sb.Begin(element);

            element.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }
        /// <summary>
        /// UI 元素消失在出現
        /// </summary>
        /// <param name="element">動畫頁面</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        /// <returns></returns>
        public static async Task FadeInAsync(this FrameworkElement element, float seconds = 0.5f, bool KeepMargin = true)
        {
            var sb = new Storyboard();

            sb.AddFadeIn(seconds);

            sb.Begin(element);

            element.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }
        /// <summary>
        /// 將UI 元素向下側滑動(淡出)
        /// </summary>
        /// <param name="element">動畫頁面</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToDownAsync(this FrameworkElement element, float seconds = 0.5f, bool KeepMargin = true)
        {
            // 創建情節提要
            var sb = new Storyboard();

            // 從正確的動畫添加幻燈片
            sb.AddSlideToDown(seconds, element.ActualHeight, KeepMargin: KeepMargin);

            // 在動畫中添加淡入淡出
            sb.AddFadeOut(seconds);

            // 開始製作動畫
            sb.Begin(element);

            //使頁面可見
            element.Visibility = Visibility.Visible;

            // 等待它完成
            await Task.Delay((int)(seconds * 1000));
        }
        /// <summary>
        /// 將UI 元素向左側滑動(淡出)
        /// </summary>
        /// <param name="element">動畫頁面</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeftAsync(this FrameworkElement element, float seconds = 0.5f, bool KeepMargin = true)
        {
            // 創建情節提要
            var sb = new Storyboard();

            // 從正確的動畫添加幻燈片
            sb.AddSlideToLeft(seconds, element.ActualWidth, KeepMargin: KeepMargin);

            // 在動畫中添加淡入淡出
            sb.AddFadeOut(seconds);

            // 開始製作動畫
            sb.Begin(element);

            //使頁面可見
            element.Visibility = Visibility.Visible;

            // 等待它完成
            await Task.Delay((int)(seconds * 1000));
        }
        /// <summary>
        /// 將UI 元素向右側滑動(淡出)
        /// </summary>
        /// <param name="element">動畫頁面</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToRightAsync(this FrameworkElement element, float seconds = 0.5f, bool KeepMargin = true)
        {
            // 創建情節提要
            var sb = new Storyboard();

            // 從正確的動畫添加幻燈片
            sb.AddSlideToRight(seconds, element.ActualWidth, KeepMargin: KeepMargin);

            // 在動畫中添加淡入淡出
            sb.AddFadeOut(seconds);

            // 開始製作動畫
            sb.Begin(element);

            //使頁面可見
            element.Visibility = Visibility.Visible;

            // 等待它完成
            await Task.Delay((int)(seconds * 1000));
        }
    }
}

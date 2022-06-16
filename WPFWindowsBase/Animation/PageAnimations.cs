using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WPFWindowsBase
{
    /// <summary>
    /// 幫助者以特定方式為頁面設置動畫
    /// </summary>
    public static class PageAnimations
    {
        /// <summary>
        /// 從右側滑動頁面(淡入)
        /// </summary>
        /// <param name="page">動畫頁面</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromUp(this Page page, float seconds)
        {
            var sb = new Storyboard();

            sb.AddSlideFromUp(seconds, page.WindowHeight);

            sb.AddFadeIn(seconds);

            sb.Begin(page);

            page.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// 從右側滑動頁面(淡入)
        /// </summary>
        /// <param name="page">動畫頁面</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromRight(this Page page, float seconds)
        {
            var sb = new Storyboard();

            sb.AddSlideFromRight(seconds, page.WindowWidth);

            sb.AddFadeIn(seconds);

            sb.Begin(page);

            page.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }
        /// <summary>
        /// 從左側滑動頁面(淡入)
        /// </summary>
        /// <param name="page">動畫頁面</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromLeft(this Page page, float seconds)
        {
            var sb = new Storyboard();

            sb.AddSlideFromLeft(seconds, page.WindowWidth);

            sb.AddFadeIn(seconds);

            sb.Begin(page);

            page.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }
        /// <summary>
        /// 頁面消失在出現
        /// </summary>
        /// <param name="page">動畫頁面</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <returns></returns>
        public static async Task FadeIn(this Page page, float seconds)
        {
            var sb = new Storyboard();

            sb.AddFadeIn(seconds);

            sb.Begin(page);

            page.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }
        /// <summary>
        /// 將頁面向左滑動(淡出)
        /// </summary>
        /// <param name="page">動畫頁面</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(this Page page, float seconds)
        {
            // 創建情節提要
            var sb = new Storyboard();
            // 從正確的動畫添加幻燈片
            sb.AddSlideToLeft(seconds, page.WindowWidth);

            // 在動畫中添加淡入淡出
            sb.AddFadeOut(seconds);

            // 開始製作動畫
            sb.Begin(page);

            //使頁面可見
            page.Visibility = Visibility.Visible;

            // 等待它完成
            await Task.Delay((int)(seconds * 1000));
        }
        /// <summary>
        /// 將頁面向右滑動(淡出)
        /// </summary>
        /// <param name="page">動畫頁面</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToRight(this Page page, float seconds)
        {
            // 創建情節提要
            var sb = new Storyboard();
            // 從正確的動畫添加幻燈片
            sb.AddSlideToRight(seconds, page.WindowWidth);

            // 在動畫中添加淡入淡出
            sb.AddFadeOut(seconds);

            // 開始製作動畫
            sb.Begin(page);

            //使頁面可見
            page.Visibility = Visibility.Visible;

            // 等待它完成
            await Task.Delay((int)(seconds * 1000));
        }
    }
}

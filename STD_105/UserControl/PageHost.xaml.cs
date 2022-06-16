using System;
using System.Windows;
using System.Windows.Controls;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// PageHost.xaml 的互動邏輯
    /// </summary>
    public partial class PageHost : UserControl
    {
        /// <summary>
        /// 目前頁面
        /// </summary>
        public BasePage CurrentPage
        {
            get => (BasePage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// 將<see cref ="CurrentPage"/>註冊為依賴屬性
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(PageHost), new UIPropertyMetadata(CurrentPagePropertyChanged));
        /// <summary>
        /// 當目前<see cref="CurrentPage"/> 屬性變更時觸發
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //取得框架
            var newPageFrame = (d as PageHost).NewPage;
            var oldPageFrame = (d as PageHost).OldPage;
            //取得目前頁面的內容
            var oldPageContent = newPageFrame.Content;

            newPageFrame.Content = null;

            //將上一頁移至舊頁面框架
            oldPageFrame.Content = oldPageContent;
            //當Loaded事件觸發時在上一頁製作動畫
            //由於框架]移動，此調用之後
            if (oldPageContent is BasePage oldPage)
                oldPage.ShouldAnimateOut = true;

            newPageFrame.Content = e.NewValue;

            // 回收記憶體
            GC.Collect();

        }
        public PageHost()
        {
            InitializeComponent();
        }
    }
}

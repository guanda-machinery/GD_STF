using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// PopupPageHost.xaml 的互動邏輯
    /// </summary>
    public partial class PopupPageHost : UserControl
    {
        public PopupPageHost()
        {
            InitializeComponent();
        }
        public BasePage CurrentPage
        {
            get => (BasePage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(PopupPageHost), new UIPropertyMetadata(CurrentPagePropertyChanged));
        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //取得框架
            Frame newFrame = (d as PopupPageHost).newPage;
            Frame oldFrame = (d as PopupPageHost).oldPage;

            //取得目前頁面的內容
            object oldPageContent = newFrame.Content;
            //清空頁面
            newFrame.Content = null;
            //將上一頁移至舊頁面框架
            oldFrame.Content = oldPageContent;
            //當Loaded事件觸發時在上一頁製作動畫
            //由於框架]移動，此調用之後
            if (oldPageContent is BasePage oldPage)
                oldPage.ShouldAnimateOut = true;

            newFrame.Content = e.NewValue;
            // 回收記憶體
            GC.Collect();
        }
    }
}

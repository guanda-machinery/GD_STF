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
    /// OfficePageHost.xaml 的互動邏輯
    /// </summary>
    public partial class OfficePageHost : UserControl
    {
        public OfficePageHost()
        {
            InitializeComponent();
        }

        public BasePage CurrentPage
        {
            get => (BasePage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(OfficePageHost), new UIPropertyMetadata(CurrentPagePropertyChanged));
        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            /*
            //取得框架
            Frame newFrame = (d as OfficePageHost).newPage;
            Frame oldFrame = (d as OfficePageHost).oldPage;
            //取得目前頁面的內容
            object oldPageContent = newFrame.Content;
            //清空頁面
            newFrame.Content = null;
            //將上一頁移至舊頁面框架            
            oldFrame.Content = null;           
            oldFrame.Content = oldPageContent;
            //當Loaded事件觸發時在上一頁製作動畫
            //由於框架]移動，此調用之後
            if (oldPageContent is BasePage oldPage)
                oldPage.ShouldAnimateOut = true;           
            
            newFrame.Content = e.NewValue;
            // 回收記憶體
            GC.Collect();
            GC.WaitForPendingFinalizers();
            */

            //保存舊頁面會讓ScrollBox有問題導致頁面凍結，所以辦公室頁面不再保存舊頁面
            Frame newFrame = (d as OfficePageHost).newPage;
            newFrame.Content = e.NewValue;
            // 回收記憶體
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}

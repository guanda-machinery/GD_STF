using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
namespace WPFWindowsBase
{
    /// <summary>
    /// 用於創建<see cref ="Frame" />的NoFrameHistory附加屬性，該屬性從不顯示導航並保持導航歷史為空
    /// </summary>
    public class NoFrameHistory : BaseAttachedProperty<NoFrameHistory, bool>
    {
        /// <inheritdoc/>
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var frame = sender as Frame;

            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            ////刪除frame導航紀錄
            frame.Navigated += (s, _e) => ((Frame)s).NavigationService.RemoveBackEntry();
        }
    }
}

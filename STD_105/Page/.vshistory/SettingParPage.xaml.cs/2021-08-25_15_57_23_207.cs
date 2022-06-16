using System.Windows.Input;
using WPFSTD105.ViewModel;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// 機器參數設定
    /// </summary>
    public partial class SettingParPage : BasePage<SettingParVM>
    {
        public SettingParPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }

        /// <summary>
        /// 按下 input 按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Input_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ViewModel.InputCommand.Execute(new object());
        }
        /// <summary>
        /// 釋放 IO 監測執行續
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AbortIOThread(object sender, MouseButtonEventArgs e)
        {
            ViewModel.IOAbort();
        }
        /// <summary>
        /// 按下 output 按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Out_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ViewModel.OutCommand.Execute(new object());
        }

        private void FunctionLockControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}

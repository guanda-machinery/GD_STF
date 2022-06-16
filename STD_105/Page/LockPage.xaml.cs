using System.Windows.Input;
using WPFSTD105;
using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 上鎖畫面
    /// 備註 : <see cref="GD_STD.Enum.KEY_HOLE.LOCK"/> 狀態
    /// </summary>
    public partial class LockPage : BasePage
    {
        public LockPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IoC.Get<ApplicationVM>().CurrentPage = WPFSTD105.ApplicationPage.Home;
        }
    }
}

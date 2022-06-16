using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 送料手臂
    /// </summary>
    public partial class HandPage : BasePage<WPFSTD105.ViewModel.HandVM>
    {
        public HandPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

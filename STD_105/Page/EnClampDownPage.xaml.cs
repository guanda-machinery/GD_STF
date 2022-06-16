using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 入口下壓
    /// </summary>
    public partial class EnClampDownPage : BasePage<WPFSTD105.ViewModel.ExClampDownVM>
    {
        public EnClampDownPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }

    }
}

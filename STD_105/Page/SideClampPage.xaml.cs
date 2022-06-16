using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 夾具側壓
    /// </summary>
    public partial class SideClampPage : BasePage<WPFSTD105.ViewModel.SideClampVM>
    {
        public SideClampPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

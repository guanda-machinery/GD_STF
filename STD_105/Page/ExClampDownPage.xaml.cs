using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 出口夾具下壓
    /// </summary>
    public partial class ExClampDownPage : BasePage<WPFSTD105.ViewModel.ExClampDownVM>
    {
        public ExClampDownPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

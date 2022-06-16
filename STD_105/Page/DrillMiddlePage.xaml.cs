using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 中軸刀庫
    /// </summary>
    public partial class DrillMiddlePage : BasePage
    {
        public DrillMiddlePage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

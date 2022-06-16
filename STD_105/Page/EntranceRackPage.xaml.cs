using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 入口料架
    /// </summary>
    public partial class EntranceRackPage : BasePage
    {
        public EntranceRackPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

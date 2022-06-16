using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 右軸入口刀庫
    /// </summary>
    public partial class DrillEntranceRPage : BasePage
    {
        public DrillEntranceRPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

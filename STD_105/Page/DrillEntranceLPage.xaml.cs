using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 左軸入口刀庫
    /// </summary>
    public partial class DrillEntranceLPage : BasePage
    {
        public DrillEntranceLPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

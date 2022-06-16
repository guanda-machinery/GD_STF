using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 右軸出口刀庫
    /// </summary>
    public partial class DrillExportRPage : BasePage
    {
        public DrillExportRPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

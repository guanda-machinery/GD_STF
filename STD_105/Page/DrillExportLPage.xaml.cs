using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 左軸出口刀庫
    /// </summary>
    public partial class DrillExportLPage : BasePage
    {
        public DrillExportLPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

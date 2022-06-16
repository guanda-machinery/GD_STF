using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 警報
    /// </summary>
    public partial class AlarmPage : BasePage
    {
        public AlarmPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

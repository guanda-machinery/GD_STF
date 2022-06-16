using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 捲削機
    /// </summary>
    public partial class VolumePage : BasePage<WPFSTD105.ViewModel.VolumeVM>
    {
        public VolumePage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

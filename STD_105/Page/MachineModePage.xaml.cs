using WPFSTD105.ViewModel;
using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 選擇登入模式
    /// </summary>
    public partial class MachineModePage : BasePage<ModeSelectedVM>
    {
        public MachineModePage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

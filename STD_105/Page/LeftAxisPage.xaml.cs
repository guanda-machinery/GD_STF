using System.Windows.Controls;
using WPFWindowsBase;
using GD_STD;
using static WPFSTD105.CodesysIIS;
using GD_STD.Enum;

namespace STD_105
{
    /// <summary>
    /// 左軸
    /// </summary>
    public partial class LeftAxisPage : BasePage<WPFSTD105.ViewModel.LeftAxisVM>
    {
        public LeftAxisPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

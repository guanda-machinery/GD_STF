using System.Windows;
using System.Windows.Input;
using WPFWindowsBase;
using System.Windows.Controls;
using System;

namespace STD_105
{
    /// <summary>
    /// 首頁
    /// </summary>
    public partial class HomePage : BasePage
    {
        public Style SwitchStyle { get; set; }
        public HomePage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }
    }
}

using System.Windows.Controls;
using WPFWindowsBase;
using GD_STD;
using static WPFSTD105.CodesysIIS;

namespace STD_105
{
    /// <summary>
    /// 右軸
    /// </summary>
    public partial class RightAxisPage : BasePage<WPFSTD105.ViewModel.RightAxisVM>
    {
        public RightAxisPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
        }

        private void ChangeSpindle(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = sender as Button;
            PanelButton panelButton = new PanelButton();

            panelButton = StateClear(WPFSTD105.ViewLocator.ApplicationViewModel.PanelButton);
            WriteCodesysMemor.SetPanel(panelButton);

            switch (button.Name)
            {
                case "btn_LeftSpindle":
                    WPFSTD105.ViewLocator.ApplicationViewModel.CurrentPage = WPFSTD105.ApplicationPage.LeftAxis;
                    break;
                case "btn_MiddleSpindle":
                    WPFSTD105.ViewLocator.ApplicationViewModel.CurrentPage = WPFSTD105.ApplicationPage.MiddleAxis;
                    break;
                case "btn_RightSpindle":
                    WPFSTD105.ViewLocator.ApplicationViewModel.CurrentPage = WPFSTD105.ApplicationPage.RightAxis;
                    break;
            }
        }
        private PanelButton StateClear(PanelButton panelButton)
        {
            //panelButton.ExportRack = false;
            //panelButton.EntranceRack = false;
            //panelButton.MainAxisMode = false;
            //panelButton.Middle = new AxisAction()
            //{
            //    AxisRotation = false,
            //    AxisStop = false,
            //    AxisEffluent = false,
            //    AxisLooseKnife = false
            //};
            //panelButton.Righ = new AxisAction()
            //{
            //    AxisRotation = false,
            //    AxisStop = false,
            //    AxisEffluent = false,
            //    AxisLooseKnife = false
            //};
            //panelButton.Left = new AxisAction()
            //{
            //    AxisRotation = false,
            //    AxisStop = false,
            //    AxisEffluent = false,
            //    AxisLooseKnife = false
            //};
            panelButton.AxisEffluent = false;
            panelButton.AxisLooseKnife = false;
            panelButton.AxisRotation = false;
            panelButton.AxisStop = false;
            panelButton.MainAxisMode = false;
            panelButton.ClampDown = false;
            panelButton.SideClamp = false;
            panelButton.Volume = false;
            panelButton.DrillWarehouse = false;
            panelButton.Hand = false;
            return panelButton;
        }
    }
}

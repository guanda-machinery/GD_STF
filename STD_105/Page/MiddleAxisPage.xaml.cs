using System.Windows.Controls;
using WPFWindowsBase;
using GD_STD;
using static WPFSTD105.CodesysIIS;

namespace STD_105
{
    /// <summary>
    /// 中軸
    /// </summary>
    public partial class MiddleAxisPage : BasePage<WPFSTD105.ViewModel.MiddleAxisVM>
    {
        public MiddleAxisPage()
        {
            InitializeComponent();
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToRight;
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

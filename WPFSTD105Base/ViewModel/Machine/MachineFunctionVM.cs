using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 機台功能VM
    /// </summary>
    public class MachineFunctionVM : WPFWindowsBase.BaseViewModel
    {

        private const string Hsteel_Clamp_TabItemName = "HSteel_Clamp_TabItem";
        private const string HSteel_Grab_TabItemName = "HSteel_Grab_TabItem";
        private const string Transport_TabItemName = "Transport_TabItem";
        private const string Transport_by_Robot_TabItemName = "Transport_by_Robot_TabItem";
        private const string ToolMagazine_TabItemName = "ToolMagazine_TabItem";
        private const string Crumb_Conveyor_TabItemName = "Crumb_Conveyor_TabItem";

        public TabItem TabItemSelected
        {
            //會在切換頁面的時候切換CodesysMemor
            set
            {
                if (value != null)
                {
                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    ClearPButtonModeValue(ref PButton);

                    switch (value.Name)
                    {
                        case Hsteel_Clamp_TabItemName:
                            PButton.ClampDown = true;
                            break;
                        case HSteel_Grab_TabItemName:
                            PButton.SideClamp = true;
                            break;
                        case Transport_TabItemName:
                            PButton.EntranceRack = true;
                            break;
                        case Transport_by_Robot_TabItemName:
                            PButton.Hand = true;
                            break;
                        case ToolMagazine_TabItemName:
                            PButton.DrillWarehouse = true;
                            break;
                        case Crumb_Conveyor_TabItemName:
                            PButton.Volume = true;
                            break;
                        default:
                            break;
                    }

                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                }
            }
        }
        private void ClearPButtonModeValue(ref GD_STD.PanelButton PButton)
        {
            PButton.ClampDown = false;
            PButton.SideClamp = false;
            PButton.EntranceRack = false;
            PButton.Hand = false;
            PButton.DrillWarehouse = false;
            PButton.Volume = false;
            PButton.MainAxisMode = false;
        }


        #region 使用之vm
        /// <summary>
        /// 壓
        /// </summary>
        public Machine_HSteelClampViewModel Machine_HSteelClampVM { get; set; } = new Machine_HSteelClampViewModel();
        /// <summary>
        /// 夾
        /// </summary>
        public Machine_HSteelGrabViewModel Machine_HSteelGrabVM { get; set; } = new Machine_HSteelGrabViewModel();
        /// <summary>
        /// 輸送帶VM
        /// </summary>
        public RacksOperationVM _RacksOperationVM { get; set; } = new RacksOperationVM();
        /// <summary>
        /// 手臂夾取VM
        /// </summary>
        public GrabArm_ViewModel ArmGrab_Dpad_VM { get; set; } = new GrabArm_ViewModel();
        /// <summary>
        /// 換刀VM
        /// </summary>
        public ToolMagazineControl_ViewModel ToolMagazineControl_VM { get; set; } = new ToolMagazineControl_ViewModel();
        /// <summary>
        /// 捲屑機VM
        /// </summary>
        public DpadConveyor_ViewModel DpadConveyor_VM { get; set; } = new DpadConveyor_ViewModel();

        private bool _descriptionDisplayBoolenAll = false;
        public bool DescriptionDisplayBoolenAll 
        { 
            get
            {
                return _descriptionDisplayBoolenAll;
            }
            set
            {
                _descriptionDisplayBoolenAll = value;

                Machine_HSteelClampVM.DescriptionDisplayBoolen = _descriptionDisplayBoolenAll;
                Machine_HSteelGrabVM.DescriptionDisplayBoolen = _descriptionDisplayBoolenAll;
                _RacksOperationVM.DescriptionDisplayBoolen = _descriptionDisplayBoolenAll;
                ArmGrab_Dpad_VM.DescriptionDisplayBoolen = _descriptionDisplayBoolenAll;
                ToolMagazineControl_VM.DescriptionDisplayBoolen = _descriptionDisplayBoolenAll;
                DpadConveyor_VM.DescriptionDisplayBoolen = _descriptionDisplayBoolenAll;
            }
        }


        #endregion

    }
}

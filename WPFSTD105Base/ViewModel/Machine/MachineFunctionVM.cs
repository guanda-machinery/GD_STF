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
        private int _tabControlSelectedIndex = -1;
        public int TabControlSelectedIndex
        {
            get
            {
                GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                if (PButton.ClampDown)
                {
                    _tabControlSelectedIndex = 0;
                    return 0;
                }
                else if (PButton.SideClamp)
                {
                    _tabControlSelectedIndex = 1;
                    return 1;
                }
                else if (PButton.EntranceRack)
                {
                    _tabControlSelectedIndex = 2;
                    return 2;
                }
                else if (PButton.ExportRack)
                {
                    _tabControlSelectedIndex = 2;
                    return 2;
                }
                else if (PButton.Hand)
                {
                    _tabControlSelectedIndex = 3;
                    return 3;
                }
                else if (PButton.DrillWarehouse)
                {
                    _tabControlSelectedIndex = 4;
                    return 4;
                }
                else if (PButton.Volume)
                {
                    _tabControlSelectedIndex = 5;
                    return 5;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                ClearPButtonModeValue(ref PButton);

                switch (value)
                {
                    case 0:
                        PButton.ClampDown = true;
                        break;
                    case 1:
                        PButton.SideClamp = true;
                        break;
                    case 2:
                        PButton.EntranceRack = true;
                        break;
                    case 3:
                        PButton.Hand = true;
                        break;
                    case 4:
                        PButton.DrillWarehouse = true;
                        break;
                    case 5:
                        PButton.Volume = true;
                        break;
                    default:
                        break;
                }
                CodesysIIS.WriteCodesysMemor.SetPanel(PButton);

                _tabControlSelectedIndex = value;
            }
        }


        private void ClearPButtonModeValue(ref GD_STD.PanelButton PButton)
        {
            PButton.ClampDown = false;
            PButton.SideClamp = false;
            PButton.EntranceRack = false;
            PButton.ExportRack = false;
            PButton.Hand = false;
            PButton.DrillWarehouse = false;
            PButton.Volume = false;
            PButton.MainAxisMode = false;
        }


        #region 使用之vm
        /// <summary>
        /// 壓
        /// </summary>
        public Dpad_ViewModel Machine_HSteelClampVM { get; set; } = new Dpad_ViewModel()
        {
            Button_Left_IsEnabled = false,
            Button_Right_IsEnabled = false
        };
        /// <summary>
        /// 夾
        /// </summary>
        public Dpad_ViewModel Machine_HSteelGrabVM { get; set; } = new Dpad_ViewModel()
        {
            Button_Up_IsEnabled = false,
            Button_Down_IsEnabled = false
        };
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
        public Dpad_Joystick_ViewModel ToolMagazineControl_VM { get; set; } = new Dpad_Joystick_ViewModel()
        {
            Button_Up_IsEnabled = false,
            Button_Down_IsEnabled = false,
        };
        /// <summary>
        /// 捲屑機VM
        /// </summary>
        public Dpad_Joystick_ViewModel DpadConveyor_VM { get; set; } = new Dpad_Joystick_ViewModel()
        {
            JoyStickButton1_isEnabled = false,
            JoyStickButton2_isEnabled = false,
            JoyStickButton3_isEnabled = false,
            JoyStickButton4_isEnabled = false,
            JoyStickButton5_isEnabled = false,
            JoyStickButton6_isEnabled = false,
            Button_Up_IsEnabled = false,
            Button_Down_IsEnabled = false
        };

        private bool _descriptionDisplayBoolenAll = true;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.ViewModel
{
    public class GrabArm_ViewModel : Dpad_Joystick_ViewModel
    {
        public GrabArm_ViewModel()
        {

            this.JoyStick_BorderButton1_Trigger = ArmReleaseCommand; //arm打開
            this.JoyStick_BorderButton3_Trigger = ArmGrabCommand;//arm夾持
            this.JoyStick_BorderButton4_Trigger = ArmXaCommand;//armXa
            this.JoyStick_BorderButton5_Trigger = ArmHSteelCenterCommand;//arm H鋼中心
            this.JoyStick_BorderButton6_Trigger = ArmHSteelPointCommand;//arm H鋼尖端

            this.BorderButton_Left_Trigger = ArmXPlusCommand;//arm X+
            this.BorderButton_Up_Trigger = ArmYPlusCommand;//arm Y+
            this.BorderButton_Right_Trigger = ArmXMinusCommand;//arm X-
            this.BorderButton_Down_Trigger = ArmYMinusCommand;//arm Y-


            this.JoyStick_BorderButton1_Release = null; //arm打開
            this.JoyStick_BorderButton3_Release = null;//arm夾持
            this.JoyStick_BorderButton4_Release = null;//armXa
            this.JoyStick_BorderButton5_Release = null;//arm H鋼中心
            this.JoyStick_BorderButton6_Release = null;//arm H鋼尖端

            this.BorderButton_Left_Release = null;//arm X+
            this.BorderButton_Up_Release = null;//arm Y+
            this.BorderButton_Right_Release = null;//arm X-
            this.BorderButton_Down_Release = null;//arm Y-


        }

        public bool OptionalGrabArmButton1_isEnabled { get; set; } = true;
        public bool OptionalGrabArmButton2_isEnabled { get; set; } = false;
        public bool OptionalGrabArmButton3_isEnabled { get; set; } = false;


        //手臂放開
        private WPFWindowsBase.RelayCommand ArmReleaseCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.Hand = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        //手臂抓取
        private WPFWindowsBase.RelayCommand ArmGrabCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.Hand = true;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }


        //手臂Xa
        private WPFWindowsBase.RelayCommand ArmXaCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //PButton. = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }

        //手臂Xa
        private WPFWindowsBase.RelayCommand ArmHSteelCenterCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //PButton. = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }

        private WPFWindowsBase.RelayCommand ArmHSteelPointCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //PButton. = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }


        private WPFWindowsBase.RelayCommand ArmXPlusCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //PButton. = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        private WPFWindowsBase.RelayCommand ArmXMinusCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //PButton. = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        private WPFWindowsBase.RelayCommand ArmYPlusCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //PButton. = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        private WPFWindowsBase.RelayCommand ArmYMinusCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //PButton. = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }






        /// <summary>
        /// 下方按鈕1
        /// </summary>
        public WPFWindowsBase.RelayCommand PostRiseCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //相反訊號
                    PButton.PostRise = !PButton.PostRise;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }


    }
}
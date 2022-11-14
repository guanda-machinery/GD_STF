using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.ViewModel
{ 
    public class MainSpindle_ViewModel: Dpad_Joystick_ViewModel
    {
        public MainSpindle_ViewModel()
        {
            /*this.JoyStick_BorderButton4_Trigger = MachiningDirecationLeftCommand; //L
            this.JoyStick_BorderButton5_Trigger = MachiningDirecationUpCommand;//U
            this.JoyStick_BorderButton6_Trigger = MachiningDirecationRightCommand; //R

            this.JoyStick_BorderButton4_Trigger = null; //L
            this.JoyStick_BorderButton5_Trigger = null;//U
            this.JoyStick_BorderButton6_Trigger = null; //R*/


            /*  this.JoyStick_BorderButton1_Trigger = ZAxisPlusMoveCommand;
              this.JoyStick_BorderButton1_Release = ZAxisPlusStopCommand;
              this.JoyStick_BorderButton3_Trigger = ZAxisMinusMoveCommand;
              this.JoyStick_BorderButton3_Release = ZAxisMinusStopCommand;*/

            /*  this.Joystcik_Left_Trigger_Command = XAxisPlusMoveCommand;//X+
              this.Joystcik_Left_Release_Command = XAxisPlusStopCommand;//X+
              this.Joystcik_Right_Trigger_Command = XAxisMinusMoveCommand;//X-
              this.Joystcik_Right_Release_Command = XAxisMinusStopCommand;//X-
              this.Joystcik_Up_Trigger_Command = YAxisPlusMoveCommand;//Y+
              this.Joystcik_Up_Release_Command = YAxisPlusStopCommand;//Y+
              this.Joystcik_Down_Trigger_Command = YAxisMinusMoveCommand;//Y-
              this.Joystcik_Down_Release_Command = YAxisMinusStopCommand; //Y-*/

            GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
            PButton.ClampDown = false;
            PButton.SideClamp = false;
            PButton.EntranceRack = false;
            PButton.Hand = false;
            PButton.DrillWarehouse = false;
            PButton.Volume = false;
            PButton.MainAxisMode = true;
            CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
        }

        ~MainSpindle_ViewModel()
        {
            GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
            PButton.MainAxisMode = false;
            CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
        }


        /*

        /// <summary>
        /// 主軸移動Z+
        /// </summary>
        private WPFWindowsBase.RelayCommand ZAxisPlusMoveCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        /// <summary>
        /// 主軸移動Z+停止
        /// </summary>
        private WPFWindowsBase.RelayCommand ZAxisPlusStopCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }

        /// <summary>
        /// 主軸移動X+
        /// </summary>
        private WPFWindowsBase.RelayCommand XAxisPlusMoveCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        /// <summary>
        /// 主軸移動X+停止
        /// </summary>
        private WPFWindowsBase.RelayCommand XAxisPlusStopCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }

        /// <summary>
        /// 主軸移動Y+
        /// </summary>
        private WPFWindowsBase.RelayCommand YAxisPlusMoveCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        /// <summary>
        /// 主軸移動Y+停止
        /// </summary>
        private WPFWindowsBase.RelayCommand YAxisPlusStopCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }

        /// <summary>
        /// 主軸移動Z-
        /// </summary>
        private WPFWindowsBase.RelayCommand ZAxisMinusMoveCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        /// <summary>
        /// 主軸移動Z-停止
        /// </summary>
        private WPFWindowsBase.RelayCommand ZAxisMinusStopCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }

        /// <summary>
        /// 主軸移動X-
        /// </summary>
        private WPFWindowsBase.RelayCommand XAxisMinusMoveCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        /// <summary>
        /// 主軸移動X-停止
        /// </summary>
        private WPFWindowsBase.RelayCommand XAxisMinusStopCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }

        /// <summary>
        /// 主軸移動Y-
        /// </summary>
        private WPFWindowsBase.RelayCommand YAxisMinusMoveCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        /// <summary>
        /// 主軸移動Y-停止
        /// </summary>
        private WPFWindowsBase.RelayCommand YAxisMinusStopCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        */


        private WPFWindowsBase.RelayCommand MachiningDirecationLeftCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }

        }
        private WPFWindowsBase.RelayCommand MachiningDirecationUpCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }

        }
        private WPFWindowsBase.RelayCommand MachiningDirecationRightCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }

        }












        /// <summary>
        /// 主軸旋轉
        /// </summary>
        public WPFWindowsBase.RelayCommand MainSpindleRotateCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //鬆刀狀態下不可旋轉
                    if (PanelListening.IsAxisLooseKnife())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;  
                    PButton.AxisRotation = true;
                    PButton.AxisStop = false;
                    PButton.AxisLooseKnife = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        /// <summary>
        /// 主軸停止
        /// </summary>
        public WPFWindowsBase.RelayCommand MainSpindleStopCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.AxisRotation = false;
                    PButton.AxisStop = true;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        /// <summary>
        /// 主軸拉刀
        /// </summary>
        public WPFWindowsBase.RelayCommand LooseToolCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;
                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.AxisLooseKnife = !PButton.AxisLooseKnife;
                    PButton.AxisEffluent = false;
                    PButton.AxisRotation = false;
                    PButton.AxisStop = true;
                    
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        /// <summary>
        /// 主軸出水
        /// </summary>
        public WPFWindowsBase.RelayCommand MainSpindleCoolantCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.AxisEffluent = !PButton.AxisEffluent;
                   
                    //噴水時主軸拉刀
                    if (PButton.AxisEffluent)
                    {
                        PButton.AxisLooseKnife = false;
                    }
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
    }
}

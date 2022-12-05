using DevExpress.Xpf.Spreadsheet.UI.TypedStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPFSTD105.ViewModel
{ 
    public class MainSpindle_ViewModel: Dpad_Joystick_ViewModel
    {
        public MainSpindle_ViewModel()
        {
           //  this.JoyStick_BorderButton4_Trigger = MachiningDirecationLeftCommand; //L
           // this.JoyStick_BorderButton5_Trigger = MachiningDirecationMiddleCommand;//U
           //  this.JoyStick_BorderButton6_Trigger = MachiningDirecationRightCommand; //R
            this.JoyStick_CIRCLE_TOP_Trigger_CommandParameter= GD_STD.Enum.AXIS_SELECTED.Left;
            this.JoyStick_CIRCLE_MIDDLE_Trigger_CommandParameter = GD_STD.Enum.AXIS_SELECTED.Middle;
            this.JoyStick_CIRCLE_BOTTOM_Trigger_CommandParameter = GD_STD.Enum.AXIS_SELECTED.Right;
           
            this.JoyStick_CIRCLE_TOP_Release_CommandParameter = null;
            this.JoyStick_CIRCLE_MIDDLE_Release_CommandParameter = null;
            this.JoyStick_CIRCLE_BOTTOM_Release_CommandParameter = null;





            this.IsMainSpindleMode= true;
            GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
            PButton.AxisRotation = false;
            PButton.AxisStop = true;
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
                    if (!PanelListening.IsAxisLooseKnife())
                        return;
                    //先下停止再開始轉

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;

                    PButton.AxisRotation = false;
                    PButton.AxisStop = true;
                    PButton.AxisLooseKnife = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);

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

                    //先下開始再停止轉

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.AxisRotation = true;
                    PButton.AxisStop = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);

                    Thread.Sleep(100);
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

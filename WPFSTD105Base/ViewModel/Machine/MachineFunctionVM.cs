using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool Taskboolen = true;
        public MachineFunctionVM()
        {
            //初始化時建立一個task監督GD_STD.PanelButton
            //是否可由其他方法代替? 需查證
            Task.Run(() =>
            {
                while (Taskboolen)
                {
                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    if (PButton.ClampDown && TabControlSelectedIndex !=0)
                    {
                        TabControlSelectedIndex = 0;
                    }
                    else if (PButton.SideClamp && TabControlSelectedIndex != 1)
                    {
                        TabControlSelectedIndex = 1;
                    }
                    else if((PButton.EntranceRack || PButton.ExportRack) && TabControlSelectedIndex != 2)
                    {
                        TabControlSelectedIndex = 2;
                    }
                    else if (PButton.Hand && TabControlSelectedIndex != 3)
                    {
                        TabControlSelectedIndex = 3;
                    }
                    else if (PButton.DrillWarehouse && TabControlSelectedIndex != 4)
                    {
                        TabControlSelectedIndex = 4;
                    }
                    else if (PButton.Volume && TabControlSelectedIndex != 5)
                    {
                        TabControlSelectedIndex = 5;
                    }
                    Thread.Sleep(100);
                }

            });
        }

        ~MachineFunctionVM()
        {
            Taskboolen = false;
            //解構時清除狀態
            GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
            PButton.ClampDown = false;
            PButton.SideClamp = false;
            PButton.EntranceRack = false;
            PButton.ExportRack = false;
            PButton.Hand = false;
            PButton.DrillWarehouse = false;
            PButton.Volume = false;
            PButton.MainAxisMode = false;
            CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
        }

        private int _tabControlSelectedIndex = -1;
        public int TabControlSelectedIndex
        {
            get
            {
                   return _tabControlSelectedIndex;
            }
            set
            {
                GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                var Exboolen = PButton.ExportRack;

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
                        if(!Exboolen)   
                            PButton.EntranceRack = true;
                        else
                            PButton.ExportRack = true;
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
                //比較值 若功能沒變則不寫入
                if(!PanelButtonIsEqual(ViewLocator.ApplicationViewModel.PanelButton, PButton))
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

        private bool PanelButtonIsEqual(GD_STD.PanelButton OldButton, GD_STD.PanelButton NewPButton)
        {
            return
            OldButton.ClampDown == NewPButton.ClampDown &&
            OldButton.SideClamp == NewPButton.SideClamp &&
            OldButton.EntranceRack == NewPButton.EntranceRack &&
            OldButton.ExportRack == NewPButton.ExportRack &&
            OldButton.Hand == NewPButton.Hand &&
            OldButton.DrillWarehouse == NewPButton.DrillWarehouse &&
            OldButton.Volume == NewPButton.Volume &&
            OldButton.MainAxisMode == NewPButton.MainAxisMode;

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

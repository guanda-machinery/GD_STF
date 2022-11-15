using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.ViewModel
{
    public class DpadConveyor_ViewModel : Dpad_Joystick_ViewModel
    {
        public DpadConveyor_ViewModel()
        {
            this.JoyStickButton1_isEnabled = false;
            this.JoyStickButton2_isEnabled = false;
            this.JoyStickButton3_isEnabled = false;
            this.JoyStickButton4_isEnabled = false;
            this.JoyStickButton5_isEnabled = false;
            this.JoyStickButton6_isEnabled = false;
            this.Button_Up_IsEnabled = false;
            this.Button_Down_IsEnabled = false;

            /*this.Joystcik_Left_Trigger_Command = VolumePlusCommand;//捲削機正轉
            this.Joystcik_Right_Trigger_Command = VolumeMinusCommand; //捲削機逆轉

            this.Joystcik_Left_Release_Command= null;
            this.Joystcik_Right_Release_Command = null; */
        }



       /* private WPFWindowsBase.RelayCommand VolumePlusCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.Volume = true;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        private WPFWindowsBase.RelayCommand VolumeMinusCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.Volume = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }*/


    }
}
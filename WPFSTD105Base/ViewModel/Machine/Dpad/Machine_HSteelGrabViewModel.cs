using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.ViewModel
{
    public class Machine_HSteelGrabViewModel: Dpad_ViewModel
    {
        public Machine_HSteelGrabViewModel()
        {
            this.Button_Up_IsEnabled = false;
            this.Button_Down_IsEnabled = false;
            // this.Joystcik_Left_Trigger_Command = UnSideClampCommand ;
            // this.Joystcik_Right_Trigger_Command = SideClampCommand;
            //this.Joystcik_Left_Release_Command = null;
            //this.Joystcik_Right_Release_Command = null;
         }

        /*
        private WPFWindowsBase.RelayCommand SideClampCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.SideClamp = true;
                    // PButton.Joystick = GD_STD.Enum.JOYSTICK.NULL;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        private WPFWindowsBase.RelayCommand UnSideClampCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.SideClamp = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        */
    }
}

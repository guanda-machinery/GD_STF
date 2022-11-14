using GD_STD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    public class Machine_HSteelClampViewModel: Dpad_ViewModel 
    {
        public Machine_HSteelClampViewModel()
        {
            //切換machine mode->
            this.Button_Left_IsEnabled = false;
            this.Button_Right_IsEnabled =false;

            //this.BorderButton_Up_Trigger
            this.BorderButton_Left_Trigger = ClampCommand;
            this.BorderButton_Up_Trigger = UnClampCommand;
            this.BorderButton_Left_Release = null;
            this.BorderButton_Up_Release = null;
        }

        private WPFWindowsBase.RelayCommand  ClampCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.ClampDown = true;
                    // PButton.Joystick = GD_STD.Enum.JOYSTICK.NULL;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }


        private WPFWindowsBase.RelayCommand UnClampCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;
                    PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.ClampDown = false;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }











    }
}

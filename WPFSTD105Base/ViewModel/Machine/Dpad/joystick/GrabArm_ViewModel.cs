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
            this.Button_Down_IsEnabled = false;
        }

        public bool OptionalGrabArmButton1_isEnabled { get; set; } = true;
        public bool OptionalGrabArmButton2_isEnabled { get; set; } = false;
        public bool OptionalGrabArmButton3_isEnabled { get; set; } = false;




    }
}
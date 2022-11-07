using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.ViewModel
{
    public class Dpad_Joystick_ViewModel : Dpad_ViewModel
    {
        public Dpad_Joystick_ViewModel()
        {

        }

        public bool JoyStickButton1_isEnabled { get; set; } = true;
        public bool JoyStickButton2_isEnabled { get; set; } = true;
        public bool JoyStickButton3_isEnabled { get; set; } = true;
        public bool JoyStickButton4_isEnabled { get; set; } = true;
        public bool JoyStickButton5_isEnabled { get; set; } = true;
        public bool JoyStickButton6_isEnabled { get; set; } = true;


    }
}

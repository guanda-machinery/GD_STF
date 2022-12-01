using DevExpress.Xpf.Diagram.Native;
using GD_STD;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFSTD105.ViewModel
{
    public class Dpad_Joystick_ViewModel : Dpad_ViewModel
    {
        public Dpad_Joystick_ViewModel()
        {



        }

        public bool JoyStick_ELLIPSE_TOP_isEnabled { get; set; } = true;
        public bool JoyStick_ELLIPSE_BOTTOM_isEnabled { get; set; } = true;
        public bool JoyStick_CIRCLE_TOP_isEnabled { get; set; } = true;
        public bool JoyStick_CIRCLE_MIDDLE_isEnabled { get; set; } = true;
        public bool JoyStick_CIRCLE_BOTTOM_isEnabled { get; set; } = true;

        public object JoyStick_ELLIPSE_TOP_Trigger_CommandParameter { get; set; } = JOYSTICK.ELLIPSE_TOP_DESC;
        public object JoyStick_ELLIPSE_BOTTOM_Trigger_CommandParameter { get; set; } = JOYSTICK.ELLIPSE_BOTTOM_DESC;
        public object JoyStick_CIRCLE_TOP_Trigger_CommandParameter { get; set; } = JOYSTICK.CIRCLE_TOP_DESC;
        public object JoyStick_CIRCLE_MIDDLE_Trigger_CommandParameter { get; set; } = JOYSTICK.CIRCLE_MIDDLE_DESC;
        public object JoyStick_CIRCLE_BOTTOM_Trigger_CommandParameter { get; set; } = JOYSTICK.CIRCLE_BOTTOM_DESC;

        public object JoyStick_ELLIPSE_TOP_Release_CommandParameter { get; set; } = JOYSTICK.NULL;
        public object JoyStick_ELLIPSE_BOTTOM_Release_CommandParameter { get; set; } = JOYSTICK.NULL;
        public object JoyStick_CIRCLE_TOP_Release_CommandParameter { get; set; } = JOYSTICK.NULL;
        public object JoyStick_CIRCLE_MIDDLE_Release_CommandParameter { get; set; } = JOYSTICK.NULL;
        public object JoyStick_CIRCLE_BOTTOM_Release_CommandParameter { get; set; } = JOYSTICK.NULL;








    }
}

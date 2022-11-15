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

        public bool JoyStickButton1_isEnabled { get; set; } = true;
        public bool JoyStickButton2_isEnabled { get; set; } = true;
        public bool JoyStickButton3_isEnabled { get; set; } = true;
        public bool JoyStickButton4_isEnabled { get; set; } = true;
        public bool JoyStickButton5_isEnabled { get; set; } = true;
        public bool JoyStickButton6_isEnabled { get; set; } = true;

        public WPFWindowsBase.RelayCommand JoyStick_BorderButton1_Trigger { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton2_Trigger { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton3_Trigger { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton4_Trigger { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton5_Trigger { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton6_Trigger { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton1_Release { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton2_Release { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton3_Release { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton4_Release { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton5_Release { get; set; }
        public WPFWindowsBase.RelayCommand JoyStick_BorderButton6_Release { get; set; }


    }
}

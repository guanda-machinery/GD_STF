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

        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton1_Trigger ;
        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton2_Trigger ;
        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton3_Trigger ;
        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton4_Trigger ;
        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton5_Trigger ;
        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton6_Trigger ;

        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton1_Trigger
        {
            get 
            {
                if(_joyStick_BorderButton1_Trigger == null)
                {
                    _joyStick_BorderButton1_Trigger = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton1_Trigger;
            }
            set
            {
                _joyStick_BorderButton1_Trigger = value;
            }
        }
        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton2_Trigger
        {
            get
            {
                if (_joyStick_BorderButton2_Trigger == null)
                {
                    _joyStick_BorderButton2_Trigger = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton2_Trigger;
            }
            set
            {
                _joyStick_BorderButton2_Trigger = value;
            }
        }
        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton3_Trigger
        {
            get
            {
                if (_joyStick_BorderButton3_Trigger == null)
                {
                    _joyStick_BorderButton3_Trigger = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton3_Trigger;
            }
            set
            {
                _joyStick_BorderButton3_Trigger = value;
            }
        }
        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton4_Trigger
        {
            get
            {
                if (_joyStick_BorderButton4_Trigger == null)
                {
                    _joyStick_BorderButton4_Trigger = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton4_Trigger;
            }
            set
            {
                _joyStick_BorderButton4_Trigger = value;
            }
        }
        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton5_Trigger
        {
            get
            {
                if (_joyStick_BorderButton5_Trigger == null)
                {
                    _joyStick_BorderButton5_Trigger = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton5_Trigger;
            }
            set
            {
                _joyStick_BorderButton5_Trigger = value;
            }
        }
        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton6_Trigger
        {
            get
            {
                if (_joyStick_BorderButton6_Trigger == null)
                {
                    _joyStick_BorderButton6_Trigger = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton6_Trigger;
            }
            set
            {
                _joyStick_BorderButton6_Trigger = value;
            }
        }


        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton1_Release ;
        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton2_Release ;
        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton3_Release ;
        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton4_Release ;
        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton5_Release ;
        private WPFWindowsBase.RelayParameterizedCommand _joyStick_BorderButton6_Release ;

        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton1_Release
        {
            get
            {
                if (_joyStick_BorderButton1_Release == null)
                {
                    _joyStick_BorderButton1_Release = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton1_Release;
            }
            set
            {
                _joyStick_BorderButton1_Release = value;
            }
        }
public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton2_Release
        {
            get
            {
                if (_joyStick_BorderButton2_Release == null)
                {
                    _joyStick_BorderButton2_Release = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton2_Release;
            }
            set
            {
                _joyStick_BorderButton2_Release = value;
            }
        }
        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton3_Release
        {
            get
            {
                if (_joyStick_BorderButton3_Release == null)
                {
                    _joyStick_BorderButton3_Release = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton3_Release;
            }
            set
            {
                _joyStick_BorderButton3_Release = value;
            }
        }
        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton4_Release
        {
            get
            {
                if (_joyStick_BorderButton4_Release == null)
                {
                    _joyStick_BorderButton4_Release = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton4_Release;
            }
            set
            {
                _joyStick_BorderButton4_Release = value;
            }
        }
        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton5_Release
        {
            get
            {
                if (_joyStick_BorderButton5_Release == null)
                {
                    _joyStick_BorderButton5_Release = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton5_Release;
            }
            set
            {
                _joyStick_BorderButton5_Release = value;
            }
        }
        public WPFWindowsBase.RelayParameterizedCommand JoyStick_BorderButton6_Release
        {
            get
            {
                if (_joyStick_BorderButton6_Release == null)
                {
                    _joyStick_BorderButton6_Release = this.LeftButtonCommand;
                }
                return _joyStick_BorderButton6_Release;
            }
            set
            {
                _joyStick_BorderButton6_Release = value;
            }
        }










    }
}

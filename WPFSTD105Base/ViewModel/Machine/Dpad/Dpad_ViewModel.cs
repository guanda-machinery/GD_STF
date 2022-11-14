using GD_STD;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static WPFSTD105.ViewLocator;

namespace WPFSTD105.ViewModel
{
    /// <summary>   
    /// 虛擬搖桿的基礎VM   
    /// </summary>
    public class Dpad_ViewModel : WPFWindowsBase.BaseViewModel
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public Dpad_ViewModel()
        {
            //上下左右按鍵預設值 可在後續繼承時被修改
            BorderButton_Up_Trigger = JoyStickCommand(JOYSTICK.UP_DESC);
            BorderButton_Down_Trigger = JoyStickCommand(JOYSTICK.DOWN_DESC);
            BorderButton_Left_Trigger = JoyStickCommand(JOYSTICK.LEFT_DESC);
            BorderButton_Right_Trigger = JoyStickCommand(JOYSTICK.RIGHT_DESC);

            BorderButton_Up_Release = JoyStickCommand();
            BorderButton_Down_Release = JoyStickCommand();
            BorderButton_Left_Release = JoyStickCommand();
            BorderButton_Right_Release = JoyStickCommand();
        }

        /// <summary>
        /// 控制顯示說明文字
        /// </summary>
        private bool? _descriptionDisplayBoolen = true;
        public bool? DescriptionDisplayBoolen
        {
            get
            {
                if (_descriptionDisplayBoolen is false)
                {
                    DescriptionDisplayVisibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    DescriptionDisplayVisibility = System.Windows.Visibility.Visible;
                }
                return _descriptionDisplayBoolen;
            }
            set
            {
                _descriptionDisplayBoolen = value;
                OnPropertyChanged("DescriptionDisplayBoolen");
            }
        }


        private System.Windows.Visibility _descriptionDisplayVisibility = System.Windows.Visibility.Visible;

        public System.Windows.Visibility DescriptionDisplayVisibility
        {
            get
            {
                return _descriptionDisplayVisibility;
            }
            set
            {
                if (_descriptionDisplayVisibility != value)
                {
                    _descriptionDisplayVisibility = value;
                    OnPropertyChanged("DescriptionDisplayVisibility");
                }
            }
        }


        private bool _button_Up_IsEnabled = true;
        public bool Button_Up_IsEnabled
        {
            get { return _button_Up_IsEnabled; }
            set { _button_Up_IsEnabled = value; OnPropertyChanged("Button_Up_IsEnabled"); }
        }

        private bool _button_Down_IsEnabled = true;
        public bool Button_Down_IsEnabled
        {
            get { return _button_Down_IsEnabled; }
            set { _button_Down_IsEnabled = value; OnPropertyChanged("Button_Down_IsEnabled"); }
        }

        private bool _button_Left_IsEnabled = true;
        public bool Button_Left_IsEnabled
        {
            get { return _button_Left_IsEnabled; }
            set { _button_Left_IsEnabled = value; OnPropertyChanged("Button_Left_IsEnabled"); }
        }

        private bool _button_Right_IsEnabled = true;
        public bool Button_Right_IsEnabled
        {
            get { return _button_Right_IsEnabled; }
            set { _button_Right_IsEnabled = value; OnPropertyChanged("Button_Right_IsEnabled"); }
        }




        #region 命令處理方法
        /// <summary>
        /// 搖桿操作命令 parameter
        /// </summary>
        public WPFWindowsBase.RelayParameterizedCommand LeftButtonCommand
        {
            get
            {
                return new WPFWindowsBase.RelayParameterizedCommand(el =>
                {
                    PanelButton PButton = ApplicationViewModel.PanelButton;
                    PButton.Joystick = (JOYSTICK)el;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }

        /// <summary>
        /// 搖桿操作命令 取消
        /// </summary>
        public WPFWindowsBase.RelayCommand ButtonCancelCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton PButton = ApplicationViewModel.PanelButton;
                    PButton.Joystick = JOYSTICK.NULL;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }



        /// <summary>
        /// 搖桿操作命令 parameter
        /// </summary>
        public WPFWindowsBase.RelayCommand JoyStickCommand(JOYSTICK _joystick = JOYSTICK.NULL)
        {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton PButton = ApplicationViewModel.PanelButton;
                    PButton.Joystick = _joystick;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            
        }



        #endregion


        /// <summary>
        /// 上按鈕
        /// </summary>
        public WPFWindowsBase.RelayCommand BorderButton_Up_Trigger { get; set; }

        /// <summary>
        /// 上按鈕釋放
        /// </summary>
       public WPFWindowsBase.RelayCommand BorderButton_Up_Release { get; set; }

        /// <summary>
        /// 下按鈕
        /// </summary>
        public WPFWindowsBase.RelayCommand BorderButton_Down_Trigger { get; set; }
        /// <summary>
        /// 下按鈕釋放
        /// </summary>
        public WPFWindowsBase.RelayCommand BorderButton_Down_Release { get; set; }

        /// <summary>
        /// 左按鈕
        /// </summary>
        public WPFWindowsBase.RelayCommand BorderButton_Left_Trigger { get; set; }

        /// <summary>
        /// 左按鈕釋放
        /// </summary>
        public WPFWindowsBase.RelayCommand BorderButton_Left_Release { get; set; }

        /// <summary>
        /// 右按鈕
        /// </summary>
        public WPFWindowsBase.RelayCommand BorderButton_Right_Trigger { get; set; }
        /// <summary>
        /// 右按鈕釋放
        /// </summary>
        public WPFWindowsBase.RelayCommand BorderButton_Right_Release { get; set; }
















    }

}

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

        }

        /// <summary>
        /// 控制顯示說明文字
        /// </summary>
        public bool DescriptionDisplayBoolen { get; set; } = true;

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
                  /*
        private WPFWindowsBase.RelayParameterizedCommand _joystick_Command = null;
        /// <summary>
        /// 搖桿操作命令 含parameter
        /// </summary>
        public WPFWindowsBase.RelayParameterizedCommand Joystick_Command
        {
            get
            {
                if(_joystick_Command == null)
                {
                    _joystick_Command = new WPFWindowsBase.RelayParameterizedCommand(el =>
                    {
                        if (el is JOYSTICK _joystick)
                        {
                            PanelButton PButton = ApplicationViewModel.PanelButton;
                            PButton.Joystick = _joystick;
                            CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                        }
                    });
                }
                return _joystick_Command;
            }
            set
            {
                _joystick_Command = value;
            }
        }
                    */

        /// <summary>
        /// 搖桿操作命令 取消
        /// </summary>
        /* public WPFWindowsBase.RelayCommand ButtonCancelCommand
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
         }*/







        /// <summary>
        /// 搖桿操作命令 parameter
        /// </summary>
        /*private WPFWindowsBase.RelayCommand JoystickCommand(JOYSTICK _joystick = JOYSTICK.NULL)
        {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton PButton = ApplicationViewModel.PanelButton;
                    PButton.Joystick = _joystick;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
        }    */



        #endregion

        #region 命令處理方法
        /// <summary>
        /// 滑鼠指標在箭頭向上元素上方且按下滑鼠左按鈕時處理方法。
        /// </summary>
        public WPFWindowsBase.RelayParameterizedCommand LeftButtonCommand
        {
            get
            {
                return new WPFWindowsBase.RelayParameterizedCommand(el =>
                {
                    JOYSTICK select = (JOYSTICK)el;
                    PanelButton _ = ApplicationViewModel.PanelButton;
                    _.Joystick = select;
                    CodesysIIS.WriteCodesysMemor.SetPanel(_);
                });
            }
        }
        /// <summary>
        /// 滑鼠指標在此下橢圓形元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public WPFWindowsBase.RelayCommand ButtonUpCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton _ = ApplicationViewModel.PanelButton;
                    _.Joystick = JOYSTICK.NULL;
                    CodesysIIS.WriteCodesysMemor.SetPanel(_);
                });
            }
        }
        #endregion




        /*
        /// <summary>
        /// 上按鈕
        /// </summary>
        public WPFWindowsBase.RelayCommand Joystcik_Up_Trigger_Command { get; set; }

        /// <summary>
        /// 上按鈕釋放
        /// </summary>
       public WPFWindowsBase.RelayCommand Joystcik_Up_Release_Command { get; set; }

        /// <summary>
        /// 下按鈕
        /// </summary>
        public WPFWindowsBase.RelayCommand Joystcik_Down_Trigger_Command { get; set; }
        /// <summary>
        /// 下按鈕釋放
        /// </summary>
        public WPFWindowsBase.RelayCommand Joystcik_Down_Release_Command { get; set; }

        /// <summary>
        /// 左按鈕
        /// </summary>
        public WPFWindowsBase.RelayCommand Joystcik_Left_Trigger_Command { get; set; }

        /// <summary>
        /// 左按鈕釋放
        /// </summary>
        public WPFWindowsBase.RelayCommand Joystcik_Left_Release_Command { get; set; }

        /// <summary>
        /// 右按鈕
        /// </summary>
        public WPFWindowsBase.RelayCommand Joystcik_Right_Trigger_Command { get; set; }
        /// <summary>
        /// 右按鈕釋放
        /// </summary>
        public WPFWindowsBase.RelayCommand Joystcik_Right_Release_Command { get; set; }
                               */



        /// <summary>
        /// 定位柱
        /// </summary>
        public WPFWindowsBase.RelayCommand PostRiseCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //相反訊號
                    PButton.PostRise = !PButton.PostRise;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }












    }

}

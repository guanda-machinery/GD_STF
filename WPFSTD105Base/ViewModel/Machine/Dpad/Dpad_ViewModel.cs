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
        /// <summary>
        /// 滑鼠指標在箭頭向上元素上方且按下滑鼠左按鈕時處理方法。
        /// </summary>
        /// 


        public WPFWindowsBase.RelayParameterizedCommand LeftButtonCommand
        {
            get
            {
                return new WPFWindowsBase.RelayParameterizedCommand(el =>
                {
                    if (el is JOYSTICK)
                    {
                        JOYSTICK select = (JOYSTICK)el;
                        PanelButton _ = ApplicationViewModel.PanelButton;
                        _.Joystick = select;
                        CodesysIIS.WriteCodesysMemor.SetPanel(_);
                    }

                    if (el is GD_STD.Enum.AXIS_SELECTED)
                    {
                        AXIS_SELECTED select = (AXIS_SELECTED)el;
                        PanelButton _ = ApplicationViewModel.PanelButton;
                        _.AxisSelect = select;
                        CodesysIIS.WriteCodesysMemor.SetPanel(_);
                    }
                });
            }
        }



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




        #endregion




    }

}

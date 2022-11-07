using GD_STD;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFSTD105.ViewLocator;
using static WPFSTD105.CodesysIIS;

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
        private bool? _descriptionDisplayBoolen =true;
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
                if(_descriptionDisplayVisibility !=value)
                {
                    _descriptionDisplayVisibility = value;
                    OnPropertyChanged("DescriptionDisplayVisibility");
                }
            }
        }



        public bool Button_Up_IsEnabled { get; set; } = true;
        public bool Button_Down_IsEnabled { get; set; } = true;
        public bool Button_Left_IsEnabled { get; set; } = true;
        public bool Button_Right_IsEnabled { get; set; } = true;




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
                    PanelButton PButton = ApplicationViewModel.PanelButton;
                    PButton.Joystick = (JOYSTICK)el;
                    WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }

        /// <summary>
        /// 滑鼠指標在此下橢圓形元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public WPFWindowsBase.RelayCommand ButtonUpCommand()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                PanelButton PButton  = ApplicationViewModel.PanelButton;
                PButton.Joystick = JOYSTICK.NULL;
                WriteCodesysMemor.SetPanel(PButton);
            });
        }
        #endregion
    }

}

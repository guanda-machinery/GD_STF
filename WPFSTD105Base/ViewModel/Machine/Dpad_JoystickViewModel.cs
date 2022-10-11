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
    /// 虛擬搖桿的VM   
    /// </summary>
    public class Dpad_JoystickViewModel : WPFWindowsBase.BaseViewModel
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public Dpad_JoystickViewModel()
        {

        }

        /// <summary>
        /// 控制顯示說明文字
        /// </summary>
        private bool? _descriptionDisplayBoolen = null;
        public bool? DescriptionDisplayBoolen
        {
            get
            {
                return _descriptionDisplayBoolen;
            }
            set
            {
                if (_descriptionDisplayBoolen != value)
                {
                    _descriptionDisplayBoolen = value;
                    OnPropertyChanged("DescriptionDisplayBoolen");
                }

                if(_descriptionDisplayBoolen is true)
                {
                    _descriptionDisplayVisibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    _descriptionDisplayVisibility = System.Windows.Visibility.Collapsed;
                }
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
                    WriteCodesysMemor.SetPanel(_);
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
                PanelButton _ = ApplicationViewModel.PanelButton;
                _.Joystick = JOYSTICK.NULL;
                WriteCodesysMemor.SetPanel(_);
            });
        }
        #endregion
    }

}

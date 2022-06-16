using GD_STD;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase = WPFWindowsBase;
using static WPFSTD105.ViewLocator;
using static WPFSTD105.CodesysIIS;
namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 虛擬搖桿的VM
    /// </summary>
    public abstract class JoystickVM : WPFBase.BaseViewModel
    {        
        /// <summary>
        /// 標準建構式
        /// </summary>
        public JoystickVM()
        {
            LeftButtonCommand =  LeftButton();
            //JoystickDownPreviewMouseLeftButtonDownCommand = JoystickDownPreviewMouseLeftButtonDown();
            //JoystickLeftPreviewMouseLeftButtonDownCommand = JoystickLeftPreviewMouseLeftButtonDown();
            //JoystickRightPreviewMouseLeftButtonDownCommand = JoystickRightPreviewMouseLeftButtonDown();
            //CircleTopPreviewMouseLeftButtonDownCommand = CircleTopPreviewMouseLeftButtonDown();
            //CircleMiddlePreviewMouseLeftButtonDownCommand = CircleMiddlePreviewMouseLeftButtonDown();
            //CircleBottomPreviewMouseLeftButtonDownCommand = CircleBottomPreviewMouseLeftButtonDown();
            //EllipseTopPreviewMouseLeftButtonDownCommand = EllipseTopPreviewMouseLeftButtonDown();
            //EllipseBottomPreviewMouseLeftButtonDownCommand = EllipseBottomPreviewMouseLeftButtonDown();
            //ButtonUpCommand = ButtonUp();
        }
        /// <summary>
        /// 控制顯示說明文字
        /// </summary>
        public bool DescriptionDisplayControl { get; set; }
        /// <summary>
        /// 搖桿向上功能說明
        /// </summary>
        public string JoystickUpDESC { get; set; }
        /// <summary>
        /// 搖桿向下功能說明
        /// </summary>
        public string JoystickDownDESC { get; set; }
        /// <summary>
        /// 搖桿向左功能說明
        /// </summary>
        public string JoystickLeftDESC { get; set; }
        /// <summary>
        /// 搖桿向右功能說明
        /// </summary>
        public string JoystickRightDESC { get; set; }
        /// <summary>
        /// 搖桿面板左上按鈕功能說明
        /// </summary>
        public string EllipseTopButtonDESC { get; set; }
        /// <summary>
        /// 搖桿面板左下按鈕功能說明
        /// </summary>
        public string EllipseBottomButtonDESC { get; set; }
        /// <summary>
        /// 搖桿面板上圓形按鈕功能說明
        /// </summary>
        public string CircleTopButtonDESC { get; set; }
        /// <summary>
        /// 搖桿面板中圓形按鈕功能說明
        /// </summary>
        public string CircleMiddleButtonDESC { get; set; }
        /// <summary>
        /// 搖桿面板下圓形按鈕功能說明
        /// </summary>
        public string CircleBottomButtonDESC { get; set; }
        /// <summary>
        /// 顯示說明功能
        /// </summary>
        public bool IsEnable { get; set; }
        #region 命令
        /// <summary>
        /// 滑鼠指標在箭頭向上元素上方且按下滑鼠左按鈕時發生。
        /// </summary>
        public WPFBase.RelayParameterizedCommand LeftButtonCommand { get; set; }
        /// <summary>
        /// 滑鼠指標在此下橢圓形元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        public WPFBase.RelayCommand ButtonUpCommand { get; set; }
        #endregion

        #region 命令處理方法
        /// <summary>
        /// 滑鼠指標在箭頭向上元素上方且按下滑鼠左按鈕時處理方法。
        /// </summary>
        protected WPFBase.RelayParameterizedCommand LeftButton()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                JOYSTICK select = (JOYSTICK)el;
                PanelButton _ = ApplicationViewModel.PanelButton;
                _.Joystick = select;
                WriteCodesysMemor.SetPanel(_);
            });
        }
        /// <summary>
        /// 滑鼠指標在此下橢圓形元素上方且放開滑鼠左按鈕時發生。
        /// </summary>
        protected WPFBase.RelayCommand ButtonUp()
        {
            return new WPFBase.RelayCommand(() =>
            {
                PanelButton _ = ApplicationViewModel.PanelButton;
                _.Joystick = JOYSTICK.NULL;
                WriteCodesysMemor.SetPanel(_);
            });
        }
        #endregion
    }
}

using DevExpress.Mvvm;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using GD_STD;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFWindowsBase;
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



        public object Joystick_LEFT_DESC_Trigger_Parameter { get; set; } = JOYSTICK.LEFT_DESC;
        public object Joystick_RIGHT_DESC_Trigger_Parameter { get; set; } = JOYSTICK.RIGHT_DESC;
        public object Joystick_UP_DESC_Trigger_Parameter { get; set; } = JOYSTICK.UP_DESC;
        public object Joystick_DOWN_DESC_Trigger_Parameter { get; set; } = JOYSTICK.DOWN_DESC;

        public object Joystick_LEFT_DESC_Release_Parameter { get; set; } = JOYSTICK.NULL;
        public object Joystick_RIGHT_DESC_Release_Parameter { get; set; } = JOYSTICK.NULL;
        public object Joystick_UP_DESC_Release_Parameter { get; set; } = JOYSTICK.NULL;
        public object Joystick_DOWN_DESC_Release_Parameter{ get; set; } = JOYSTICK.NULL;







        public bool IsMainSpindleMode { get; set; } = false;






        #region 命令處理方法
        /// <summary>
        /// 滑鼠指標在箭頭向上元素上方且按下滑鼠左按鈕時處理方法。
        /// </summary>
        /// 


        public ICommand LeftButtonCommand
        {
            get
            {
                return new WPFWindowsBase.RelayParameterizedCommand(el =>
                {
                    Task.Run(() =>
                    {
                        PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                        //搖桿命令
                        if (el is JOYSTICK)
                        {
                            PButton.Joystick = (JOYSTICK)el;

                            if (PButton.Joystick == JOYSTICK.NULL)
                                Thread.Sleep(100);
                            CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                        }
                        //軸向選擇
                        if (el is GD_STD.Enum.AXIS_SELECTED)
                        {
                            PButton.AxisSelect = (AXIS_SELECTED)el;
                            CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                        }
                        //橫移料架移動
                        if (el is GD_STD.Enum.MOBILE_RACK)
                        {
                            PButton.Move_OutSide = (MOBILE_RACK)el;
                            if (PButton.Move_OutSide == MOBILE_RACK.NULL)
                                Thread.Sleep(100);
                            CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                        }
                        if (el is GD_STD.Enum.SHELF)
                        {
                            var SetShelf = (SHELF)el;

                            //上升命令
                            if (SetShelf == SHELF.LEVEL2)
                            {
                                if (PButton.Traverse_Shelf_UP == SHELF.NULL)
                                    PButton.Traverse_Shelf_UP = SHELF.LEVEL1;
                                else if (PButton.Traverse_Shelf_UP == SHELF.LEVEL1)
                                    PButton.Traverse_Shelf_UP = SHELF.LEVEL2;
                                else
                                    PButton.Traverse_Shelf_UP = SHELF.LEVEL2;
                            }
                            //下降命令
                            if (SetShelf == SHELF.NULL)
                            {
                                if (PButton.Traverse_Shelf_UP == SHELF.LEVEL2)
                                    PButton.Traverse_Shelf_UP = SHELF.LEVEL1;
                                else if (PButton.Traverse_Shelf_UP == SHELF.LEVEL1)
                                    PButton.Traverse_Shelf_UP = SHELF.NULL;
                                else
                                    PButton.Traverse_Shelf_UP = SHELF.NULL;
                            }
                            CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                        }

                        if(el is GD_STD.Enum.DRILL_POSITION)
                        {
                            var Drill_P = (GD_STD.Enum.DRILL_POSITION)el;
                            PButton.DrillSelected = Drill_P;
                            CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                        }

                        if (el == null)
                            Thread.Sleep(100);
                    });
                });
            }
        }







        /// <summary>
        /// 下壓夾具 入口命令
        /// </summary>
        /*public WPFWindowsBase.RelayCommand SelectClampDownEntranceCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton panelButton = ApplicationViewModel.PanelButton;
                    panelButton.ClampDownSelected = CLAMP_DOWN.Entrance;
                    CodesysIIS.WriteCodesysMemor.SetPanel(panelButton);
                });
            }
        }*/

        /// <summary>
        /// 下壓夾具 出口命令
        /// </summary>
        /*public WPFWindowsBase.RelayCommand SelectClampDownExportCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton panelButton = ApplicationViewModel.PanelButton;
                    panelButton.ClampDownSelected = CLAMP_DOWN.Export;
                    CodesysIIS.WriteCodesysMemor.SetPanel(panelButton);
                });
            }
        }*/

        #endregion
    }

}

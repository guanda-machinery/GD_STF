using GD_STD;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFSTD105.ViewLocator;
using static WPFSTD105.CodesysIIS;
using System.Threading;
using System.Windows.Input;
using System.Runtime.Remoting.Messaging;
using DevExpress.CodeParser;
using WPFSTD105.FluentAPI;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 設備料架 
    /// </summary>
    public class RacksOperationVM : Dpad_ViewModel
    {
        private bool _rockSpeed;
        private bool _openRoll;

        /// <summary>
        /// 標準建構式
        /// </summary>
        public RacksOperationVM()
        {
            //手動操作時只需頂到最上面或最下面
            Joystick_UP_DESC_Trigger_Parameter = SHELF.LEVEL2;
            Joystick_UP_DESC_Release_Parameter = null;

            Joystick_DOWN_DESC_Trigger_Parameter = SHELF.NULL;
            Joystick_DOWN_DESC_Release_Parameter = null;

            Joystick_LEFT_DESC_Trigger_Parameter = MOBILE_RACK.INSIDE;
            Joystick_LEFT_DESC_Release_Parameter = MOBILE_RACK.NULL;

            Joystick_RIGHT_DESC_Trigger_Parameter = MOBILE_RACK.OUTER;
            Joystick_RIGHT_DESC_Release_Parameter = MOBILE_RACK.NULL;



            STDSerialization ser = new STDSerialization();
            FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();

            EntranceRack_MaxValue = optionSettings.EntranceTraverseNumber;
            ExportRack_MaxValue = optionSettings.ExportTraverseNumber;

        }

        //解構





        #region 公開屬性

        /// <summary>
        /// 啟動動力滾輪
        /// </summary>
        public bool OpenRoll
        {
            get => _openRoll;
            set
            {
                PanelButton panel = ApplicationViewModel.PanelButton;
                panel.OpenRoll = value;
                _openRoll = value;
                WriteCodesysMemor.SetPanel(panel);
            }
        }
        /// <summary>
        /// 入口料架唯讀狀態
        /// </summary>
        public bool EntranceReadOnly
        {
            get
            {
                return GD_STD.Properties.Optional.Default.EntranceTraverseNumber != 0;
            }
        }
        /// <summary>
        /// 出口料架唯讀狀態
        /// </summary>
        public bool ExportReadOnly
        {
            get
            {
                return GD_STD.Properties.Optional.Default.ExportTraverseNumber != 0;
            }
        }
        /// <summary>
        /// 料架速度 低速 return false 高速 return true 
        /// </summary>
        public bool RockSpeed
        {
            get
            {
                return _rockSpeed;
            }
            set
            {
                PanelButton panel = ApplicationViewModel.PanelButton;

                panel.HighSpeed = value;
                WriteCodesysMemor.SetPanel(panel);
                _rockSpeed = value;
            }
        }


        /// <summary>
        /// 紀錄目前橫移是否再前進狀態。前進中回傳 true，沒有前進回傳false。
        /// </summary>
        public bool MoveFrontState { get; set; }
        /// <summary>
        /// 紀錄目前橫移是否再後退狀態。前進中回傳 true，沒有前進回傳false。
        /// </summary>
        public bool MoveBackState { get; set; }
        /// <summary>
        /// 移動料架上升段數
        /// </summary>
        public int RiseLevel { get; set; }
        /// <summary>
        /// 目前數量
        /// </summary>
        public double CurrentValue { get; set; } = 2;



        public double EntranceRack_CurrentValue { get; set; } = 2;

        /// <summary>
        /// 目前數量-出口
        /// </summary>
        public double ExportRack_CurrentValue { get; set; } = 2;

        /// <summary>
        /// 
        /// </summary>

        STDSerialization ser = new STDSerialization();
                          
        public double EntranceRack_MaxValue
        {
            get
            {
                FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();
                return optionSettings.EntranceTraverseNumber;
            }
           /* set
            {
                FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();
                optionSettings.EntranceTraverseNumber = value;
                FluentAPI.OptionSettings optionSettings = ser.SetOptionSettings();
                return optionSettings.EntranceTraverseNumber;
            }*/
        }
        /// <summary>
        /// 目前數量-出口
        /// </summary>
        public double ExportRack_MaxValue
        {
            get
            {
                FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();
                return optionSettings.ExportTraverseNumber;
            }
           /* set
            {

            }*/
        }


        /// <summary>
        /// 入口按鈕可用
        /// </summary>
        public bool EntranceRack_IsEnable
        {
            get
            {
                return EntranceRack_MaxValue > 0;
            }
        }
        /// <summary>
        /// 出口按鈕可用
        /// </summary>
        public bool ExportRack_IsEnable
        {
            get
            {
                return ExportRack_MaxValue > 0;
            }
        }



        #endregion




            #region 命令

            /// <summary>
            /// 動力滾輪命令
            /// </summary>
        public WPFWindowsBase.RelayParameterizedCommand RollerCommand
        {
            get
            {
                return new WPFWindowsBase.RelayParameterizedCommand(el =>
                {
                    PanelButton panelButton = ApplicationViewModel.PanelButton;
                    panelButton.RollMove = (MOBILE_RACK)el;
                    if (panelButton.RollMove == MOBILE_RACK.NULL)
                    {
                        panelButton.Clutch = false;

                    }
                    else
                    {
                        panelButton.Clutch = true;
                    }
                    WriteCodesysMemor.SetPanel(panelButton);
                });
            }
        }
        /// <summary>
        /// 選擇入口料架命令
        /// </summary>
        public WPFWindowsBase.RelayCommand SelectExportRackCommand { get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton panelButton = ApplicationViewModel.PanelButton;
                    panelButton.ExportRack = true;
                    panelButton.EntranceRack = false;

                    WriteCodesysMemor.SetPanel(panelButton);
                });
            }
        }

        /// <summary>
        /// 選擇出口料架命令
        /// </summary>
        public WPFWindowsBase.RelayCommand SelectEntranceRackCommand 
        { 
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton panelButton = ApplicationViewModel.PanelButton;
                    panelButton.ExportRack = false;
                    panelButton.EntranceRack = true;
                    WriteCodesysMemor.SetPanel(panelButton);
                });
            }
        }

        /// <summary>
        /// 橫移料架上升(舊)
        /// </summary>
        public WPFWindowsBase.RelayParameterizedCommand RiseCommand
        {
            get
            {
                return new WPFWindowsBase.RelayParameterizedCommand(el =>
                {
                    int value = Convert.ToInt32(el);
                    int rise = RiseLevel + value;
                    if (rise >= 0 && rise <= 2)
                    {
                        RiseLevel += value;
                        PanelButton panelButton = ApplicationViewModel.PanelButton;
                        panelButton.Traverse_Shelf_UP = (SHELF)RiseLevel;
                        WriteCodesysMemor.SetPanel(panelButton);
                    }
                });
            }
        }
        /// <summary>
        /// 橫移料架往前移動(舊)
        /// </summary>
        public WPFWindowsBase.RelayParameterizedCommand MoveCommand
        {
            get
            {
                return new WPFWindowsBase.RelayParameterizedCommand(el =>
                {
                    PanelButton panelButton = ApplicationViewModel.PanelButton;
                    MoveFrontState = !MoveFrontState;
                    panelButton.Move_OutSide = (MOBILE_RACK)el;
                    WriteCodesysMemor.SetPanel(panelButton);
                });
            }
        }
        /// <summary>
        /// 選擇橫移料架數量命令(舊)
        /// </summary>
        public WPFWindowsBase.RelayCommand SelectCountCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton panelButton = ApplicationViewModel.PanelButton;
                    panelButton.Count = CountResult(panelButton.Count, panelButton.EntranceRack ? GD_STD.Properties.Optional.Default.EntranceTraverseNumber : GD_STD.Properties.Optional.Default.ExportTraverseNumber);
                    CurrentValue = panelButton.Count;
                    WriteCodesysMemor.SetPanel(panelButton);
                });
            }
        }


        /// <summary>
        /// 選擇橫移料架數量命令 入口
        /// </summary>
        public WPFWindowsBase.RelayCommand SelectCount_EntranceRack_Command
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton panelButton = ApplicationViewModel.PanelButton;
                    //panelButton.Count = CountResult(panelButton.Count, GD_STD.Properties.Optional.Default.EntranceTraverseNumber );
                    panelButton.Count = CountResult(panelButton.Count, Convert.ToByte(EntranceRack_MaxValue));
                    EntranceRack_CurrentValue = panelButton.Count;
                    WriteCodesysMemor.SetPanel(panelButton);
                });
            }
        }
        /// <summary>
        /// 選擇橫移料架數量命令 出口
        /// </summary>
        public WPFWindowsBase.RelayCommand SelectCount_ExportRack_Command
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    PanelButton panelButton = ApplicationViewModel.PanelButton;
                    //panelButton.Count = CountResult(panelButton.Count, GD_STD.Properties.Optional.Default.ExportTraverseNumber);
                    panelButton.Count = CountResult(panelButton.Count, Convert.ToByte(ExportRack_MaxValue));
                    ExportRack_CurrentValue = panelButton.Count;
                    WriteCodesysMemor.SetPanel(panelButton);
                });
            }
        }

        /// <summary>
        /// 定位柱
        /// </summary>
        public ICommand PostRiseCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;
                    //PButton.postrise無法運作 原因不明
                    PanelButton PButton = ApplicationViewModel.PanelButton;
                    //相反訊號
                    if(PButton.Joystick != JOYSTICK.ELLIPSE_BOTTOM_DESC)
                    {
                        PButton.Joystick = JOYSTICK.ELLIPSE_BOTTOM_DESC;
                    }
                    else
                    {            
                        PButton.Joystick = JOYSTICK.NULL;
                    }

                    WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }




        #endregion

        #region 私有方法
        private short CountResult(short count, byte maxValue)
        {
            if (count >= maxValue)
            {
                return 2;
            }
            else
            {
                return Convert.ToInt16(count + 1);                
            }
        }





        #endregion
    }
}

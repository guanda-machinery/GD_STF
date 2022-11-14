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
            this.Button_Down_IsEnabled = false;

            PanelButton panelButton = ApplicationViewModel.PanelButton;
            //如果都沒有選擇出入或口料架
            if (!panelButton.EntranceRack && !panelButton.ExportRack)
            {
                //如果出口料架不是唯讀
                if (EntranceReadOnly)
                {
                    panelButton.EntranceRack = true;
                }
                else
                {
                    panelButton.ExportRack = true;
                }
                WriteCodesysMemor.SetPanel(panelButton);
            }
        }

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
                return GD_STD.Properties.Optional.Default.EntranceTraverseNumber !=0;
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

        ///// <summary>
        ///// 移動料架上升或下降，控制開關。 (綁定VM)
        ///// </summary>
        //public bool RisePowr
        //{
        //    get => ApplicationViewModel.PanelButton.EntranceRack ? _EntranceRisePowr : _ExportRisePowr;
        //    set
        //    {
        //        if (ApplicationViewModel.PanelButton.EntranceRack)
        //        {
        //            _EntranceRisePowr = value;
        //        }
        //        else
        //        {
        //            _ExportRisePowr = value;
        //        }
        //    }
        //}
        ///// <summary>
        ///// 橫移料架移動，控制開關
        ///// </summary>
        //public bool MovePowr
        //{
        //    get => ApplicationViewModel.PanelButton.EntranceRack ? _EntranceMovePowr : _ExportMovePowr;
        //    set
        //    {
        //        if (ApplicationViewModel.PanelButton.EntranceRack)
        //        {
        //            _EntranceMovePowr = value;
        //        }
        //        else
        //        {
        //            _ExportMovePowr = value;
        //        }
        //    }
        //}
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
        /// 橫移料架上升
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
        /// 橫移料架往前移動
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
        /// 選擇橫移料架數量命令
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

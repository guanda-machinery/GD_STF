using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.ViewModel
{
    public class ToolMagazineControl_ViewModel : Dpad_Joystick_ViewModel
    {
        public ToolMagazineControl_ViewModel()
        {
            
            this.Button_Up_IsEnabled = false;
            this.Button_Down_IsEnabled = false;

         //   this.JoyStick_BorderButton1_Trigger = ToolMagazine_A_Command; //刀庫A
          //  this.JoyStick_BorderButton3_Trigger = ToolMagazine_B_Command; //刀庫B
           // this.JoyStick_BorderButton4_Trigger = ToolMagazine_C_Command;//刀庫C
            //this.JoyStick_BorderButton5_Trigger = ToolMagazine_D_Command;//刀庫D
            //this.JoyStick_BorderButton6_Trigger = ToolMagazine_E_Command; //刀庫E

            //this.Joystcik_Left_Trigger_Command = ToolMagazine_Out_Command; //刀庫out ->直接繼承joystick
            //this.Joystcik_Right_Trigger_Command = ToolMagazine_In_Command; //刀庫in ->直接繼承joystick

           // this.JoyStick_BorderButton1_Release = null; //刀庫B
           // this.JoyStick_BorderButton3_Release = null; //刀庫B
           // this.JoyStick_BorderButton4_Release = null;//刀庫C
           // this.JoyStick_BorderButton5_Release = null;//刀庫D
           // this.JoyStick_BorderButton6_Release = null; //刀庫E

            //this.Joystcik_Left_Release_Command = null; //刀庫out ->直接繼承joystick
            //this.Joystcik_Right_Release_Command = null; //刀庫in ->直接繼承joystick

            //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
            //PButton.DrillWarehouse = true;
            //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);

              UpDate();

        }



        private void UnusedSave()
        {
            for (int i = 0; i < UnusedSelected.Count; i++)
            {
                DrillChange(UnusedSelected[i]);
            }
            CodesysIIS.WriteCodesysMemor.SetDrillWarehouse(_DrillWarehouse);//寫入記憶體
        }


        private void UpDate()
        {
            _DrillWarehouse = CodesysIIS.ReadCodesysMemor.GetDrillWarehouse(); //讀取記憶體目前的刀庫參數

            //查看刀庫
            if (!_MecOptional.LeftExport) //沒有左軸出口刀庫
            {
                GD_STD.Base.DrillSetting drill = _DrillWarehouse.LeftExport[0];
                drill.IsCurrent = true;
                _DrillWarehouse.LeftExport = new GD_STD.Base.DrillSetting[1] { drill };

            }
            if (!_MecOptional.LeftEntrance) //如果沒有入口刀庫
            {
                this.LeftEntranceIsEnabled = false;
            }
            if (!_MecOptional.RightExport)
            {
                GD_STD.Base.DrillSetting drill = _DrillWarehouse.RightExport[0];
                drill.IsCurrent = true;
                _DrillWarehouse.RightExport = new GD_STD.Base.DrillSetting[1] { drill };
            }
            if (!_MecOptional.RightEntrance)
            {
                this.RightEntranceIsEnabled = false;
            }
            if (!_MecOptional.Middle)
            {
                DrillEditing = false;
                GD_STD.Base.DrillSetting drill = _DrillWarehouse.Middle[0];
                drill.IsCurrent = true;
                _DrillWarehouse.Middle = new GD_STD.Base.DrillSetting[1] { drill };
            }
            /*選擇刀庫的位置,並顯示在介面上(裝載在主軸上的刀具)*/
            //選擇中軸刀庫
            if (IsSelectdMiddle)
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.Middle));
            //選擇左軸入料口刀庫
            else if (IsSelectdLeftEntrance)
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftEntrance));
            //選擇左軸出料口刀庫
            else if (IsSelectdLeftExport)
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftExport));
            //選擇右軸入料口刀庫
            else if (IsSelectdRightEntrance)
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightEntrance));
            //選擇右軸出料口刀庫
            else if (IsSelectdRightExport)
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightExport));
            /*選擇刀庫的位置,並顯示在介面上(裝載在主軸上面的刀具)*/
            //選擇中軸
            if (IsCurrentMiddle)
            {
                UseSelected = DrillTools(_DrillWarehouse.Middle, true);
            }
            //選擇左軸
            else if (IsCurrentLeft)
            {
                UseSelected = DrillTools(_DrillWarehouse.LeftEntrance, true).Count != 0 ? DrillTools(_DrillWarehouse.LeftEntrance, true) : DrillTools(_DrillWarehouse.LeftExport, true);
            }
            //選擇右軸
            else if (IsCurrentRight)
            {
                UseSelected = DrillTools(_DrillWarehouse.RightEntrance, true).Count != 0 ? DrillTools(_DrillWarehouse.RightEntrance, true) : DrillTools(_DrillWarehouse.RightExport, true);
            }

            CancelEnabled();
        }


        /// <summary>
        /// 使用者刀庫設定
        /// </summary>
        private GD_STD.DrillWarehouse _DrillWarehouse { get; set; }
        private FluentAPI.OptionSettings _MecOptional = new STDSerialization().GetOptionSettings();

        /// <summary>
        /// 取消目前刀具唯讀狀態
        /// </summary>
        private void CancelEnabled()
        {
            if (UseSelected.Count > 0)
            {
                List<_drillSetting> drills = new List<_drillSetting>();
                _drillSetting _ = UseSelected[0];
                //_.IsCurrent = false;
                drills.Add(_);
                UseSelected = new ObservableCollection<_drillSetting>(drills);
            }
        }
        /// <summary>
        /// A刀庫 上軸
        /// </summary>
        private WPFWindowsBase.RelayCommand ToolMagazine_A_Command
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    ClearSelectd();
                    this.IsSelectdMiddle = true;
                    DrillEditing = _MecOptional.Middle;
                    UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.Middle));
                    UseSelected = DrillTools(_DrillWarehouse.Middle, true);
                });
            }
        }
        /// <summary>
        /// B刀庫 左軸
        /// </summary>
        private WPFWindowsBase.RelayCommand ToolMagazine_B_Command
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    ClearSelectd();
                    this.IsSelectdLeftExport = true;
                    DrillEditing = _MecOptional.LeftExport;
                    UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftExport));
                    UseSelected = DrillTools(_DrillWarehouse.LeftExport, true);
                });
            }
        }

        /// <summary>
        /// C刀庫 右軸
        /// </summary>
        private WPFWindowsBase.RelayCommand ToolMagazine_C_Command
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    ClearSelectd();
                    this.IsSelectdRightExport = true;
                    DrillEditing = _MecOptional.RightExport;
                    UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightExport));
                    UseSelected = DrillTools(_DrillWarehouse.RightExport, true);
                });
            }
        }
        /// <summary>
        /// D刀庫 左軸
        /// </summary>
        private WPFWindowsBase.RelayCommand ToolMagazine_D_Command
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    ClearSelectd();
                    this.IsSelectdLeftEntrance = true;
                    DrillEditing = _MecOptional.LeftEntrance;
                    UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftEntrance));
                    UseSelected = DrillTools(_DrillWarehouse.LeftEntrance, true);
                });
            }
        }
        /// <summary>
        /// E刀庫 右軸
        /// </summary>
        private WPFWindowsBase.RelayCommand ToolMagazine_E_Command
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    ClearSelectd();
                    this.IsSelectdRightEntrance = true;
                    DrillEditing = _MecOptional.RightEntrance;
                    UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightEntrance));
                    UseSelected = DrillTools(_DrillWarehouse.RightEntrance, true);
                });
            }
        }







        /*
        private WPFWindowsBase.RelayCommand ToolMagazine_Out_Command
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //直接使用左右鍵

                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    PButton.Joystick = GD_STD.Enum.JOYSTICK.LEFT_DESC;
                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        private WPFWindowsBase.RelayCommand ToolMagazine_In_Command
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    if (PanelListening.SLPEMS() || PanelListening.SLPAlarm())
                        return;

                    //GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    //CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                });
            }
        }
        */


        public ObservableCollection<_drillSetting> UnusedSelected { get; set; } = new ObservableCollection<_drillSetting>();
        /// <summary>
        /// 裝載在主軸上的刀庫
        /// </summary>
        public ObservableCollection<_drillSetting> UseSelected { get; set; } = new ObservableCollection<_drillSetting>();

        /// <summary>
        /// 刀具品牌
        /// </summary>
        public GD_STD.DrillBrands DrillBrands
        {
            get
            {
                var _drillBrands = new STDSerialization().GetDrillBrands();
                if (_drillBrands.Count == 0)
                {
                    _drillBrands.Add(GD_STD.DrillBrand.GetNull());
                }
                return _drillBrands;
            }
        }
        /// <summary>
        /// 選擇<see cref="DrillWarehouse.LeftExport"/>
        /// </summary>
        public bool IsSelectdLeftExport { get; set; }
        /// <summary>
        /// 選擇<see cref="DrillWarehouse.RightExport"/>
        /// </summary>
        public bool IsSelectdRightExport { get; set; }
        /// <summary>
        /// 選擇<see cref="DrillWarehouse.LeftEntrance"/>
        /// </summary>
        public bool IsSelectdLeftEntrance { get; set; }
        /// <summary>
        /// 選擇<see cref="DrillWarehouse.RightEntrance"/>
        /// </summary>
        public bool IsSelectdRightEntrance { get; set; }
        /// <summary>
        /// 選擇中軸配裝的刀具
        /// </summary>
        public bool IsCurrentMiddle { get; set; } = true;
        /// <summary>
        /// 編輯為裝載在主軸上的刀具設定
        /// </summary>
        public bool DrillEditing { get; set; } = true;
        /// <summary>
        /// 選擇左軸配裝的刀具
        /// </summary>
        public bool IsCurrentLeft { get; set; }
        /// <summary>
        /// 選擇右軸配裝的刀具
        /// </summary>
        public bool IsCurrentRight { get; set; }
        /// <summary>
        /// 是否顯示中軸刀庫的按鈕
        /// </summary>
        public bool MiddleIsEnabled { get; set; }
        /// <summary>
        /// 是否顯示左軸出口刀庫的按鈕 
        /// </summary>
        public bool LeftExportIsEnabled { get; set; }
        /// <summary>
        /// 是否顯示右軸出口刀庫的按鈕 
        /// </summary>
        public bool RightExportIsEnabled { get; set; }
        /// <summary>
        /// 是否顯示 <see cref="DrillWarehouse.RightEntrance"/> 的按鈕 
        /// </summary>
        public bool RightEntranceIsEnabled { get; set; }
        /// <summary>
        /// 是否顯示 <see cref="DrillWarehouse.LeftEntrance"/> 的按鈕 
        /// </summary>
        public bool LeftEntranceIsEnabled { get; set; }
        /// <summary>
        /// 選擇<see cref="DrillWarehouse.Middle"/>
        /// </summary>
        public bool IsSelectdMiddle { get; set; } = true;


        private void ClearSelectd()
        {
            this.IsSelectdLeftEntrance = false;
            this.IsSelectdLeftExport = false;
            this.IsSelectdMiddle = false;
            this.IsSelectdRightEntrance = false;
            this.IsSelectdRightExport = false;
        }

        /// <summary>
        /// 轉換物件為類型
        /// </summary>
        /// <returns></returns>
        private List<_drillSetting> GetDrillSetting(GD_STD.Base.DrillSetting[] drillSettings)
        {
            List<_drillSetting> result = new List<_drillSetting>();
            for (int i = 0; i < drillSettings.Length; i++)
            {
                result.Add(new _drillSetting(drillSettings[i], DrillBrands));
                result[i].Index = i + 1;
            }
            return result;
        }

        private void DrillChange(_drillSetting drillSetting)
        {
            int index = drillSetting.Index - 1;//刀庫陣列位置

            GD_STD.Base.DrillSetting result = drillSetting.GetStruc();//轉換結構
            //判斷用戶選中的刀庫
            if (IsSelectdMiddle)
                _DrillWarehouse.Middle[index] = result;
            else if (IsSelectdLeftExport)
                _DrillWarehouse.LeftExport[index] = result;
            else if (IsSelectdRightExport)
                _DrillWarehouse.RightExport[index] = result;
            else if (IsSelectdLeftEntrance)
                _DrillWarehouse.LeftEntrance[index] = result;
            else if (IsSelectdRightEntrance)
                _DrillWarehouse.RightEntrance[index] = result;
        }

        /// <summary>
        /// 分類刀庫
        /// </summary>
        /// <param name="drillSettings">刀庫列表</param>
        /// <param name="isUse">是使用的刀庫</param>
        /// <returns></returns>
        private ObservableCollection<_drillSetting> DrillTools(GD_STD.Base.DrillSetting[] drillSettings, bool isUse)
        {
            return new ObservableCollection<_drillSetting>(from el in drillSettings where el.IsCurrent == isUse select new _drillSetting(el, DrillBrands));
        }

    }



}

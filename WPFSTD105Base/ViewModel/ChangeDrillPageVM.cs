using GD_STD;
using GD_STD.Enum;
using GD_STD.IBase;
using GD_STD.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using WPFSTD105.Listening;
using static GD_STD.Attribute.CodesysAttribute;
using static WPFSTD105.Properties.MecSetting;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;
using WPFSTD105.Properties;
using GD_STD.Base;
using WPFSTD105.Attribute;
using System.Windows;
using static WPFSTD105.SettingHelper;
using GD_STD.Phone;
using GD_STD.Data;
using DevExpress.Utils.Extensions;
using System.Reflection;
using DevExpress.Data.Extensions;
using System.Windows.Controls;
using WPFWindowsBase;
using DevExpress.XtraSpreadsheet.Model;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using log4net;
using WPFSTD105.FluentAPI;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 刀庫模型
    /// </summary>
    public class ChangeDrillPageVM : WPFSTD105.ViewModel.JoystickVM
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public ChangeDrillPageVM() : base()
        {
            EllipseBottomButtonDESC = "B刀庫";
            CircleTopButtonDESC = "C刀庫";
            CircleMiddleButtonDESC = "D刀庫";
            CircleBottomButtonDESC = "E刀庫";
            JoystickLeftDESC = "刀庫推出";
            JoystickRightDESC = "刀庫收回";
            CurrentLeftCommand = CurrentLeft();
            CurrentRightCommand = CurrentRight();
            CurrentMiddleCommand = CurrentMiddle();
            MiddleCommand = Middle();
            LeftExportCommand = LeftExport();
            RightExportCommand = RightExport();
            LeftEntranceCommand = LeftEntrance();
            RightEntranceCommand = RightEntrance();
            UseSaveCommand = UseSave();
            UnusedSaveCommand = UnusedSave();
            UpDateCommand = UpDateDrill();
            DrillBrands =  new STDSerialization().GetDrillBrands();
            if (DrillBrands.Count == 0)
            {
                DrillBrands.Add(DrillBrand.GetNull());
            }
            MiddleIsEnabled = _MecOptional.Middle;
            LeftEntranceIsEnabled = _MecOptional.LeftEntrance;
            LeftExportIsEnabled = _MecOptional.LeftExport;
            RightEntranceIsEnabled = _MecOptional.RightEntrance;
            RightExportIsEnabled = _MecOptional.RightExport;
            UpDate();

        }
        #region 公開屬性
        /// <summary>
        /// 未裝載在主軸上的刀庫
        /// </summary>
        public ObservableCollection<_drillSetting> UnusedSelected { get; set; }
        /// <summary>
        /// 裝載在主軸上的刀庫
        /// </summary>
        public ObservableCollection<_drillSetting> UseSelected { get; set; } = new ObservableCollection<_drillSetting>();
        /// <summary>
        /// 刀具品牌
        /// </summary>
        public DrillBrands DrillBrands { get; set; }
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



        //20221220
        /// <summary>
        /// 是否顯示中軸刀庫的按鈕
        /// </summary>
        public Visibility MiddleVisibility { get { return GD_STD.Properties.Optional.Default.Middle ? Visibility.Visible : Visibility.Collapsed; } }
        /// <summary>
        /// 是否顯示左軸出口刀庫的按鈕 
        /// </summary>
        public Visibility LeftExportVisibility { get { return GD_STD.Properties.Optional.Default.LeftExport ? Visibility.Visible : Visibility.Collapsed; } }
        /// <summary>
        /// 是否顯示右軸出口刀庫的按鈕 
        /// </summary>
        public Visibility RightExportVisibility { get { return GD_STD.Properties.Optional.Default.RightExport ? Visibility.Visible : Visibility.Collapsed; } }
        /// <summary>
        /// 是否顯示 <see cref="DrillWarehouse.RightEntrance"/> 的按鈕 
        /// </summary>
        public Visibility RightEntranceVisibility { get { return GD_STD.Properties.Optional.Default.RightEntrance ? Visibility.Visible : Visibility.Collapsed; } }
        /// <summary>
        /// 是否顯示 <see cref="DrillWarehouse.LeftEntrance"/> 的按鈕 
        /// </summary>
        public Visibility LeftEntranceVisibility { get { return GD_STD.Properties.Optional.Default.LeftEntrance ? Visibility.Visible : Visibility.Collapsed; } }

        public bool ToolMActivityButtonVisibilityBoolean { get; set; } = true;







        /// <summary>
        /// 選擇<see cref="DrillWarehouse.Middle"/>
        /// </summary>
        public bool IsSelectdMiddle { get; set; } = true;
        #endregion
        #region 私有屬性
        private FluentAPI.OptionSettings _MecOptional = new STDSerialization().GetOptionSettings();
        private DRILL_POSITION DRILL_POSITION { get; set; } = DRILL_POSITION.MIDDLE;

        private GD_STD.DrillWarehouse _drillwarehouse = null;
        /// <summary>
        /// 使用者刀庫設定
        /// </summary>
        private GD_STD.DrillWarehouse _DrillWarehouse
        {
            get
            {
                if (_drillwarehouse == null)
                    _drillwarehouse = ReadCodesysMemor.GetDrillWarehouse();
                return _drillwarehouse;
            }
            set
            {
                _drillwarehouse = value;
            }
        }
        #endregion
        #region 命令
        /// <summary>
        /// 變更 <see cref="UseSelected"/>
        /// </summary>
        public ICommand UseSaveCommand { get; set; }
        private WPFBase.RelayCommand UseSave()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (UseSelected.Count > 0)
                {

                }
            });
        }
        /// <summary>
        /// 變更 <see cref="UnusedSelected"/>
        /// </summary>
        public ICommand UnusedSaveCommand { get; set; }
        private WPFBase.RelayCommand UnusedSave()
        {
            return new WPFBase.RelayCommand(() =>
            {
                for (int i = 0; i < UnusedSelected.Count; i++)
                {
                    DrillChange(UnusedSelected[i]);
                }
                WriteCodesysMemor.SetDrillWarehouse(_DrillWarehouse);//寫入記憶體
            });
        }

        /// <summary>
        /// 選擇中<see cref="GD_STD.DrillWarehouse.Middle"/>命令
        /// </summary>
        public ICommand MiddleCommand { get; set; }
        private WPFBase.RelayCommand Middle()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearSelectd();
                this.IsSelectdMiddle = true;
                //DrillEditing = _MecOptional.Middle;
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.Middle));
                UseSelected = DrillTools(_DrillWarehouse.Middle, true);
                UpDate();
            });
        }

        /// <summary>
        /// 選擇中<see cref="GD_STD.DrillWarehouse.LeftExport"/>命令
        /// </summary>
        public ICommand LeftExportCommand { get; set; }
        private WPFBase.RelayCommand LeftExport()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearSelectd();
                this.IsSelectdLeftExport = true;
                //DrillEditing = _MecOptional.LeftExport;
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftExport));
                UseSelected = DrillTools(_DrillWarehouse.LeftExport, true);
                UpDate();
            });
        }

        /// <summary>
        /// 選擇中<see cref="GD_STD.DrillWarehouse.RightExport"/>命令
        /// </summary>
        public ICommand RightExportCommand { get; set; }
        private WPFBase.RelayCommand RightExport()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearSelectd();
                this.IsSelectdRightExport = true;
                //DrillEditing = _MecOptional.RightExport;
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightExport));
                UseSelected = DrillTools(_DrillWarehouse.RightExport, true);
                UpDate();
            });
        }

        /// <summary>
        /// 更新刀庫命令
        /// </summary>
        public WPFBase.RelayCommand UpDateCommand { get; set; }
        /// <summary>
        /// 更新刀庫
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayCommand UpDateDrill()
        {
            return new WPFBase.RelayCommand(() =>
            {
                UpDate();
            });
        }
        /// <summary>
        /// 選擇中<see cref="GD_STD.DrillWarehouse.LeftEntrance"/>命令
        /// </summary>
        public ICommand LeftEntranceCommand { get; set; }
        private WPFBase.RelayCommand LeftEntrance()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearSelectd();
                this.IsSelectdLeftEntrance = true;
                //DrillEditing = _MecOptional.LeftEntrance;
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftEntrance));
                UseSelected = DrillTools(_DrillWarehouse.LeftEntrance, true);
                UpDate();
            });
        }

        /// <summary>
        /// 選擇中<see cref="GD_STD.DrillWarehouse.RightEntrance"/>命令
        /// </summary>
        public ICommand RightEntranceCommand { get; set; }
        private WPFBase.RelayCommand RightEntrance()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearSelectd();
                this.IsSelectdRightEntrance = true;
                //DrillEditing = _MecOptional.RightEntrance;
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightEntrance));
                UseSelected = DrillTools(_DrillWarehouse.RightEntrance, true);
                UpDate();
            });
        }
        /// <summary>
        /// 選中 <see cref="IsCurrentMiddle"/> 命令
        /// </summary>
        public ICommand CurrentMiddleCommand { get; set; }
        private WPFBase.RelayCommand CurrentMiddle()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearCurrentSelectd();
                this.IsCurrentMiddle = true;
                UseSelected = DrillTools(_DrillWarehouse.Middle, true);
                DRILL_POSITION = DRILL_POSITION.MIDDLE;
                CancelEnabled();
            });
        }

        /// <summary>
        /// 選中 <see cref="IsCurrentLeft"/> 命令
        /// </summary>
        public ICommand CurrentLeftCommand { get; set; }
        private WPFBase.RelayCommand CurrentLeft()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearCurrentSelectd();
                this.IsCurrentLeft = true;
                UseSelected = DrillTools(_DrillWarehouse.LeftExport, true);
                if (UseSelected.Count == 0) //如果再左軸出口處找不到目前使用的刀庫
                {
                    UseSelected = DrillTools(_DrillWarehouse.LeftEntrance, true);
                    if (UseSelected.Count != 0)
                    {
                        DRILL_POSITION = DRILL_POSITION.ENTRANCE_L;
                    }
                }
                else
                {
                    DRILL_POSITION = DRILL_POSITION.EXPORT_L;
                }
                CancelEnabled();
            });
        }

        /// <summary>
        /// 選中 <see cref="IsCurrentRight"/> 命令
        /// </summary>
        public ICommand CurrentRightCommand { get; set; }
        private WPFBase.RelayCommand CurrentRight()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearCurrentSelectd();
                this.IsCurrentRight = true;
                UseSelected = DrillTools(_DrillWarehouse.RightExport, true);
                if (UseSelected.Count == 0) //如果再左軸出口處找不到目前使用的刀庫
                {
                    UseSelected = DrillTools(_DrillWarehouse.RightEntrance, true);
                    if (UseSelected.Count != 0)
                    {
                        DRILL_POSITION = DRILL_POSITION.ENTRANCE_R;
                    }
                }
                else
                {
                    DRILL_POSITION = DRILL_POSITION.EXPORT_R;
                }
                CancelEnabled();
            });
        }


        public WPFBase.RelayParameterizedCommand ToolMagazineActiveCommand
        { 
            get
            {
                return new WPFBase.RelayParameterizedCommand(el =>
                {
                    _DrillWarehouse = ReadCodesysMemor.GetDrillWarehouse();
                    //A  MiddleCommand = Middle(); this.IsSelectdMiddle = true;
                    //B  LeftExportCommand = LeftExport(); this.IsSelectdLeftExport = true;
                    //C  RightExportCommand = RightExport(); this.IsSelectdRightExport = true;
                    //D  LeftEntranceCommand = LeftEntrance(); this.IsSelectdLeftEntrance = true;
                    //E  RightEntranceCommand = RightEntrance(); this.IsSelectdRightEntrance = true;
                    if (el is bool)
                    {
                        var Activity = (bool)el;
                        if (Optional.Default.Middle && this.IsSelectdMiddle)
                        {
                            _MecOptional.Middle = Activity;
                            if (!Activity)
                            {
                                for (int i = 0; i < _DrillWarehouse.Middle.Length; i++)
                                {
                                    _DrillWarehouse.Middle[i] = new DrillSetting();
                                    _DrillWarehouse.Middle[i].IsCurrent = (i == 0);
                                }
                            }
                        }

                        if (Optional.Default.LeftExport && this.IsSelectdLeftExport)
                        {
                            _MecOptional.LeftExport = Activity;
                            if (!Activity)
                            {
                                for (int i = 0; i < _DrillWarehouse.LeftExport.Length; i++)
                                {
                                    _DrillWarehouse.LeftExport[i] = new DrillSetting();
                                    _DrillWarehouse.LeftExport[i].IsCurrent = (i == 0);
                                }
                            }
                        }

                        if (Optional.Default.LeftEntrance && this.IsSelectdLeftEntrance)
                        {
                            _MecOptional.LeftEntrance = Activity;
                            if (!Activity)
                            {
                                for (int i = 0; i < _DrillWarehouse.LeftEntrance.Length; i++)
                                {
                                    _DrillWarehouse.LeftEntrance[i] = new DrillSetting();
                                    _DrillWarehouse.LeftEntrance[i].IsCurrent = (i == 0);
                                }
                            }
                        }

                        if (Optional.Default.RightEntrance && this.IsSelectdRightEntrance)
                        {
                            _MecOptional.RightEntrance = Activity;
                            if (!Activity)
                            {
                                for (int i = 0; i < _DrillWarehouse.RightEntrance.Length; i++)
                                {
                                    _DrillWarehouse.RightEntrance[i] = new DrillSetting();
                                    _DrillWarehouse.RightEntrance[i].IsCurrent = (i == 0);
                                }
                            }
                        }

                        if (Optional.Default.RightExport && this.IsSelectdRightExport)
                        {
                            _MecOptional.RightExport = Activity;
                            if (!Activity)
                            {
                                for (int i = 0; i < _DrillWarehouse.RightExport.Length; i++)
                                {
                                    _DrillWarehouse.RightExport[i] = new DrillSetting();
                                    _DrillWarehouse.RightExport[i].IsCurrent = (i == 0);
                                }
                            }
                        }

                        //寫入設定檔
                        new STDSerialization().SetOptionSettings(_MecOptional);

                        //寫入codesys
                        MecOptional mecOptional = JsonConvert.DeserializeObject<MecOptional>(_MecOptional.ToString());
                        WriteCodesysMemor.SetMecOptional(mecOptional);//寫入記憶體''

                        WriteCodesysMemor.SetDrillWarehouse(_DrillWarehouse);//寫入記憶體
                        UpDate();
                    }


                });
            }
        }







        #endregion
        #region 私有方法
        private void UpDate()
        {
            _DrillWarehouse = ReadCodesysMemor.GetDrillWarehouse(); //讀取記憶體目前的刀庫參數
            //查看刀庫
           /* if (!_MecOptional.LeftExport) //沒有左軸出口刀庫
            {
                //DrillSetting drill = _DrillWarehouse.LeftExport[0];
                //drill.IsCurrent = true;
                // _DrillWarehouse.LeftExport = new DrillSetting[1] { drill };

            }
            if (!_MecOptional.LeftEntrance) //如果沒有入口刀庫
            {
                //this.LeftEntranceIsEnabled = false;
            }
            if (!_MecOptional.RightExport)
            {
                //DrillSetting drill = _DrillWarehouse.RightExport[0];
                //drill.IsCurrent = true;
                //_DrillWarehouse.RightExport = new DrillSetting[1] { drill };
                for (int i = 1; i < _DrillWarehouse.RightExport.Length; i++)
                {
                    _DrillWarehouse.RightExport[i] = new DrillSetting();
                    _DrillWarehouse.RightExport[i].IsCurrent = false;
                }
                _DrillWarehouse.RightExport[0].IsCurrent = true;
            }
            if (!_MecOptional.RightEntrance)
            {
                //this.RightEntranceIsEnabled = false;
            }
            if (!_MecOptional.Middle)
            {
                //DrillEditing = false;
                //DrillSetting drill = _DrillWarehouse.Middle[0];
                //drill.IsCurrent = true;
                //_DrillWarehouse.Middle = new DrillSetting[1] { drill };
                for (int i = 1; i < _DrillWarehouse.Middle.Length; i++)
                {
                    _DrillWarehouse.Middle[i] = new DrillSetting();
                    _DrillWarehouse.Middle[i].IsCurrent = false;
                }
                _DrillWarehouse.Middle[0].IsCurrent = true;
            }*/
            /*選擇刀庫的位置,並顯示在介面上(裝載在主軸上的刀具)*/
            //選擇中軸刀庫
            if (IsSelectdMiddle)
            {
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.Middle));
                ToolMActivityButtonVisibilityBoolean = !_MecOptional.Middle;

            }
            //選擇左軸入料口刀庫
            else if (IsSelectdLeftEntrance)
            {
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftEntrance));
                ToolMActivityButtonVisibilityBoolean = !_MecOptional.LeftEntrance;
            }
            //選擇左軸出料口刀庫
            else if (IsSelectdLeftExport)
            {
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftExport));
                ToolMActivityButtonVisibilityBoolean = !_MecOptional.LeftExport;
            }
            //選擇右軸入料口刀庫
            else if (IsSelectdRightEntrance)
            {
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightEntrance));
                ToolMActivityButtonVisibilityBoolean = !_MecOptional.RightEntrance;
            }
            //選擇右軸出料口刀庫
            else if (IsSelectdRightExport)
            {
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightExport));
                ToolMActivityButtonVisibilityBoolean = !_MecOptional.RightExport;
            }
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
        /// 分類刀庫
        /// </summary>
        /// <param name="drillSettings">刀庫列表</param>
        /// <param name="isUse">是使用的刀庫</param>
        /// <returns></returns>
        private ObservableCollection<_drillSetting> DrillTools(DrillSetting[] drillSettings, bool isUse)
        {
            return new ObservableCollection<_drillSetting>(from el in drillSettings where el.IsCurrent == isUse select new _drillSetting(el, DrillBrands));
        }
        /// <summary>
        /// 轉換物件為類型
        /// </summary>
        /// <returns></returns>
        private List<_drillSetting> GetDrillSetting(DrillSetting[] drillSettings)
        {
            List<_drillSetting> result = new List<_drillSetting>();
            for (int i = 0; i < drillSettings.Length; i++)
            {
                result.Add(new _drillSetting(drillSettings[i], DrillBrands));
                result[i].Index = i+ 1;
            }
            return result;
        }
        /// <summary>
        /// 變更刀庫設定的值
        /// </summary>
        /// <param name="drillSetting">要變更的鑽頭的資訊</param>
        private void DrillChange(_drillSetting drillSetting)
        {
            int index = drillSetting.Index - 1;//刀庫陣列位置

            DrillSetting result = drillSetting.GetStruc();//轉換結構
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
        /// 清除當前刀庫選擇
        /// </summary>
        private void ClearCurrentSelectd()
        {
            IsCurrentMiddle = false;
            IsCurrentLeft = false;
            IsCurrentRight = false;
        }
        /// <summary>
        /// 清除選中的刀庫
        /// </summary>
        private void ClearSelectd()
        {
            this.IsSelectdLeftEntrance = false;
            this.IsSelectdLeftExport = false;
            this.IsSelectdMiddle = false;
            this.IsSelectdRightEntrance = false;
            this.IsSelectdRightExport = false;
        }
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
        #endregion
    }
    /// <summary>
    /// 刀庫設定
    /// </summary>
    [Serializable]
    public class _drillSetting : DrillBrand
    {
        DrillBrands _DrillBrands { get; set; }
        DrillSetting _DrillSetting { get; set; }
        ///// <summary>
        ///// 設定檔索引
        ///// </summary>
        //private int settingIndex = -1;
        /// <summary>
        /// 設定檔案名稱
        /// </summary>
        private string settingName;
        private short limit;

        /// <summary>
        /// 設定檔名稱
        /// </summary>

        public string SettingName
        {
            get
            {
                return settingName;
            }
            set
            {
                settingName = value;

                int settingIndex = _DrillBrands.FindIndex(el => el.DataName == value);
                Dia = _DrillBrands[settingIndex].Dia;
                Rpm =_DrillBrands[settingIndex].Rpm;
                Name = _DrillBrands[settingIndex].Name;
                f = _DrillBrands[settingIndex].f;
                Length =  _DrillBrands[settingIndex].SumLength;
                Limit = _DrillSetting.Limit;
                DrillSetting drill = new DrillSetting()
                {
                    Dia =  _DrillBrands[settingIndex].Dia,
                    DrillType = _DrillBrands[settingIndex].DrillType,
                    GUID = System.Text.Encoding.ASCII.GetBytes(_DrillBrands[settingIndex].Guid.ToString()),
                    F = _DrillBrands[settingIndex].F,
                    KnifeHandle = _DrillBrands[settingIndex].HolderLength,
                    Index = _DrillSetting.Index,
                    Length = _DrillBrands[settingIndex].DrillLength,
                    Limit = _DrillSetting.Limit,
                    Position = _DrillSetting.Position,
                    Rpm = _DrillBrands[settingIndex].Rpm,
                    IsCurrent = _DrillSetting.IsCurrent
                };
                _DrillSetting = drill;
            }
        }
        public float Length { get; set; }
        /// <summary>
        /// 刀庫位置
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 鑽頭類型索引
        /// </summary>
        public int DrillTypeIndex { get; set; }
        /// <summary>
        /// 鑽孔極限
        /// </summary>
        public short Limit
        {
            get
            {
                return limit;
            }
            set
            {
                DrillSetting _ = _DrillSetting;
                _.Limit = value;
                _DrillSetting = _;
                limit =value;

            }
        }
        public DrillSetting GetStruc()
        {
            return _DrillSetting;
        }
        /// <summary>
        /// 刀具 VM
        /// </summary>
        /// <param name="drillSetting"></param>
        /// <param name="drills"></param>
        public _drillSetting(DrillSetting drillSetting, DrillBrands drills)
        {
            char[] cGuid = drillSetting.GUID
                                                            .Where(el => el != 0)
                                                            .Select(el => Convert.ToChar(el))
                                                            .ToArray();
            string sGuid = new string(cGuid);
            Index =drillSetting.Index;
            _DrillSetting = drillSetting;

            int settingIndex = drills.FindIndex(el => el.Guid.ToString() == sGuid);
            _DrillBrands = drills;
            if (settingIndex != -1)
            {
                SettingName = drills[settingIndex].DataName;
            }
            else
            {
                SettingName = drills[0].DataName;
            }
            //drills.FindIndex(el => el.Guid.ToString() == sGuid);
            //Index = drillSetting.Index;
            //SettingName = SofSetting.Default.DrillBrands[drillSetting.SettingIndex].DataName;
            //DrillTypeIndex = (int)drillSetting.DrillType;
            //Limit = drillSetting.Limit;
        }
    }
}

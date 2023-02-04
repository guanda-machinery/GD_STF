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
using DevExpress.Xpf.WindowsUI;

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
           // UpDateCommand = UpDateDrill();
            DrillBrands =  new STDSerialization().GetDrillBrands();
            if (DrillBrands.Count == 0)
            {
                DrillBrands.Add(DrillBrand.GetNull());
            }
            //MiddleIsEnabled = _MecOptional.Middle;
            //LeftEntranceIsEnabled = _MecOptional.LeftEntrance;
            //LeftExportIsEnabled = _MecOptional.LeftExport;
            //RightEntranceIsEnabled = _MecOptional.RightEntrance;
            //RightExportIsEnabled = _MecOptional.RightExport;
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
        public bool IsSelectedLeftExport { get; set; }
        /// <summary>
        /// 選擇<see cref="DrillWarehouse.RightExport"/>
        /// </summary>
        public bool IsSelectedRightExport { get; set; }
        /// <summary>
        /// 選擇<see cref="DrillWarehouse.LeftEntrance"/>
        /// </summary>
        public bool IsSelectedLeftEntrance { get; set; }
        /// <summary>
        /// 選擇<see cref="DrillWarehouse.RightEntrance"/>
        /// </summary>
        public bool IsSelectedRightEntrance { get; set; }
        /// <summary>
        /// 選擇中軸配裝的刀具
        /// </summary>
        public bool IsCurrentMiddle { get; set; } = true;
        /// <summary>
        /// 編輯為裝載在主軸上的刀具設定
        /// </summary>
        //public bool DrillEditing { get; set; } = true;
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
       // public bool MiddleIsEnabled { get; set; }
        /// <summary>
        /// 是否顯示左軸出口刀庫的按鈕 
        /// </summary>
       // public bool LeftExportIsEnabled { get; set; }
        /// <summary>
        /// 是否顯示右軸出口刀庫的按鈕 
        /// </summary>
       // public bool RightExportIsEnabled { get; set; }
        /// <summary>
        /// 是否顯示 <see cref="DrillWarehouse.RightEntrance"/> 的按鈕 
        /// </summary>
        //public bool RightEntranceIsEnabled { get; set; }
        /// <summary>
        /// 是否顯示 <see cref="DrillWarehouse.LeftEntrance"/> 的按鈕 
        /// </summary>
        //public bool LeftEntranceIsEnabled { get; set; }



        public bool ToolMActivityButtonVisibilityBoolean
        {
            get
            {
                if (IsSelectedMiddle)
                    return !_MecOptional.Middle;
                else if (IsSelectedLeftEntrance)
                    return !_MecOptional.LeftEntrance;
                else if (IsSelectedLeftExport)
                    return !_MecOptional.LeftExport;
                else if (IsSelectedRightEntrance)
                    return !_MecOptional.RightEntrance;
                //選擇右軸出料口刀庫
                else if (IsSelectedRightExport)
                    return !_MecOptional.RightExport;
                else
                    return false;
            }
        }







        /// <summary>
        /// 選擇<see cref="DrillWarehouse.Middle"/>
        /// </summary>
        public bool IsSelectedMiddle { get; set; } = true;
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
                if (UnusedSelected.Count > 0)
                {
                    //跳出提示表示不可有兩隻主軸刀
                    if ((UnusedSelected.ToList().FindAll(x => x.IsCurrent)).Count>=2)
                    {
                        //有兩個以上的主軸刀
                        WinUIMessageBox.Show(null,
                            $"設定錯誤，不可在同一主軸上同時裝載兩把以上的主軸刀",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             DevExpress.Xpf.Core.FloatingMode.Window);
                        return;
                    }

                    //如果本次設定有選擇主軸刀才需要檢查
                    if (UnusedSelected.FindIndex(x => (x.IsCurrent)) != -1)
                    {
                        var ToolOverlappingError = false;
                        var SpidleName= "未知軸";

                        //檢查左軸的刀
                        if (IsSelectedLeftEntrance && _DrillWarehouse.LeftExport.FindIndex(x => (x.IsCurrent)) != -1)
                        {
                            SpidleName = "左軸";
                            ToolOverlappingError = true;
                        }
                        if (IsSelectedLeftExport && _DrillWarehouse.LeftEntrance.FindIndex(x => (x.IsCurrent)) != -1)
                        {
                            SpidleName = "左軸";
                            ToolOverlappingError = true;
                        }
                        //檢查右軸的刀
                        if (IsSelectedRightEntrance && _DrillWarehouse.RightExport.FindIndex(x => (x.IsCurrent)) != -1)
                        {
                            SpidleName = "右軸";
                            ToolOverlappingError = true;
                        }
                        if (IsSelectedRightExport && _DrillWarehouse.RightEntrance.FindIndex(x => (x.IsCurrent)) != -1)
                        {
                            SpidleName = "右軸";
                            ToolOverlappingError = true;
                        }


                        if (ToolOverlappingError)
                        {
                            WinUIMessageBox.Show(null,
                                $"設定錯誤，{SpidleName}上的另一刀庫已經存在主軸刀",
                                "通知",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning,
                                MessageBoxResult.None,
                                MessageBoxOptions.None,
                                DevExpress.Xpf.Core.FloatingMode.Window);
                            return;
                        }
                    }

                    //沒有設定檔就不可設定為主軸刀
                    var NullDataIsCurrentIndex = UnusedSelected.FindIndex(x =>
                    ( x.SettingName == DrillBrand.GetNull().DataName || string.IsNullOrEmpty(x.SettingName)) 
                    && x.IsCurrent);
                    if (NullDataIsCurrentIndex != -1)
                    {
                        //沒有設定檔就不可設定為主軸刀
                        WinUIMessageBox.Show(null,
                            $"刀庫位置：{UnusedSelected[NullDataIsCurrentIndex].Index}沒有設定檔資料，不可設定為主軸刀",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             DevExpress.Xpf.Core.FloatingMode.Window);
                        return;
                    }



                    for (int i = 0; i < UnusedSelected.Count; i++)
                    {
                        DrillChange(UnusedSelected[i]);
                    }
                    WriteCodesysMemor.SetDrillWarehouse(_DrillWarehouse);//寫入記憶體

                    WinUIMessageBox.Show(null,
                        $"設定成功",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        DevExpress.Xpf.Core.FloatingMode.Window);

                    UpDate();
                }
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
                this.IsSelectedMiddle = true;
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
                this.IsSelectedLeftExport = true;
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
                this.IsSelectedRightExport = true;
                //DrillEditing = _MecOptional.RightExport;
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightExport));
                UseSelected = DrillTools(_DrillWarehouse.RightExport, true);
                UpDate();
            });
        }

        /// <summary>
        /// 更新刀庫命令
        /// </summary>
       // public WPFBase.RelayCommand UpDateCommand { get; set; }
        /// <summary>
        /// 更新刀庫
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayCommand UpDateCommand
        {
            get
            {
                return new WPFBase.RelayCommand(() =>
                {
                    UpDate();
                });
            }
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
                this.IsSelectedLeftEntrance = true;
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
                this.IsSelectedRightEntrance = true;
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
              //  CancelEnabled();
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
                //CancelEnabled();
                UpDate();
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
                //CancelEnabled();
            });
        }


        public WPFBase.RelayParameterizedCommand ToolMagazineActiveCommand
        { 
            get
            {
                return new WPFBase.RelayParameterizedCommand(el =>
                {
                    _DrillWarehouse = ReadCodesysMemor.GetDrillWarehouse();
                    //A  MiddleCommand = Middle(); this.IsSelectedMiddle = true;
                    //B  LeftExportCommand = LeftExport(); this.IsSelectedLeftExport = true;
                    //C  RightExportCommand = RightExport(); this.IsSelectedRightExport = true;
                    //D  LeftEntranceCommand = LeftEntrance(); this.IsSelectedLeftEntrance = true;
                    //E  RightEntranceCommand = RightEntrance(); this.IsSelectedRightEntrance = true;
                    if (el is bool)
                    {
                        var Activity = (bool)el;

                        if(!Activity)
                        {
                            //跳出提示提醒會刪除刀具資料
                            var Result =  WinUIMessageBox.Show(null,
                                $"關閉刀庫後該刀庫上的所有刀具資料都會被清除，請確認是否要關閉",
                                "通知",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning,
                                MessageBoxResult.None,
                                MessageBoxOptions.None,
                                DevExpress.Xpf.Core.FloatingMode.Window);

                            if (Result == MessageBoxResult.No || Result == MessageBoxResult.No)
                            {
                                return;
                            }
                        }





                        if (Optional.Default.Middle && this.IsSelectedMiddle)
                        {
                            _MecOptional.Middle = Activity;
                            if (!Activity)
                            {
                                for (int i = 0; i < _DrillWarehouse.Middle.Length; i++)
                                {
                                    _DrillWarehouse.Middle[i] = new DrillSetting() ; 
                                    _DrillWarehouse.Middle[i].IsCurrent = false;
                                }
                               // _DrillWarehouse.Middle[0].IsCurrent = true;
                            }
                        }

                        if (Optional.Default.LeftExport && this.IsSelectedLeftExport)
                        {
                            _MecOptional.LeftExport = Activity;
                            if (!Activity)
                            {
                                for (int i = 0; i < _DrillWarehouse.LeftExport.Length; i++)
                                {
                                    _DrillWarehouse.LeftExport[i] = new DrillSetting();
                                    _DrillWarehouse.LeftExport[i].IsCurrent = false;
                                }

                                //如果現在不是選擇左側入口刀具，
                               /*if(!_DrillWarehouse.LeftEntrance.ToList().Exists(x=>x.IsCurrent))
                                {
                                    _DrillWarehouse.LeftExport[0].IsCurrent = true;
                                }*/

                            }
                        }

                        if (Optional.Default.LeftEntrance && this.IsSelectedLeftEntrance)
                        {
                            _MecOptional.LeftEntrance = Activity;
                            if (!Activity)
                            {
                                for (int i = 0; i < _DrillWarehouse.LeftEntrance.Length; i++)
                                {
                                    _DrillWarehouse.LeftEntrance[i] = new DrillSetting();
                                    _DrillWarehouse.LeftEntrance[i].IsCurrent = false;
                                }

                                /*if (!_DrillWarehouse.LeftExport.ToList().Exists(x => x.IsCurrent))
                                {
                                    _DrillWarehouse.LeftEntrance[0].IsCurrent = true;
                                }*/

                            }
                        }

                        if (Optional.Default.RightEntrance && this.IsSelectedRightEntrance)
                        {
                            _MecOptional.RightEntrance = Activity;
                            if (!Activity)
                            {
                                for (int i = 0; i < _DrillWarehouse.RightEntrance.Length; i++)
                                {
                                    _DrillWarehouse.RightEntrance[i] = new DrillSetting();
                                    _DrillWarehouse.RightEntrance[i].IsCurrent = false;
                                }
                                /*
                                if (!_DrillWarehouse.RightExport.ToList().Exists(x => x.IsCurrent))
                                {
                                    _DrillWarehouse.RightEntrance[0].IsCurrent = true;
                                }*/
                            }
                        }

                        if (Optional.Default.RightExport && this.IsSelectedRightExport)
                        {
                            _MecOptional.RightExport = Activity;
                            if (!Activity)
                            {
                                for (int i = 0; i < _DrillWarehouse.RightExport.Length; i++)
                                {
                                    _DrillWarehouse.RightExport[i] = new DrillSetting();
                                   _DrillWarehouse.RightExport[i].IsCurrent = false;
                                }
                                /*
                                if (!_DrillWarehouse.RightEntrance.ToList().Exists(x => x.IsCurrent))
                                {
                                    _DrillWarehouse.RightExport[0].IsCurrent = true;
                                }*/
                            }
                        }

                        //寫入設定檔
                        new STDSerialization().SetOptionSettings(_MecOptional);

                        //寫入codesys
                        MecOptional mecOptional = JsonConvert.DeserializeObject<MecOptional>(_MecOptional.ToString());
                        WriteCodesysMemor.SetMecOptional(mecOptional);//寫入記憶體''
                        WriteCodesysMemor.SetDrillWarehouse(_DrillWarehouse);//寫入記憶體
                    }


                   var Temp_Mid =  this.IsSelectedMiddle ;
                    var Temp_LEx = this.IsSelectedLeftExport ;
                    var Temp_REx= this.IsSelectedRightExport ;
                    var Temp_LEn = this.IsSelectedLeftEntrance ;
                    var Temp_REn = this.IsSelectedRightEntrance ;
                    ClearSelectd();
                    this.IsSelectedMiddle = Temp_Mid;
                    this.IsSelectedLeftExport = Temp_LEx;
                    this.IsSelectedRightExport = Temp_REx;
                    this.IsSelectedLeftEntrance = Temp_LEn;
                    this.IsSelectedRightEntrance = Temp_REn;

                    //DrillEditing = _MecOptional.Middle;
                    UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.Middle));
                    UseSelected = DrillTools(_DrillWarehouse.Middle, true);
                    UpDate();


                });
            }
        }


        /// <summary>
        /// 刀庫讀取
        /// </summary>
        /// <returns></returns>
        public ICommand ReadDrillWarehouse
        {
            get
            {
                return new WPFBase.RelayCommand(() =>
                {
                    var Temp_Mid = this.IsSelectedMiddle;
                    var Temp_LEx = this.IsSelectedLeftExport;
                    var Temp_REx = this.IsSelectedRightExport;
                    var Temp_LEn = this.IsSelectedLeftEntrance;
                    var Temp_REn = this.IsSelectedRightEntrance;
                    ClearSelectd();
                    this.IsSelectedMiddle = Temp_Mid;
                    this.IsSelectedLeftExport = Temp_LEx;
                    this.IsSelectedRightExport = Temp_REx;
                    this.IsSelectedLeftEntrance = Temp_LEn;
                    this.IsSelectedRightEntrance = Temp_REn;

                    /*UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftEntrance));
                    UseSelected = DrillTools(_DrillWarehouse.LeftEntrance, true);
                    UpDate();*/
                });
            }
        }

        /// <summary>
        /// 刀庫存檔
        /// </summary>
        /// <returns></returns>
        public ICommand SaveDrillWarehouse
        {
            get
            {
                return new WPFBase.RelayCommand(() =>
                {
                    var Temp_Mid = this.IsSelectedMiddle;
                    var Temp_LEx = this.IsSelectedLeftExport;
                    var Temp_REx = this.IsSelectedRightExport;
                    var Temp_LEn = this.IsSelectedLeftEntrance;
                    var Temp_REn = this.IsSelectedRightEntrance;
                    ClearSelectd();
                    this.IsSelectedMiddle = Temp_Mid;
                    this.IsSelectedLeftExport = Temp_LEx;
                    this.IsSelectedRightExport = Temp_REx;
                    this.IsSelectedLeftEntrance = Temp_LEn;
                    this.IsSelectedRightEntrance = Temp_REn;

                    /*UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftEntrance));
                    UseSelected = DrillTools(_DrillWarehouse.LeftEntrance, true);
                    UpDate();*/
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
            if (IsSelectedMiddle)
            {
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.Middle));
                //ToolMActivityButtonVisibilityBoolean = !_MecOptional.Middle;
            }
            //選擇左軸入料口刀庫
            else if (IsSelectedLeftEntrance)
            {
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftEntrance));
                //ToolMActivityButtonVisibilityBoolean = !_MecOptional.LeftEntrance;
            }
            //選擇左軸出料口刀庫
            else if (IsSelectedLeftExport)
            {
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftExport));
                //ToolMActivityButtonVisibilityBoolean = !_MecOptional.LeftExport;
            }
            //選擇右軸入料口刀庫
            else if (IsSelectedRightEntrance)
            {
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightEntrance));
                //ToolMActivityButtonVisibilityBoolean = !_MecOptional.RightEntrance;
            }
            //選擇右軸出料口刀庫
            else if (IsSelectedRightExport)
            {
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightExport));
                //ToolMActivityButtonVisibilityBoolean = !_MecOptional.RightExport;
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

            //CancelEnabled();
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
            if (IsSelectedMiddle)
                _DrillWarehouse.Middle[index] = result;
            else if (IsSelectedLeftExport)
                _DrillWarehouse.LeftExport[index] = result;
            else if (IsSelectedRightExport)
                _DrillWarehouse.RightExport[index] = result;
            else if (IsSelectedLeftEntrance)
                _DrillWarehouse.LeftEntrance[index] = result;
            else if (IsSelectedRightEntrance)
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
            this.IsSelectedLeftEntrance = false;
            this.IsSelectedLeftExport = false;
            this.IsSelectedMiddle = false;
            this.IsSelectedRightEntrance = false;
            this.IsSelectedRightExport = false;
        }



        /// <summary>
        /// 取消目前刀具唯讀狀態
        /// </summary>
        /*private void CancelEnabled()
        {
            if (UseSelected.Count > 0)
            {
                List<_drillSetting> drills = new List<_drillSetting>();
                _drillSetting _ = UseSelected[0];
                //_.IsCurrent = false;
                drills.Add(_);
                UseSelected = new ObservableCollection<_drillSetting>(drills);
            }
        }*/
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
                this.Dia = _DrillBrands[settingIndex].Dia;
                this.Rpm = _DrillBrands[settingIndex].Rpm;
                this.Name = _DrillBrands[settingIndex].Name;
                this.f = _DrillBrands[settingIndex].f;
                this.Length = _DrillBrands[settingIndex].SumLength;
                this.Limit = _DrillSetting.Limit;
                this.IsCurrent =_DrillSetting.IsCurrent;
                this._DrillSetting = new DrillSetting()
                {
                    Dia = _DrillBrands[settingIndex].Dia,
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
        /// 已加工深度
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

                limit = value;
            }
        }

        private bool _IsCurrent;
        /// <summary>
        /// 主軸刀
        /// </summary>
        public bool IsCurrent
        {
            get
            {
                return _IsCurrent;
            }
            set
            {
                DrillSetting _ = _DrillSetting;
                _.IsCurrent = value;
                _DrillSetting = _;
                _IsCurrent = value;
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
            Index = drillSetting.Index;
            _DrillSetting = drillSetting;
            _DrillBrands = drills;
            if (drillSetting.GUID != null)
            {
                char[] cGuid = drillSetting.GUID
                                                                .Where(el => el != 0)
                                                                .Select(el => Convert.ToChar(el))
                                                                .ToArray();
                string sGuid = new string(cGuid);
                int settingIndex = drills.FindIndex(el => el.Guid.ToString() == sGuid);
                if (settingIndex != -1)
                {
                    SettingName = drills[settingIndex].DataName;
                }
                else
                {
                    SettingName = GetNull().DataName;
                }
            }
            else
            {
                SettingName = GetNull().DataName;
            }



        }
    }
}

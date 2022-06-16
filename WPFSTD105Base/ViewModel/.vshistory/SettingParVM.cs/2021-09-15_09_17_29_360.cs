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

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 參數設定模型
    /// </summary>
    public class SettingParVM : WPFBase.BaseViewModel
    {

        /// <inheritdoc/>
        public SettingParVM()
        {
            /*綁定命令*/
            MiddleCommand = Middle();
            LeftExportCommand = LeftExport();
            RightExportCommand = RightExport();
            LeftEntranceCommand = LeftEntrance();
            RightEntranceCommand = RightEntrance();
            UseSaveCommand = UseSave();
            UnusedSaveCommand = UnusedSave();
            UpdateCommand = Update();
            CurrentLeftCommand = CurrentLeft();
            CurrentRightCommand = CurrentRight();
            CurrentMiddleCommand = CurrentMiddle();
            CloseSavePosition = ClickCancelButton();
            /*工程模式才可開啟此功能*/
            if (ApplicationViewModel.EngineeringMode)
            {
                SaveHydCommand = SaveOil();
                ReductionHydCommand = ReductionOil();
                SaveLubCommand = SaveLub();
                ReductionLubCommand = ReductionLub();
                SaveCutCommand = SaveCut();
                ReductionCutCommand = ReductionCut();
            }
            CloseSaveAsWindowsCommand = CloseSaveAs();
            InputCommand = Input();
            OutCommand = Output();
            AxisCommand = Axis();
            WarehouseCommand = Warehouse();
            LubCommand = Lub();
            SensorCommand = Sensor();
            OAxisCommand = OAxis();
            OSensorCommand = OSensor();
            _LeftExport = ToAxis4D(Default.LeftExportDrillSetting);
            _LeftEntrance = ToAxis4D(Default.LeftEntranceDrillSetting);
            _Middle = ToAxis4D(Default.MiddleDrillSetting);
            _RightEntrance = ToAxis4D(Default.RightEntranceDrillSetting);
            _RightExport = ToAxis4D(Default.RightExportDrillSetting);
            UpDateDrill();
            //SaveCommand = Save();
            DeleteCommand = Delete();
            UpdateTreeCommand = UpdateTree();
            FilterCommand = ClickFilterButton();
            FilterMaterialCommand = FilterMaterial();
            SegmentString = ClickTreeViewItem();
            if (ApplicationViewModel.ProjectName == null)//如果沒有專案名稱
            {
                ApplicationViewModel.ActionLoadProfile = new Action(LoadProfile);//等待新件專案或開啟專案使用委派方法幫用戶載入檔案
            }
            else
            {
                LoadProfile();//直接載入
            }
            NewCommand = New();
            DeleteMaterialCommand = DeleteMaterial();
            UpdateMaterialCommand = UpdateMaterial();
            //SaveModelRoSystem = SaveProfile();
            //SaveMaterialCommand = SaveMaterial();
            STDSerialization ser = new STDSerialization();
            DrillBrands = ser.GetDrillBrands();

            SaveDrillBrandsCommand = SaveDrillBrands();
            NewDrillBrandsCommand = NewDrillBrands();
            initializationInsertionData();
        }

        #region 公開方法
        /// <summary>
        /// 釋放 <see cref="IOThread"/>
        /// </summary>
        public void IOAbort()
        {
            if (IOThread != null)
            {
                IOThread.Abort();
            }
        }
        /// <summary>
        /// 重新讀取 Codesys 刀庫資訊
        /// </summary>
        public void UpDateDrill()
        {
            //CodeSys斷電保持 直接撈取即可
            _DrillWarehouse = ReadCodesysMemor.GetDrillWarehouse(); //讀取記憶體目前的刀庫參數

            Default.DrillWarehouse = _DrillWarehouse;//將在記憶體內的刀庫設定賦予軟體設定黨

            _OillSystem = ReadCodesysMemor.GetOill();

            //Location(_DrillWarehouse);//產生刀庫位置
            Location(_OillSystem.HydraulicSystem); ;//產生液壓系統項目
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
            //液壓油系統參數
            HydList = new ObservableCollection<_hydraulicSystem>(GetHydraulicSystems(_OillSystem.HydraulicSystem));
            this.SideReady = HydList[0].SideReady;
            this.Deviation = HydList[0].Deviation;
            this.DownReady = this.DownReady;
            this._LubFrequency = _OillSystem.LubricantSystem.Frequency;
            this._LubTime = _OillSystem.LubricantSystem.Time;
            this._CutFrequency = _OillSystem.CutOilSystem.Frequency;



            ////初始化刀庫 因為有可能第一次交機沒有刀具在上面 必須初始化一個目前刀具的空間出來
            //if (Isinitialization(_DrillWarehouse.Middle))
            //    _DrillWarehouse.Middle[0] = initialization(_DrillWarehouse.Middle);
            //if (Isinitialization(_DrillWarehouse.LeftExport))
            //    _DrillWarehouse.LeftExport[0] = initialization(_DrillWarehouse.LeftExport);
            //if (Isinitialization(_DrillWarehouse.RightExport))
            //    _DrillWarehouse.RightExport[0] = initialization(_DrillWarehouse.RightExport);
            //WriteCodesysMemor.SetDrillWarehouse(_DrillWarehouse);//寫入初始狀態
            WriteCodesysMemor.SetOill(_OillSystem);//寫入初始狀態
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

            //用戶未購買的實體刀庫改變成唯讀狀態
            if (_DrillWarehouse.RightEntrance.Length == 0)
            {
                this.RightIsEnabled = false;
            }
            if (_DrillWarehouse.LeftEntrance.Length == 0)
            {
                this.LeftIsEnabled = false;
            }
            CancelEnabled();
        }
        /// <summary>
        /// 讀取輸入點
        /// </summary>
        public void ReadInput()
        {
            try
            {
                //TODO:先註解等待小霖完成
                //Input input = ReadCodesysMemor.GetInput();//讀取記憶體
                //if (InSelectAxis)
                //{
                //    //左軸輸入
                //    LeftAxis = new ObservableCollection<put>()
                //        {
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_X_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.L_X_Origin)), Status = input.L_X_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_X_LimitBack)), Contact =  GetIODescription(input.GetType(),nameof(input.L_X_LimitBack)), Status = input.L_X_LimitBack},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_Y_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.L_Y_Origin)), Status = input.L_Y_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_Y_LimitBack)), Contact =  GetIODescription(input.GetType(),nameof(input.L_Y_LimitBack)), Status = input.L_Y_LimitBack},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_Y_LimitFornt)), Contact =  GetIODescription(input.GetType(),nameof(input.L_Y_LimitFornt)), Status = input.L_Y_LimitFornt},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_Z_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.L_Z_Origin)), Status = input.L_Z_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_Z_LimitBack)), Contact =  GetIODescription(input.GetType(),nameof(input.L_Z_LimitBack)), Status = input.L_Z_LimitBack},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_Z_LimitFront)), Contact =  GetIODescription(input.GetType(),nameof(input.L_Z_LimitFront)), Status = input.L_Z_LimitFront},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_Spindle_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.L_Spindle_Origin)), Status = input.L_Spindle_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_SpindleClip)), Contact =  GetIODescription(input.GetType(),nameof(input.L_SpindleClip)), Status = input.L_SpindleClip},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_SpindleLoosen)), Contact =  GetIODescription(input.GetType(),nameof(input.L_SpindleLoosen)), Status = input.L_SpindleLoosen},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.LX_MX_Touch)), Contact =  GetIODescription(input.GetType(),nameof(input.LX_MX_Touch)), Status = input.LX_MX_Touch},
                //        };
                //    //中軸輸入
                //    MiddleAxis = new ObservableCollection<put>()
                //        {
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_X_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.M_X_Origin)), Status = input.M_X_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_X_LimitBack)), Contact =  GetIODescription(input.GetType(),nameof(input.M_X_LimitBack)), Status = input.M_X_LimitBack},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_X_LimitFornt)), Contact =  GetIODescription(input.GetType(),nameof(input.M_X_LimitFornt)), Status = input.M_X_LimitFornt},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_Y_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.M_Y_Origin)), Status = input.M_Y_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_Y_LimitBack)), Contact =  GetIODescription(input.GetType(),nameof(input.M_Y_LimitBack)), Status = input.M_Y_LimitBack},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_Y_LimitFornt)), Contact =  GetIODescription(input.GetType(),nameof(input.M_Y_LimitFornt)), Status = input.M_Y_LimitFornt},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_Z_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.M_Z_Origin)), Status = input.M_Z_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_Z_LimitBack)), Contact =  GetIODescription(input.GetType(),nameof(input.M_Z_LimitBack)), Status = input.M_Z_LimitBack},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_Z_LimitFornt)), Contact =  GetIODescription(input.GetType(),nameof(input.M_Z_LimitFornt)), Status = input.M_Z_LimitFornt},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.MX_RX_Touch)), Contact =  GetIODescription(input.GetType(),nameof(input.MX_RX_Touch)), Status = input.MX_RX_Touch},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_SpindleClip)), Contact =  GetIODescription(input.GetType(),nameof(input.M_SpindleClip)), Status = input.M_SpindleClip},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_SpindleLoosen)), Contact =  GetIODescription(input.GetType(),nameof(input.M_SpindleLoosen)), Status = input.M_SpindleLoosen},
                //    };
                //    //右軸輸入
                //    RightAxis = new ObservableCollection<put>()
                //    {

                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_X_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.R_X_Origin)), Status = input.R_X_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_X_LimitBack)), Contact =  GetIODescription(input.GetType(),nameof(input.R_X_LimitBack)), Status = input.R_X_LimitBack},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_X_LimitFornt)), Contact =  GetIODescription(input.GetType(),nameof(input.R_X_LimitFornt)), Status = input.R_X_LimitFornt},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_Y_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.R_Y_Origin)), Status = input.R_Y_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_Y_LimitBack)), Contact =  GetIODescription(input.GetType(),nameof(input.R_Y_LimitBack)), Status = input.R_Y_LimitBack},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_Y_LimitFornt)), Contact =  GetIODescription(input.GetType(),nameof(input.R_Y_LimitFornt)), Status = input.R_Y_LimitFornt},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_Z_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.R_Z_Origin)), Status = input.R_Z_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_Z_LimitBack)), Contact =  GetIODescription(input.GetType(),nameof(input.R_Z_LimitBack)), Status = input.R_Z_LimitBack},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_Z_LimitFornt)), Contact =  GetIODescription(input.GetType(),nameof(input.R_Z_LimitFornt)), Status = input.R_Z_LimitFornt},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_Spindle_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.R_Spindle_Origin)), Status = input.R_Spindle_Origin},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_SpindleClip)), Contact =  GetIODescription(input.GetType(),nameof(input.R_SpindleClip)), Status = input.R_SpindleClip},
                //        new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_SpindleLoosen)), Contact =  GetIODescription(input.GetType(),nameof(input.R_SpindleLoosen)), Status = input.R_SpindleLoosen},
                //    };
                //}
                //else if (InSelectWarehouse)
                //{
                //    //左軸輸入
                //    LeftAxis = new ObservableCollection<put>()
                //{
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_IN_DrillHomeOrigin)), Contact =  GetIODescription(input.GetType(),nameof(input.L_IN_DrillHomeOrigin)), Status = input.L_IN_DrillHomeOrigin},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_IN_DrillHomeChange)), Contact =  GetIODescription(input.GetType(),nameof(input.L_IN_DrillHomeChange)), Status = input.L_IN_DrillHomeChange},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_OUT_DrillHomeOrigin)), Contact =  GetIODescription(input.GetType(),nameof(input.L_OUT_DrillHomeOrigin)), Status = input.L_OUT_DrillHomeOrigin},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_OUT_DrillHomeChange)), Contact =  GetIODescription(input.GetType(),nameof(input.L_OUT_DrillHomeChange)), Status = input.L_OUT_DrillHomeChange},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_DrillLength)), Contact =  GetIODescription(input.GetType(),nameof(input.L_DrillLength)), Status = input.L_DrillLength},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_IN_DrillHome_1)), Contact =  GetIODescription(input.GetType(),nameof(input.L_IN_DrillHome_1)), Status = input.L_IN_DrillHome_1},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_IN_DrillHome_2)), Contact =  GetIODescription(input.GetType(),nameof(input.L_IN_DrillHome_2)), Status = input.L_IN_DrillHome_2},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_IN_DrillHome_3)), Contact =  GetIODescription(input.GetType(),nameof(input.L_IN_DrillHome_3)), Status = input.L_IN_DrillHome_3},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_IN_DrillHome_4)), Contact =  GetIODescription(input.GetType(),nameof(input.L_IN_DrillHome_4)), Status = input.L_IN_DrillHome_4},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_OUT_DrillHome_1)), Contact =  GetIODescription(input.GetType(),nameof(input.L_OUT_DrillHome_1)), Status = input.L_OUT_DrillHome_1},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_OUT_DrillHome_2)), Contact =  GetIODescription(input.GetType(),nameof(input.L_OUT_DrillHome_2)), Status = input.L_OUT_DrillHome_2},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_OUT_DrillHome_3)), Contact =  GetIODescription(input.GetType(),nameof(input.L_OUT_DrillHome_3)), Status = input.L_OUT_DrillHome_3},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_OUT_DrillHome_4)), Contact =  GetIODescription(input.GetType(),nameof(input.L_OUT_DrillHome_4)), Status = input.L_OUT_DrillHome_4},
                //};
                //    //中軸輸入
                //    MiddleAxis = new ObservableCollection<put>()
                //{
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_DrillHomeOrigin)), Contact =  GetIODescription(input.GetType(),nameof(input.M_DrillHomeOrigin)), Status = input.M_DrillHomeOrigin},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_DrillHomeChange)), Contact =  GetIODescription(input.GetType(),nameof(input.M_DrillHomeChange)), Status = input.M_DrillHomeChange},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_DrillHome_1)), Contact =  GetIODescription(input.GetType(),nameof(input.M_DrillHome_1)), Status = input.M_DrillHome_1},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_DrillHome_2)), Contact =  GetIODescription(input.GetType(),nameof(input.M_DrillHome_2)), Status = input.M_DrillHome_2},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_DrillHome_3)), Contact =  GetIODescription(input.GetType(),nameof(input.M_DrillHome_3)), Status = input.M_DrillHome_3},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_DrillHome_4)), Contact =  GetIODescription(input.GetType(),nameof(input.M_DrillHome_4)), Status = input.M_DrillHome_4},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_DrillHome_5)), Contact =  GetIODescription(input.GetType(),nameof(input.M_DrillHome_5)), Status = input.M_DrillHome_5},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_DrillLength)), Contact =  GetIODescription(input.GetType(),nameof(input.M_DrillLength)), Status = input.M_DrillLength},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Z_HighSensorDoor_Close)), Contact =  GetIODescription(input.GetType(),nameof(input.Z_HighSensorDoor_Close)), Status = input.Z_HighSensorDoor_Close},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Z_HighSensorDoor_Open)), Contact =  GetIODescription(input.GetType(),nameof(input.Z_HighSensorDoor_Open)), Status = input.Z_HighSensorDoor_Open},

                //};
                //    //右軸輸入
                //    RightAxis = new ObservableCollection<put>()
                //{
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_IN_DrillHomeOrigin)), Contact =  GetIODescription(input.GetType(),nameof(input.R_IN_DrillHomeOrigin)), Status = input.R_IN_DrillHomeOrigin},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_IN_DrillHomeChange)), Contact =  GetIODescription(input.GetType(),nameof(input.R_IN_DrillHomeChange)), Status = input.R_IN_DrillHomeChange},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_OUT_DrillHomeOrigin)), Contact =  GetIODescription(input.GetType(),nameof(input.R_OUT_DrillHomeOrigin)), Status = input.R_OUT_DrillHomeOrigin},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_OUT_DrillHomeChange)), Contact =  GetIODescription(input.GetType(),nameof(input.R_OUT_DrillHomeChange)), Status = input.R_OUT_DrillHomeChange},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_DrillLength)), Contact =  GetIODescription(input.GetType(),nameof(input.R_DrillLength)), Status = input.R_DrillLength},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_IN_DrillHome_1)), Contact =  GetIODescription(input.GetType(),nameof(input.R_IN_DrillHome_1)), Status = input.R_IN_DrillHome_1},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_IN_DrillHome_2)), Contact =  GetIODescription(input.GetType(),nameof(input.R_IN_DrillHome_2)), Status = input.R_IN_DrillHome_2},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_IN_DrillHome_3)), Contact =  GetIODescription(input.GetType(),nameof(input.R_IN_DrillHome_3)), Status = input.R_IN_DrillHome_3},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_IN_DrillHome_4)), Contact =  GetIODescription(input.GetType(),nameof(input.R_IN_DrillHome_4)), Status = input.R_IN_DrillHome_4},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_OUT_DrillHome_1)), Contact =  GetIODescription(input.GetType(),nameof(input.R_OUT_DrillHome_1)), Status = input.R_OUT_DrillHome_1},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_OUT_DrillHome_2)), Contact =  GetIODescription(input.GetType(),nameof(input.R_OUT_DrillHome_2)), Status = input.R_OUT_DrillHome_2},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_OUT_DrillHome_3)), Contact =  GetIODescription(input.GetType(),nameof(input.R_OUT_DrillHome_3)), Status = input.R_OUT_DrillHome_3},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_OUT_DrillHome_4)), Contact =  GetIODescription(input.GetType(),nameof(input.R_OUT_DrillHome_4)), Status = input.R_OUT_DrillHome_4},
                //};
                //}
                //else if (InSelectLub)
                //{
                //    //左軸輸入
                //    LeftAxis = new ObservableCollection<put>()
                //{
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_X_lubricating)), Contact =  GetIODescription(input.GetType(),nameof(input.L_X_lubricating)), Status = input.L_X_lubricating},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_Y_lubricating)), Contact =  GetIODescription(input.GetType(),nameof(input.L_Y_lubricating)), Status = input.L_Y_lubricating},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_Z_lubricating)), Contact =  GetIODescription(input.GetType(),nameof(input.L_Z_lubricating)), Status = input.L_Z_lubricating},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_IN_Clip_Down_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.L_IN_Clip_Down_Origin)), Status = input.L_IN_Clip_Down_Origin},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.L_OUT_Clip_Down_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.L_OUT_Clip_Down_Origin)), Status = input.L_OUT_Clip_Down_Origin},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Side_IN_Clip_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.Side_IN_Clip_Origin)), Status = input.Side_IN_Clip_Origin},
                //};
                //    //中軸輸入
                //    MiddleAxis = new ObservableCollection<put>()
                //{
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_X_lubricating)), Contact =  GetIODescription(input.GetType(),nameof(input.M_X_lubricating)), Status = input.M_X_lubricating},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_Y_lubricating)), Contact =  GetIODescription(input.GetType(),nameof(input.M_Y_lubricating)), Status = input.M_Y_lubricating},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.M_Z_lubricating)), Contact =  GetIODescription(input.GetType(),nameof(input.M_Z_lubricating)), Status = input.M_Z_lubricating},
                //};
                //    //右軸輸入
                //    RightAxis = new ObservableCollection<put>()
                //{
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_X_lubricating)), Contact =  GetIODescription(input.GetType(),nameof(input.R_X_lubricating)), Status = input.R_X_lubricating},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_Y_lubricating)), Contact =  GetIODescription(input.GetType(),nameof(input.R_Y_lubricating)), Status = input.R_Y_lubricating},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_Z_lubricating)), Contact =  GetIODescription(input.GetType(),nameof(input.R_Z_lubricating)), Status = input.R_Z_lubricating},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_IN_Clip_Down_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.R_IN_Clip_Down_Origin)), Status = input.R_IN_Clip_Down_Origin},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.R_OUT_Clip_Down_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.R_OUT_Clip_Down_Origin)), Status = input.R_OUT_Clip_Down_Origin},
                //};
                //}
                //else if (InSensor)
                //{
                //    //其他輸入
                //    LeftAxis = new ObservableCollection<put>()
                //{
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.TH_RY)), Contact =  GetIODescription(input.GetType(),nameof(input.TH_RY)), Status = input.TH_RY},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.HydraulicPSI)), Contact =  GetIODescription(input.GetType(),nameof(input.HydraulicPSI)), Status = input.HydraulicPSI},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.AirPSI)), Contact =  GetIODescription(input.GetType(),nameof(input.AirPSI)), Status = input.AirPSI},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Feed_Slow_Down_Point)), Contact =  GetIODescription(input.GetType(),nameof(input.Feed_Slow_Down_Point)), Status = input.Feed_Slow_Down_Point},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Feed_Origin)), Contact =  GetIODescription(input.GetType(),nameof(input.Feed_Origin)), Status = input.Feed_Origin},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.FinishOut)), Contact =  GetIODescription(input.GetType(),nameof(input.FinishOut)), Status = input.FinishOut},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Total_Lubricatig)), Contact =  GetIODescription(input.GetType(),nameof(input.Total_Lubricatig)), Status = input.Total_Lubricatig},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Electrical_BOX_Colse)), Contact =  GetIODescription(input.GetType(),nameof(input.Electrical_BOX_Colse)), Status = input.Electrical_BOX_Colse},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Electrical_BOX_Lock)), Contact =  GetIODescription(input.GetType(),nameof(input.Electrical_BOX_Lock)), Status = input.Electrical_BOX_Lock},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Case_1_Close)), Contact =  GetIODescription(input.GetType(),nameof(input.Case_1_Close)), Status = input.Case_1_Close},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Case_1_Lock)), Contact =  GetIODescription(input.GetType(),nameof(input.Case_1_Lock)), Status = input.Case_1_Lock},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Case_2_Close)), Contact =  GetIODescription(input.GetType(),nameof(input.Case_2_Close)), Status = input.Case_2_Close},
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Case_2_Lock )), Contact =  GetIODescription(input.GetType(),nameof(input.Case_2_Lock )), Status = input.Case_2_Lock },
                //    new put(){ Name = GetCodesysName(input.GetType(),nameof(input.Case_3_Close)), Contact =  GetIODescription(input.GetType(),nameof(input.Case_3_Close)), Status = input.Case_3_Close},
                //};
                //}
            }
            catch (Exception)
            {
                //Thread.CurrentThread.Abort();
            }

        }
        /// <summary>
        /// 讀取輸出點
        /// </summary>
        public void ReadOut()
        {
            try
            {
                //TODO:先註解等待小霖完成
                //Output output = ReadCodesysMemor.GetOutput();//讀取記憶體

                //if (OutSelectAxis)
                //{
                //    //左軸輸出
                //    LeftAxis = new ObservableCollection<put>()
                // {
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_Spray_OilFog)), Contact =  GetIODescription(output.GetType(),nameof(output.L_Spray_OilFog)), Status = output.L_Spray_OilFog},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_Spary_Air)), Contact =  GetIODescription(output.GetType(),nameof(output.L_Spary_Air)), Status = output.L_Spary_Air},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_Spindle_Change_Air)), Contact =  GetIODescription(output.GetType(),nameof(output.L_Spindle_Change_Air)), Status = output.L_Spindle_Change_Air},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_Spindle_Loosen)), Contact =  GetIODescription(output.GetType(),nameof(output.L_Spindle_Loosen)), Status = output.L_Spindle_Loosen},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_IN_DrillHome_Origin)), Contact =  GetIODescription(output.GetType(),nameof(output.L_IN_DrillHome_Origin)), Status = output.L_IN_DrillHome_Origin},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_IN_DrillHome_RollOut)), Contact =  GetIODescription(output.GetType(),nameof(output.L_IN_DrillHome_RollOut)), Status = output.L_IN_DrillHome_RollOut},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_OUT_DrillHome_Origin)), Contact =  GetIODescription(output.GetType(),nameof(output.L_OUT_DrillHome_Origin)), Status = output.L_OUT_DrillHome_Origin},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_OUT_DrillHome_RollOut)), Contact =  GetIODescription(output.GetType(),nameof(output.L_OUT_DrillHome_RollOut)), Status = output.L_OUT_DrillHome_RollOut},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_IN_Clip_Down_Origin)), Contact =  GetIODescription(output.GetType(),nameof(output.L_IN_Clip_Down_Origin)), Status = output.L_IN_Clip_Down_Origin},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_IN_Clip_Down)), Contact =  GetIODescription(output.GetType(),nameof(output.L_IN_Clip_Down)), Status = output.L_IN_Clip_Down},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_OUT_Clip_Down_Origin)), Contact =  GetIODescription(output.GetType(),nameof(output.L_OUT_Clip_Down_Origin)), Status = output.L_OUT_Clip_Down_Origin},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.L_OUT_Clip_Down)), Contact =  GetIODescription(output.GetType(),nameof(output.L_OUT_Clip_Down)), Status = output.L_OUT_Clip_Down},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.SIDE_Clip_Origin)), Contact =  GetIODescription(output.GetType(),nameof(output.SIDE_Clip_Origin)), Status = output.SIDE_Clip_Origin},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.SIDE_Clip)), Contact =  GetIODescription(output.GetType(),nameof(output.SIDE_Clip)), Status = output.SIDE_Clip},
                //};
                //    //中軸輸出
                //    MiddleAxis = new ObservableCollection<put>()
                //{
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.M_Spray_OilFog)), Contact =  GetIODescription(output.GetType(),nameof(output.M_Spray_OilFog)), Status = output.M_Spray_OilFog},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.M_Spary_Air)), Contact =  GetIODescription(output.GetType(),nameof(output.M_Spary_Air)), Status = output.M_Spary_Air},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.M_Spindle_Change_Air)), Contact =  GetIODescription(output.GetType(),nameof(output.M_Spindle_Change_Air)), Status = output.M_Spindle_Change_Air},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.M_Spindle_Loosen)), Contact =  GetIODescription(output.GetType(),nameof(output.M_Spindle_Loosen)), Status = output.M_Spindle_Loosen},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.M_DirllHome_Origin)), Contact =  GetIODescription(output.GetType(),nameof(output.M_DirllHome_Origin)), Status = output.M_DirllHome_Origin},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.M_DrillHome_Change)), Contact =  GetIODescription(output.GetType(),nameof(output.M_DrillHome_Change)), Status = output.M_DrillHome_Change},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Z_High_Sensor_Door_Open)), Contact =  GetIODescription(output.GetType(),nameof(output.Z_High_Sensor_Door_Open)), Status = output.Z_High_Sensor_Door_Open},
                //};
                //    //右軸輸出
                //    RightAxis = new ObservableCollection<put>()
                //{
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_Spray_OilFog)), Contact =  GetIODescription(output.GetType(),nameof(output.R_Spray_OilFog)), Status = output.R_Spray_OilFog},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_Spary_Air)), Contact =  GetIODescription(output.GetType(),nameof(output.R_Spary_Air)), Status = output.R_Spary_Air},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_Spindle_Change_Air)), Contact =  GetIODescription(output.GetType(),nameof(output.R_Spindle_Change_Air)), Status = output.R_Spindle_Change_Air},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_Spindle_Loosen)), Contact =  GetIODescription(output.GetType(),nameof(output.R_Spindle_Loosen)), Status = output.R_Spindle_Loosen},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_IN_DrillHome_Origin)), Contact =  GetIODescription(output.GetType(),nameof(output.R_IN_DrillHome_Origin)), Status = output.R_IN_DrillHome_Origin},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_IN_DrillHome_RollOut)), Contact =  GetIODescription(output.GetType(),nameof(output.R_IN_DrillHome_RollOut)), Status = output.R_IN_DrillHome_RollOut},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_OUT_DrillHome_Origin)), Contact =  GetIODescription(output.GetType(),nameof(output.R_OUT_DrillHome_Origin)), Status = output.R_OUT_DrillHome_Origin},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_OUT_DrillHome_RollOut)), Contact =  GetIODescription(output.GetType(),nameof(output.R_OUT_DrillHome_RollOut)), Status = output.R_OUT_DrillHome_RollOut},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_IN_Clip_Down_Origin)), Contact =  GetIODescription(output.GetType(),nameof(output.R_IN_Clip_Down_Origin)), Status = output.R_IN_Clip_Down_Origin},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_IN_Clip_Down)), Contact =  GetIODescription(output.GetType(),nameof(output.R_IN_Clip_Down)), Status = output.R_IN_Clip_Down},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_OUT_Clip_Down_Origin)), Contact =  GetIODescription(output.GetType(),nameof(output.R_OUT_Clip_Down_Origin)), Status = output.R_OUT_Clip_Down_Origin},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.R_OUT_Clip_Down)), Contact =  GetIODescription(output.GetType(),nameof(output.R_OUT_Clip_Down)), Status = output.R_OUT_Clip_Down},
                //};
                //}
                //else
                //{
                //    //其他輸出
                //    LeftAxis = new ObservableCollection<put>()
                //{
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Hydraulic_On)), Contact =  GetIODescription(output.GetType(),nameof(output.Hydraulic_On)), Status = output.Hydraulic_On},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Volume_Scrap_On)), Contact =  GetIODescription(output.GetType(),nameof(output.Volume_Scrap_On)), Status = output.Volume_Scrap_On},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Finish_Motor_On)), Contact =  GetIODescription(output.GetType(),nameof(output.Finish_Motor_On)), Status = output.Finish_Motor_On},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Materials_Side_Move_CCW)), Contact =  GetIODescription(output.GetType(),nameof(output.Materials_Side_Move_CCW)), Status = output.Materials_Side_Move_CCW},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Materials_Side_Move_CW)), Contact =  GetIODescription(output.GetType(),nameof(output.Materials_Side_Move_CW)), Status = output.Materials_Side_Move_CW},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Materials_Side_Move_Speed_1)), Contact =  GetIODescription(output.GetType(),nameof(output.Materials_Side_Move_Speed_1)), Status = output.Materials_Side_Move_Speed_1},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Materials_Side_Move_Speed_2)), Contact =  GetIODescription(output.GetType(),nameof(output.Materials_Side_Move_Speed_2)), Status = output.Materials_Side_Move_Speed_2},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Electrical_BOX_Lock)), Contact =  GetIODescription(output.GetType(),nameof(output.Electrical_BOX_Lock)), Status = output.Electrical_BOX_Lock},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Case_1_Lock)), Contact =  GetIODescription(output.GetType(),nameof(output.Case_1_Lock)), Status = output.Case_1_Lock},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Case_2_Lock)), Contact =  GetIODescription(output.GetType(),nameof(output.Case_2_Lock)), Status = output.Case_2_Lock},
                //    new put(){ Name = GetCodesysName(output.GetType(),nameof(output.Case_3_Lock)), Contact =  GetIODescription(output.GetType(),nameof(output.Case_3_Lock)), Status = output.Case_3_Lock},
                //};
                //}
            }
            catch (Exception)
            {
                //Thread.CurrentThread.Abort();
            }
        }
        #endregion

        #region 命令
        /// <summary>
        /// 新增刀具設定的列表命令
        /// </summary>
        public ICommand NewDrillBrandsCommand { get; set; }
        private WPFBase.RelayCommand NewDrillBrands()
        {
            return new WPFBase.RelayCommand(() =>
            {
                
            });
        }
        /// <summary>
        /// 儲存刀具設定的列表命令
        /// </summary>
        public ICommand SaveDrillBrandsCommand { get; set; }
        private WPFBase.RelayCommand SaveDrillBrands()
        {
            return new WPFBase.RelayCommand(() => 
            {
                STDSerialization ser = new STDSerialization();//序列化處理器
                ser.SetDrillBrands(DrillBrands);//存取刀設定
            });
        }
        /// <summary>
        /// 刪除材質
        /// </summary>
        public ICommand DeleteMaterialCommand { get; set; }
        private WPFBase.RelayParameterizedCommand DeleteMaterial()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                if (e is SteelMaterial material) //判斷材質是否重複
                {
                    STDSerialization std = new STDSerialization(); //序列化處理器
                    _Materials.Remove(el => el.Name == material.Name);
                    Materials = SerializationHelper.Clone(_Materials); //複製物件
                    std.SetMaterial(_Materials);
                    ObservableCollection<SteelMaterial> system = std.GetMaterial(ModelPath.Material); //系統物件
                    if (system.FindIndex(el => el.Name == material.Name) != -1) //如果系統存在相同物件
                    {
                        MessageBoxResult message = MessageBox.Show($"在系統找到相同名稱，請問是否連同系統的也一並刪除 ?", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                        if (message == MessageBoxResult.Yes)//如果確定要變更系統
                        {
                            system.Remove(el => el.Name == material.Name); //刪除相同物件
                            std.SetMaterial(ModelPath.Material, system);//變更系統內設定的材質
                        }
                    }
                }
            });
        }
        /// <summary>
        /// 更新材質
        /// </summary>
        public ICommand UpdateMaterialCommand { get; set; }

        private WPFBase.RelayParameterizedCommand UpdateMaterial()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                STDSerialization std = new STDSerialization();//序列化器
                ObservableCollection<SteelMaterial> system = std.GetMaterial(ModelPath.Material); //系統內設定的材質
                int systemIndex = system.IndexOf(el => el.Name == Materials[MatrialIndex].Name);//找出系統有相同名稱的位置
                if (system.IndexOf(el => el.Name == Materials[MatrialIndex].Name) != -1) //如果系統有相同材質名稱
                {
                    MessageBoxResult message = MessageBox.Show($"在系統找到相同名稱，請問是否連同系統的也一並更新 ?", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    if (message == MessageBoxResult.Yes)//如果確定要變更系統
                    {
                        system[systemIndex] = SerializationHelper.Clone(SelectMaterial); //變更系統物件
                        std.SetMaterial(ModelPath.Material, system);//變更系統內設定的材質
                    }
                }
                _Materials[MatrialIndex] = SerializationHelper.Clone(SelectMaterial); //變更背景物件
                Materials = SerializationHelper.Clone(_Materials); //變更顯示物件
                std.SetMaterial(Materials);//變更模型物件
            });
        }
        /// <summary>
        /// 新增材質
        /// </summary>
        public ICommand NewCommand { get; set; }

        public WPFBase.RelayCommand New()
        {
            return new WPFBase.RelayCommand(() =>
            {
                SaveAsWindowsControl = false;
            });
        }
        /// <summary>
        /// 存取材質
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayParameterizedCommand SaveMaterial()
        {
            return new WPFBase.RelayParameterizedCommand(par =>
            {
                if (Materials.IndexOf(el => el.Name == SelectMaterial.Name) == -1) //判斷材質是否重複
                {
                    string str = par.ToString();//選擇的選項 系統 or 模型
                    STDSerialization std = new STDSerialization();//序列化器
                    _Materials.Add(SerializationHelper.Clone<SteelMaterial>(SelectMaterial));//加入到背景
                    Materials = SerializationHelper.Clone(_Materials);//加入到顯示畫面
                    std.SetMaterial(Materials);//存入在模型設定檔
                    if (str == "系統")//如果要變更系統材質
                    {
                        ObservableCollection<SteelMaterial> system = std.GetMaterial(ModelPath.Material); //系統內設定的材質
                        if (system.IndexOf(el => el.Name == SelectMaterial.Name) == -1) //如果系統內沒有這材質
                        {
                            system.Add(SelectMaterial); //加入到系統內的材質列表
                            std.SetMaterial(ModelPath.Material, system);//變更系統內設定的材質
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"已有存在相同名稱", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                }
                SaveAsWindowsControl = true;
            });
        }
        /// <summary>
        /// 關閉另存視窗
        /// </summary>
        public ICommand CloseSaveAsWindowsCommand { get; set; }
        private WPFBase.RelayCommand CloseSaveAs()
        {
            return new WPFBase.RelayCommand(() =>
            {
                SaveAsWindowsControl = true;
            });
        }

        /// <summary>
        /// 觸發 Input 命令，並且開啟<see cref="IOThread"/>持續監聽
        /// </summary>
        /// <remarks>
        /// 釋放 <see cref="IOThread"/> 方法<see cref="IOAbort"/>
        /// </remarks>
        public ICommand InputCommand { get; set; }
        private WPFBase.RelayCommand Input()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (IOThread != null)
                {
                    IOThread.Abort();
                }

                IOThread = new Thread(new ThreadStart(GetInput));
                IOThread.IsBackground = true;
                IOThread.Start();
            });
        }
        /// <summary>
        /// 觸發 Output 命令，並且開啟<see cref="IOThread"/>持續監聽
        /// </summary>
        public ICommand OutCommand { get; set; }
        private WPFBase.RelayCommand Output()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (IOThread != null)
                {
                    IOThread.Abort();
                }

                IOThread = new Thread(new ThreadStart(GetOutput));
                IOThread.IsBackground = true;
                IOThread.Start();
            });
        }

        /// <summary>
        /// 還原液壓油參數命令
        /// </summary>
        public ICommand ReductionHydCommand { get; set; }
        private WPFBase.RelayCommand ReductionOil()
        {
            return new WPFBase.RelayCommand(() =>
            {
                //TODO: 尚未實測過 此處實測完畢請寫入
            });
        }
        /// <summary>
        /// 存取液壓油參數命令
        /// </summary>
        public ICommand SaveHydCommand { get; set; }
        private WPFBase.RelayCommand SaveOil()
        {
            return new WPFBase.RelayCommand(() =>
            {
                List<HydraulicSystem> result = new List<HydraulicSystem>();
                foreach (var item in HydList)
                {
                    result.Add(item.ConverterStruc(item, SideReady, DownReady, Deviation));
                }
                this._OillSystem.HydraulicSystem = result.ToArray();
                WriteCodesysMemor.SetHydraulic(_OillSystem);
            });
        }

        /// <summary>
        /// 還原液壓油參數命令
        /// </summary>
        public ICommand ReductionCutCommand { get; set; }
        private WPFBase.RelayCommand ReductionCut()
        {
            return new WPFBase.RelayCommand(() =>
            {
                //TODO: 尚未實測過 此處實測完畢請寫入
            });
        }

        /// <summary>
        /// 存取切削油參數命令
        /// </summary>
        public ICommand SaveCutCommand { get; set; }
        private WPFBase.RelayCommand SaveCut()
        {
            return new WPFBase.RelayCommand(() =>
            {
                WriteCodesysMemor.SetCut(_OillSystem);
            });
        }

        /// <summary>
        /// 還原滑道油參數命令
        /// </summary>
        public ICommand ReductionLubCommand { get; set; }
        private WPFBase.RelayCommand ReductionLub()
        {
            return new WPFBase.RelayCommand(() =>
            {
                //TODO: 尚未實測過 此處實測完畢請寫入
            });
        }
        /// <summary>
        /// 存取滑道油參數命令
        /// </summary>
        public ICommand SaveLubCommand { get; set; }
        private WPFBase.RelayCommand SaveLub()
        {
            return new WPFBase.RelayCommand(() =>
            {
                WriteCodesysMemor.SetLubricant(_OillSystem);
            });
        }

        /// <summary>
        /// 更新 Codesys 命令
        /// </summary>
        public ICommand UpdateCommand { get; set; }
        private WPFBase.RelayCommand Update()
        {
            return new WPFBase.RelayCommand(() =>
            {
                UpDateDrill();
            });
        }

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
                    _drillSetting _dr = UseSelected[UseSelected.Count - 1];
                    DrillSetting _ = _dr.ConverterStruc();
                    _.IsCurrent = true;
                    int index = _.Index - 1; //刀庫位置
                    switch (DRILL_POSITION)
                    {
                        case DRILL_POSITION.EXPORT_L:
                            _DrillWarehouse.LeftExport[index] = _;
                            break;
                        case DRILL_POSITION.EXPORT_R:
                            _DrillWarehouse.RightExport[index] = _;
                            break;
                        case DRILL_POSITION.MIDDLE:
                            _DrillWarehouse.Middle[index] = _;
                            break;
                        case DRILL_POSITION.ENTRANCE_L:
                            _DrillWarehouse.LeftEntrance[index] = _;
                            break;
                        case DRILL_POSITION.ENTRANCE_R:
                            _DrillWarehouse.RightEntrance[index] = _;
                            break;
                        default:
                            Debugger.Break();
                            break;
                    }
                    Default.DrillWarehouse = _DrillWarehouse;
                    Default.Save();
                    WriteCodesysMemor.SetDrillWarehouse(_DrillWarehouse);//寫入記憶體
                    UpDateDrill();
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
                Default.DrillWarehouse = _DrillWarehouse;
                Default.Save();
                WriteCodesysMemor.SetDrillWarehouse(_DrillWarehouse);//寫入記憶體
            });
        }

        /// <summary>
        /// 選擇中<see cref="DrillWarehouse.Middle"/>命令
        /// </summary>
        public ICommand MiddleCommand { get; set; }
        private WPFBase.RelayCommand Middle()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearSelectd();
                this.IsSelectdMiddle = true;
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.Middle));
                //UseSelected = DrillTools(_DrillWarehouse.Middle, true);
            });
        }

        /// <summary>
        /// 選擇中<see cref="DrillWarehouse.LeftExport"/>命令
        /// </summary>
        public ICommand LeftExportCommand { get; set; }
        private WPFBase.RelayCommand LeftExport()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearSelectd();
                this.IsSelectdLeftExport = true;
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftExport));
                //UseSelected = DrillTools(_DrillWarehouse.LeftExport, true);
            });
        }

        /// <summary>
        /// 選擇中<see cref="DrillWarehouse.RightExport"/>命令
        /// </summary>
        public ICommand RightExportCommand { get; set; }
        private WPFBase.RelayCommand RightExport()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearSelectd();
                this.IsSelectdRightExport = true;
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightExport));
            });
        }

        /// <summary>
        /// 選擇中<see cref="DrillWarehouse.LeftEntrance"/>命令
        /// </summary>
        public ICommand LeftEntranceCommand { get; set; }
        private WPFBase.RelayCommand LeftEntrance()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearSelectd();
                this.IsSelectdLeftEntrance = true;
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.LeftEntrance));
                //UseSelected = DrillTools(_DrillWarehouse.LeftEntrance, true);
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
                UnusedSelected = new ObservableCollection<_drillSetting>(GetDrillSetting(_DrillWarehouse.RightEntrance));
                //UseSelected = DrillTools(_DrillWarehouse.RightEntrance, true);
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
        /// <summary>
        /// 軸向 output <see cref="OutSelectAxis"/> 命令
        /// </summary>
        public ICommand OAxisCommand { get; set; }
        private WPFBase.RelayCommand OAxis()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearOutputSelected();

                OutSelectAxis = true;
                InChangeHeader = "上軸";
            });
        }

        /// <summary>
        /// 軸向 output <see cref="OutSensor"/> 命令
        /// </summary>
        public ICommand OSensorCommand { get; set; }
        private WPFBase.RelayCommand OSensor()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearOutputSelected();

                OutSensor = true;
                OutChangeHeader = "其他";
            });
        }

        /// <summary>
        /// 軸向 input <see cref="InSelectAxis"/> 命令
        /// </summary>
        public ICommand AxisCommand { get; set; }
        private WPFBase.RelayCommand Axis()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearInputSelected();

                InSelectAxis = true;
                InChangeHeader = "上軸";
            });
        }

        /// <summary>
        /// 刀庫 input <see cref="InSelectWarehouse"/> 命令
        /// </summary>
        public ICommand WarehouseCommand { get; set; }
        private WPFBase.RelayCommand Warehouse()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearInputSelected();

                InSelectWarehouse = true;
                InChangeHeader = "上軸";
            });
        }

        /// <summary>
        /// 電阻尺與潤滑 input <see cref="InSelectLub"/> 命令
        /// </summary>
        public ICommand LubCommand { get; set; }
        private WPFBase.RelayCommand Lub()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearInputSelected();

                InSelectLub = true;
                InChangeHeader = "上軸";
            });
        }

        /// <summary>
        /// 其他感知器 <see cref="InSensor"/> 命令
        /// </summary>
        public ICommand SensorCommand { get; set; }
        private WPFBase.RelayCommand Sensor()
        {
            return new WPFBase.RelayCommand(() =>
            {
                ClearInputSelected();

                InSensor = true;
                InChangeHeader = "其他";
            });
        }
        ///// <summary>
        ///// 存取參數命令
        ///// </summary>
        //public ICommand SaveCommand { get; set; }
        ///// <summary>
        ///// 存取參數
        ///// </summary>
        ///// <returns></returns>
        //public WPFBase.RelayCommand Save()
        //{
        //    return new WPFBase.RelayCommand(() =>
        //    {
        //        DisplaySavePosition = true;
        //    });
        //}
        /// <summary>
        /// 存取系統或模型的命令
        /// </summary>
        public ICommand SaveModelRoSystem { get; set; }
        /// <summary>
        /// 存取斷面規格
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayParameterizedCommand SaveProfile()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                string str = el.ToString();
                string strType = ((OBJETC_TYPE)_SelectType).ToString();
                PropertyInfo showPropertyInfo = this.GetType().GetProperty(strType);//反射在畫面顯示的資料列表
                PropertyInfo backPropertyInfo = this.GetType().GetProperty($@"_{strType}", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);//反射在背景的資料列表欄位
                object getValue = (ObservableCollection<SteelAttr>)backPropertyInfo.GetValue(this); //反射在背景的資料列表
                ObservableCollection<SteelAttr> value = _Save((ObservableCollection<SteelAttr>)getValue);//加入設定好的值
                backPropertyInfo.SetValue(this, value); //存取資料到背景列表
                showPropertyInfo.SetValue(this, SerializationHelper.Clone(value));//存取資料到顯示畫面的列表

                if (str == "系統")//如果確定要變更系統
                {
                    ObservableCollection<SteelAttr> system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\{strType}.inp");//系統的斷面規格
                    SteelAttr steelAttr = GetSettingSteelAttr(); //取得設定好的斷面規格
                    if (system.IndexOf(e => e.Profile == steelAttr.Profile) != -1) //如果有相同的斷面規格
                    {
                        system.Remove(e => e.Profile == steelAttr.Profile);//刪除
                    }
                    system.Add(steelAttr);//加入到系統
                    SerializationHelper.SerializeBinary(system, $@"{ModelPath.Profile}\{strType}.inp"); //變更系統斷面規格檔案
                }
                SerializationHelper.SerializeBinary(value, $@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\{strType}.inp");//變更模型設定的斷面規格
                SaveAsWindowsControl = true;
            });
        }

        /// <summary>
        /// 更新樹狀圖
        /// </summary>
        public ICommand UpdateTreeCommand { get; set; }
        /// <summary>
        /// 更新資料
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayParameterizedCommand UpdateTree()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                DisplaySavePosition = true;
                if (e is SteelAttr steelAttr)
                {
                    string strType = steelAttr.Type.ToString(); //選擇的斷面規格類型
                    PropertyInfo showPropertyInfo = this.GetType().GetProperty(strType);//反射在畫面顯示的資料列表
                    PropertyInfo backPropertyInfo = this.GetType().GetProperty($@"_{strType}", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);//反射在背景的資料列表欄位
                    ObservableCollection<SteelAttr> value = (ObservableCollection<SteelAttr>)backPropertyInfo.GetValue(this);//反射在背景的資料列表
                    ObservableCollection<SteelAttr> system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ModelPath.Profile}\{strType}.inp");//系統內設定的斷面規格
                    int backIndex = value.IndexOf(el => el.Profile == steelAttr.Profile);//背景列表位置 
                    int systemIndex = system.IndexOf(el => el.Profile == steelAttr.Profile);//選擇系統列表的位置
                    SteelAttr update = SelectSteelAtte;//變更過的資料
                    update.H = InsertionData[0].Value;
                    update.W = InsertionData[1].Value;
                    update.t1 = InsertionData[2].Value;
                    update.t2 = InsertionData[3].Value;
                    update.Profile = ProfileName;
                    value[backIndex] = update; //變更模型資料
                    showPropertyInfo.SetValue(this, SerializationHelper.Clone(value));//變更到顯示列表
                    backPropertyInfo.SetValue(this, value);//變更到背景列表
                    if (systemIndex != -1)//如果系統存在相同物件
                    {
                        MessageBoxResult message = MessageBox.Show($"在系統找到相同名稱，請問是否連同系統的也一並更新 ?", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                        if (message == MessageBoxResult.Yes)//如果確定要變更系統
                        {
                            system[systemIndex] = SerializationHelper.Clone(update);//變更模型資料
                            SerializationHelper.SerializeBinary(system, $@"{ModelPath.Profile}\{strType}.inp");//變更系統內設定的斷面規格
                        }
                    }
                    SerializationHelper.SerializeBinary(value, $@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\{strType}.inp");//變更模型設定的斷面規格
                }
            });
        }
        /// <summary>
        /// 刪除命令
        /// </summary>
        public ICommand DeleteCommand { get; set; }
        private WPFBase.RelayParameterizedCommand Delete()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                DisplaySavePosition = true;
                if (e is SteelAttr steelAttr)
                {
                    string strType = ((OBJETC_TYPE)steelAttr.Type).ToString();
                    PropertyInfo showPropertyInfo = this.GetType().GetProperty(strType);//反射在畫面顯示的資料列表
                    PropertyInfo backPropertyInfo = this.GetType().GetProperty($@"_{strType}", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);//反射在背景的資料列表欄位

                    ObservableCollection<SteelAttr> value = (ObservableCollection<SteelAttr>)backPropertyInfo.GetValue(this);//反射在背景的資料列表
                    value.Remove(el => el.Profile == steelAttr.Profile);
                    showPropertyInfo.SetValue(this, SerializationHelper.Clone(value));
                    ObservableCollection<SteelAttr> system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ModelPath.Profile}\{strType}.inp");//系統內設定的斷面規格

                    if (system.IndexOf(el => el.Profile == steelAttr.Profile) != -1)//如果系統存在相同物件
                    {
                        MessageBoxResult message = MessageBox.Show($"在系統找到相同名稱，請問是否連同系統的也一並刪除 ?", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                        if (message == MessageBoxResult.Yes)//如果確定要變更系統
                        {
                            system.Remove(el => el.Profile == steelAttr.Profile);
                            SerializationHelper.SerializeBinary(system, $@"{ModelPath.Profile}\{strType}.inp");//變更系統內設定的斷面規格
                        }
                    }
                    SerializationHelper.SerializeBinary(value, $@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\{strType}.inp");//變更模型設定的斷面規格
                }
            });
        }
        /// <summary>
        /// 材質過濾命令
        /// </summary>
        public ICommand FilterMaterialCommand { get; set; }
        public WPFBase.RelayParameterizedCommand FilterMaterial()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                if (!isDuplication)
                {
                    if (el.ToString() != "*")
                    {
                        Materials = new ObservableCollection<SteelMaterial>(_Materials.Where(e => e.Name.Contains(el.ToString())).ToList());
                    }
                    else if (el.ToString() != "*" || el.ToString() != "")
                    {
                        Materials = new ObservableCollection<SteelMaterial>(_Materials.ToList());
                    }
                }
            });
        }
        /// <summary>
        /// 斷面規格過濾器指令
        /// </summary>
        public ICommand FilterCommand { get; set; }
        private WPFBase.RelayParameterizedCommand ClickFilterButton()
        {
            return new WPFBase.RelayParameterizedCommand((object e) =>
            {
                if (!isDuplication)
                {
                    if (e.ToString() != "*")
                    {
                        RH = new ObservableCollection<SteelAttr>(_RH.Where(el => el.Profile.Contains(e.ToString())).ToList());
                        BH = new ObservableCollection<SteelAttr>(_BH.Where(el => el.Profile.Contains(e.ToString())).ToList());
                        CH = new ObservableCollection<SteelAttr>(_CH.Where(el => el.Profile.Contains(e.ToString())).ToList());
                        BOX = new ObservableCollection<SteelAttr>(_BOX.Where(el => el.Profile.Contains(e.ToString())).ToList());
                        L = new ObservableCollection<SteelAttr>(_L.Where(el => el.Profile.Contains(e.ToString())).ToList());
                    }
                    else if (e.ToString() != "*" || e.ToString() != "")
                    {
                        RH = new ObservableCollection<SteelAttr>(_RH);
                        BH = new ObservableCollection<SteelAttr>(_BH);
                        CH = new ObservableCollection<SteelAttr>(_CH);
                        BOX = new ObservableCollection<SteelAttr>(_BOX);
                        L = new ObservableCollection<SteelAttr>(_L);
                    }
                }
            });
        }
        /// <summary>
        /// 斷面規格名稱
        /// </summary>
        public ICommand SegmentString { get; set; }
        private WPFBase.RelayParameterizedCommand ClickTreeViewItem()
        {
            return new WPFBase.RelayParameterizedCommand((object e) =>
            {
                InsertionData.Clear();
                if (e is SteelAttr steelAttr)
                {
                    SelectSteelAtte = steelAttr;
                    ProfileName = SelectSteelAtte.Profile;
                    SelectType = (int)steelAttr.Type;
                }
                initializationInsertionData();
            });
        }
        /// <summary>
        /// 關閉儲存位置視窗
        /// </summary>
        public ICommand CloseSavePosition { get; set; }
        private WPFBase.RelayCommand ClickCancelButton()
        {
            return new WPFBase.RelayCommand(() =>
            {
                DisplaySavePosition = false;
            });
        }
        #endregion

        #region 私有屬性
        /// <summary>
        /// 監聽 IO 執行緒
        /// </summary>
        private Thread IOThread = null;
        private List<Axis4D> _LeftExport = new List<Axis4D>();
        private List<Axis4D> _RightExport = new List<Axis4D>();
        private List<Axis4D> _LeftEntrance = new List<Axis4D>();
        private List<Axis4D> _RightEntrance = new List<Axis4D>();
        private List<Axis4D> _Middle = new List<Axis4D>();
        /// <summary>
        /// 使用者刀庫設定
        /// </summary>
        private GD_STD.DrillWarehouse _DrillWarehouse { get; set; }
        /// <summary>
        /// 油壓系統
        /// </summary>
        private OillSystem _OillSystem { get; set; }
        private DRILL_POSITION DRILL_POSITION { get; set; } = DRILL_POSITION.MIDDLE;
        /// <summary>
        /// 潤滑油系統/循環次數
        /// </summary>
        private short _LubFrequency { get; set; }
        /// <summary>
        /// 潤滑油系統/循環時間
        /// </summary>
        private short _LubTime { get; set; }
        /// <summary>
        /// 切消油/打油頻率
        /// </summary>
        private short _CutFrequency { get; set; }
        /// <summary>
        /// RH 讀取到的資料列表
        /// </summary>
        private ObservableCollection<SteelAttr> _RH { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// BH 讀取到的資料列表
        /// </summary>
        private ObservableCollection<SteelAttr> _BH { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// BOX 讀取到的資料列表
        /// </summary>
        private ObservableCollection<SteelAttr> _BOX { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// CH 讀取到的資料列表
        /// </summary>
        private ObservableCollection<SteelAttr> _CH { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// L 讀取到的資料列表
        /// </summary>
        private ObservableCollection<SteelAttr> _L { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// 材質讀取到的資料列表
        /// </summary>
        private ObservableCollection<SteelMaterial> _Materials { get; set; }
        private int _MatrialIndex = -1;
        #endregion

        #region 公開屬性
        /// <summary>
        /// 鑽頭品牌列表
        /// </summary>
        public ObservableCollection<DrillBrand> DrillBrands { get; set; }
        /// <summary>
        /// 另存視窗控制
        /// </summary>
        public bool SaveAsWindowsControl { get; set; } = true;

        /// <summary>
        /// 選擇<see cref="DrillWarehouse.Middle"/>
        /// </summary>
        public bool IsSelectdMiddle { get; set; } = true;
        /// <summary>
        /// 選擇 Input 軸向按鈕
        /// </summary>
        public bool InSelectAxis { get; set; } = true;
        /// <summary>
        /// 選擇 Input 刀庫按鈕
        /// </summary>
        public bool InSelectWarehouse { get; set; } = false;
        /// <summary>
        /// 選擇 Input 電阻尺與潤滑油按鈕
        /// </summary>
        public bool InSelectLub { get; set; } = false;
        /// <summary>
        /// 選擇 Input 感測器按鈕
        /// </summary>
        public bool InSensor { get; set; } = false;
        /// <summary>
        /// 選擇 Output 軸向按鈕
        /// </summary>
        public bool OutSelectAxis { get; set; } = true;
        /// <summary>
        /// 選擇 Out 感測器按鈕
        /// </summary>
        public bool OutSensor { get; set; } = false;
        /// <summary>
        /// Input 會變更的標題。查詢綁定的業面內找
        /// </summary>
        public string InChangeHeader { get; set; } = "左軸";
        /// <summary>
        /// Output 會變更的標題。查詢綁定的業面內找
        /// </summary>
        public string OutChangeHeader { get; set; } = "左軸";
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
        /// 選擇左軸配裝的刀具
        /// </summary>
        public bool IsCurrentLeft { get; set; }
        /// <summary>
        /// 選擇右軸配裝的刀具
        /// </summary>
        public bool IsCurrentRight { get; set; }
        /// <summary>
        /// 是否顯示 <see cref="DrillWarehouse.RightEntrance"/> 的按鈕 
        /// </summary>
        public bool RightIsEnabled { get; set; } = true;
        /// <summary>
        /// 是否顯示 <see cref="DrillWarehouse.LeftEntrance"/> 的按鈕 
        /// </summary>
        public bool LeftIsEnabled { get; set; } = true;

        /// <summary>
        /// 側壓預備位置
        /// </summary>
        public short SideReady { get; set; }

        /// <summary>
        /// 下壓預備位置
        /// </summary>
        public short DownReady { get; set; }

        /// <summary>
        /// 誤差距離
        /// </summary>
        public float Deviation { get; set; }
        /// <summary>
        /// 潤滑油系統/循環次數
        /// </summary>
        public short LubFrequency
        {
            get => _LubFrequency;
            set
            {
                _OillSystem.LubricantSystem = new LubricantSystem()
                {
                    Frequency = value,
                    Time = _OillSystem.LubricantSystem.Time
                };
                _LubFrequency = value;
            }
        }
        /// <summary>
        /// 潤滑油系統/循環時間
        /// </summary>
        public short LubTime
        {
            get => _LubTime;
            set
            {
                _OillSystem.LubricantSystem = new LubricantSystem()
                {
                    Time = value,
                    Frequency = _OillSystem.LubricantSystem.Frequency
                };
                _LubTime = value;
            }
        }
        /// <summary>
        /// 切消油/打油頻率
        /// </summary>
        public short CutFrequency
        {
            get => _CutFrequency;
            set
            {
                _OillSystem.CutOilSystem = new CutOilSystem() { Frequency = value };
                _CutFrequency = value;
            }
        }
        /// <summary>
        /// 未裝載在主軸上的刀庫
        /// </summary>
        public ObservableCollection<_drillSetting> UnusedSelected { get; set; }
        /// <summary>
        /// 裝載在主軸上的刀庫
        /// </summary>
        public ObservableCollection<_drillSetting> UseSelected { get; set; } = new ObservableCollection<_drillSetting>();
        /// <summary>
        /// 液壓系統參數列表
        /// </summary>
        public ObservableCollection<_hydraulicSystem> HydList { get; set; }
        /// <summary>
        /// 左軸 
        /// </summary>
        public ObservableCollection<put> LeftAxis { get; set; } = new ObservableCollection<put>();
        /// <summary>
        /// 中軸 
        /// </summary>
        public ObservableCollection<put> MiddleAxis { get; set; } = new ObservableCollection<put>();
        /// <summary>
        /// 右軸 
        /// </summary>
        public ObservableCollection<put> RightAxis { get; set; } = new ObservableCollection<put>();
        /// <summary>
        /// 斷面規格的資料容器 for DataGrid
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<DataGridData> InsertionData { get; set; } = new ObservableCollection<DataGridData>();
        /// <summary>
        /// 選擇的參數檔案
        /// </summary>
        public SteelAttr SelectSteelAtte { get => _SelectSteelAtte; set => _SelectSteelAtte = value; }
        /// <summary>
        /// 顯示Beam 2D圖
        /// </summary>
        public bool VisibilityH { get; set; } = false;
        /// <summary>
        /// 顯示CH 2D圖
        /// </summary>
        public bool VisibilityCH { get; set; } = false;
        /// <summary>
        /// 顯示BOX 2D圖
        /// </summary>
        public bool VisibilityBOX { get; set; } = false;
        /// <summary>
        /// 顯示L 2D圖
        /// </summary>
        public bool VisibilityL { get; set; } = false;
        /// <summary>
        /// 斷面規格類型
        /// </summary>
        public int SelectType
        {
            get => _SelectType;
            set
            {
                _SelectType = value;
                VisibilityBOX = false;
                VisibilityH = false;
                VisibilityCH = false;
                VisibilityBOX = false;
                VisibilityL = false;
                switch ((OBJETC_TYPE)value)
                {
                    case OBJETC_TYPE.BH:
                    case OBJETC_TYPE.RH:
                        VisibilityH = true;
                        break;
                    case OBJETC_TYPE.CH:
                        VisibilityCH = true;
                        break;
                    case OBJETC_TYPE.L:
                        VisibilityL = true;
                        break;
                    case OBJETC_TYPE.BOX:
                        VisibilityBOX = true;
                        break;
                    default:
                        break;
                }
                initializationInsertionData();
            }
        }
        /// <summary>
        /// 斷面規格
        /// </summary>
        public string ProfileName { get; set; } = string.Empty;
        /// <summary>
        /// RH 要顯示集合列表
        /// </summary>
        public ObservableCollection<SteelAttr> RH { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// BH 要顯示集合列表
        /// </summary>
        public ObservableCollection<SteelAttr> BH { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// BOX 要顯示集合列表
        /// </summary>
        public ObservableCollection<SteelAttr> BOX { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// CH 要顯示集合列表
        /// </summary>
        public ObservableCollection<SteelAttr> CH { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// L 要顯示集合列表
        /// </summary>
        public ObservableCollection<SteelAttr> L { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// 材質要顯示的集合
        /// </summary>
        public ObservableCollection<SteelMaterial> Materials { get; set; } = new ObservableCollection<SteelMaterial>();
        /// <summary>
        /// 綁定儲存位置視窗顯示
        /// </summary>
        public bool DisplaySavePosition { get; set; }
        /// <summary>
        /// 材質選擇索引列表
        /// </summary>
        public int MatrialIndex
        {
            get => _MatrialIndex;
            set
            {
                _MatrialIndex = value;
                if (value != -1)
                {
                    SelectMaterial = SerializationHelper.Clone<SteelMaterial>(Materials[value]);
                }
            }
        }
        /// <summary>
        /// 選擇的材質
        /// </summary>
        public SteelMaterial SelectMaterial { get; set; }
        #endregion

        #region 私有方法
        /// <summary>
        /// 取得設定的斷面規格
        /// </summary>
        /// <returns></returns>
        private SteelAttr GetSettingSteelAttr()
        {
            SteelAttr data = new SteelAttr();
            data.Type = (OBJETC_TYPE)_SelectType;
            data.Kg = SelectSteelAtte.Kg;
            data.H = InsertionData[0].Value;
            data.W = InsertionData[1].Value;
            data.t1 = InsertionData[2].Value;
            data.t2 = (OBJETC_TYPE)_SelectType == OBJETC_TYPE.L || (OBJETC_TYPE)_SelectType == OBJETC_TYPE.BOX ? InsertionData[2].Value : InsertionData[3].Value;
            data.Profile = ProfileName;
            return data;
        }
        /// <summary>
        /// 載入斷面規格與材質列表
        /// </summary>
        private void LoadProfile()
        {
            STDSerialization ser = new STDSerialization();
            RH = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\RH.inp");
            BH = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\BH.inp");
            BOX = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\BOX.inp");
            CH = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\CH.inp");
            L = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\L.inp");
            _Materials = ser.GetMaterial();
            Materials = ser.GetMaterial();
            _RH = new ObservableCollection<SteelAttr>(RH);
            _BH = new ObservableCollection<SteelAttr>(BH);
            _BOX = new ObservableCollection<SteelAttr>(BOX);
            _CH = new ObservableCollection<SteelAttr>(CH);
            _L = new ObservableCollection<SteelAttr>(L);
        }
        /// <summary>
        /// 清除 input 當前選擇
        /// </summary>
        private void ClearInputSelected()
        {
            InSelectAxis = false;
            InSelectWarehouse = false;
            InSelectLub = false;
            InSensor = false;
        }
        /// <summary>
        /// 清除 output 當前選擇
        /// </summary>
        private void ClearOutputSelected()
        {
            this.OutSelectAxis = false;
            this.OutSensor = false;
        }
        /// <summary>
        /// 連續讀取 input
        /// </summary>
        private void GetInput()
        {
            Thread.Sleep((int)LEVEL.MEDIUM);
            while (true)
            {
                ReadInput();
            }
        }

        /// <summary>
        /// 連續讀取 Output
        /// </summary>
        private void GetOutput()
        {
            Thread.Sleep((int)LEVEL.MEDIUM);
            while (true)
            {
                ReadOut();

            }
        }
        ///// <summary>
        ///// 需要初始化(僅限無刀庫版本)
        ///// </summary>
        ///// <returns>如果需要回傳 true, 不需要 false</returns>
        //private bool Isinitialization(DrillSetting[] drillSettings)
        //{
        //    if (drillSettings.Length == 1) //如果是無刀庫版本
        //    {
        //        //軟體第一次初始化
        //        if (!drillSettings[0].IsCurrent)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        ///// <summary>
        ///// 也許程式第一次執行抓不到刀需要初始化(僅限無刀庫版本)
        ///// </summary>
        ///// <returns></returns>
        //private DrillSetting initialization(DrillSetting[] drillSettings)
        //{
        //    //軟體第一次初始化
        //    _drillSetting _ = new _drillSetting(new DrillSetting());
        //    _.IsCurrent = true; //初始化無刀庫版本
        //    _.Index = 1;
        //    return _.ConverterStruc();
        //}
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
        /// 分類刀庫
        /// </summary>
        /// <param name="drillSettings">刀庫列表</param>
        /// <param name="isUse">是使用的刀庫</param>
        /// <returns></returns>
        private ObservableCollection<_drillSetting> DrillTools(DrillSetting[] drillSettings, bool isUse)
        {
            return new ObservableCollection<_drillSetting>(from el in drillSettings where el.IsCurrent == isUse select new _drillSetting(el));
        }
        /// <summary>
        /// 變更刀庫設定的值
        /// </summary>
        /// <param name="drillSetting">要變更的鑽頭的資訊</param>
        private void DrillChange(_drillSetting drillSetting)
        {
            int index = drillSetting.Index - 1;//刀庫陣列位置

            DrillSetting result = drillSetting.ConverterStruc();//轉換結構
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
        ///// <summary>
        ///// 產生位置並賦予換刀座標
        ///// </summary>
        ///// <param name="drillSettings"></param>
        ///// <param name="position"></param>
        //public static void Location(DrillSetting[] drillSettings, List<Axis4D> position)
        //{
        //    for (int i = 0; i < drillSettings.Length; i++)
        //    {
        //        drillSettings[i].Index = Convert.ToInt16(i + 1);
        //        drillSettings[i].Position = position[i];
        //    }
        //}
        /// <summary>
        /// 產生項目
        /// </summary>
        /// <param name="hydraulicSystems"></param>
        /// <returns></returns>
        private void Location(HydraulicSystem[] hydraulicSystems)
        {
            for (int i = 0; i < hydraulicSystems.Length; i++)
            {
                hydraulicSystems[i].Index = Convert.ToInt16(i + 1);
            }
        }

        /// <summary>
        /// 產生全部刀庫位置
        /// </summary>
        /// <returns></returns>
        private void Location(GD_STD.DrillWarehouse drillSettings)
        {
            drillSettings.Middle = SettingHelper.Location(drillSettings.Middle, this._Middle);
            drillSettings.LeftExport = SettingHelper.Location(drillSettings.LeftExport, this._LeftExport);
            drillSettings.RightExport = SettingHelper.Location(drillSettings.RightExport, this._RightExport);
            drillSettings.LeftEntrance = SettingHelper.Location(drillSettings.LeftEntrance, this._LeftEntrance);
            drillSettings.RightEntrance = SettingHelper.Location(drillSettings.RightEntrance, this._RightEntrance);
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
                result.Add(new _drillSetting(drillSettings[i]));
            }
            return result;
        }
        /// <summary>
        /// 轉換物件為類型
        /// </summary>
        /// <param name="hydraulicSystems"></param>
        /// <returns></returns>
        private List<_hydraulicSystem> GetHydraulicSystems(HydraulicSystem[] hydraulicSystems)
        {
            List<_hydraulicSystem> result = new List<_hydraulicSystem>();
            for (int i = 0; i < hydraulicSystems.Length; i++)
            {
                result.Add(new _hydraulicSystem(hydraulicSystems[i]));
            }
            return result;
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
                _.IsCurrent = false;
                drills.Add(_);
                UseSelected = new ObservableCollection<_drillSetting>(drills);
            }
        }
        /// <summary>
        /// 初始化 <see cref="InsertionData"/>
        /// </summary>
        private void initializationInsertionData()
        {
            InsertionData.Clear();
            InsertionData.Add(new DataGridData() { Property = "高度", Symbol = "H", Value = SelectSteelAtte.H, Unit = "mm" });
            InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
            InsertionData.Add(new DataGridData() { Property = "腹板", Symbol = "t1", Value = SelectSteelAtte.t1, Unit = "mm" });
            //if ((OBJETC_TYPE)_SelectType == OBJETC_TYPE.L || (OBJETC_TYPE)_SelectType == OBJETC_TYPE.BOX)
            //    return;

            InsertionData.Add(new DataGridData() { Property = "翼板", Symbol = "t2", Value = SelectSteelAtte.t2, Unit = "mm" });
        }

        /// <summary>
        /// 儲存到資料格
        /// </summary>
        /// <param name="list">要儲存的集合</param>
        private ObservableCollection<SteelAttr> _Save(ObservableCollection<SteelAttr> list)
        {
            int index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Profile == ProfileName)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                SteelAttr data = GetSettingSteelAttr();
                List<SteelAttr> result = new List<SteelAttr>(list.ToList());
                result.Add(data);

                //斷面規格字串排序
                SortProfile(result);

                return new ObservableCollection<SteelAttr>(result);
            }
            else
            {
                MessageBox.Show("新增斷面規格失敗。\n找到相同斷面規格名稱，請按更新鍵。", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                return list;
            }
        }


        /// <summary>
        /// 儲存到資料格
        /// </summary>
        /// <param name="list">要儲存的集合</param>
        private ObservableCollection<SteelAttr> _Update(ObservableCollection<SteelAttr> list)
        {
            int index = list.IndexOf(SelectSteelAtte);

            if (index != -1)
            {
                SteelAttr data = new SteelAttr();
                data.Kg = SelectSteelAtte.Kg;
                data.H = InsertionData[0].Value;
                data.W = InsertionData[1].Value;
                data.t1 = InsertionData[2].Value;
                data.t2 = (OBJETC_TYPE)_SelectType == OBJETC_TYPE.L || (OBJETC_TYPE)_SelectType == OBJETC_TYPE.BOX ? InsertionData[2].Value : InsertionData[3].Value;
                data.Profile = ProfileName;
                List<SteelAttr> result = new List<SteelAttr>(list.ToList());
                result[index] = data;

                //斷面規格字串排序
                SortProfile(result);

                return new ObservableCollection<SteelAttr>(result);
            }
            else
            {
                MessageBox.Show("新增斷面規格失敗。\n找到相同斷面規格名稱，請按更新鍵。", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                return list;
            }
        }
        /// <summary>
        /// 排序斷面規格名稱
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<SteelAttr> SortProfile(List<SteelAttr> data)
        {
            data.Sort((x, y) =>
            {
                if (x == null || y == null)
                {
                    throw new ArgumentException("Parameters can't be null");
                }

                var nameA = x.Profile as string;
                var nameB = y.Profile as string;
                var arr1 = nameA.ToCharArray();
                var arr2 = nameB.ToCharArray();

                var i = 0;
                var j = 0;

                while (i < arr1.Length && j < arr2.Length)
                {
                    if (char.IsDigit(arr1[i]) && char.IsDigit(arr2[j]))
                    {
                        string s1 = "", s2 = "";
                        while (i < arr1.Length && char.IsDigit(arr1[i]))
                        {
                            s1 += arr1[i];
                            i++;
                        }
                        while (j < arr2.Length && char.IsDigit(arr2[j]))
                        {
                            s2 += arr2[j];
                            j++;
                        }
                        if (double.Parse(s1) > double.Parse(s2))
                        {
                            return 1;
                        }
                        if (double.Parse(s1) < double.Parse(s2))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        if (arr1[i] > arr2[j])
                        {
                            return 1;
                        }
                        if (arr1[i] < arr2[j])
                        {
                            return -1;
                        }
                        i++;
                        j++;
                    }
                }

                if (arr1.Length == arr2.Length)
                {
                    return 0;
                }
                else
                {
                    return arr1.Length > arr2.Length ? 1 : -1;
                }
            });
            return data;
        }
        private bool isDuplication;
        private SteelAttr _SelectSteelAtte = new SteelAttr();

        private int _SelectType { get; set; } = -1;
        #endregion

        #region VM類型
        public class _drillSetting : IDrillSetting
        {
            public double Dia { get; set; }
            public DRILL_TYPE DrillType
            {
                get => (DRILL_TYPE)IntDrillType;
                set
                {
                    IntDrillType = (short)value;
                }
            }
            /// <summary>
            /// <see cref="DRILL_TYPE"/> 號碼
            /// </summary>
            public short IntDrillType { get; set; }
            public float FeedQuantity { get; set; }
            public short Index { get; set; }
            public bool IsCurrent { get; set; }
            public float Length { get; set; }
            public DRILL_LEVEL Level
            {
                get => (DRILL_LEVEL)IntLevel;
                set
                {
                    IntLevel = (short)value;
                }
            }
            /// <inheritdoc/>
            public short IntLevel { get; set; }
            /// <inheritdoc/>
            public short Limit { get; set; }
            /// <inheritdoc/>
            public double Rpm { get; set; }
            /// <inheritdoc/>
            public Axis4D Position { get; set; }
            /// <inheritdoc/>
            public bool Change { get; set; }
            /// <summary>
            /// 刀柄長
            /// </summary>
            public float KnifeHandle { get; set; }
            /// <summary>
            /// <see cref="_drillSetting"/> 轉換 <see cref="DrillSetting"/>
            /// </summary>
            /// <returns></returns>
            public DrillSetting ConverterStruc()
            {
                return new DrillSetting()
                {
                    Dia = this.Dia,
                    DrillType = this.DrillType,
                    FeedQuantity = this.FeedQuantity,
                    Index = this.Index,
                    IsCurrent = this.IsCurrent,
                    Length = this.Length,
                    Level = this.Level,
                    Limit = this.Limit,
                    Rpm = this.Rpm,
                    Position = this.Position,
                    Change = true,
                    KnifeHandle = KnifeHandle
                };
            }
            /// <summary>
            /// 建構式
            /// </summary>
            /// <param name="drillSetting"></param>
            public _drillSetting(DrillSetting drillSetting)
            {
                this.Dia = drillSetting.Dia;
                this.DrillType = drillSetting.DrillType;
                this.FeedQuantity = drillSetting.FeedQuantity;
                this.Index = drillSetting.Index;
                this.IsCurrent = drillSetting.IsCurrent;
                this.Length = drillSetting.Length;
                this.Level = drillSetting.Level;
                this.Limit = drillSetting.Limit;
                this.Rpm = drillSetting.Rpm;
                this.Change = drillSetting.Change;
                this.Position = drillSetting.Position;
                this.KnifeHandle = drillSetting.KnifeHandle;
            }
        }
        /// <summary>
        /// 接點
        /// </summary>
        public class put
        {
            /// <summary>
            /// 名稱
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 敘述類型
            /// </summary>
            public string Contact { get; set; }
            /// <summary>
            /// 狀態
            /// </summary>
            public bool Status { get; set; }
        }
        public class _hydraulicSystem : IHydraulicSystem
        {
            public short Index { get; set; }
            public ushort Power { get; set; }
            public short MinRange { get; set; }
            public short MaxRange { get; set; }
            public short SideReady { get; set; }
            public short DownReady { get; set; }
            public float Deviation { get; set; }
            /// <summary>
            /// 備註
            /// </summary>
            public string Remarks { get; set; } = "H型鋼";
            public HydraulicSystem ConverterStruc(_hydraulicSystem hyd, short sideReady, short downReady, float deviation)
            {
                return new HydraulicSystem()
                {
                    Index = Index,
                    Power = Power,
                    Deviation = deviation,
                    DownReady = downReady,
                    MaxRange = MaxRange,
                    MinRange = MinRange,
                    SideReady = sideReady,
                };
            }
            public _hydraulicSystem(IHydraulicSystem hyd)
            {
                this.Index = hyd.Index;
                this.Power = hyd.Power;
                this.MinRange = hyd.MinRange;
                this.MaxRange = hyd.MaxRange;
                this.SideReady = hyd.SideReady;
                this.DownReady = hyd.DownReady;
                this.Deviation = hyd.Deviation;
            }
        }
        #endregion
    }
    /// <summary>
    /// 資料容器
    /// </summary>
    public class DataGridData
    {
        /// <summary>
        /// 性質
        /// </summary>
        public string Property { get; set; }
        /// <summary>
        /// 符號
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public float Value { get; set; }
        /// <summary>
        /// 單位
        /// </summary>
        public string Unit { get; set; }
    }
}

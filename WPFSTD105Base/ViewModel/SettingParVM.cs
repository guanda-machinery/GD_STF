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
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;
using System.IO;
using System.Collections;
using SectionData;
using SplitLineSettingData;

using DevExpress.Xpf.Dialogs;
using WPFSTD105;

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
            UpdateCommand = Update();
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
            SaveModelRoSystemCommand = SaveAs();//儲存新增的斷面規格
            ProfileSaveModelRoSystemCommand = SaveProfile();
            MaterialSaveModelRoSystemCommand = SaveMaterial();
            //如果是機器操作介面
            if (CommonViewModel.GetType() == typeof(ApplicationVM))
            {
                UpDate();
                DrillBrands = new STDSerialization().GetDrillBrands();
                if (DrillBrands.FindIndex(el => el.Guid == DrillBrand.GetNull().Guid) != -1)
                {
                    DrillBrands.RemoveAt(0);
                }
            }
            DeleteCommand = Delete();
            UpdateTreeCommand = UpdateTree();
            FilterCommand = ClickFilterButton();
            FilterMaterialCommand = FilterMaterial();
            SegmentString = ClickTreeViewItem();
            if (CommonViewModel.ProjectName == null)//如果沒有專案名稱
            {
                ApplicationViewModel.ActionLoadProfile = new Action(LoadProfile);//等待新件專案或開啟專案使用委派方法幫用戶載入檔案
            }
            else
            {
                LoadProfile();//直接載入
            }

            TransformInpCommand = TransformInp();//20220728 張燕華 轉換出inp檔案
            NewCommand = New();
            DeleteMaterialCommand = DeleteMaterial();
            UpdateMaterialCommand = UpdateMaterial();

            SaveDrillBrandsCommand = SaveDrillBrands();
            NewDrillBrandsCommand = NewDrillBrands();
            DeleteDrillBrandsCommand = DeleteDrillBrands();
            initializationInsertionData();//20220801 張燕華 因為不同素材具有不同規格屬性, 若無參數則預設為H型鋼規格屬性

            CheckParameterSettingDirectoryExists();//20220819 張燕華 檢查參數設定資料夾是否存在, 若否則新增

            //initializationProcessingZoneData();//20220818 張燕華 加工區域設定 - 檢查設定值檔案存在, 若否則新增預設設定值
            ShowProcessingZoneCommand = ShowProcessingZone();//20220810 張燕華 加工區域設定 - 顯示斷面規格設定圖片
            ShowProcessingSettingValueCommand = ShowProcessingSettingValue();//20220811 張燕華 加工區域設定 - 加工方式
            NewProcessingZoneCommand = NewProcessingZone();//20220811 張燕華 加工區域設定 - 新增加工區域設定數值
            AllSelectedToggleCommand = AllSelectedToggle();//20220811 張燕華 加工區域設定 - CheckBox全選按鈕
            ModifyProcessingZoneCommand = ModifyProcessingZone();//20220812 張燕華 加工區域設定 - 修改按鈕
            GoBackProcessingZoneCommand = GoBackProcessingZone();//20220812 張燕華 加工區域設定 - 復原按鈕

            initializationSplitLineSettingData();//20220816 張燕華 切割線設定 - 讀取目前檔案中的設定值
            ShowHowManyPartsRelatedComboboxCommand = ShowHowManyPartsRelatedCombobox();//20220823 張燕華 切割線設定 - 設定切割等分
            NewSplitLineCommand = NewSplitLine();//20220816 張燕華 切割線設定 - 新增設定數值
            ToggleAllSplitLineCheckboxCommand = ToggleAllSplitLineCheckbox();//20220816 張燕華 切割線設定 - CheckBox全選按鈕
            GoBackSplitLineCommand = GoBackSplitLine();//20220816 張燕華 切割線設定 - 復原按鈕

            SoftwareVersionInstallCommand = SoftwareVersionInstall();//20220819 張燕華 軟體版本-安裝按鈕
            ImportLogoCommand = ImportLogo();//20221006 張燕華 報表LOGO
            CopyAndSaveLogoCommand = CopyAndSaveLogo();
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
        public void UpDate()
        {
            //CodeSys斷電保持 直接撈取即可

            _OillSystem = ReadCodesysMemor.GetOill();

            //Location(_DrillWarehouse);//產生刀庫位置
            Location(_OillSystem.HydraulicSystem); ;//產生液壓系統項目

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
        /// <summary>
        /// 檢查參數設定資料夾是否存在, 若否則新增 20220819 張燕華 
        /// </summary>
        private void CheckParameterSettingDirectoryExists()
        {
            STDSerialization ser_dir = new STDSerialization();
            bool checkSectionTypeProcessingDataFile = ser_dir.CheckParameterSettingDirectory();
        }
        /// <summary>
        /// 初始化切割線設定 20220816 張燕華
        /// </summary>
        private void initializationProcessingZoneData()
        {
            //檢查使用者專案中是否已經存在設定檔案H_DRILLING_processingzone.lis, BOX_DRILLING_processingzone.lis, CH_DRILLING_processingzone.lis
            STDSerialization ser_file = new STDSerialization();
            bool[] checkSectionTypeProcessingDataFile = ser_file.CheckSectionTypeProcessingDataFile();

            if (checkSectionTypeProcessingDataFile[0] == false)//若設定值檔案H_DRILLING_processingzone.lis不存在
            {
                //新增設定值檔案
                STDSerialization ser = new STDSerialization();
                ObservableCollection<SectionTypeProcessingData> listSectionData = new ObservableCollection<SectionTypeProcessingData>();
                listSectionData.Add(new SectionTypeProcessingData()
                {
                    SectionCategoryType = "H",
                    ProcessingBehavior = 0,
                    A = 25,
                    B = 15,
                    C = 5
                });
                ser.SetSectionTypeProcessingData(listSectionData);//加入到H型鋼的斷面加工資料檔案
            }
            if (checkSectionTypeProcessingDataFile[1] == false)//若設定值檔案BOX_DRILLING_processingzone.lis不存在
            {
                //新增設定值檔案
                STDSerialization ser = new STDSerialization();
                ObservableCollection<SectionTypeProcessingData> listSectionData = new ObservableCollection<SectionTypeProcessingData>();
                listSectionData.Add(new SectionTypeProcessingData()
                {
                    SectionCategoryType = "BOX",
                    ProcessingBehavior = 0,
                    A = 15,
                    B = 15
                });
                ser.SetSectionTypeProcessingData(listSectionData);//加入到H型鋼的斷面加工資料檔案
            }
            if (checkSectionTypeProcessingDataFile[2] == false)//若設定值檔案CH_DRILLING_processingzone.lis不存在
            {
                //新增設定值檔案
                STDSerialization ser = new STDSerialization();
                ObservableCollection<SectionTypeProcessingData> listSectionData = new ObservableCollection<SectionTypeProcessingData>();
                listSectionData.Add(new SectionTypeProcessingData()
                {
                    SectionCategoryType = "CH",
                    ProcessingBehavior = 0,
                    A = 15,
                    B = 15
                });
                ser.SetSectionTypeProcessingData(listSectionData);//加入到H型鋼的斷面加工資料檔案
            }
        }
        #endregion

        #region 命令
        /// <summary>
        /// 加工區域 - 新增加工區域設定數值 20220810 張燕華
        /// </summary>
        public ICommand NewProcessingZoneCommand { get; set; }
        private WPFBase.RelayParameterizedCommand NewProcessingZone()
        {
            return new WPFBase.RelayParameterizedCommand((object cbxSelectedIndex) =>
            {
            switch (Convert.ToInt32(cbxSelectedIndex))
            {
                case (int)SectionProcessing_SteelType.H:
                    //若型鋼型態&加工行為&數值checkbox皆無勾選, 需提示錯誤
                    if (chb_SteelType && chb_ProcessingBehavior && chb_H_Avalue && chb_H_Bvalue && chb_H_Cvalue == true)
                    {
                        if (ProcessingZone_A >= 25 && ProcessingZone_B >= 14 && ProcessingZone_C >= 3)
                        {
                            STDSerialization ser = new STDSerialization(); //序列化處理器
                            ObservableCollection<SectionTypeProcessingData> listSectionData = new ObservableCollection<SectionTypeProcessingData>();
                            listSectionData.Add(new SectionTypeProcessingData()
                            {
                                SectionCategoryType = "H",
                                ProcessingBehavior = SelectProcessingBehavior,
                                A = ProcessingZone_A,
                                B = ProcessingZone_B,
                                C = ProcessingZone_C
                            });
                            ser.SetSectionTypeProcessingData(listSectionData);//加入到H型鋼的斷面加工資料檔案

                            WinUIMessageBox.Show(null,
                                                 $"新建完成",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);

                            chb_SteelType = false;
                            chb_ProcessingBehavior = false;
                            chb_H_Avalue = false;
                            chb_H_Bvalue = false;
                            chb_H_Cvalue = false;
                        }
                        else
                        {
                            WinUIMessageBox.Show(null,
                                                 $"請輸入適當的設定值",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);
                        }
                    }
                    else
                    {
                        WinUIMessageBox.Show(null,
                                             $"請確認各選項並勾選",
                                             "通知",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation,
                                             MessageBoxResult.None,
                                             MessageBoxOptions.None,
                                             FloatingMode.Popup);
                    }
                    break;
                case (int)SectionProcessing_SteelType.BOX:
                    //若型鋼型態&加工行為&數值checkbox皆無勾選, 需提示錯誤
                    if (chb_SteelType && chb_ProcessingBehavior && chb_BOX_Avalue && chb_BOX_Bvalue == true)
                    {
                        if (ProcessingZone_A >= 15 && ProcessingZone_B >= 15)
                        {
                            STDSerialization ser = new STDSerialization(); //序列化處理器
                            ObservableCollection<SectionTypeProcessingData> listSectionData = new ObservableCollection<SectionTypeProcessingData>();
                            listSectionData.Add(new SectionTypeProcessingData()
                            {
                                SectionCategoryType = "BOX",
                                ProcessingBehavior = SelectProcessingBehavior,
                                A = ProcessingZone_A,
                                B = ProcessingZone_B,
                            });
                            ser.SetSectionTypeProcessingData(listSectionData);//加入到BOX的斷面加工資料檔案

                            WinUIMessageBox.Show(null,
                                                 $"新建完成",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);

                            chb_SteelType = false;
                            chb_ProcessingBehavior = false;
                            chb_BOX_Avalue = false;
                            chb_BOX_Bvalue = false;
                        }
                        else
                        {
                            WinUIMessageBox.Show(null,
                                                 $"請輸入適當的設定值",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);
                        }
                    }
                    else
                    {
                        WinUIMessageBox.Show(null,
                                             $"請確認各選項並勾選",
                                             "通知",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation,
                                             MessageBoxResult.None,
                                             MessageBoxOptions.None,
                                             FloatingMode.Popup);
                    }
                    break;
                case (int)SectionProcessing_SteelType.CH:
                    //若型鋼型態&加工行為&數值checkbox皆無勾選, 需提示錯誤
                    if (chb_SteelType && chb_ProcessingBehavior && chb_CH_Avalue && chb_CH_Bvalue == true)
                    {
                        if (ProcessingZone_A >= 15 && ProcessingZone_B >= 15)
                        {
                            STDSerialization ser = new STDSerialization(); //序列化處理器
                            ObservableCollection<SectionTypeProcessingData> listSectionData = new ObservableCollection<SectionTypeProcessingData>();
                            listSectionData.Add(new SectionTypeProcessingData()
                            {
                                SectionCategoryType = "CH",
                                ProcessingBehavior = SelectProcessingBehavior,
                                A = ProcessingZone_A,
                                B = ProcessingZone_B,
                            });
                            ser.SetSectionTypeProcessingData(listSectionData);//加入到CH的斷面加工資料檔案

                            WinUIMessageBox.Show(null,
                                                 $"新建完成",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);

                            chb_SteelType = false;
                            chb_ProcessingBehavior = false;
                            chb_CH_Avalue = false;
                            chb_CH_Bvalue = false;
                        }
                        else
                        {
                            WinUIMessageBox.Show(null,
                                                 $"請輸入適當的設定值",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);
                        }
                    }
                    else
                    {
                        WinUIMessageBox.Show(null,
                                             $"請確認各選項並勾選",
                                             "通知",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation,
                                             MessageBoxResult.None,
                                             MessageBoxOptions.None,
                                             FloatingMode.Popup);
                    }
                    break;
                default:
                    WinUIMessageBox.Show(null,
                                         $"請選擇型鋼型態並確認",
                                         "通知",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Exclamation,
                                         MessageBoxResult.None,
                                         MessageBoxOptions.None,
                                         FloatingMode.Popup);
                    break;
                }
            });
        }
        /// <summary>
        /// 加工區域 - CheckBox全選按鈕 20220810 張燕華
        /// </summary>
        public ICommand AllSelectedToggleCommand { get; set; }
        private WPFBase.RelayParameterizedCommand AllSelectedToggle()
        {
            return new WPFBase.RelayParameterizedCommand((object cbxSelectedIndex) =>
            {
                switch (Convert.ToInt32(cbxSelectedIndex))
                {
                    case (int)SectionProcessing_SteelType.H:
                        if (chb_SteelType && chb_ProcessingBehavior && chb_H_Avalue && chb_H_Bvalue && chb_H_Cvalue == true)
                        {
                            chb_SteelType = false;
                            chb_ProcessingBehavior = false;
                            chb_H_Avalue = false;
                            chb_H_Bvalue = false;
                            chb_H_Cvalue = false;
                        }
                        else
                        {
                            chb_SteelType = true;
                            chb_ProcessingBehavior = true;
                            chb_H_Avalue = true;
                            chb_H_Bvalue = true;
                            chb_H_Cvalue = true;
                        }
                        break;
                    case (int)SectionProcessing_SteelType.BOX:
                        if (chb_SteelType && chb_ProcessingBehavior && chb_BOX_Avalue && chb_BOX_Bvalue == true)
                        {
                            chb_SteelType = false;
                            chb_ProcessingBehavior = false;
                            chb_BOX_Avalue = false;
                            chb_BOX_Bvalue = false;
                        }
                        else
                        {
                            chb_SteelType = true;
                            chb_ProcessingBehavior = true;
                            chb_BOX_Avalue = true;
                            chb_BOX_Bvalue = true;
                        }
                        break;
                    case (int)SectionProcessing_SteelType.CH:
                        if (chb_SteelType && chb_ProcessingBehavior && chb_CH_Avalue && chb_CH_Bvalue == true)
                        {
                            chb_SteelType = false;
                            chb_ProcessingBehavior = false;
                            chb_CH_Avalue = false;
                            chb_CH_Bvalue = false;
                        }
                        else
                        {
                            chb_SteelType = true;
                            chb_ProcessingBehavior = true;
                            chb_CH_Avalue = true;
                            chb_CH_Bvalue = true;
                        }
                        break;
                    default:
                        WinUIMessageBox.Show(null,
                                             $"請先選擇型鋼型態",
                                             "通知",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation,
                                             MessageBoxResult.None,
                                             MessageBoxOptions.None,
                                             FloatingMode.Popup);

                        break;
                }
            });
        }
        /// <summary>
        /// 加工區域 - 修改按鈕 20220812 張燕華
        /// </summary>
        public ICommand ModifyProcessingZoneCommand { get; set; }
        private WPFBase.RelayCommand ModifyProcessingZone()
        {
            return new WPFBase.RelayCommand(() =>
            {
                //清除備份的加工區域數值
                BackupData_ProcessingZone.Clear();

                //已勾選數值的大小錯誤flag
                bool f_ChbChecked_ValueError = false;
                
                switch (SelectSectionType)
                {
                    case (int)SectionProcessing_SteelType.H:
                        if ((chb_SteelType && chb_ProcessingBehavior) == true && (chb_H_Avalue || chb_H_Bvalue || chb_H_Cvalue) == true)
                        {
                            if (chb_H_Avalue == true && ProcessingZone_A < 25) f_ChbChecked_ValueError = true;
                            if (chb_H_Bvalue == true && ProcessingZone_B < 14) f_ChbChecked_ValueError = true;
                            if (chb_H_Cvalue == true && ProcessingZone_C < 3)  f_ChbChecked_ValueError = true;

                            if (f_ChbChecked_ValueError == false)
                            {
                                //讀入加工區域設定值
                                STDSerialization serH = new STDSerialization(); //序列化處理器
                                BackupData_ProcessingZone = serH.GetSectionTypeProcessingData("H", SelectProcessingBehavior);//備份當前加工區域數值


                                STDSerialization ser = new STDSerialization();
                                ObservableCollection<SectionTypeProcessingData> listSectionData = new ObservableCollection<SectionTypeProcessingData>();
                                listSectionData.Add(new SectionTypeProcessingData()//以前次加工區域數值初始化數值
                                {
                                    SectionCategoryType = BackupData_ProcessingZone[0].SectionCategoryType,
                                    ProcessingBehavior = BackupData_ProcessingZone[0].ProcessingBehavior,
                                    A = BackupData_ProcessingZone[0].A,
                                    B = BackupData_ProcessingZone[0].B,
                                    C = BackupData_ProcessingZone[0].C,
                                });
                                if (chb_H_Avalue == true) { listSectionData[0].A = ProcessingZone_A; }
                                if (chb_H_Bvalue == true) { listSectionData[0].B = ProcessingZone_B; }
                                if (chb_H_Cvalue == true) { listSectionData[0].C = ProcessingZone_C; }
                                ser.SetSectionTypeProcessingData(listSectionData);//寫入已更新的數值

                                //取消所有checkbox
                                chb_SteelType = false;
                                chb_ProcessingBehavior = false;
                                chb_H_Avalue = false;
                                chb_H_Bvalue = false;
                                chb_H_Cvalue = false;

                                //打開復原按鈕
                                GoBackButtonEnabled = true;

                                WinUIMessageBox.Show(null,
                                                     $"修改完成(不符限制條件的數值將不儲存)",
                                                     "通知",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Exclamation,
                                                     MessageBoxResult.None,
                                                     MessageBoxOptions.None,
                                                     FloatingMode.Popup);
                            }
                            else
                            {
                                WinUIMessageBox.Show(null,
                                                     $"請輸入適當的設定值",
                                                     "通知",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Exclamation,
                                                     MessageBoxResult.None,
                                                     MessageBoxOptions.None,
                                                     FloatingMode.Popup);

                                f_ChbChecked_ValueError = false; //關閉檢查flag
                            }
                        }
                        else
                        {
                            WinUIMessageBox.Show(null,
                                                 $"請確認種類設定並至少勾選一個數值來修改",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);
                        }
                        break;
                    case (int)SectionProcessing_SteelType.BOX:
                        if ((chb_SteelType && chb_ProcessingBehavior) == true && (chb_BOX_Avalue || chb_BOX_Bvalue) == true)
                        {
                            if (chb_BOX_Avalue == true && ProcessingZone_A < 15) f_ChbChecked_ValueError = true;
                            if (chb_BOX_Bvalue == true && ProcessingZone_B < 15) f_ChbChecked_ValueError = true;

                            if (f_ChbChecked_ValueError == false)
                            {
                                //讀入加工區域設定值
                                STDSerialization serBOX = new STDSerialization(); //序列化處理器
                                BackupData_ProcessingZone = serBOX.GetSectionTypeProcessingData("BOX", SelectProcessingBehavior);//備份當前加工區域數值


                                STDSerialization ser = new STDSerialization(); //序列化處理器
                                ObservableCollection<SectionTypeProcessingData> listSectionData = new ObservableCollection<SectionTypeProcessingData>();
                                listSectionData.Add(new SectionTypeProcessingData()//以前次加工區域數值初始化數值
                                {
                                    SectionCategoryType = BackupData_ProcessingZone[0].SectionCategoryType,
                                    ProcessingBehavior = BackupData_ProcessingZone[0].ProcessingBehavior,
                                    A = BackupData_ProcessingZone[0].A,
                                    B = BackupData_ProcessingZone[0].B,
                                });
                                if (chb_BOX_Avalue == true) { listSectionData[0].A = ProcessingZone_A; }
                                if (chb_BOX_Bvalue == true) { listSectionData[0].B = ProcessingZone_B; }
                                ser.SetSectionTypeProcessingData(listSectionData);//寫入已更新的數值

                                //取消所有checkbox
                                chb_SteelType = false;
                                chb_ProcessingBehavior = false;
                                chb_BOX_Avalue = false;
                                chb_BOX_Bvalue = false;

                                //打開復原按鈕
                                GoBackButtonEnabled = true;

                                WinUIMessageBox.Show(null,
                                                     $"修改完成(不符限制條件的數值將不儲存)",
                                                     "通知",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Exclamation,
                                                     MessageBoxResult.None,
                                                     MessageBoxOptions.None,
                                                     FloatingMode.Popup);
                            }
                            else
                            {
                                WinUIMessageBox.Show(null,
                                                     $"請輸入適當的設定值",
                                                     "通知",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Exclamation,
                                                     MessageBoxResult.None,
                                                     MessageBoxOptions.None,
                                                     FloatingMode.Popup);

                                f_ChbChecked_ValueError = false; //關閉檢查flag
                            }
                        }
                        else
                        {
                            WinUIMessageBox.Show(null,
                                                 $"請確認種類設定並至少勾選一個數值來修改",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);
                        }
                        break;
                    case (int)SectionProcessing_SteelType.CH:
                        if ((chb_SteelType && chb_ProcessingBehavior) == true && (chb_CH_Avalue || chb_CH_Bvalue) == true)
                        {
                            if (chb_CH_Avalue == true && ProcessingZone_A < 15) f_ChbChecked_ValueError = true;
                            if (chb_CH_Bvalue == true && ProcessingZone_B < 15) f_ChbChecked_ValueError = true;

                            if (f_ChbChecked_ValueError == false)
                            {
                                //讀入加工區域設定值
                                STDSerialization serCH = new STDSerialization(); //序列化處理器
                                BackupData_ProcessingZone = serCH.GetSectionTypeProcessingData("CH", SelectProcessingBehavior);//備份當前加工區域數值


                                STDSerialization ser = new STDSerialization(); //序列化處理器
                                ObservableCollection<SectionTypeProcessingData> listSectionData = new ObservableCollection<SectionTypeProcessingData>();
                                listSectionData.Add(new SectionTypeProcessingData()//以前次加工區域數值初始化數值
                                {
                                    SectionCategoryType = BackupData_ProcessingZone[0].SectionCategoryType,
                                    ProcessingBehavior = BackupData_ProcessingZone[0].ProcessingBehavior,
                                    A = BackupData_ProcessingZone[0].A,
                                    B = BackupData_ProcessingZone[0].B,
                                });
                                if (chb_CH_Avalue == true) { listSectionData[0].A = ProcessingZone_A; }
                                if (chb_CH_Bvalue == true) { listSectionData[0].B = ProcessingZone_B; }
                                ser.SetSectionTypeProcessingData(listSectionData);//寫入已更新的數值

                                //取消所有checkbox
                                chb_SteelType = false;
                                chb_ProcessingBehavior = false;
                                chb_CH_Avalue = false;
                                chb_CH_Bvalue = false;

                                //打開復原按鈕
                                GoBackButtonEnabled = true;

                                WinUIMessageBox.Show(null,
                                                     $"修改完成(不符限制條件的數值將不儲存)",
                                                     "通知",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Exclamation,
                                                     MessageBoxResult.None,
                                                     MessageBoxOptions.None,
                                                     FloatingMode.Popup);
                            }
                            else
                            {
                                WinUIMessageBox.Show(null,
                                                     $"請輸入適當的設定值",
                                                     "通知",
                                                     MessageBoxButton.OK,
                                                     MessageBoxImage.Exclamation,
                                                     MessageBoxResult.None,
                                                     MessageBoxOptions.None,
                                                     FloatingMode.Popup);

                                f_ChbChecked_ValueError = false; //關閉檢查flag
                            }
                        }
                        else
                        {
                            WinUIMessageBox.Show(null,
                                                 $"請確認種類設定並至少勾選一個數值來修改",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);
                        }
                        break;
                    default:
                        WinUIMessageBox.Show(null,
                                             $"請先選擇型鋼型態",
                                             "通知",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation,
                                             MessageBoxResult.None,
                                             MessageBoxOptions.None,
                                             FloatingMode.Popup);

                        break;
                }
            });
        }
        /// <summary>
        /// 加工區域 - 復原按鈕 20220812 張燕華
        /// </summary>
        public ICommand GoBackProcessingZoneCommand { get; set; }
        private WPFBase.RelayCommand GoBackProcessingZone()
        {
            return new WPFBase.RelayCommand(() =>
            {
                STDSerialization ser = new STDSerialization(); //序列化處理器
                ObservableCollection<SectionTypeProcessingData> listSectionData = new ObservableCollection<SectionTypeProcessingData>();
                switch (SelectSectionType)
                {
                    case (int)SectionProcessing_SteelType.H:
                        ProcessingZone_A = BackupData_ProcessingZone[0].A;
                        ProcessingZone_B = BackupData_ProcessingZone[0].B;
                        ProcessingZone_C = BackupData_ProcessingZone[0].C;

                        listSectionData.Add(new SectionTypeProcessingData()
                        {
                            SectionCategoryType = "H",
                            ProcessingBehavior = SelectProcessingBehavior,
                            A = ProcessingZone_A,
                            B = ProcessingZone_B,
                            C = ProcessingZone_C,
                        });
                        ser.SetSectionTypeProcessingData(listSectionData);//加入到H的斷面加工資料檔案
                        break;
                    case (int)SectionProcessing_SteelType.BOX:
                        ProcessingZone_A = BackupData_ProcessingZone[0].A;
                        ProcessingZone_B = BackupData_ProcessingZone[0].B;

                        listSectionData.Add(new SectionTypeProcessingData()
                        {
                            SectionCategoryType = "BOX",
                            ProcessingBehavior = SelectProcessingBehavior,
                            A = ProcessingZone_A,
                            B = ProcessingZone_B,
                        });
                        ser.SetSectionTypeProcessingData(listSectionData);//加入到BOX的斷面加工資料檔案
                        break;
                    case (int)SectionProcessing_SteelType.CH:
                        ProcessingZone_A = BackupData_ProcessingZone[0].A;
                        ProcessingZone_B = BackupData_ProcessingZone[0].B;

                        listSectionData.Add(new SectionTypeProcessingData()
                        {
                            SectionCategoryType = "CH",
                            ProcessingBehavior = SelectProcessingBehavior,
                            A = ProcessingZone_A,
                            B = ProcessingZone_B,
                        });
                        ser.SetSectionTypeProcessingData(listSectionData);//加入到CH的斷面加工資料檔案
                        break;
                    default:
                        break;
                }

                GoBackButtonEnabled = false; //關閉復原按鈕

                WinUIMessageBox.Show(null,
                                     $"復原完成",
                                     "通知",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Exclamation,
                                     MessageBoxResult.None,
                                     MessageBoxOptions.None,
                                     FloatingMode.Popup);
            });
        }
        /// <summary>
        /// 加工區域 - 顯示加工區域圖片 20220810 張燕華
        /// </summary>
        public ICommand ShowProcessingZoneCommand { get; set; }
        private WPFBase.RelayParameterizedCommand ShowProcessingZone()
        {
            return new WPFBase.RelayParameterizedCommand((object cbxSelectedIndex) =>
            {
                GoBackButtonEnabled = false; //關閉復原按鈕
                SelectProcessingBehavior = (int)ProcessingBehavior.DRILLING; //預設開啟"鑽孔", 若當前SelectProcessingBehavior不是"鑽孔", 則會進入ShowProcessingSettingValue()
                VisibleH_Process = false;
                VisibleBOX_Process = false;
                VisibleCH_Process = false;
                
                switch (Convert.ToInt32(cbxSelectedIndex))
                {
                    case (int)SectionProcessing_SteelType.H:
                        VisibleH_Process = true;
                        //讀入加工區域設定值
                        STDSerialization serH = new STDSerialization(); //序列化處理器
                        ObservableCollection<SectionTypeProcessingData> sectionTypeProcessingData_H = serH.GetSectionTypeProcessingData("H", SelectProcessingBehavior);//讀取H_加工方式_processingzone.lis
                        SelectProcessingBehavior = sectionTypeProcessingData_H[0].ProcessingBehavior;
                        ProcessingZone_A = sectionTypeProcessingData_H[0].A;
                        ProcessingZone_B = sectionTypeProcessingData_H[0].B;
                        ProcessingZone_C = sectionTypeProcessingData_H[0].C;
                        break;
                    case (int)SectionProcessing_SteelType.BOX:
                        VisibleBOX_Process = true;
                        //讀入加工區域設定值
                        STDSerialization serBOX = new STDSerialization(); //序列化處理器
                        ObservableCollection<SectionTypeProcessingData> sectionTypeProcessingData_BOX = serBOX.GetSectionTypeProcessingData("BOX", SelectProcessingBehavior);//讀取BOX_加工方式_processingzone.lis
                        SelectProcessingBehavior = sectionTypeProcessingData_BOX[0].ProcessingBehavior;
                        ProcessingZone_A = sectionTypeProcessingData_BOX[0].A;
                        ProcessingZone_B = sectionTypeProcessingData_BOX[0].B;
                        break;
                    case (int)SectionProcessing_SteelType.CH:
                        VisibleCH_Process = true;
                        //讀入加工區域設定值
                        STDSerialization serCH = new STDSerialization(); //序列化處理器
                        ObservableCollection<SectionTypeProcessingData> sectionTypeProcessingData_CH = serCH.GetSectionTypeProcessingData("CH", SelectProcessingBehavior);//讀取CH_加工方式_processingzone.lis
                        SelectProcessingBehavior = sectionTypeProcessingData_CH[0].ProcessingBehavior;
                        ProcessingZone_A = sectionTypeProcessingData_CH[0].A;
                        ProcessingZone_B = sectionTypeProcessingData_CH[0].B;
                        break;
                    default:
                        break;
                }

                if(SelectSectionType != -1)//若已選擇型鋼型態, 則開啟修改和復原按鈕
                {
                    NewProcessingZoneButtonEnabled = true;
                    AllSelectedToggleButtonEnabled = true;
                    ModifyButtonEnabled = true;
                    //GoBackButtonEnabled = true;
                }
            });
        }
        /// <summary>
        /// 加工區域 - 因加工方式變動來顯示加工區域數值 20220811 張燕華
        /// </summary>
        public ICommand ShowProcessingSettingValueCommand { get; set; }
        private WPFBase.RelayParameterizedCommand ShowProcessingSettingValue()
        {
            return new WPFBase.RelayParameterizedCommand((object cbxSelectedIndex_ProcessingBehavior) =>
            {
                GoBackButtonEnabled = false; //關閉復原按鈕

                if ((VisibleH_Process || VisibleBOX_Process || VisibleCH_Process))//若前次已有選擇型鋼型態的話
                {
                    switch (SelectSectionType)
                    {
                        case (int)SectionProcessing_SteelType.H:
                            //VisibleH_Process = true;
                            //讀入加工區域設定值
                            STDSerialization serH_PSV = new STDSerialization(); //序列化處理器
                            ObservableCollection<SectionTypeProcessingData> sectionTypeProcessingData_H_PSV = serH_PSV.GetSectionTypeProcessingData("H", Convert.ToInt32(cbxSelectedIndex_ProcessingBehavior));//讀取H_加工方式_processingzone.lis
                            SelectProcessingBehavior = sectionTypeProcessingData_H_PSV[0].ProcessingBehavior;
                            ProcessingZone_A = sectionTypeProcessingData_H_PSV[0].A;
                            ProcessingZone_B = sectionTypeProcessingData_H_PSV[0].B;
                            ProcessingZone_C = sectionTypeProcessingData_H_PSV[0].C;
                            break;
                        case (int)SectionProcessing_SteelType.BOX:
                            //VisibleBOX_Process = true;
                            //讀入加工區域設定值
                            STDSerialization serBOX_PSV = new STDSerialization(); //序列化處理器
                            ObservableCollection<SectionTypeProcessingData> sectionTypeProcessingData_BOX_PSV = serBOX_PSV.GetSectionTypeProcessingData("BOX", Convert.ToInt32(cbxSelectedIndex_ProcessingBehavior));//讀取BOX_加工方式_processingzone.lis
                            SelectProcessingBehavior = sectionTypeProcessingData_BOX_PSV[0].ProcessingBehavior;
                            ProcessingZone_A = sectionTypeProcessingData_BOX_PSV[0].A;
                            ProcessingZone_B = sectionTypeProcessingData_BOX_PSV[0].B;
                            break;
                        case (int)SectionProcessing_SteelType.CH:
                            //VisibleCH_Process = true;
                            //讀入加工區域設定值
                            STDSerialization serCH_PSV = new STDSerialization(); //序列化處理器
                            ObservableCollection<SectionTypeProcessingData> sectionTypeProcessingData_CH_PSV = serCH_PSV.GetSectionTypeProcessingData("CH", Convert.ToInt32(cbxSelectedIndex_ProcessingBehavior));//讀取CH_加工方式_processingzone.lis
                            SelectProcessingBehavior = sectionTypeProcessingData_CH_PSV[0].ProcessingBehavior;
                            ProcessingZone_A = sectionTypeProcessingData_CH_PSV[0].A;
                            ProcessingZone_B = sectionTypeProcessingData_CH_PSV[0].B;
                            break;
                        default:
                            break;
                    }
                }
                else if(Convert.ToInt32(cbxSelectedIndex_ProcessingBehavior) != 0)//若前次已有選擇型鋼型態的話, 接著判斷若加工方式不是第一個(鑽孔)的話
                {
                    if(SelectSectionType == -1)//若型鋼型態未選擇
                    {
                        WinUIMessageBox.Show(null,
                                             $"請選擇型鋼型態",
                                             "通知",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation,
                                             MessageBoxResult.None,
                                             MessageBoxOptions.None,
                                             FloatingMode.Popup);
                    }
                    else
                    {
                        //do nothing
                        //待此command method結束後回到ShowProcessingZone()繼續
                    }
                }
                else//若未選擇型鋼型態且加工方式為選單第一個
                {
                    WinUIMessageBox.Show(null,
                                         $"請選擇型鋼型態",
                                         "通知",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Exclamation,
                                         MessageBoxResult.None,
                                         MessageBoxOptions.None,
                                         FloatingMode.Popup);
                }
            });
        }
        /// <summary>
        /// 切割線設定 - 切割幾等分 20220823 張燕華
        /// </summary>
        public ICommand ShowHowManyPartsRelatedComboboxCommand { get; set; }
        private WPFBase.RelayParameterizedCommand ShowHowManyPartsRelatedCombobox()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                //若設定值檔案存在, 載入所有combobox的item source
                SplitLineSettingClass SplitLineComboBox = new SplitLineSettingClass();
                cbb_A_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_A, e.ToString());
                cbb_B_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_B, e.ToString());
                cbb_C_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_C, e.ToString());
                cbb_D_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_D, e.ToString());
            });
        }
        /// <summary>
        /// 切割線設定 - 新增設定數值 20220816 張燕華
        /// </summary>
        public ICommand NewSplitLineCommand { get; set; }
        private WPFBase.RelayCommand NewSplitLine()
        {
            return new WPFBase.RelayCommand(() =>
            {
                //若型鋼型態&加工行為&數值checkbox皆無勾選, 需提示錯誤
                if (chb_PointA_SplitLineSetting && chb_PointB_SplitLineSetting && chb_PointC_SplitLineSetting && chb_PointD_SplitLineSetting && chb_Thickness_SplitLineSetting && chb_RemainingLength_SplitLineSetting == true)
                {
                    //儲存使用者設定的資料到檔案
                    STDSerialization ser = new STDSerialization(); //序列化處理器
                    ObservableCollection<SplitLineSettingClass> listSplitLineData = new ObservableCollection<SplitLineSettingClass>();
                    listSplitLineData.Add(new SplitLineSettingClass()
                    {
                        HowManyParts = HowManyParts_Value,
                        A = PointA_Value,
                        B = PointB_Value,
                        C = PointC_Value,
                        D = PointD_Value,
                        Thickness = CutThickness,
                        RemainingLength = SplitRemainingLength
                    });
                    ser.SetSplitLineData(listSplitLineData);

                    WinUIMessageBox.Show(null,
                                         $"新建完成",
                                         "通知",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Exclamation,
                                         MessageBoxResult.None,
                                         MessageBoxOptions.None,
                                         FloatingMode.Popup);

                    //儲存使用者到目前為止使用系統時所有新增的切割線資料
                    BackupSplitLineSettingData.Add(new SplitLineSettingClass()
                    {
                        HowManyParts = HowManyParts_Value,
                        A = PointA_Value,
                        B = PointB_Value,
                        C = PointC_Value,
                        D = PointD_Value,
                        Thickness = CutThickness,
                        RemainingLength = SplitRemainingLength
                    });
                    //設定當前復原設定值陣列最後一個元素的index
                    CurrentIndex_BackupSplitLineSettingData = BackupSplitLineSettingData.Count - 1;

                    //新增動作完畢後取消所有checkbox勾選
                    chb_PointA_SplitLineSetting = false;
                    chb_PointB_SplitLineSetting = false;
                    chb_PointC_SplitLineSetting = false;
                    chb_PointD_SplitLineSetting = false;
                    chb_Thickness_SplitLineSetting = false;
                    chb_RemainingLength_SplitLineSetting = false;
                }
                else
                {
                    WinUIMessageBox.Show(null,
                                         $"請確認各選項並勾選",
                                         "通知",
                                         MessageBoxButton.OK,
                                         MessageBoxImage.Exclamation,
                                         MessageBoxResult.None,
                                         MessageBoxOptions.None,
                                         FloatingMode.Popup);
                }
            });
        }
        /// <summary>
        /// 切割線設定 - CheckBox全選按鈕 20220816 張燕華
        /// </summary>
        public ICommand ToggleAllSplitLineCheckboxCommand { get; set; }
        private WPFBase.RelayCommand ToggleAllSplitLineCheckbox()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (chb_PointA_SplitLineSetting && chb_PointB_SplitLineSetting && chb_PointC_SplitLineSetting && chb_PointD_SplitLineSetting && chb_Thickness_SplitLineSetting && chb_RemainingLength_SplitLineSetting == true)
                {
                    chb_PointA_SplitLineSetting = false;
                    chb_PointB_SplitLineSetting = false;
                    chb_PointC_SplitLineSetting = false;
                    chb_PointD_SplitLineSetting = false;
                    chb_Thickness_SplitLineSetting = false;
                    chb_RemainingLength_SplitLineSetting = false;
                }
                else
                {
                    chb_PointA_SplitLineSetting = true;
                    chb_PointB_SplitLineSetting = true;
                    chb_PointC_SplitLineSetting = true;
                    chb_PointD_SplitLineSetting = true;
                    chb_Thickness_SplitLineSetting = true;
                    chb_RemainingLength_SplitLineSetting = true;
                }
            });
        }
        /// <summary>
        /// 切割線設定 - 復原按鈕 20220817 張燕華
        /// </summary>
        public ICommand GoBackSplitLineCommand { get; set; }
        private WPFBase.RelayCommand GoBackSplitLine()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (BackupSplitLineSettingData.Count <= 1) //若復原陣列裡只有最初從檔案載入的值
                {
                    WinUIMessageBox.Show(null,
                                     $"沒有可供復原的設定值",
                                     "通知",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Exclamation,
                                     MessageBoxResult.None,
                                     MessageBoxOptions.None,
                                     FloatingMode.Popup);
                }
                else
                {
                    if (CurrentIndex_BackupSplitLineSettingData == 0) //已提示過已是最初檔案值,若仍按復原的話則提示
                    {
                        WinUIMessageBox.Show(null,
                                             $"已是最初載入的設定值，可按新增來儲存本次的設定值。",
                                             "通知",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Exclamation,
                                             MessageBoxResult.None,
                                             MessageBoxOptions.None,
                                             FloatingMode.Popup);
                    }
                    else
                    {
                        HowManyParts_Value = PointA_Value = BackupSplitLineSettingData[CurrentIndex_BackupSplitLineSettingData - 1].HowManyParts;

                        SplitLineSettingClass SplitLineComboBox = new SplitLineSettingClass();
                        cbb_A_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_A, HowManyParts_Value);
                        cbb_B_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_B, HowManyParts_Value);
                        cbb_C_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_C, HowManyParts_Value);
                        cbb_D_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_D, HowManyParts_Value);

                        PointA_Value = BackupSplitLineSettingData[CurrentIndex_BackupSplitLineSettingData - 1].A;
                        PointB_Value = BackupSplitLineSettingData[CurrentIndex_BackupSplitLineSettingData - 1].B;
                        PointC_Value = BackupSplitLineSettingData[CurrentIndex_BackupSplitLineSettingData - 1].C;
                        PointD_Value = BackupSplitLineSettingData[CurrentIndex_BackupSplitLineSettingData - 1].D;
                        CutThickness = BackupSplitLineSettingData[CurrentIndex_BackupSplitLineSettingData - 1].Thickness;
                        SplitRemainingLength = BackupSplitLineSettingData[CurrentIndex_BackupSplitLineSettingData - 1].RemainingLength;

                        if (CurrentIndex_BackupSplitLineSettingData - 1 >= 1) //尚有值可以還原
                        {
                            CurrentIndex_BackupSplitLineSettingData = CurrentIndex_BackupSplitLineSettingData - 1;

                            WinUIMessageBox.Show(null,
                                                 $"復原完成，請按新增來儲存本次的設定值。",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);
                        }
                        else //提示已復原到最初從檔案載入的值
                        {
                            CurrentIndex_BackupSplitLineSettingData = 0;

                            WinUIMessageBox.Show(null,
                                                 $"已復原至最初載入的設定值，可按新增來儲存本次的設定值。",
                                                 "通知",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Exclamation,
                                                 MessageBoxResult.None,
                                                 MessageBoxOptions.None,
                                                 FloatingMode.Popup);
                        }
                    }
                    
                }
            });
        }
        /// <summary>
        /// 軟體版本 - 安裝 20220819 張燕華
        /// </summary>
        public ICommand SoftwareVersionInstallCommand { get; set; }
        private WPFBase.RelayCommand SoftwareVersionInstall()
        {
            return new WPFBase.RelayCommand(() =>
            {
                WinUIMessageBox.Show(null,
                                     $"已是最新版本",
                                     "通知",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Exclamation,
                                     MessageBoxResult.None,
                                     MessageBoxOptions.None,
                                     FloatingMode.Popup);
            });
        }
        /// <summary>
        /// 報表LOGO 20221006 張燕華
        /// </summary>
        public ICommand ImportLogoCommand { get; set; }
        private WPFWindowsBase.RelayCommand ImportLogo()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                DXOpenFileDialog dX = new DXOpenFileDialog();
                dX.Filter = "Image Files|*.PNG;*.JPG;*.BMP";
                dX.ShowDialog();//Show 視窗
                LogoPath = dX.FileName;
                if (LogoPath != String.Empty) IsLogoPathSelected = true;
            });
        }
        /// <summary>
        /// 複製儲存已選擇的報表LOGO 20221006 張燕華
        /// </summary>
        public ICommand CopyAndSaveLogoCommand { get; set; }
        private WPFWindowsBase.RelayCommand CopyAndSaveLogo()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                string dirPath = ApplicationVM.FileReportLogo();
                if (Directory.Exists(dirPath))
                {
                    Console.WriteLine("The directory {0} already exists.", dirPath);
                }
                else
                {
                    Directory.CreateDirectory(dirPath);
                    Console.WriteLine("The directory {0} was created.", dirPath);
                }

                File.Copy(LogoPath, ApplicationVM.FileReportLogo()+@"\ReportLogo.png", true);//複製報表LOG到專案資料夾

                WinUIMessageBox.Show(null,
                    $"選擇完成",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
            });
        }
        /// <summary>
        /// 刪除刀庫內容
        /// </summary>
        public ICommand DeleteDrillBrandsCommand { get; set; }
        private WPFBase.RelayParameterizedCommand DeleteDrillBrands()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                DrillBrand drillBrand = (DrillBrand)e;
                GD_STD.DrillWarehouse drillWarehouse = ReadCodesysMemor.GetDrillWarehouse();
                var _ = drillWarehouse.Middle
                                      .Union(drillWarehouse.LeftEntrance)
                                      .Union(drillWarehouse.LeftExport)
                                      .Union(drillWarehouse.RightEntrance)
                                      .Union(drillWarehouse.RightExport)
                                      .Where(el => System.Text.Encoding.ASCII.GetString(el.GUID) == drillBrand.Guid.ToString());
                if (_.Count() == 0)
                {
                    DrillBrands.Remove((DrillBrand)e);
                }
                else
                {
                    //MessageBox.Show("刀具設定正在使用中請勿刪除。", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    WinUIMessageBox.Show(null,
                    $"刀具設定正在使用中請勿刪除",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                }
            });
        }
        /// <summary>
        /// 新增刀具設定的列表命令
        /// </summary>
        public ICommand NewDrillBrandsCommand { get; set; }
        private WPFBase.RelayCommand NewDrillBrands()
        {
            return new WPFBase.RelayCommand(() =>
            {
                int number = 1;
                while (true)
                {
                    string dataName = $"新設定檔 {number}";//設定檔案的名稱
                    int index = DrillBrands.IndexOf(el => el.DataName == dataName); //查詢相同名稱
                    if (index == -1)//找不到名稱
                    {
                        DrillBrands.Add(new DrillBrand { DataName = dataName, Guid = Guid.NewGuid() });//加入新的設定檔
                        return;
                    }
                    else
                    {
                        number++;
                    }
                }
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
                STDSerialization ser = new STDSerialization();
                DrillBrands save = new DrillBrands(DrillBrands);
                save.Insert(0, DrillBrand.GetNull());
                ser.SetDrillBrands(save);
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
        /// 20220728 張燕華 轉換出inp檔案->RH 要顯示集合列表
        /// </summary>
        public ObservableCollection<SteelAttr> RH_old_system { get; set; } = new ObservableCollection<SteelAttr>();
        public ObservableCollection<SteelAttr> RH_new_system { get; set; } = new ObservableCollection<SteelAttr>();

        /// <summary>
        /// 20221108 張燕華 新增斷面規格到系統的中間暫存資料
        /// </summary>
        private SteelAttr steelAttr_system = new SteelAttr();

        /// <summary>
        /// 20220728 張燕華 轉換出inp檔案
        /// </summary>
        public ICommand TransformInpCommand { get; set; }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM.New()' 的 XML 註解
        public WPFBase.RelayParameterizedCommand TransformInp()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM.New()' 的 XML 註解
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                //Step1:讀取excel中各規格
                // 讀取TXT檔內文串
                StreamReader str = new StreamReader(@"轉換inp\斷面規格_表1.txt");
                /// <summary>
                /// 檔案中欄位名稱字串
                /// </summary>
                string sColumnName;
                /// <summary>
                /// 檔案中斷面規格的資料字串
                /// </summary>
                string sSteelAttrFromFile;
                /// <summary>
                /// 檔案中斷面規格的所有大類別, EX:BOX
                /// </summary>
                ArrayList alSteelAttrCategory = new ArrayList();
                /// <summary>
                /// 檔案中斷面規格的所有分類, EX:TUBE-->X
                /// </summary>
                ArrayList alSteelAttrType = new ArrayList();
                /// <summary>
                /// 檔案中斷面規格的所有大類別&所有分類
                /// </summary>
                ArrayList alSteelAttrCategoryType = new ArrayList();
                /// <summary>
                /// 檔案中斷面規格的資料
                /// </summary>
                ArrayList alSteelAttrFromFile = new ArrayList();
                /// <summary>
                /// 素材類型
                /// </summary>
                string steel_type = "";

                //讀取斷面規格檔案內容並儲存至變數
                sColumnName = str.ReadLine();
                string[] ColumnName = sColumnName.Split('\t', '\n');
                try
                {
                    sSteelAttrFromFile = str.ReadLine();

                    while (sSteelAttrFromFile != null)
                    {

                        string[] SteelAttrFromFile = sSteelAttrFromFile.Split('\t', '\n');
                        string section_name_arrow = "-->";

                        if (SteelAttrFromFile[1] == "")
                        {
                            if (SteelAttrFromFile[0].Contains(section_name_arrow))
                            {
                                alSteelAttrType.Add(SteelAttrFromFile[0]);
                                string[] ssteel_type = SteelAttrFromFile[0].Split('-');
                                steel_type = ssteel_type[0];
                            }
                            else
                            {
                                alSteelAttrCategory.Add(SteelAttrFromFile[0]);
                            }
                            alSteelAttrCategoryType.Add(SteelAttrFromFile[0]);
                        }
                        else
                        {
                            SteelAttrInExcel steel_attr = new SteelAttrInExcel();
                            if (steel_type == "TUBE" || steel_type == "BOX" || steel_type == "BH" || steel_type == "H" || steel_type == "RH" || steel_type == "I" || steel_type == "L" || steel_type == "[" || steel_type == "U(CH)" || steel_type == "BT" || steel_type == "CT" || steel_type == "T")
                            {
                                steel_attr.section_name = SteelAttrFromFile[0];
                                steel_attr.h = Convert.ToSingle(SteelAttrFromFile[1]);
                                steel_attr.b = Convert.ToSingle(SteelAttrFromFile[2]);
                                steel_attr.t1 = Convert.ToSingle(SteelAttrFromFile[3]);
                                steel_attr.t2 = Convert.ToSingle(SteelAttrFromFile[4]);
                                steel_attr.r1 = Convert.ToSingle(SteelAttrFromFile[5]);
                                steel_attr.r2 = Convert.ToSingle(SteelAttrFromFile[6]);
                                steel_attr.surface_area = Convert.ToSingle(SteelAttrFromFile[7]);
                                steel_attr.section_area = Convert.ToSingle(SteelAttrFromFile[8]);
                                steel_attr.weight_per_unit = Convert.ToSingle(SteelAttrFromFile[9]);
                                steel_attr.density = Convert.ToInt32(SteelAttrFromFile[10]);
                            }
                            else if (steel_type == "C")
                            {
                                steel_attr.section_name = SteelAttrFromFile[0];
                                steel_attr.h = Convert.ToSingle(SteelAttrFromFile[1]);
                                steel_attr.b = Convert.ToSingle(SteelAttrFromFile[2]);
                                steel_attr.e = Convert.ToSingle(SteelAttrFromFile[3]);
                                steel_attr.t2 = Convert.ToSingle(SteelAttrFromFile[4]);
                                steel_attr.r1 = Convert.ToSingle(SteelAttrFromFile[5]);
                                steel_attr.r2 = Convert.ToSingle(SteelAttrFromFile[6]);
                                steel_attr.surface_area = Convert.ToSingle(SteelAttrFromFile[7]);
                                steel_attr.section_area = Convert.ToSingle(SteelAttrFromFile[8]);
                                steel_attr.weight_per_unit = Convert.ToSingle(SteelAttrFromFile[9]);
                                steel_attr.density = Convert.ToInt32(SteelAttrFromFile[10]);
                            }
                            else if (steel_type == "PIPE")
                            {
                                steel_attr.section_name = SteelAttrFromFile[0];
                                steel_attr.diameter = Convert.ToSingle(SteelAttrFromFile[1]);
                                steel_attr.b = Convert.ToSingle(SteelAttrFromFile[2]);
                                steel_attr.t1 = Convert.ToSingle(SteelAttrFromFile[3]);
                                steel_attr.t2 = Convert.ToSingle(SteelAttrFromFile[4]);
                                steel_attr.r1 = Convert.ToSingle(SteelAttrFromFile[5]);
                                steel_attr.r2 = Convert.ToSingle(SteelAttrFromFile[6]);
                                steel_attr.surface_area = Convert.ToSingle(SteelAttrFromFile[7]);
                                steel_attr.section_area = Convert.ToSingle(SteelAttrFromFile[8]);
                                steel_attr.weight_per_unit = Convert.ToSingle(SteelAttrFromFile[9]);
                                steel_attr.density = Convert.ToInt32(SteelAttrFromFile[10]);
                            }
                            else if (steel_type == "TURN BUCKLE")
                            {
                                steel_attr.section_name = SteelAttrFromFile[0];
                                steel_attr.h = Convert.ToSingle(SteelAttrFromFile[1]);
                                steel_attr.b = Convert.ToSingle(SteelAttrFromFile[2]);
                            }
                            else if (steel_type == "WELD")
                            {
                                steel_attr.section_name = SteelAttrFromFile[0];
                                steel_attr.diameter = Convert.ToSingle(SteelAttrFromFile[1]);
                                steel_attr.b = Convert.ToSingle(SteelAttrFromFile[2]);
                                steel_attr.t1 = Convert.ToSingle(SteelAttrFromFile[3]);
                                steel_attr.t2 = Convert.ToSingle(SteelAttrFromFile[4]);
                            }
                            else if (steel_type == "SA")
                            {
                                steel_attr.section_name = SteelAttrFromFile[0];
                                steel_attr.h = Convert.ToSingle(SteelAttrFromFile[1]);
                                steel_attr.b = Convert.ToSingle(SteelAttrFromFile[2]);
                                steel_attr.t1 = Convert.ToSingle(SteelAttrFromFile[3]);
                                steel_attr.t2 = Convert.ToSingle(SteelAttrFromFile[4]);
                            }
                            else if (steel_type == "格柵板踏階")
                            {
                                steel_attr.section_name = SteelAttrFromFile[0];
                                steel_attr.h = Convert.ToSingle(SteelAttrFromFile[1]);
                                steel_attr.b = Convert.ToSingle(SteelAttrFromFile[2]);
                            }
                            else if (steel_type == "FB")
                            {
                                steel_attr.section_name = SteelAttrFromFile[0];
                                steel_attr.h = Convert.ToSingle(SteelAttrFromFile[1]);
                                steel_attr.b = Convert.ToSingle(SteelAttrFromFile[2]);
                                steel_attr.t1 = Convert.ToSingle(SteelAttrFromFile[3]);
                                steel_attr.t2 = Convert.ToSingle(SteelAttrFromFile[4]);
                                steel_attr.surface_area = Convert.ToSingle(SteelAttrFromFile[7]);
                                steel_attr.section_area = Convert.ToSingle(SteelAttrFromFile[8]);
                                steel_attr.weight_per_unit = Convert.ToSingle(SteelAttrFromFile[9]);
                                steel_attr.density = Convert.ToInt32(SteelAttrFromFile[10]);
                            }
                            else if (steel_type == "RB")
                            {
                                steel_attr.section_name = SteelAttrFromFile[0];
                                steel_attr.diameter = Convert.ToSingle(SteelAttrFromFile[1]);
                                steel_attr.surface_area = Convert.ToSingle(SteelAttrFromFile[7]);
                                steel_attr.section_area = Convert.ToSingle(SteelAttrFromFile[8]);
                                steel_attr.weight_per_unit = Convert.ToSingle(SteelAttrFromFile[9]);
                                steel_attr.density = Convert.ToInt32(SteelAttrFromFile[10]);
                            }
                            else if (steel_type == "重型螺帽" || steel_type == "螺帽")
                            {
                                steel_attr.section_name = SteelAttrFromFile[0];
                                steel_attr.diameter = Convert.ToSingle(SteelAttrFromFile[1]);
                            }
                            else 
                            {
                                MessageBox.Show($"ERROR: 無定義的{steel_type}斷面規格類別!");
                                str.Close();
                                return;
                            }

                            alSteelAttrFromFile.Add(steel_attr);
                        }

                        sSteelAttrFromFile = str.ReadLine();
                    }
                    //close the file
                    str.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Exception: " + error.Message);
                }
                finally
                {
                    //MessageBox.Show("Executing finally block.");
                    if (steel_type == "")
                        MessageBox.Show("ERROR: 檔案內素材類型錯誤!建立的.inp無名稱!");
                }

                //Step2:讀取原inp檔(以RH.inp為範本)
                RH_old_system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"轉換inp\RH_template.inp");
                SerializationHelper.SerializeBinary(RH_old_system, $@"轉換inp\" + steel_type + ".inp");//變更系統內設定的斷面規格

                //Step3:刪除原inp檔中的規格資料(以RH.inp為範本)
                foreach (SteelAttr attr in RH_old_system)
                {
                    ObservableCollection<SteelAttr> system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"轉換inp\" + steel_type + ".inp");//系統內設定的斷面規格

                    system.Remove(el1 => el1.Profile == attr.Profile);
                    SerializationHelper.SerializeBinary(system, $@"轉換inp\" + steel_type + ".inp");//變更系統內設定的斷面規格
                }
                RH_new_system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"轉換inp\" + steel_type + ".inp");

                //Step4:將excel中規格新增至inp中
                //開始後續新增斷面規格到inp的動作, 仿照SaveAs()內容
                if (el.ToString() == "btn_toinp")
                    IsProfile = true;

                if (IsProfile)
                {
                    if (alSteelAttrType.Count == 1)
                    {
                        ObservableCollection<SteelAttr> system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"轉換inp\" + steel_type + ".inp");//已清空的系統斷面規格

                        foreach (SteelAttrInExcel SingleSectionSpec in alSteelAttrFromFile)
                        {
                            SteelAttr steelAttr_Office = new SteelAttr();

                            if(steel_type == "C")
                            {
                                steelAttr_Office.Type = OBJECT_TYPE.C;
                                steelAttr_Office.Profile = SingleSectionSpec.section_name;
                                steelAttr_Office.H = SingleSectionSpec.h;
                                steelAttr_Office.W = SingleSectionSpec.b;
                                steelAttr_Office.e = SingleSectionSpec.e;
                                steelAttr_Office.t2 = SingleSectionSpec.t2;
                                steelAttr_Office.r1 = SingleSectionSpec.r1;
                                steelAttr_Office.r2 = SingleSectionSpec.r2;
                                steelAttr_Office.surface_area = SingleSectionSpec.surface_area;
                                steelAttr_Office.section_area = SingleSectionSpec.section_area;
                                steelAttr_Office.Kg = SingleSectionSpec.weight_per_unit;
                                steelAttr_Office.density = SingleSectionSpec.density;
                            }
                            else if (steel_type == "PIPE")
                            {
                                steelAttr_Office.Type = OBJECT_TYPE.PIPE;
                                steelAttr_Office.Profile = SingleSectionSpec.section_name;
                                steelAttr_Office.diameter = SingleSectionSpec.diameter;
                                steelAttr_Office.W = SingleSectionSpec.b;
                                steelAttr_Office.t1 = SingleSectionSpec.t1;
                                steelAttr_Office.t2 = SingleSectionSpec.t2;
                                steelAttr_Office.r1 = SingleSectionSpec.r1;
                                steelAttr_Office.r2 = SingleSectionSpec.r2;
                                steelAttr_Office.surface_area = SingleSectionSpec.surface_area;
                                steelAttr_Office.section_area = SingleSectionSpec.section_area;
                                steelAttr_Office.Kg = SingleSectionSpec.weight_per_unit;
                                steelAttr_Office.density = SingleSectionSpec.density;
                            }
                            else if (steel_type == "TURN BUCKLE")
                            {
                                steelAttr_Office.Type = OBJECT_TYPE.TURN_BUCKLE;
                                steelAttr_Office.Profile = SingleSectionSpec.section_name;
                                steelAttr_Office.H = SingleSectionSpec.h;
                                steelAttr_Office.W = SingleSectionSpec.b;
                            }
                            else if (steel_type == "WELD")
                            {
                                steelAttr_Office.Type = OBJECT_TYPE.WELD;
                                steelAttr_Office.Profile = SingleSectionSpec.section_name;
                                steelAttr_Office.diameter = SingleSectionSpec.diameter;
                                steelAttr_Office.W = SingleSectionSpec.b;
                                steelAttr_Office.t1 = SingleSectionSpec.t1;
                                steelAttr_Office.t2 = SingleSectionSpec.t2;
                            }
                            else if (steel_type == "SA")
                            {
                                steelAttr_Office.Type = OBJECT_TYPE.SA;
                                steelAttr_Office.Profile = SingleSectionSpec.section_name;
                                steelAttr_Office.H = SingleSectionSpec.h;
                                steelAttr_Office.W = SingleSectionSpec.b;
                                steelAttr_Office.t1 = SingleSectionSpec.t1;
                                steelAttr_Office.t2 = SingleSectionSpec.t2;
                            }
                            else if (steel_type == "格柵板踏階")
                            {
                                steelAttr_Office.Type = OBJECT_TYPE.GRATING;
                                steelAttr_Office.Profile = SingleSectionSpec.section_name;
                                steelAttr_Office.H = SingleSectionSpec.h;
                                steelAttr_Office.W = SingleSectionSpec.b;
                            }
                            else if (steel_type == "FB")
                            {
                                steelAttr_Office.Type = OBJECT_TYPE.FB;
                                steelAttr_Office.Profile = SingleSectionSpec.section_name;
                                steelAttr_Office.H = SingleSectionSpec.h;
                                steelAttr_Office.W = SingleSectionSpec.b;
                                steelAttr_Office.t1 = SingleSectionSpec.t1;
                                steelAttr_Office.t2 = SingleSectionSpec.t2;
                                steelAttr_Office.surface_area = SingleSectionSpec.surface_area;
                                steelAttr_Office.section_area = SingleSectionSpec.section_area;
                                steelAttr_Office.Kg = SingleSectionSpec.weight_per_unit;
                                steelAttr_Office.density = SingleSectionSpec.density;
                            }
                            else if (steel_type == "RB")
                            {
                                steelAttr_Office.Type = OBJECT_TYPE.RB;
                                steelAttr_Office.Profile = SingleSectionSpec.section_name;
                                steelAttr_Office.diameter = SingleSectionSpec.diameter;
                                steelAttr_Office.surface_area = SingleSectionSpec.surface_area;
                                steelAttr_Office.section_area = SingleSectionSpec.section_area;
                                steelAttr_Office.Kg = SingleSectionSpec.weight_per_unit;
                                steelAttr_Office.density = SingleSectionSpec.density;
                            }
                            else if (steel_type == "重型螺帽" || steel_type == "螺帽")
                            {
                                switch (steel_type)
                                {
                                    case "重型螺帽":
                                        steelAttr_Office.Type = OBJECT_TYPE.HNUT;
                                        break;
                                    case "螺帽":
                                        steelAttr_Office.Type = OBJECT_TYPE.NUT;
                                        break;default:
                                        throw new Exception("無法辨認斷面規格類型");
                                }
                                steelAttr_Office.Profile = SingleSectionSpec.section_name;
                                steelAttr_Office.diameter = SingleSectionSpec.diameter;
                            }
                            else
                            {
                                switch (steel_type)
                                {
                                    case "TUBE":
                                        steelAttr_Office.Type = OBJECT_TYPE.TUBE;
                                        break;
                                    case "BOX":
                                        steelAttr_Office.Type = OBJECT_TYPE.BOX;
                                        break;
                                    case "BH":
                                        steelAttr_Office.Type = OBJECT_TYPE.BH;
                                        break;
                                    case "H":
                                        steelAttr_Office.Type = OBJECT_TYPE.H;
                                        break;
                                    case "RH":
                                        steelAttr_Office.Type = OBJECT_TYPE.RH;
                                        break;
                                    case "I":
                                        steelAttr_Office.Type = OBJECT_TYPE.I;
                                        break;
                                    case "L":
                                        steelAttr_Office.Type = OBJECT_TYPE.L;
                                        break;
                                    case "[":
                                        steelAttr_Office.Type = OBJECT_TYPE.LB;
                                        break;
                                    case "U(CH)":
                                        steelAttr_Office.Type = OBJECT_TYPE.CH;
                                        break;
                                    case "BT":
                                        steelAttr_Office.Type = OBJECT_TYPE.BT;
                                        break;
                                    case "CT":
                                        steelAttr_Office.Type = OBJECT_TYPE.CT;
                                        break;
                                    case "T":
                                        steelAttr_Office.Type = OBJECT_TYPE.T;
                                        break;
                                    default:
                                        MessageBox.Show("ERROR: 無定義的斷面規格類別!");
                                        throw new Exception("無法辨認斷面規格類型");
                                }
                                steelAttr_Office.Profile = SingleSectionSpec.section_name;
                                steelAttr_Office.H = SingleSectionSpec.h;
                                steelAttr_Office.W = SingleSectionSpec.b;
                                steelAttr_Office.t1 = SingleSectionSpec.t1;
                                steelAttr_Office.t2 = SingleSectionSpec.t2;
                                steelAttr_Office.r1 = SingleSectionSpec.r1;
                                steelAttr_Office.r2 = SingleSectionSpec.r2;
                                steelAttr_Office.surface_area = SingleSectionSpec.surface_area;
                                steelAttr_Office.section_area = SingleSectionSpec.section_area;
                                steelAttr_Office.Kg = SingleSectionSpec.weight_per_unit;
                                steelAttr_Office.density = SingleSectionSpec.density;
                            }

                            system.Add(steelAttr_Office);//加入到系統
                        }
                        SerializationHelper.SerializeBinary(system, $@"轉換inp\" + steel_type + ".inp"); //變更系統斷面規格檔案
                        ObservableCollection<SteelAttr> RH_new_system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"轉換inp\" + steel_type + ".inp");//新的系統斷面規格
                        MessageBox.Show("通知: "+ steel_type + ".inp已產生!");
                    }
                }
            });
        }
        /// <summary>
        /// 新增材質
        /// </summary>
        public ICommand NewCommand { get; set; }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM.New()' 的 XML 註解
        public WPFBase.RelayParameterizedCommand New()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM.New()' 的 XML 註解
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                if (el.ToString() == "btn_matrial")
                    IsProfile = false;
                else
                {
                    IsProfile = true;
                }
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
                    //MessageBox.Show($"已有存在相同名稱", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    WinUIMessageBox.Show(null,
                    $"已有存在相同名稱",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
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
                UpDate();
            });
        }

        ///// <summary>
        ///// 變更 <see cref="UseSelected"/>
        ///// </summary>
        //public ICommand UseSaveCommand { get; set; }
        //private WPFBase.RelayCommand UseSave()
        //{
        //    return new WPFBase.RelayCommand(() =>
        //    {
        //        if (UseSelected.Count > 0)
        //        {
        //            //_drillSetting _dr = UseSelected[UseSelected.Count - 1];
        //            //DrillSetting _ = _dr.DrillSetting;
        //            //_.IsCurrent = true;
        //            //int index = _.Index - 1; //刀庫位置
        //            //switch (DRILL_POSITION)
        //            //{
        //            //    case DRILL_POSITION.EXPORT_L:
        //            //        _DrillWarehouse.LeftExport[index] = _;
        //            //        break;
        //            //    case DRILL_POSITION.EXPORT_R:
        //            //        _DrillWarehouse.RightExport[index] = _;
        //            //        break;
        //            //    case DRILL_POSITION.MIDDLE:
        //            //        _DrillWarehouse.Middle[index] = _;
        //            //        break;
        //            //    case DRILL_POSITION.ENTRANCE_L:
        //            //        _DrillWarehouse.LeftEntrance[index] = _;
        //            //        break;
        //            //    case DRILL_POSITION.ENTRANCE_R:
        //            //        _DrillWarehouse.RightEntrance[index] = _;
        //            //        break;
        //            //    default:
        //            //        Debugger.Break();
        //            //        break;
        //            //}
        //            //Default.DrillWarehouse = _DrillWarehouse;
        //            //Default.Save();
        //            //WriteCodesysMemor.SetDrillWarehouse(_DrillWarehouse);//寫入記憶體
        //            //UpDateDrill();
        //        }
        //    });
        //}


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
        /// 另存模型Or系統
        /// </summary>
        public ICommand SaveModelRoSystemCommand { get; set; }
        /// <summary>
        /// 斷面規格存取系統或模型的命令
        /// </summary>
        public ICommand ProfileSaveModelRoSystemCommand { get; set; }
        /// <summary>
        ///  斷面規格存取系統或模型的命令
        /// </summary>
        public ICommand MaterialSaveModelRoSystemCommand{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayParameterizedCommand SaveAs()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                if (IsProfile)
                {
                    string str = el.ToString();
                    string strType = ((OBJECT_TYPE)_SelectType).ToString();
                    PropertyInfo showPropertyInfo = this.GetType().GetProperty(strType);//反射在畫面顯示的資料列表
                    PropertyInfo backPropertyInfo = this.GetType().GetProperty($@"_{strType}", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);//反射在背景的資料列表欄位
                    object getValue = (ObservableCollection<SteelAttr>)backPropertyInfo.GetValue(this); //反射在背景的資料列表
                    ObservableCollection<SteelAttr> value = _Save((ObservableCollection<SteelAttr>)getValue);//加入設定好的值
                    backPropertyInfo.SetValue(this, value); //存取資料到背景列表
                    showPropertyInfo.SetValue(this, SerializationHelper.Clone(value));//存取資料到顯示畫面的列表

                    if (str == "模型&系統")//如果確定要變更系統
                    {
                        //ObservableCollection<SteelAttr> system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\{strType}.inp");//系統的斷面規格
                        ObservableCollection<SteelAttr> system = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>(System.AppDomain.CurrentDomain.BaseDirectory + $@"Profile\{strType}.inp");//系統的斷面規格(在輸出目錄中)
                        //SteelAttr steelAttr = GetSettingSteelAttr(); //取得設定好的斷面規格
                        if (system.IndexOf(e => e.Profile == steelAttr_system.Profile) != -1) //如果有相同的斷面規格
                        {
                            system.Remove(e => e.Profile == steelAttr_system.Profile);//刪除
                        }
                        system.Add(steelAttr_system);//加入到系統
                        //SerializationHelper.SerializeBinary(system, $@"{ModelPath.Profile}\{strType}.inp"); //變更系統斷面規格檔案
                        SerializationHelper.SerializeBinary(system, System.AppDomain.CurrentDomain.BaseDirectory + $@"Profile\{strType}.inp"); //變更系統斷面規格檔案
                    }
                    SerializationHelper.SerializeBinary(value, $@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\{strType}.inp");//變更模型設定的斷面規格
                }
                else
                {
                    if (Materials.IndexOf(e => e.Name == SelectMaterial.Name) == -1) //判斷材質是否重複
                    {
                        string str = el.ToString();//選擇的選項 系統 or 模型
                        STDSerialization std = new STDSerialization();//序列化器
                        _Materials.Add(SerializationHelper.Clone<SteelMaterial>(SelectMaterial));//加入到背景
                        Materials = SerializationHelper.Clone(_Materials);//加入到顯示畫面
                        std.SetMaterial(Materials);//存入在模型設定檔
                        if (str == "系統")//如果要變更系統材質
                        {
                            ObservableCollection<SteelMaterial> system = std.GetMaterial(ModelPath.Material); //系統內設定的材質
                            if (system.IndexOf(e => e.Name == SelectMaterial.Name) == -1) //如果系統內沒有這材質
                            {
                                system.Add(SelectMaterial); //加入到系統內的材質列表
                                std.SetMaterial(ModelPath.Material, system);//變更系統內設定的材質
                            }
                        }
                    }
                    else
                    {
                        //MessageBox.Show($"已有存在相同名稱", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                        WinUIMessageBox.Show(null,
                            $"已有存在相同名稱",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                            FloatingMode.Popup);
                    }
                }

                SaveAsWindowsControl = true;
            });
        }
        /// <summary>
        /// 存取斷面規格
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayParameterizedCommand SaveProfile()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                string str = el.ToString();
                string strType = ((OBJECT_TYPE)_SelectType).ToString();
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
                    //20220805 張燕華 新增斷面規格屬性
                    update.r1 = InsertionData[4].Value;
                    update.r2 = InsertionData[5].Value;
                    update.surface_area = InsertionData[6].Value;
                    update.section_area = InsertionData[7].Value;
                    update.Kg = InsertionData[8].Value;
                    update.density = InsertionData[9].Value;

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
                    string strType = ((OBJECT_TYPE)steelAttr.Type).ToString();
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
        private WPFBase.RelayParameterizedCommand FilterMaterial()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                if (!isDuplication)
                {
                    if (el.ToString() != "*")
                    {
                        Materials = new ObservableCollection<SteelMaterial>(_Materials.Where(e => e.Name.ToLower().Contains(el.ToString().ToLower())).ToList());
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
                        RH = new ObservableCollection<SteelAttr>(_RH.Where(el => el.Profile.ToLower().Contains(e.ToString().ToLower())).ToList());
                        BH = new ObservableCollection<SteelAttr>(_BH.Where(el => el.Profile.ToLower().Contains(e.ToString().ToLower())).ToList());
                        CH = new ObservableCollection<SteelAttr>(_CH.Where(el => el.Profile.ToLower().Contains(e.ToString().ToLower())).ToList());
                        BOX = new ObservableCollection<SteelAttr>(_BOX.Where(el => el.Profile.ToLower().Contains(e.ToString().ToLower())).ToList());
                        L = new ObservableCollection<SteelAttr>(_L.Where(el => el.Profile.ToLower().Contains(e.ToString().ToLower())).ToList());
                        //20220804 張燕華 新增斷面規格目錄
                        H = new ObservableCollection<SteelAttr>(_H.Where(el => el.Profile.ToLower().Contains(e.ToString().ToLower())).ToList());
                        TUBE = new ObservableCollection<SteelAttr>(_TUBE.Where(el => el.Profile.ToLower().Contains(e.ToString().ToLower())).ToList());
                        LB = new ObservableCollection<SteelAttr>(_LB.Where(el => el.Profile.ToLower().Contains(e.ToString().ToLower())).ToList());
                    }
                    else if (e.ToString() != "*" || e.ToString() != "")
                    {
                        RH = new ObservableCollection<SteelAttr>(_RH);
                        BH = new ObservableCollection<SteelAttr>(_BH);
                        CH = new ObservableCollection<SteelAttr>(_CH);
                        BOX = new ObservableCollection<SteelAttr>(_BOX);
                        L = new ObservableCollection<SteelAttr>(_L);
                        //20220804 張燕華 新增斷面規格目錄
                        H = new ObservableCollection<SteelAttr>(_H);
                        TUBE = new ObservableCollection<SteelAttr>(_TUBE);
                        LB = new ObservableCollection<SteelAttr>(_LB);
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
                    initializationInsertionData(steelAttr);//20220801 張燕華 根據不同素材來顯示其Property欄位
                }
                initializationInsertionData();//20220801 張燕華 因為不同素材具有不同規格屬性, 若無參數則預設為H型鋼規格屬性
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
        /// 油壓系統
        /// </summary>
        private OillSystem _OillSystem { get; set; }
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
        /// TUBE 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _TUBE { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// H 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _H { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// I 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _I { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// LB 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _LB { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// BT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _BT { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// CT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _CT { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// T 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _T { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// C 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _C { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// PIPE 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _PIPE { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// TURN BUCKLE 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _TURN_BUCKLE { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// WELD 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _WELD { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// SA 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _SA { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// 格柵板踏階GRATING 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _GRATING { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// FB 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _FB { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// RB 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _RB { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// 重型螺帽HNUT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _HNUT { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// 螺帽NUT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        private ObservableCollection<SteelAttr> _NUT { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// 材質讀取到的資料列表
        /// </summary>
        private ObservableCollection<SteelMaterial> _Materials { get; set; }
        private int _MatrialIndex = -1;
        #endregion


        #region 公開屬性
        ///// <summary>
        ///// 鑽頭品牌列表
        ///// </summary>
        //public ObservableCollection<DrillBrand> DrillBrands { get; set; }
        /// <summary>
        /// 判斷儲存的對象是斷面規格還是材質，False存材質、True存斷面規格
        /// </summary>
        public bool IsProfile { get; set; }
        /// <summary>
        /// 另存視窗控制
        /// </summary>
        public bool SaveAsWindowsControl { get; set; } = true;
        /// <summary>
        /// 刀庫設定檔
        /// </summary>
        public DrillBrands DrillBrands { get; set; }

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
        /// 加工區域 - 斷面規格的資料容器 張燕華
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SectionTypeProcessingData> BackupData_ProcessingZone { get; set; } = new ObservableCollection<SectionTypeProcessingData>();
        /// <summary>
        /// 斷面規格的資料容器 for DataGrid
        /// </summary>
        /// <returns></returns>
        public int CurrentIndex_BackupSplitLineSettingData { get; set; } = 0;
        /// <summary>
        /// 斷面規格的資料容器 for DataGrid
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SplitLineSettingClass> BackupSplitLineSettingData { get; set; } = new ObservableCollection<SplitLineSettingClass>();
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
        /// 顯示H型鋼加工區域
        /// </summary>
        public bool VisibleH_Process { get; set; } = false;
        /// <summary>
        /// 顯示方管加工區域
        /// </summary>
        public bool VisibleBOX_Process { get; set; } = false;
        /// <summary>
        /// 顯示槽鐵加工區域
        /// </summary>
        public bool VisibleCH_Process { get; set; } = false;
        /// <summary>
        /// 型鋼型態 for 型鋼加工區域設定
        /// </summary>
        public bool chb_SteelType { get; set; } = false;
        /// <summary>
        /// 加工方式 for 型鋼加工區域設定
        /// </summary>
        public bool chb_ProcessingBehavior { get; set; } = false;
        /// <summary>
        /// H型鋼A值 for 型鋼加工區域設定
        /// </summary>
        public bool chb_H_Avalue { get; set; } = false;
        /// <summary>
        /// H型鋼B值 for 型鋼加工區域設定
        /// </summary>
        public bool chb_H_Bvalue { get; set; } = false;
        /// <summary>
        /// H型鋼C值 for 型鋼加工區域設定
        /// </summary>
        public bool chb_H_Cvalue { get; set; } = false;
        /// <summary>
        /// 方管A值 for 型鋼加工區域設定
        /// </summary>
        public bool chb_BOX_Avalue { get; set; } = false;
        /// <summary>
        /// 方管B值 for 型鋼加工區域設定
        /// </summary>
        public bool chb_BOX_Bvalue { get; set; } = false;
        /// <summary>
        /// 槽鐵A值 for 型鋼加工區域設定
        /// </summary>
        public bool chb_CH_Avalue { get; set; } = false;
        /// <summary>
        /// 槽鐵B值 for 型鋼加工區域設定
        /// </summary>
        public bool chb_CH_Bvalue { get; set; } = false;
        /// <summary>
        /// 打點位置(A) for 切割線設定
        /// </summary>
        public bool chb_PointA_SplitLineSetting { get; set; } = false;
        /// <summary>
        /// 打點位置(B) for 切割線設定
        /// </summary>
        public bool chb_PointB_SplitLineSetting { get; set; } = false;
        /// <summary>
        /// 打點位置(C) for 切割線設定
        /// </summary>
        public bool chb_PointC_SplitLineSetting { get; set; } = false;
        /// <summary>
        /// 打點位置(D) for 切割線設定
        /// </summary>
        public bool chb_PointD_SplitLineSetting { get; set; } = false;
        /// <summary>
        /// 切割厚度 for 切割線設定
        /// </summary>
        public bool chb_Thickness_SplitLineSetting { get; set; } = false;
        /// <summary>
        /// 最小餘料長度 for 切割線設定
        /// </summary>
        public bool chb_RemainingLength_SplitLineSetting { get; set; } = false;
        /// <summary>
        /// 選擇記號等分 item source for 切割線設定
        /// </summary>
        public List<string> cbb_HowManyParts { get; set; } = new List<string>();
        /// <summary>
        /// combobox A值 item source for 切割線設定
        /// </summary>
        public List<string> cbb_A_ItemSource { get; set; } = new List<string>();
        /// <summary>
        /// combobox B值 item source for 切割線設定
        /// </summary>
        public List<string> cbb_B_ItemSource { get; set; } = new List<string>();
        /// <summary>
        /// combobox C值 item source for 切割線設定
        /// </summary>
        public List<string> cbb_C_ItemSource { get; set; } = new List<string>();
        /// <summary>
        /// combobox D值 item source for 切割線設定
        /// </summary>
        public List<string> cbb_D_ItemSource { get; set; } = new List<string>();
        /// <summary>
        /// 選擇記號等分
        /// </summary>
        public string HowManyParts_Value { get; set; }
        /// <summary>
        /// 打點位置(A值)
        /// </summary>
        public string PointA_Value { get; set; }
        /// <summary>
        /// 打點位置(B值)
        /// </summary>
        public string PointB_Value { get; set; }
        /// <summary>
        /// 打點位置(C值)
        /// </summary>
        public string PointC_Value { get; set; }
        /// <summary>
        /// 打點位置(D值)
        /// </summary>
        public string PointD_Value { get; set; }
        /// <summary>
        /// 切割厚度
        /// </summary>
        public int CutThickness { get; set; } = 0; 
        /// <summary>
        /// 最小餘料長度
        /// </summary>
        public double SplitRemainingLength { get; set; } = 500.00;
        /// <summary>
        /// 型鋼型態 - 型鋼加工區域設定
        /// </summary>
        public int SelectSectionType
        {
            get => _SelectSectionType;
            set
            {
                _SelectSectionType = value;
            }
        }
        /// <summary>
        /// 加工方式 - 型鋼加工區域設定
        /// </summary>
        public int SelectProcessingBehavior
        {
            get => _SelectProcessingBehavior;
            set
            {
                _SelectProcessingBehavior = value;
            }
        }
        /// <summary>
        /// 型鋼加工區域設定 - 加工區域A
        /// </summary>
        public int ProcessingZone_A { get; set; } = 0;
        /// <summary>
        /// 型鋼加工區域設定 - 加工區域B
        /// </summary>
        public int ProcessingZone_B { get; set; } = 0;
        /// <summary>
        /// 型鋼加工區域設定 - 加工區域C
        /// </summary>
        public int ProcessingZone_C { get; set; } = 0;
        /// <summary>
        /// 型鋼加工區域設定 - 新增按鈕開關
        /// </summary>
        public bool NewProcessingZoneButtonEnabled { get; set; } = false;
        /// <summary>
        /// 型鋼加工區域設定 - 全選按鈕開關
        /// </summary>
        public bool AllSelectedToggleButtonEnabled { get; set; } = false;
        /// <summary>
        /// 型鋼加工區域設定 - 修改按鈕開關
        /// </summary>
        public bool ModifyButtonEnabled { get; set; } = false;
        /// <summary>
        /// 型鋼加工區域設定 - 復原按鈕開關
        /// </summary>
        public bool GoBackButtonEnabled { get; set; } = false;
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
                switch ((OBJECT_TYPE)value)
                {
                    case OBJECT_TYPE.BH:
                    case OBJECT_TYPE.H://20220802 張燕華 新增斷面規格
                    case OBJECT_TYPE.RH:
                        VisibilityH = true;
                        break;
                    case OBJECT_TYPE.LB://20220802 張燕華 新增斷面規格
                    case OBJECT_TYPE.CH:
                        VisibilityCH = true;
                        break;
                    case OBJECT_TYPE.L:
                        VisibilityL = true;
                        break;
                    case OBJECT_TYPE.TUBE://20220802 張燕華 新增斷面規格
                    case OBJECT_TYPE.BOX:
                        VisibilityBOX = true;
                        break;
                    default:
                        break;
                }
                initializationInsertionData();//20220801 張燕華 因為不同素材具有不同規格屬性, 若無參數則預設為H型鋼規格屬性
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
        /// TUBE 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> TUBE { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// H 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> H { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// I 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> I { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// LB 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> LB { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// BT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> BT { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// CT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> CT { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// T 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> T { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// C 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> C { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// PIPE 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> PIPE { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// TURN BUCKLE 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> TURN_BUCKLE { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// WELD 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> WELD { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// SA 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> SA { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// 格柵板踏階GRATING 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> GRATING { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// FB 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> FB { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// RB 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> RB { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// 重型螺帽HNUT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> HNUT { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// 螺帽NUT 要顯示集合列表 - 20220729 張燕華
        /// </summary>
        public ObservableCollection<SteelAttr> NUT { get; set; } = new ObservableCollection<SteelAttr>();
        /// <summary>
        /// 材質要顯示的集合
        /// </summary>
        public ObservableCollection<SteelMaterial> Materials { get; set; } = new ObservableCollection<SteelMaterial>();
        /// <summary>
        /// 報表路徑
        /// </summary>
        public string LogoPath { get; set; } = string.Empty; 
        /// <summary>
        /// 報表路徑是否已選擇
        /// </summary>
        public bool IsLogoPathSelected { get; set; } = false; 
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
            data.Type = (OBJECT_TYPE)_SelectType;
            //data.Kg = SelectSteelAtte.Kg;
            data.H = InsertionData[0].Value;
            data.W = InsertionData[1].Value;
            data.t1 = InsertionData[2].Value;
            data.t2 = (OBJECT_TYPE)_SelectType == OBJECT_TYPE.L || (OBJECT_TYPE)_SelectType == OBJECT_TYPE.BOX ? InsertionData[2].Value : InsertionData[3].Value;
            data.Profile = ProfileName;
            //20220804 張燕華 新增斷規格目錄
            data.r1 = InsertionData[4].Value;
            data.r2 = InsertionData[5].Value;
            data.surface_area = InsertionData[6].Value;
            data.section_area = InsertionData[7].Value;
            data.Kg = InsertionData[8].Value;
            data.density = InsertionData[9].Value;
            //突出肢長度e(mm)
            //data.e
            //直徑d(mm)
            //data.diameter
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
            //20220729 張燕華 斷面規格目錄-增加斷面規格
            TUBE = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\TUBE.inp");
            H = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\H.inp");
            I = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\I.inp");
            LB = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\LB.inp");
            BT = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\BT.inp");
            CT = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\CT.inp");
            T = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\T.inp");
            C = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\C.inp");
            PIPE = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\PIPE.inp");
            TURN_BUCKLE = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\TURN_BUCKLE.inp");
            WELD = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\WELD.inp");
            SA = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\SA.inp");
            GRATING = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\GRATING.inp");
            FB = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\FB.inp");
            RB = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\RB.inp");
            HNUT = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\HNUT.inp");
            NUT = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryModel()}\{ModelPath.Profile}\NUT.inp");
            
            _Materials = ser.GetMaterial();
            Materials = ser.GetMaterial();

            _RH = new ObservableCollection<SteelAttr>(RH);
            _BH = new ObservableCollection<SteelAttr>(BH);
            _BOX = new ObservableCollection<SteelAttr>(BOX);
            _CH = new ObservableCollection<SteelAttr>(CH);
            _L = new ObservableCollection<SteelAttr>(L);
            //20220729 張燕華 斷面規格目錄-增加斷面規格
            _TUBE = new ObservableCollection<SteelAttr>(TUBE);
            _H = new ObservableCollection<SteelAttr>(H);
            _I = new ObservableCollection<SteelAttr>(I);
            _LB = new ObservableCollection<SteelAttr>(LB);
            _BT = new ObservableCollection<SteelAttr>(BT);
            _CT = new ObservableCollection<SteelAttr>(CT);
            _T = new ObservableCollection<SteelAttr>(T);
            _C = new ObservableCollection<SteelAttr>(C);
            _PIPE = new ObservableCollection<SteelAttr>(PIPE);
            _TURN_BUCKLE = new ObservableCollection<SteelAttr>(TURN_BUCKLE);
            _WELD = new ObservableCollection<SteelAttr>(WELD);
            _SA = new ObservableCollection<SteelAttr>(SA);
            _GRATING = new ObservableCollection<SteelAttr>(GRATING);
            _FB = new ObservableCollection<SteelAttr>(FB);
            _RB = new ObservableCollection<SteelAttr>(RB);
            _HNUT = new ObservableCollection<SteelAttr>(HNUT);
            _NUT = new ObservableCollection<SteelAttr>(NUT);
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
        /// 初始化 <see cref="InsertionData"/> 20220803 張燕華 若沒有參數的狀況, 以H型鋼為預設規格屬性
        /// </summary>
        private void initializationInsertionData()
        {
            InsertionData.Clear();

            InsertionData.Add(new DataGridData() { Property = "高度", Symbol = "H", Value = SelectSteelAtte.H, Unit = "mm" });
            InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
            InsertionData.Add(new DataGridData() { Property = "腹板厚度", Symbol = "s/t1", Value = SelectSteelAtte.t1, Unit = "mm" });
            InsertionData.Add(new DataGridData() { Property = "翼板厚度", Symbol = "t/t2", Value = SelectSteelAtte.t2, Unit = "mm" });
            InsertionData.Add(new DataGridData() { Property = "圓角半徑", Symbol = "r1", Value = SelectSteelAtte.r1, Unit = "mm" });
            InsertionData.Add(new DataGridData() { Property = "圓角半徑", Symbol = "r2", Value = SelectSteelAtte.r2, Unit = "mm" });
            InsertionData.Add(new DataGridData() { Property = "表面積", Symbol = "", Value = SelectSteelAtte.surface_area, Unit = "m2/m" });
            InsertionData.Add(new DataGridData() { Property = "斷面積", Symbol = "", Value = SelectSteelAtte.section_area, Unit = "m2" });
            InsertionData.Add(new DataGridData() { Property = "單位長度重量", Symbol = "", Value = SelectSteelAtte.Kg, Unit = "kg/m" });
            InsertionData.Add(new DataGridData() { Property = "密度", Symbol = "", Value = SelectSteelAtte.density, Unit = "kg/m3" });
        }

        /// <summary>
        /// 初始化 <see cref="InsertionData"/> 20220803 張燕華 若有參數的狀況, 則根據規格屬性顯示欄位
        /// </summary>
        private void initializationInsertionData(SteelAttr steelAttr)
        {
            InsertionData.Clear();

            //20220801 張燕華 根據素材類型來顯示規格屬性
            if (steelAttr.Type == OBJECT_TYPE.TUBE || steelAttr.Type == OBJECT_TYPE.BOX || steelAttr.Type == OBJECT_TYPE.BH || steelAttr.Type == OBJECT_TYPE.H || steelAttr.Type == OBJECT_TYPE.RH || steelAttr.Type == OBJECT_TYPE.I || steelAttr.Type == OBJECT_TYPE.L || steelAttr.Type == OBJECT_TYPE.LB || steelAttr.Type == OBJECT_TYPE.CH || steelAttr.Type == OBJECT_TYPE.BT || steelAttr.Type == OBJECT_TYPE.CT || steelAttr.Type == OBJECT_TYPE.T)
            {
                InsertionData.Add(new DataGridData() { Property = "高度", Symbol = "H", Value = SelectSteelAtte.H, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "腹板厚度", Symbol = "s/t1", Value = SelectSteelAtte.t1, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "翼板厚度", Symbol = "t/t2", Value = SelectSteelAtte.t2, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "圓角半徑", Symbol = "r1", Value = SelectSteelAtte.r1, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "圓角半徑", Symbol = "r2", Value = SelectSteelAtte.r2, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "表面積", Symbol = "", Value = SelectSteelAtte.surface_area, Unit = "m2/m" });
                InsertionData.Add(new DataGridData() { Property = "斷面積", Symbol = "", Value = SelectSteelAtte.section_area, Unit = "m2" });
                InsertionData.Add(new DataGridData() { Property = "單位長度重量", Symbol = "", Value = SelectSteelAtte.Kg, Unit = "kg/m" });
                InsertionData.Add(new DataGridData() { Property = "密度", Symbol = "", Value = SelectSteelAtte.density, Unit = "kg/m3" });
            }
            else if (steelAttr.Type == OBJECT_TYPE.C)
            {
                InsertionData.Add(new DataGridData() { Property = "高度", Symbol = "H", Value = SelectSteelAtte.H, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "突出肢長度", Symbol = "e", Value = SelectSteelAtte.e, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "翼板厚度", Symbol = "t/t2", Value = SelectSteelAtte.t2, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "圓角半徑", Symbol = "r1", Value = SelectSteelAtte.r1, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "圓角半徑", Symbol = "r2", Value = SelectSteelAtte.r2, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "表面積", Symbol = "", Value = SelectSteelAtte.surface_area, Unit = "m2/m" });
                InsertionData.Add(new DataGridData() { Property = "斷面積", Symbol = "", Value = SelectSteelAtte.section_area, Unit = "m2" });
                InsertionData.Add(new DataGridData() { Property = "單位長度重量", Symbol = "", Value = SelectSteelAtte.Kg, Unit = "kg/m" });
                InsertionData.Add(new DataGridData() { Property = "密度", Symbol = "", Value = SelectSteelAtte.density, Unit = "kg/m3" });
            }
            else if (steelAttr.Type == OBJECT_TYPE.PIPE)
            {
                InsertionData.Add(new DataGridData() { Property = "直徑", Symbol = "D", Value = SelectSteelAtte.diameter, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "腹板厚度", Symbol = "s/t1", Value = SelectSteelAtte.t1, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "翼板厚度", Symbol = "t/t2", Value = SelectSteelAtte.t2, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "圓角半徑", Symbol = "r1", Value = SelectSteelAtte.r1, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "圓角半徑", Symbol = "r2", Value = SelectSteelAtte.r2, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "表面積", Symbol = "", Value = SelectSteelAtte.surface_area, Unit = "m2/m" });
                InsertionData.Add(new DataGridData() { Property = "斷面積", Symbol = "", Value = SelectSteelAtte.section_area, Unit = "m2" });
                InsertionData.Add(new DataGridData() { Property = "單位長度重量", Symbol = "", Value = SelectSteelAtte.Kg, Unit = "kg/m" });
                InsertionData.Add(new DataGridData() { Property = "密度", Symbol = "", Value = SelectSteelAtte.density, Unit = "kg/m3" });
            }
            else if (steelAttr.Type == OBJECT_TYPE.TURN_BUCKLE)
            {
                InsertionData.Add(new DataGridData() { Property = "高度", Symbol = "H", Value = SelectSteelAtte.H, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
            }
            else if (steelAttr.Type == OBJECT_TYPE.WELD)
            {
                InsertionData.Add(new DataGridData() { Property = "直徑", Symbol = "D", Value = SelectSteelAtte.diameter, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "腹板厚度", Symbol = "s/t1", Value = SelectSteelAtte.t1, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "翼板厚度", Symbol = "t/t2", Value = SelectSteelAtte.t2, Unit = "mm" });
            }
            else if (steelAttr.Type == OBJECT_TYPE.SA)
            {
                InsertionData.Add(new DataGridData() { Property = "高度", Symbol = "H", Value = SelectSteelAtte.H, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "腹板厚度", Symbol = "s/t1", Value = SelectSteelAtte.t1, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "翼板厚度", Symbol = "t/t2", Value = SelectSteelAtte.t2, Unit = "mm" });
            }
            else if (steelAttr.Type == OBJECT_TYPE.GRATING)
            {
                InsertionData.Add(new DataGridData() { Property = "高度", Symbol = "H", Value = SelectSteelAtte.H, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
            }
            else if (steelAttr.Type == OBJECT_TYPE.FB)
            {
                InsertionData.Add(new DataGridData() { Property = "高度", Symbol = "H", Value = SelectSteelAtte.H, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "腹板厚度", Symbol = "s/t1", Value = SelectSteelAtte.t1, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "翼板厚度", Symbol = "t/t2", Value = SelectSteelAtte.t2, Unit = "mm" });
                
                InsertionData.Add(new DataGridData() { Property = "表面積", Symbol = "", Value = SelectSteelAtte.surface_area, Unit = "m2/m" });
                InsertionData.Add(new DataGridData() { Property = "斷面積", Symbol = "", Value = SelectSteelAtte.section_area, Unit = "m2" });
                InsertionData.Add(new DataGridData() { Property = "單位長度重量", Symbol = "", Value = SelectSteelAtte.Kg, Unit = "kg/m" });
                InsertionData.Add(new DataGridData() { Property = "密度", Symbol = "", Value = SelectSteelAtte.density, Unit = "kg/m3" });
            }
            else if (steelAttr.Type == OBJECT_TYPE.RB)
            {
                InsertionData.Add(new DataGridData() { Property = "直徑", Symbol = "D", Value = SelectSteelAtte.diameter, Unit = "mm" });

                InsertionData.Add(new DataGridData() { Property = "表面積", Symbol = "", Value = SelectSteelAtte.surface_area, Unit = "m2/m" });
                InsertionData.Add(new DataGridData() { Property = "斷面積", Symbol = "", Value = SelectSteelAtte.section_area, Unit = "m2" });
                InsertionData.Add(new DataGridData() { Property = "單位長度重量", Symbol = "", Value = SelectSteelAtte.Kg, Unit = "kg/m" });
                InsertionData.Add(new DataGridData() { Property = "密度", Symbol = "", Value = SelectSteelAtte.density, Unit = "kg/m3" });
            }
            else if (steelAttr.Type == OBJECT_TYPE.HNUT || steelAttr.Type == OBJECT_TYPE.NUT)
            {
                InsertionData.Add(new DataGridData() { Property = "直徑", Symbol = "D", Value = SelectSteelAtte.diameter, Unit = "mm" });
            }
            else
            {
                InsertionData.Add(new DataGridData() { Property = "高度", Symbol = "H", Value = SelectSteelAtte.H, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "寬度", Symbol = "W", Value = SelectSteelAtte.W, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "腹板厚度", Symbol = "s/t1", Value = SelectSteelAtte.t1, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "翼板厚度", Symbol = "t/t2", Value = SelectSteelAtte.t2, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "圓角半徑", Symbol = "r1", Value = SelectSteelAtte.r1, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "圓角半徑", Symbol = "r2", Value = SelectSteelAtte.r2, Unit = "mm" });
                InsertionData.Add(new DataGridData() { Property = "表面積", Symbol = "", Value = SelectSteelAtte.surface_area, Unit = "m2/m" });
                InsertionData.Add(new DataGridData() { Property = "斷面積", Symbol = "", Value = SelectSteelAtte.section_area, Unit = "m2" });
                InsertionData.Add(new DataGridData() { Property = "單位長度重量", Symbol = "", Value = SelectSteelAtte.Kg, Unit = "kg/m" });
                InsertionData.Add(new DataGridData() { Property = "密度", Symbol = "", Value = SelectSteelAtte.density, Unit = "kg/m3" });
            }
        }

        /// <summary>
        /// 初始化切割線設定 20220816 張燕華
        /// </summary>
        private void initializationSplitLineSettingData()
        {
            ////檢查使用者專案中是否已經存在設定檔案SplitLineSetting.lis
            //STDSerialization ser_file = new STDSerialization();
            //bool checkSplitLineDataFile = ser_file.CheckSplitLineDataFile();
            //
            //if (checkSplitLineDataFile == false)//若設定值檔案不存在
            //{
            //    //新增設定值檔案
            //    STDSerialization ser_AddFile = new STDSerialization(); //序列化處理器
            //    ObservableCollection<SplitLineSettingClass> listSplitLineData = new ObservableCollection<SplitLineSettingClass>();
            //    listSplitLineData.Add(new SplitLineSettingClass()
            //    {
            //        HowManyParts = "5",
            //        A = "1/5",
            //        B = "4/5",
            //        C = "1/5",
            //        D = "4/5",
            //        Thickness = 3,
            //        RemainingLength = 500.00
            //    });
            //    ser_AddFile.SetSplitLineData(listSplitLineData);
            //}

            // 2022/11/09 張燕華 判斷"型鋼加工區域設定"&"切割線設定"的初始設定值是否存在for改版前舊專案
            ApplicationVM appVM = new ApplicationVM();
            bool DirCreate = appVM.CheckParameterSettingDirectoryPath();

            string[] files = Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory + $@"DefaultParameterSetting", "*.lis");
            foreach (string file in files)
            {
                string[] filename = file.Split('\\');
                string a = $@"{ApplicationVM.DirectoryDefaultParameterSetting()}\{filename[filename.Length - 1]}";
                if (!File.Exists($@"{ApplicationVM.DirectoryDefaultParameterSetting()}\{filename[filename.Length - 1]}"))
                {
                    File.Copy(file, $@"{ApplicationVM.DirectoryDefaultParameterSetting()}\{filename[filename.Length - 1]}");
                }
            }

            //讀入SplitLineSetting.lis中切割線設定值
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<SplitLineSettingClass> ReadSplitLineSettingData = ser.GetSplitLineData();//備份當前加工區域數值
            
            HowManyParts_Value = ReadSplitLineSettingData[0].HowManyParts;
            PointA_Value = ReadSplitLineSettingData[0].A;
            PointB_Value = ReadSplitLineSettingData[0].B;
            PointC_Value = ReadSplitLineSettingData[0].C;
            PointD_Value = ReadSplitLineSettingData[0].D;
            CutThickness = ReadSplitLineSettingData[0].Thickness;
            SplitRemainingLength = ReadSplitLineSettingData[0].RemainingLength;
            

            //char[] HowManyParts_AB = new char[ReadSplitLineSettingData[0].A.Length];
            //using (StringReader sr = new StringReader(ReadSplitLineSettingData[0].A))
            //{
            //    // Read 13 characters from the string into the array.
            //    sr.Read(HowManyParts_AB, 2, 3);
            //}
            //char[] HowManyParts_CD = new char[ReadSplitLineSettingData[0].C.Length];
            //using (StringReader sr = new StringReader(ReadSplitLineSettingData[0].C))
            //{
            //    // Read 13 characters from the string into the array.
            //    sr.Read(HowManyParts_CD, 2, 3);
            //}

            //若設定值檔案存在, 載入所有combobox的item source
            SplitLineSettingClass SplitLineComboBox = new SplitLineSettingClass();

            cbb_HowManyParts = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_HowManyParts, HowManyParts_Value);
            cbb_A_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_A, HowManyParts_Value);
            cbb_B_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_B, HowManyParts_Value);
            cbb_C_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_C, HowManyParts_Value);
            cbb_D_ItemSource = SplitLineComboBox.GetSplitLineItemSource(SplitLineCombobox.cbb_D, HowManyParts_Value);

            //儲存使用者到目前為止使用系統時所有新增的切割線資料
            BackupSplitLineSettingData.Add(new SplitLineSettingClass()
            {
                HowManyParts = HowManyParts_Value,
                A = PointA_Value,
                B = PointB_Value,
                C = PointC_Value,
                D = PointD_Value,
                Thickness = CutThickness,
                RemainingLength = SplitRemainingLength
            });
            //設定當前復原設定值陣列最後一個元素的index
            CurrentIndex_BackupSplitLineSettingData = BackupSplitLineSettingData.Count - 1;
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
                steelAttr_system = GetSettingSteelAttr();
                List<SteelAttr> result = new List<SteelAttr>(list.ToList());
                result.Add(data);

                //斷面規格字串排序
                SortProfile(result);

                return new ObservableCollection<SteelAttr>(result);
            }
            else
            {
                //MessageBox.Show("新增斷面規格失敗。\n找到相同斷面規格名稱，請按更新鍵。", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"新增斷面規格失敗。\n找到相同斷面規格名稱，請按更新鍵",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
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
                data.t2 = (OBJECT_TYPE)_SelectType == OBJECT_TYPE.L || (OBJECT_TYPE)_SelectType == OBJECT_TYPE.BOX ? InsertionData[2].Value : InsertionData[3].Value;
                data.Profile = ProfileName;
                List<SteelAttr> result = new List<SteelAttr>(list.ToList());
                result[index] = data;

                //斷面規格字串排序
                SortProfile(result);

                return new ObservableCollection<SteelAttr>(result);
            }
            else
            {
                //MessageBox.Show("新增斷面規格失敗。\n找到相同斷面規格名稱，請按更新鍵。", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                    $"新增斷面規格失敗。\n找到相同斷面規格名稱，請按更新鍵",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
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
                    throw new ArgumentException("參數不能為空");
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
#pragma warning disable CS0649 // 從未指派欄位 'SettingParVM.isDuplication'，會持續使用其預設值 false
        private bool isDuplication;
#pragma warning restore CS0649 // 從未指派欄位 'SettingParVM.isDuplication'，會持續使用其預設值 false
        private SteelAttr _SelectSteelAtte = new SteelAttr();

        private int _SelectType { get; set; } = -1;
        private int _SelectProcessingBehavior { get; set; } = -1;
        private int _SelectSectionType { get; set; } = -1;
        #endregion

        #region VM類型
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
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem' 的 XML 註解
        public class _hydraulicSystem : IHydraulicSystem
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem' 的 XML 註解
        {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.Index' 的 XML 註解
            public short Index { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.Index' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.Power' 的 XML 註解
            public ushort Power { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.Power' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.MinRange' 的 XML 註解
            public short MinRange { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.MinRange' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.MaxRange' 的 XML 註解
            public short MaxRange { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.MaxRange' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.SideReady' 的 XML 註解
            public short SideReady { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.SideReady' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.DownReady' 的 XML 註解
            public short DownReady { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.DownReady' 的 XML 註解
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.Deviation' 的 XML 註解
            public float Deviation { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.Deviation' 的 XML 註解
            /// <summary>
            /// 備註
            /// </summary>
            public string Remarks { get; set; } = "H型鋼";
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.ConverterStruc(SettingParVM._hydraulicSystem, short, short, float)' 的 XML 註解
            public HydraulicSystem ConverterStruc(_hydraulicSystem hyd, short sideReady, short downReady, float deviation)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem.ConverterStruc(SettingParVM._hydraulicSystem, short, short, float)' 的 XML 註解
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
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem._hydraulicSystem(IHydraulicSystem)' 的 XML 註解
            public _hydraulicSystem(IHydraulicSystem hyd)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SettingParVM._hydraulicSystem._hydraulicSystem(IHydraulicSystem)' 的 XML 註解
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

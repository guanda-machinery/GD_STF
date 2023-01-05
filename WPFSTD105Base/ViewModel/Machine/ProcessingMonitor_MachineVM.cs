using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WPFBase = WPFWindowsBase;
using static WPFWindowsBase.Expand;
using WPFSTD105.Model;
using devDept.Eyeshot.Translators;
using System.IO;
using devDept.Geometry;
using devDept.Eyeshot.Entities;
using WPFSTD105.Attribute;
using System.Diagnostics;
using System.Threading;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
using GD_STD.Phone;
using System.Runtime.InteropServices;
using GD_STD;
using GD_STD.Enum;
using System.ServiceModel;
using GD_STD.Base;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using DevExpress.Xpf.Grid;
using System.Text;
using System.Windows.Media;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;
using System.Windows.Controls;
using DevExpress.Mvvm;
using devDept.Eyeshot;
using DevExpress.Data.Extensions;
using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using Microsoft.Office.Interop.Word;
using Task = System.Threading.Tasks.Task;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using DevExpress.Charts.Model;
using ProtoBuf.Meta;
using DocumentFormat.OpenXml.EMMA;
using DevExpress.Xpf.Core.FilteringUI;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using GD_STD.Properties;
using Newtonsoft.Json;
using DevExpress.Xpf.Spreadsheet.UI.TypedStyles;
using WPFSTD105.FluentAPI;
using System.Windows.Threading;
using GrapeCity.Documents.Pdf.Structure;
using DocumentFormat.OpenXml.InkML;
using DevExpress.CodeParser;
using CodesysIIS;
using System.Reflection;
using DevExpress.Xpf.Gauges;
using DevExpress.Xpo.DB;
using DocumentFormat.OpenXml.Wordprocessing;
using DevExpress.XtraPrinting;
using DocumentFormat.OpenXml.Drawing;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 加工監控VM
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ProcessingMonitor_MachineVM : WPFBase.BaseViewModel, IDisposable/*, MonitorDuplex.IMonitorDuplexCallback*/
    {

        /// <summary>
        /// 建構式
        /// </summary>
        public ProcessingMonitor_MachineVM()
        {
            STDSerialization ser = new STDSerialization();
            Display3DViewerCommand = Display3DViewer();
            Finish_UndoneDataViews = ser.GetMaterialDataView();
            if(Finish_UndoneDataViews!= null)
                Finish_UndoneDataViews.ForEach(el => el.PositionEnum = PositionStatusEnum.初始化);
            _SynchronizationContext = SynchronizationContext.Current;
            _WorkMaterials = new WorkMaterial[Finish_UndoneDataViews.Count];
            SelectedMaterial_Info_Button_Visibility = Visibility.Collapsed;

            //初始化後載入備份檔案 並產生
            //AllDrillBoltsDict
            Finish_UndoneDataViews.ForEach(el =>
            {
                var DrillBData = ser.GetDrillBolts(el.MaterialNumber);
                if (DrillBData != null)
                    AllDrillBoltsDict[el.MaterialNumber] = DrillBData;
                //else
                   // AllDrillBoltsDict[el.MaterialNumber] = GetDrillBoltsItemCollection(el);
            });

            //將設定的手臂模式寫入記憶體
            FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();//
            Transport_by_Hand_RadioButtonIsChecked = !optionSettings.HandAuto;
            //打開選配->自動模式->按鈕取消底線
            ser.SetOptionSettings(optionSettings);//寫入記錄
            MecOptional mecOptional = JsonConvert.DeserializeObject<MecOptional>(optionSettings.ToString());
            WriteCodesysMemor.SetMecOptional(mecOptional);//寫入記憶體

            if (ViewLocator.ApplicationViewModel.AppManualConnect)
            {
                Input_by_SmartPhone_RadioButtonIsChecked = true;
                Input_by_Computer_RadioButtonIsChecked = false;
            }



            //啟動一執行序持續掃描各種按鈕及錯誤訊息

            var ReadErrorInfoTask = Task.Factory.StartNew(() =>
            {
                //string Local_ErrorInfo = string.Empty;
                var errorList = new List<ERROR_CODE>();

                while (TaskBoolean)
                {
                    try
                    {
                        ERROR_CODE error = ApplicationViewModel.PanelButton.Alarm;
                        if (error != ERROR_CODE.Null)
                        {
                            if (!errorList.Exists(x=> x == error))
                            {
                                var Local_ErrorInfo = string.Empty;
                                if (error == ERROR_CODE.Unknown)
                                {
                                    string errorCode = ReadCodesysMemor.GetUnknownCode().Replace("\0", "");
                                    Local_ErrorInfo = $"未預期的錯誤 : {errorCode}";
                                }
                                else
                                {
                                    Local_ErrorInfo = error.GetType().GetField(error.ToString()).GetCustomAttribute<ErrorCodeAttribute>()?.Description;
                                }
                                errorList.Add(error);
                                AddOperatingLog(LogSourceEnum.Machine, Local_ErrorInfo, true);
                            }
                        }
                        else
                        {
                            errorList.Clear();
                        }

                        //一般狀態下才檢查 若兩個都未點選則不檢查

                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });

            Task.Factory.StartNew(() =>
            {
                string Local_ErrorInfo = string.Empty;
                //掃描是否有手機連線
                while (TaskBoolean)
                {
                    if (Input_by_SmartPhone_RadioButtonVMEnable &&
                         Input_by_Computer_RadioButtonVMEnable &&
                         !Input_by_SmartPhone_RadioButtonIsChecked)
                    {
                        if (ViewLocator.ApplicationViewModel.AppManualConnect)
                        {
                            //核對兩次 確保狀態正確
                            Thread.Sleep(1500);
                            if (ViewLocator.ApplicationViewModel.AppManualConnect)
                            {
                                Input_by_SmartPhone_RadioButtonIsChecked = true;
                                Input_by_Computer_RadioButtonIsChecked = false;
                            }
                        }
                    }

                    //監視是否有完成配對的 有配對才可點選台車選項
                    PositionStatusEnum[] AlreadyPairEnumArray = new PositionStatusEnum[] { PositionStatusEnum.已配對, PositionStatusEnum.從手機配對, PositionStatusEnum.從軟體配對 };
                    OBJECT_TYPE[] CantUseTrolleyOBJECT_TYPEArray = new OBJECT_TYPE[] { OBJECT_TYPE.CH, OBJECT_TYPE.LB, OBJECT_TYPE.TUBE, OBJECT_TYPE.BOX };

                    var PairIndex = Finish_UndoneDataViews.FindIndex(x => AlreadyPairEnumArray.Contains(x.PositionEnum));
                    //存在配對
                    if (PairIndex != -1)
                        PostageEnable = true;
                    else
                        PostageEnable = false;



                    var Channel_Index = Finish_UndoneDataViews.FindIndex(x => AlreadyPairEnumArray.Contains(x.PositionEnum) &&
                    CantUseTrolleyOBJECT_TYPEArray.Contains(x.ObjectType) );
                    //如果有槽鐵配對完成 禁用台車
                    if (Channel_Index != -1)
                        TrolleyEnable = true;
                    else
                    {
                   
                        TrolleyEnable = false;
                        //跳出提示告訴使用者必須使用續接
                        /*MessageBoxResult messageBoxResult = WinUIMessageBox.Show(null,
                            $"復原的素材有包含正在加工中的素材，請確認是否要移除加工中的素材",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning,
                            MessageBoxResult.None,
                            MessageBoxOptions.ServiceNotification,
                            FloatingMode.Adorner);*/
                    }

                    Thread.Sleep(1000);
                }
            });


            //定期存檔素材編輯孔位
            Task.Run(() =>
            {
                while (TaskBoolean)
                {
                    //如果值不一樣才存檔
                    foreach (var el in AllDrillBoltsDict)
                    {
                        if (!el.Value.Equals(ser.GetDrillBolts(el.Key)))
                            ser.SetDrillBolts(el.Key, el.Value);
                    }
                    Thread.Sleep(3000);
                }

            });

            Task.Run(() =>
            {
                AddOperatingLog(LogSourceEnum.Software, "啟用輪巡", false);
                while (true)
                {
                    try
                    {
                        WriteTourData();

                        for (int i = 0; i < 5; i++)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                AddOperatingLog(LogSourceEnum.Software, "停用輪巡", false);
            });
        }

        #region 公開屬性
        /// <summary>
        /// 控制3D頁面顯示
        /// </summary>
        public bool ThreeDimensionalDisplayControl { get; set; } = true;



        private int _mCurrent = -1;
        /// <summary>
        /// 當前值
        /// </summary>
        public int MCurrent { get { return _mCurrent; } set { _mCurrent = value; OnPropertyChanged(nameof(MCurrent)); } } 

        /// <summary>
        /// 送料許可(最上層)
        /// </summary>
        public bool PostageEnable { get; set; } = false;

        /// <summary>
        /// 台車許可(最上層)
        /// </summary>
        public bool TrolleyEnable { get; set; } = false;




        /// <summary>
        /// 
        /// </summary>
        public WPFBase.RelayParameterizedCommand RowAddCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(el =>
                {
                    if (el is IEnumerable<GD_STD.Data.MaterialDataView>)
                        foreach (var EachMaterial in el as IEnumerable<GD_STD.Data.MaterialDataView>)
                        {
                            var EachMaterialIndex = Convert.ToInt16(Finish_UndoneDataViews.IndexOf(EachMaterial));
                            if (EachMaterialIndex != -1)
                            {
                                try
                                {
                                    using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                                    using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                                    {
                                        var index = read.GetIndex().ToList();//
                                        index.RemoveAll(x => (x == -1));//是否有必要移除-1 ?
                                        var workOther = read.GetWorkOther();

                                        //如果已經存在加工序列->跳過
                                        if (index.Exists(x=> x ==EachMaterialIndex))
                                        {
                                            //如果在完成清單狀態
                                            if (_Finish.Exists(x=> x == EachMaterialIndex))
                                            {
                                                WinUIMessageBox.Show(null,
                                                    $"素材編號：{EachMaterial.MaterialNumber}已經加工完成",
                                                    $"通知",
                                                    MessageBoxButton.OK,
                                                    MessageBoxImage.Exclamation,
                                                    MessageBoxResult.None,
                                                    MessageBoxOptions.None,
                                                    FloatingMode.Adorner);
                                                continue;
                                            }
                                            else
                                            {
                                                if (EachMaterialIndex == workOther.Current)
                                                {
                                                    WinUIMessageBox.Show(null,
                                                        $"素材編號：{EachMaterial.MaterialNumber}正在加工",
                                                        $"通知",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Exclamation,
                                                        MessageBoxResult.None,
                                                        MessageBoxOptions.None,
                                                        FloatingMode.Adorner);
                                                    continue;
                                                }
                                                else
                                                {
                                                    WinUIMessageBox.Show(null,
                                                        $"素材編號：{EachMaterial.MaterialNumber}已經在加工序列內，",
                                                        $"通知",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Exclamation,
                                                        MessageBoxResult.None,
                                                        MessageBoxOptions.None,
                                                        FloatingMode.Adorner);
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Finish_UndoneDataViews[EachMaterialIndex].PositionEnum = PositionStatusEnum.已配對;
                                        }

                                        var workIndex = index.Count;
                                        long cWork = _WorkOffset + _WorkSize * workIndex;
                                        //long cInsert = cWork + Marshal.OffsetOf<WorkMaterial>(nameof(WorkMaterial.Insert)).ToInt64();
                                        //index.Insert(workIndex, Convert.ToInt16(Finish_UndoneDataViews.IndexOf(EachMaterial)));//插到指定位置
                                        index.Add(EachMaterialIndex);//最末端
                                        //write.SetMonitorWorkOffset(new byte[] { 1 }, cInsert); //寫入準備加工的陣列位置
                                        write.SetMonitorWorkOffset(index.ToArray().ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置
                                   
                                    
                                    }

                                    AddOperatingLog(LogSourceEnum.Machine, $"加入素材編號：{EachMaterial.MaterialNumber}成功", false);
                                    WinUIMessageBox.Show(null,
                                        $"加入素材編號：{EachMaterial.MaterialNumber}成功",
                                        $"通知",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Exclamation,
                                        MessageBoxResult.None,
                                        MessageBoxOptions.None,
                                        FloatingMode.Adorner);

                                }
                                catch (Exception ex)
                                {
                                    //return false;
                                }
                            }
                        }
                });
            }
        }

        public DevExpress.Mvvm.ICommand<DevExpress.Mvvm.Xpf.RowClickArgs> RowDoubleClickCommand
        {
            get
            {
                return new DevExpress.Mvvm.DelegateCommand<DevExpress.Mvvm.Xpf.RowClickArgs>(RowDoubleClick);
            }
        }

        [DevExpress.Mvvm.DataAnnotations.Command]
        private void RowDoubleClick(DevExpress.Mvvm.Xpf.RowClickArgs args)
        {

            //檢查是否是電腦模式->若不是則跳出彈窗
            if (!Input_by_Computer_RadioButtonIsChecked)
            {
                WinUIMessageBox.Show(null,
                    $"需切換到電腦模式才可加入加工素材",
                    $"通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Adorner);
                return;
            }
            //若是則可開始加入零件
            //電腦模式下才可加入零件
            if (Input_by_Computer_RadioButtonIsChecked)
            {
                if (args.Item is MaterialDataView)
                {
                    Task.Run(() =>
                    {
                        DevExpress.Xpf.Core.SplashScreenManager InsertMProcessingScreenWin = DevExpress.Xpf.Core.SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });
                        InsertMProcessingScreenWin.Show();
                        InsertMProcessingScreenWin.ViewModel.Status = "正在加入零件";
                        InsertMaterial(InsertMProcessingScreenWin.ViewModel, args.Item as MaterialDataView ); 
                        InsertMProcessingScreenWin.Close();
                    });
                }
                else
                {
                    Debugger.Break();
                }
            }
        }


        private bool InsertMaterial(DXSplashScreenViewModel DXviewmodel,MaterialDataView MaterialData, bool showWinUIMessageBox = true)
        {
            var MaterialIndex = Finish_UndoneDataViews.FindIndex(x => (x == MaterialData));
            if (MaterialIndex != -1)
            {
                //若選擇等待配對以外的零件則顯示不可配對
                if (Finish_UndoneDataViews[MaterialIndex].PositionEnum == PositionStatusEnum.等待配對)
                {
                    //該素材沒有零件 跳出提示
                    if (Finish_UndoneDataViews[MaterialIndex].Parts.Count == 0)
                    {
                        AddOperatingLog(LogSourceEnum.Machine, $"素材編號：{MaterialData.MaterialNumber}內不包含可加工的零件", true);
                        DXviewmodel.Status = $"不包含可加工的零件";
                        return false;
                    }
                    else
                    {
                        DXviewmodel.Status = $"註冊素材編號：{MaterialData.MaterialNumber}";
                        //使用api註冊
                        var Result = MachineAndPhoneAPI.AppServerCommunicate.SetRegisterAssembly(
                            ApplicationViewModel.ProjectName,
                            Finish_UndoneDataViews[MaterialIndex].MaterialNumber,
                            Finish_UndoneDataViews[MaterialIndex].Material,
                            Finish_UndoneDataViews[MaterialIndex].Profile,
                            Finish_UndoneDataViews[MaterialIndex].LengthStr,
                            out var RegisterResult);
                        if (Result)
                        {
                            //error202->Material not found 
                            if (RegisterResult.data != null)
                            {
                                //一次只會加入一筆資料 所以回傳資料list行直接取取[0]
                                if (RegisterResult.data[0].errorCode == 0)
                                {
                                    Finish_UndoneDataViews[MaterialIndex].PositionEnum = PositionStatusEnum.從軟體配對;
                                    Finish_UndoneDataViews[MaterialIndex].MachiningStartTime = null;
                                    Finish_UndoneDataViews[MaterialIndex].MachiningEndTime = null;
                                   // ScheduleGridC.RefreshGrid();
                                    RefreshScheduleGridC();
                                    SelectedMaterial_Info_Button_Visibility = Visibility.Visible;

                                    DXviewmodel.Status = $"加入素材編號：{MaterialData.MaterialNumber}成功";
                                    AddOperatingLog(LogSourceEnum.Machine, $"加入素材編號：{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}成功", false);
                                    ScheduleGridC.Dispatcher.Invoke(() =>
                                    {
                                        if (showWinUIMessageBox)
                                        {
                                            WinUIMessageBox.Show(null,
                                            $"加入素材編號：{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}成功",
                                            $"通知",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Exclamation,
                                            MessageBoxResult.None,
                                            MessageBoxOptions.None,
                                            FloatingMode.Adorner);
                                        }
                                    });
                                    return true;
                                }
                                else
                                {
                                    DXviewmodel.Status = $"加入素材編號：{MaterialData.MaterialNumber}失敗";
                                    AddOperatingLog(LogSourceEnum.Machine, $"加入素材編號：{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}失敗", true);
                                    Thread.Sleep(100);
                                    AddOperatingLog(LogSourceEnum.Machine, $"錯誤資訊：{RegisterResult.data[0].errorCode},{RegisterResult.data[0].errorMessage}", true);
                                }

                            }
                            else
                            {
                                DXviewmodel.Status = $"註冊素材編號：{MaterialData.MaterialNumber}失敗";
                                ScheduleGridC.Dispatcher.Invoke(() =>
                                {
                                    WinUIMessageBox.Show(null,
                                    $"與伺服器溝通成功，但加入素材編號：{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}失敗",
                                    $"通知",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation,
                                    MessageBoxResult.None,
                                    MessageBoxOptions.None,
                                    FloatingMode.Adorner);
                                });
                                AddOperatingLog(LogSourceEnum.Machine, $"與伺服器溝通成功，但加入素材編號：{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}失敗", false);
                                AddOperatingLog(LogSourceEnum.Machine, $"RegisterResult.data沒有回傳值", false);
                            }
                        }
                        else
                        {
                            DXviewmodel.Status = $"與伺服器溝通失敗";
                            ScheduleGridC.Dispatcher.Invoke(() =>
                            {
                                WinUIMessageBox.Show(null,
                            $"與伺服器溝通失敗",
                            $"通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                            FloatingMode.Adorner);
                            });
                            AddOperatingLog(LogSourceEnum.Machine, "與伺服器溝通失敗！", true);
                        }
                    }
                }
                else if (Finish_UndoneDataViews[MaterialIndex].PositionEnum == PositionStatusEnum.不可配對)
                {
                    DXviewmodel.Status = $"素材{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}因尺寸問題無法排入加工";
                    ScheduleGridC.Dispatcher.Invoke(() =>
                    {
                        WinUIMessageBox.Show(null,
                                        $"素材{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}因尺寸問題無法排入加工",
                                        $"通知",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Exclamation,
                                        MessageBoxResult.None,
                                        MessageBoxOptions.None,
                                        FloatingMode.Adorner);
                    });
                    AddOperatingLog(LogSourceEnum.Machine, $"素材{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}因尺寸問題無法排入加工", true);
                }
                else
                {
                    DXviewmodel.Status = $"素材編號：{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}不是{PositionStatusEnum.等待配對}之素材";
                    AddOperatingLog(LogSourceEnum.Machine, $"素材編號：{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}不是{PositionStatusEnum.等待配對}之素材");
                }
            }
            else
            {
                //基本上不會發生 除非資料結構錯誤
                AddOperatingLog(LogSourceEnum.Machine, $"在加工清單找不到素材編號：{MaterialData.MaterialNumber}之素材", true);
            }

            return false;
        }

        private ObservableCollection<MaterialDataView> _finish_UndoneDataViews = new ObservableCollection<MaterialDataView>();
        /// <summary>
        /// 未加工-已完成合併清單
        /// </summary>
        public ObservableCollection<MaterialDataView> Finish_UndoneDataViews
        {
            get
            {
                return _finish_UndoneDataViews;
            }
            set
            {
                _finish_UndoneDataViews = value;
                OnPropertyChanged("Finish_UndoneDataViews");
            }
        }



        private MaterialDataView _finish_UndoneDataViews_SelectedItem;
        /// <summary>
        /// 選擇素材Gridcontrol會觸發之binding
        /// </summary>
        public MaterialDataView Finish_UndoneDataViews_SelectedItem
        {
            get
            {
                return _finish_UndoneDataViews_SelectedItem;
            }
            set
            {
                _finish_UndoneDataViews_SelectedItem = value;
                if (_finish_UndoneDataViews_SelectedItem != null)
                {
                    AddOperatingLog(LogSourceEnum.Software, $"已選擇素材編號：{_finish_UndoneDataViews_SelectedItem.MaterialNumber}");

                    //AllDrillBoltsDict[_finish_UndoneDataViews_SelectedItem.MaterialNumber].ForEach(el => el.Value.PinTestMode = DrillPin_Mode_RadioButtonIsEnable);
                   

                    MachiningCombinational_DrillBoltsItemSource = GetDrillBoltsItemCollection(DrillPin_Mode_RadioButtonIsEnable ,  _finish_UndoneDataViews_SelectedItem);
                    //如果切換時有已排程但未加工的零件->清理掉狀態並重新上傳
                    ClearPairedMachineData(_finish_UndoneDataViews_SelectedItem);
                }
            }
        }
        public ObservableCollection<MaterialDataView> Finish_UndoneDataViews_SelectedItems { get; } = new ObservableCollection<MaterialDataView>();


        /// <summary>
        /// 編輯後全部的加工總數都會存在這裡供比較 用素材編號<see cref="string"/>來分類
        /// </summary>
        private Dictionary<string, Dictionary<FACE, DrillBoltsBase>> AllDrillBoltsDict { get; set; } = new Dictionary<string, Dictionary<FACE, DrillBoltsBase>>();

        /// <summary>
        /// 實際要送出的加工陣列組
        /// </summary>
        private Dictionary<string, Dictionary<FACE, List<Drill>>> MachineDrillkeyValueDict { get; set; } = new Dictionary<string, Dictionary<FACE, List<Drill>>>();


        public ObservableCollection<OperatingLog> LogDataList { get; set; } = new ObservableCollection<OperatingLog>();
        private void AddOperatingLog(LogSourceEnum LogSourceEnum, string _Logstring, bool _IsAlert = false)
        {
            //當歷程記錄AddOperatingLog與ShowMessageBox顯示同時使用時，需先執行ShowMessageBox，
            //否則會發生歷程記錄無法自動捲動之情況

            _SynchronizationContext.Send(t =>
            {
                LogDataList.Add(new OperatingLog { LogString = _Logstring, LogSource = LogSourceEnum, LogDatetime = DateTime.Now, IsAlert = _IsAlert });
            }, null);
            RefreshLogGridC();

            //需加入自動捲動功能
        }

        /// <summary>
        /// 素材(上有多個零件)加工孔位數量清單
        /// </summary>
        public Dictionary<FACE, DrillBoltsBase> MachiningCombinational_DrillBoltsItemSource { get; set; } = new Dictionary<FACE, DrillBoltsBase>();

        /// <summary>
        /// 單一零件加工孔位表
        /// </summary>
        public Dictionary<FACE, DrillBoltsBase> MachiningDetail_DrillBoltsItemSource { get; set; } = new Dictionary<FACE, DrillBoltsBase>();

        //產生加工參考表->標示工件要如何加工 實際加工時會參考此處之設定值
        private Dictionary<FACE, DrillBoltsBase> GetDrillBoltsItemCollection(bool PinMode , MaterialDataView view)
        {
            var Dict = new Dictionary<FACE, DrillBoltsBase>();
            if (view == null)
            {
                return Dict;
            }
            else
            {
                if (view.PositionEnum == PositionStatusEnum.等待配對)
                    SelectedMaterial_Info_Button_Visibility = Visibility.Visible;
                else if(view.PositionEnum == PositionStatusEnum.初始化)
                    SelectedMaterial_Info_Button_Visibility = Visibility.Visible;
                else
                    SelectedMaterial_Info_Button_Visibility = Visibility.Collapsed;

                _CreateDMFileTask?.Wait(); //等待 Task CreateFile 完成 link:ProcessingMonitorVM.cs:CreateFile()
                try
                {
                    if (File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{view.MaterialNumber}.dm"))
                    {
                        //若沒有紀錄則產生一個新的原始加工表
                        if (!MachineDrillkeyValueDict.ContainsKey(view.MaterialNumber))
                        {
                            MachineDrillkeyValueDict[view.MaterialNumber] = GenerateMachiningDataPairs(view);
                        }

                        var DrillBoltsListDict = new Dictionary<FACE, DrillBoltsBase>();
                        foreach (var keyValuePair in MachineDrillkeyValueDict[view.MaterialNumber])
                        {
                            foreach (var DrillData in keyValuePair.Value)
                            {
                                if (!DrillBoltsListDict.ContainsKey(keyValuePair.Key))
                                    DrillBoltsListDict.Add(keyValuePair.Key, new DrillBoltsBase());

                                var DrillBIndex = DrillBoltsListDict[keyValuePair.Key].DrillBoltList.FindIndex(x => (x.Origin_WorkAXIS_MODE == DrillData.AXIS_MODE && x.DrillHoleDiameter == DrillData.Dia));
                                if (DrillBIndex != -1)
                                {
                                    DrillBoltsListDict[keyValuePair.Key].DrillBoltList[DrillBIndex].DrillHoleCount++;
                                }
                                else
                                {
                                    DrillBoltsListDict[keyValuePair.Key].PinTestMode = PinMode;
                                    if (PinMode)
                                    {
                                        if (DrillData.AXIS_MODE == AXIS_MODE.POINT)
                                        {
                                            DrillBoltsListDict[keyValuePair.Key].DrillBoltList.Add(new DrillBolt()
                                            {
                                                DrillWork = true,
                                                Origin_WorkAXIS_MODE = DrillData.AXIS_MODE,
                                                DrillHoleCount = 1,
                                                Origin_DrillHoleDiameter = DrillData.Dia,
                                            });
                                        }
                                        else if (DrillData.AXIS_MODE == AXIS_MODE.PIERCE)
                                        {
                                            DrillBoltsListDict[keyValuePair.Key].DrillBoltList.Add(new DrillBolt()
                                            {
                                                DrillWork = true,
                                                Origin_WorkAXIS_MODE = DrillData.AXIS_MODE,
                                                DrillHoleCount = 1,
                                                Origin_DrillHoleDiameter = DrillData.Dia,
                                                WorkAXIS_modeIsChanged = true,
                                                Changed_WorkAXIS_MODE = AXIS_MODE.POINT
                                            });
                                        }
                                        else
                                        {
                                            DrillBoltsListDict[keyValuePair.Key].DrillBoltList.Add(new DrillBolt()
                                            {
                                                DrillWork = false,
                                                Origin_WorkAXIS_MODE = DrillData.AXIS_MODE,
                                                DrillHoleCount = 1,
                                                Origin_DrillHoleDiameter = DrillData.Dia,
                                            });
                                        }
                                    }
                                    else
                                    {
                                        DrillBoltsListDict[keyValuePair.Key].DrillBoltList.Add(new DrillBolt()
                                        {
                                            DrillWork = true,
                                            Origin_WorkAXIS_MODE = DrillData.AXIS_MODE,
                                            DrillHoleCount = 1,
                                            Origin_DrillHoleDiameter = DrillData.Dia,
                                        });
                                    }
                                }
                            }
                        }

                        //Debugger.Break();
                        //先顯示差異 再加入空缺的
                        if (AllDrillBoltsDict.ContainsKey(view.MaterialNumber))
                        {
                            //將舊的值取出修改DrillWork後 重新綁定
                            foreach (var el in AllDrillBoltsDict[view.MaterialNumber])
                            {
                                if (DrillBoltsListDict.ContainsKey(el.Key))
                                {
                                    DrillBoltsListDict[el.Key].Dia_Identification = el.Value.Dia_Identification;
                                    DrillBoltsListDict[el.Key].UnitaryToolTop = el.Value.UnitaryToolTop;
                                    foreach (var Db in el.Value.DrillBoltList)
                                    {
                                        int DBIndex = DrillBoltsListDict[el.Key].DrillBoltList.FindIndex(x => (x.Actual_WorkAXIS_MODE == Db.Actual_WorkAXIS_MODE && x.DrillHoleDiameter == Db.DrillHoleDiameter));
                                        if (DBIndex != -1)
                                        {
                                            DrillBoltsListDict[el.Key].DrillBoltList[DBIndex].DrillWork = Db.DrillWork;
                                        }
                                    }
                                }
                            }
                        }
                        //這樣指定後兩邊資料會產生繫結 後面的DrillBoltsListInfo值變更會影響到前面的AllDrillBoltsDict內的List
                        //之後需要改資料時只需更動任一資料型
                        //AllDrillBoltsDict[view.MaterialNumber].Dia_Identification;
                        AllDrillBoltsDict[view.MaterialNumber] = DrillBoltsListDict;

                        //紀錄目前的加工類型? 是否要寫在這?
                        //


                        //如果有紀錄->查詢->變更DrillWork的值
                        return DrillBoltsListDict;
                    }
                }
                catch (Exception ex)
                {

                }
                return Dict;
            }
        }

        /// <summary>
        /// 依據原始dm產生未變更的加工陣列
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private Dictionary<FACE, List<Drill>> GenerateMachiningDataPairs(MaterialDataView view)
        {
            Dictionary<FACE, List<Drill>> keyValue = null;
            _SynchronizationContext.Send(t =>
            {
            var _BufferModel = new devDept.Eyeshot.Model();
            _BufferModel.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            _BufferModel.InitializeViewports();
            _BufferModel.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());
            _BufferModel.Clear();

                Dictionary<FACE, List<Drill>> keyValuePairs = new Dictionary<FACE, List<Drill>>();
                STDSerialization ser = new STDSerialization();
                ReadFile readFile = ser.ReadMaterialModel(view.MaterialNumber);
                _BufferModel.Clear();
                readFile.DoWork();
                readFile.AddToScene(_BufferModel);
                var steelPart = ser.GetPart($"{view.Profile.GetHashCode()}")[0];
                _BufferModel.Entities.ForEach(el =>
                {
                    BlockReference blockReference = (BlockReference)el;
                    if (blockReference.Attributes.ContainsKey("Bolts"))
                    {
                        Entity[] entities = blockReference.Explode(_BufferModel.Blocks); //返回圖塊引用單個實體列表
                        BoltAttr attr = (BoltAttr)entities[0].EntityData;
                        if (!keyValuePairs.ContainsKey(attr.Face))
                        {
                            keyValuePairs.Add(attr.Face, new List<Drill>());
                        }

                        if (attr.Face == FACE.TOP)
                        {
                            keyValuePairs[attr.Face].AddRange(BoltAsDrill(entities, new Transformation(new Point3D(0, steelPart.H, 0), Vector3D.AxisX, new Vector3D(0, -1), Vector3D.AxisZ)));
                        }
                        else
                        {
                            keyValuePairs[attr.Face].AddRange(BoltAsDrill(entities));
                        }
                    }
                    else if (blockReference.Attributes.ContainsKey("Steel"))
                    {
                        SteelAttr steelAttr = (SteelAttr)blockReference.EntityData;
                    }

                });

                keyValue = keyValuePairs;
            }, null);
            while(keyValue == null)
            {
                Thread.Sleep(100);
            }
            return keyValue;
        }


        /// <summary>
        /// 取得素材加工資料
        /// </summary>
        /// <param name="view"></param>
        /*
        private void GetDrillBoltsItemCollection(MaterialPartDetail view)
        {
            if (view == null)
            {
                MachiningCombinational_DrillBoltsItemSource = new Dictionary<FACE, DrillBoltsBase>();
            }

            _CreateDMFileTask?.Wait(); //等待 Task CreateFile 完成 link:ProcessingMonitorVM.cs:CreateFile()
            _SynchronizationContext.Send(t =>
            {
                try
                {
                    var _BufferModel = new devDept.Eyeshot.Model();
                    _BufferModel.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
                    _BufferModel.InitializeViewports();
                    _BufferModel.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());

                    _BufferModel.Clear();
                    Dictionary<FACE, List<Drill>> keyValuePairs = new Dictionary<FACE, List<Drill>>();
                    if (File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{view.MaterialNumber}.dm"))
                    {
                        STDSerialization ser = new STDSerialization();

                        ReadFile readFile = ser.ReadMaterialModel(view.MaterialNumber);
                        _BufferModel.Clear();
                        readFile.DoWork();
                        readFile.AddToScene(_BufferModel);
                        //}, null);
                        //FACE face = FACE.TOP;
                        var steelPart = ser.GetPart($"{view.Profile.GetHashCode()}")[0];

                        _BufferModel.Entities.ForEach(el =>
                        {
                            BlockReference blockReference = (BlockReference)el;
                            if (blockReference.Attributes.ContainsKey("Bolts"))
                            {
                                Entity[] entities = blockReference.Explode(_BufferModel.Blocks); //返回圖塊引用單個實體列表
                                BoltAttr attr = (BoltAttr)entities[0].EntityData;


                                if (!keyValuePairs.ContainsKey(attr.Face))
                                {
                                    keyValuePairs.Add(attr.Face, new List<Drill>());
                                }

                                if (attr.Face == FACE.TOP)
                                {
                                    keyValuePairs[attr.Face].AddRange(BoltAsDrill(entities, new Transformation(new Point3D(0, steelPart.H, 0), Vector3D.AxisX, new Vector3D(0, -1), Vector3D.AxisZ)));
                                }
                                else
                                {
                                    keyValuePairs[attr.Face].AddRange(BoltAsDrill(entities));
                                }
                            }
                            else if (blockReference.Attributes.ContainsKey("Steel"))
                            {
                                SteelAttr steelAttr = (SteelAttr)blockReference.EntityData;
                            }
                        });



















                        var DrillBoltsListInfo = new DrillBoltsBase();
                        foreach (var keyValuePair in keyValuePairs)
                        {
                            foreach (var DrillData in keyValuePair.Value)
                            {
                                if (DrillBoltsListInfo.DrillBoltList.Exists(x => (x.WorkAXIS_MODE == DrillData.AXIS_MODE && x.DrillHoleDiameter == DrillData.Dia)))
                                {
                                    DrillBoltsListInfo.DrillBoltList.Find(x => (x.WorkAXIS_MODE == DrillData.AXIS_MODE && x.DrillHoleDiameter == DrillData.Dia)).DrillHoleCount++;
                                }
                                else
                                {
                                    DrillBoltsListInfo.DrillBoltList.Add(new DrillBolt()
                                    {
                                        WorkAXIS_MODE = DrillData.AXIS_MODE,
                                        DrillHoleCount = 1,
                                        Origin_DrillHoleDiameter = DrillData.Dia
                                    });
                                }
                            }

                            MachiningDetail_DrillBoltsItemSource.Add(keyValuePair.Key, DrillBoltsListInfo);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }, null);
        }

        */





        /// <summary>
        /// 未加工-已完成清單-細項
        /// </summary>
        public ObservableCollection<MaterialPartDetail> Finish_UndoneDataViewsDetail
        {
            get
            {
                var _MPartDetail = new ObservableCollection<MaterialPartDetail>();
                foreach (var FUdataviews in Finish_UndoneDataViews)
                {
                    if (FUdataviews != null)
                        if (FUdataviews.Parts.Count != 0)
                            foreach (var FUPart in FUdataviews.Parts)
                                //加入素材<->零件 先素材再零件    
                                _MPartDetail.Add(new MaterialPartDetail()
                                {
                                    MaterialNumber = FUdataviews.MaterialNumber,//排版編號
                                    Profile = FUdataviews.Profile,//斷面規格
                                    Material = FUdataviews.Material,//材質
                                    AssemblyNumber = FUPart.AssemblyNumber,
                                    PartNumber = FUPart.PartNumber,
                                    Length = FUPart.Length,
                                    Position = FUdataviews.PositionEnum,
                                });
                }
                return _MPartDetail;

            }
        }

        private MaterialPartDetail _finish_UndoneDataViewsDetail_SelectedItem;
        /// <summary>
        /// 展開零件資訊
        /// </summary>
        public MaterialPartDetail Finish_UndoneDataViewsDetail_SelectedItem
        {
            get
            {
                return _finish_UndoneDataViewsDetail_SelectedItem;
            }
            set
            {
                _finish_UndoneDataViewsDetail_SelectedItem = value;

                if (_finish_UndoneDataViewsDetail_SelectedItem != null)
                {
                    //GetDrillBoltsItemCollection(_finish_UndoneDataViewsDetail_SelectedItem);
                    AddOperatingLog(LogSourceEnum.Software, $"已選擇零件編號：{_finish_UndoneDataViewsDetail_SelectedItem.PartNumber}");
                }
            }
        }

        /// <summary>
        /// 展開素材內的零件所使用的資料表
        /// </summary>
        public class MaterialPartDetail
        {
            /// <summary>
            /// /// 排版編號
            /// /// </summary>
            public string MaterialNumber { get; set; }
            /// <summary>
            /// 斷面規格
            /// </summary>
            public string Profile { get; set; }
            /// <summary>
            /// 材質
            /// </summary>
            public string Material { get; set; }
            /// <summary>
            /// 構件編號
            /// </summary>
            public string AssemblyNumber { get; set; }
            /// <summary>
            /// 零件編號
            /// </summary>
            public string PartNumber { get; set; }
            /// <summary>
            /// 零件長度
            /// </summary>
            public double? Length { get; set; }
            /// <summary>
            /// 位置
            /// </summary>
            public PositionStatusEnum Position { get; set; }
        }


        public GridControl ScheduleGridC { get; set; } 
        public GridControl LogGridC { get; set; }
        /// <summary>
        /// 未加工的控制項
        /// </summary>
        //public GridControl UndoneGrid { get; set; }
        /// <summary>
        /// 加工完成的控制項
        /// </summary>
        //public GridControl FinishGrid { get; set; }
        /// <summary>
        /// 選擇的資料行
        /// </summary>
        public MaterialDataView SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _DrawModel.Clear();
                STDSerialization ser = new STDSerialization();
                _SelectedItem = value;
                ReadFile file = ser.ReadMaterialModel(value.MaterialNumber);
                file.DoWork();
                file.AddToScene(_DrawModel);
                _DrawModel.Refresh();
                _DrawModel.ZoomFit();//設置道適合的視口
                _DrawModel.Invalidate();
            }
        }

        public WPFSTD105.ModelExt _DrawModel { get; set; } = new WPFSTD105.ModelExt();


        /// <summary>
        /// 搜尋字串
        /// </summary>
        public string MaterialGridControlSearchString { get; set; }

        private bool _MachiningCombinational_List_GridControl_IsSelected = false;
        /// <summary>
        /// 已選擇MachiningCombinational_List_GridControl
        /// </summary>
        public bool MachiningCombinational_List_GridControl_IsSelected
        {
            get
            {
                return _MachiningCombinational_List_GridControl_IsSelected;
            }
            set
            {
                if (_MachiningCombinational_List_GridControl_IsSelected != value)
                {
                    _MachiningCombinational_List_GridControl_IsSelected = value;
                    //↓可即時反應
                    OnPropertyChanged("MachiningCombinational_List_GridControl_IsSelected");
                }
            }
        }







        #endregion

        #region 私有屬性

        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private MaterialDataView _SelectedItem;
        private bool _DsposedValue;
        //private STDSerialization _Ser = new STDSerialization();
        private Task _CreateDMFileTask;
        private Task _WriteCodesysTask;
        private SynchronizationContext _SynchronizationContext;//= SynchronizationContext.Current;
        /// <summary>
        /// 完成的 Index
        /// </summary>
        private List<short> _Finish { get; set; } = new List<short>();

        /// <summary>
        /// 加工列表
        /// </summary>
        private WorkMaterial[] _WorkMaterials { get; set; } = null;
        /// <summary>
        /// 發送過的加工陣列
        /// </summary>
        private List<short> _SendIndex { get; set; } = new List<short>();
        /// <summary>
        /// 暫存模型
        /// </summary>
        //private devDept.Eyeshot.Model _BufferModel { get; set; }
        #region 偏移量
        //int _WorkIndex = 0;//加工陣列索引
        private long _WorkOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.WorkMaterial)).ToInt64();
        private int _WorkSize = Marshal.SizeOf(typeof(WorkMaterial));
        private long _MaterialNumberOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.MaterialNumber)).ToInt64();
        private long _ProfileOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.Profile)).ToInt64();
        private long _PartNumberOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.PartNumber)).ToInt64();
        private long _LengthOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.Length)).ToInt64();
        private long _MaterialOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.Material)).ToInt64();
        private long _AssemblyNumberOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.AssemblyNumber)).ToInt64();
        private long _hOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.H)).ToInt64();
        private long _wOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.W)).ToInt64();
        private long _t1Offset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.t1)).ToInt64();
        private long _t2Offset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.t2)).ToInt64();
        private long _GuidOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.GUID)).ToInt64();
        private long _BoltsCountLOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.BoltsCountL)).ToInt64();
        private long _BoltsCountMOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.BoltsCountM)).ToInt64();
        private long _BoltsCountROffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.BoltsCountR)).ToInt64();
        private long _DrLOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.DrLeft)).ToInt64();
        private long _DrROffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.DrRight)).ToInt64();
        private long _DrMOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.DrMiddle)).ToInt64();

        private long _ObjectTypeOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.ProfileType)).ToInt64();
        #endregion
        #endregion

        #region 命令

        /// <summary>
        /// 續接專案命令
        /// </summary>
        public WPFBase.RelayCommand ContinueCommand
        {
            get
            {
                return new WPFBase.RelayCommand(() =>
                {
                    using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                        write.SetMonitorWorkOffset(new byte[1] { 1 }, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.ContinueWork)).ToInt64()); //寫入準備加工的陣列位置
                });
            }
        }
        /// <summary>
        /// 呼叫台車命令
        /// </summary>
        public WPFBase.RelayCommand CallCarCommand
        {
            get
            {
                return new WPFBase.RelayCommand(() =>
                {
                    ushort[] reply = null;
                    using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                    using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                    {
                        Type type = typeof(MonitorWork);
                        long moveOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.Move)).ToInt64(); //index 偏移量
                        byte[] value = new byte[1] { 1 };
                        write.SetMonitorWorkOffset(value, moveOffset);//呼叫台車
                        reply = read.GetInstantMessage();
                    }
                    var _ = reply.Where(el => el != 0); //Codesys 回復訊息
                    if (_.Count() > 0)
                    {
                        char[] chars = _.Select(el => Convert.ToChar(el)).ToArray(); //轉換字元
                        string str = new string(chars);//轉換文字
                                                       //System.Windows.MessageBox.Show($"{str}", "通知", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information, System.Windows.MessageBoxResult.None, System.Windows.MessageBoxOptions.ServiceNotification);
                        WinUIMessageBox.Show(null,
                            $"{str}",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                            FloatingMode.Window);
                    }
                });
            }
        }



        /// <summary>
        /// 加入命令
        /// </summary>
        public WPFBase.RelayParameterizedCommand AddCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(el =>
                {
                    var MDV_SelectedItems = el as IEnumerable<GD_STD.Data.MaterialDataView>;
                    if (MDV_SelectedItems != null)
                    {
                        foreach (var dataView in MDV_SelectedItems)
                        {
                            short[] index;
                            using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                            {
                                index = read.GetIndex();
                            }
                            short[] value = new short[index.Length + 1];
                            Array.Copy(index, value, index.Length);
                            value[value.Length - 1] = Convert.ToInt16(Finish_UndoneDataViews.IndexOf(dataView));
                            long indexOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.Index)).ToInt64(); //index 偏移量
                            byte[] writeByte = value.ToByteArray();
                            using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                                write.SetMonitorWorkOffset(writeByte, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置

                            var FIndex = Finish_UndoneDataViews.FindIndex(x => x == dataView);
                            if (FIndex != -1)
                            {
                                Finish_UndoneDataViews[FIndex].PositionEnum = PositionStatusEnum.已配對;
                            }
                        }
                    }
                });
            }
        }

        

       public WPFBase.RelayParameterizedCommand SelectedItemsAddCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(el =>
                {

                    if (el is IEnumerable<GD_STD.Data.MaterialDataView>)
                    {
                        var MDV_SelectedItems = (el as IEnumerable<GD_STD.Data.MaterialDataView>).ToList();
                        
                        if (MDV_SelectedItems != null)
                        {
                            Task.Run(() =>
                            {
                                DevExpress.Xpf.Core.SplashScreenManager InsertMProcessingScreenWin = DevExpress.Xpf.Core.SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });
                                InsertMProcessingScreenWin.Show();
                                for (int i = 0; i < MDV_SelectedItems.Count; i++)
                                {
                                    if (!Input_by_Computer_RadioButtonIsChecked)
                                    {
                                        WinUIMessageBox.Show(null,
                                            $"需切換到電腦模式才可加入加工素材",
                                            $"通知",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Exclamation,
                                            MessageBoxResult.None,
                                            MessageBoxOptions.None,
                                            FloatingMode.Adorner);
                                        return;
                                    }
                                    //若是則可開始加入零件
                                    //電腦模式下才可加入零件
                                    if (Input_by_Computer_RadioButtonIsChecked)
                                    {

                                        InsertMProcessingScreenWin.ViewModel.Status = $"正在加入零件{i + 1}/{MDV_SelectedItems.Count}";
                                        InsertMaterial(InsertMProcessingScreenWin.ViewModel, MDV_SelectedItems[i],false);
                                       
                                    }
                                }
                                InsertMProcessingScreenWin.Close();
                            });
                        }
                    }
                    else
                    {
                        WinUIMessageBox.Show(null,
                            $"SelectedItemsAddCommand出現問題 el不是IEnumerable<GD_STD.Data.MaterialDataView>",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                            FloatingMode.Window);
                    }

                });
            }
        }


        /// <summary>
        /// 強制完成
        /// </summary>
        public WPFBase.RelayParameterizedCommand FinishCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(el =>
                {
                    var MDV_SelectedItems = (el as IEnumerable<GD_STD.Data.MaterialDataView>).ToList();

                    if (MDV_SelectedItems.Exists(e => (e.PositionEnum == PositionStatusEnum.加工中)))
                    {
                        MDV_SelectedItems.RemoveAll(e => (e.PositionEnum == PositionStatusEnum.加工中));
                    }

                    if (MDV_SelectedItems != null)
                    {
                        foreach (var dataView in MDV_SelectedItems)
                        {
                            int selected = Finish_UndoneDataViews.IndexOf(dataView);
                            if (selected != -1)
                            {
                                if (_WorkMaterials[selected].MaterialNumber == null)
                                {
                                    _WorkMaterials[selected].MaterialNumber = new ushort[20];
                                    Array.Copy(Finish_UndoneDataViews[selected].MaterialNumber.ToCharArray(), _WorkMaterials[selected].MaterialNumber, Finish_UndoneDataViews[selected].MaterialNumber.Length);
                                }
                                if (_WorkMaterials[selected].AssemblyNumber == null)
                                {
                                    _WorkMaterials[selected].AssemblyNumber = new ushort[25];
                                }
                                //_WorkMaterials[selected].ProfileType = 

                                _WorkMaterials[selected].Finish = 100;
                                _WorkMaterials[selected].IsExport = true;
                                _WorkMaterials[selected].Position = -2;
                                long cWork = _WorkOffset + (_WorkSize * selected);
                                using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                                {
                                    WriteCodesysMemor.SetMonitorWorkOffset(_WorkMaterials[selected].ToByteArray(), cWork); //發送加工陣列
                                }
                                STDSerialization ser = new STDSerialization();
                                ser.SetWorkMaterialBackup(_WorkMaterials[selected]);
                                Finish_UndoneDataViews[selected].Schedule = 100;
                                Finish_UndoneDataViews[selected].PositionEnum = PositionStatusEnum.完成;       //"完成";
                                if (!_Finish.Exists(x => x == (short)selected))
                                {
                                    _Finish.Add(Convert.ToInt16(selected));
                                }

                                if (!_SendIndex.Exists(x => x == (short)selected))
                                {
                                    _SendIndex.Add(Convert.ToInt16(selected));
                                }

                                AddOperatingLog(LogSourceEnum.Machine, $"將素材編號{Finish_UndoneDataViews[selected].MaterialNumber}設定為完成加工");
                            }
                            RefreshScheduleGridC();
                        }
                    }

                });
            }
        }

        public WPFBase.RelayParameterizedCommand DeleteCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(el =>
                {
                    var MDV_SelectedItems = el as IEnumerable<GD_STD.Data.MaterialDataView>;
                    if (MDV_SelectedItems != null)
                    {
                        foreach (var dataView in MDV_SelectedItems)
                        {
                            short[] index;
                            using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                            {
                                index = read.GetIndex();
                            }
                            int selected = Finish_UndoneDataViews.IndexOf(dataView);
                            int iIndex = Array.IndexOf(index, selected);
                            if (iIndex != -1)
                            {
                                long indexOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.Index)).ToInt64(); //index 偏移量
                                index[iIndex] = -1;
                                var value = index.Where(e => e != -1).ToArray();
                                var writeByte = value.ToByteArray();
                                using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                                    write.SetMonitorWorkOffset(writeByte, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置
                            }

                        }
                    }
                });

            }
        }

        //狀態註銷 復原素材狀態->會送資料給機台更改狀態 然後刪除備份檔案
        public WPFBase.RelayParameterizedCommand RecoverCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(el =>
                {
                    try
                    {
                        var MDV_SelectedItems = (el as IEnumerable<GD_STD.Data.MaterialDataView>).ToList();
                        if (MDV_SelectedItems.Exists(e => (e.PositionEnum == PositionStatusEnum.加工中)))
                        {
                            //若有包含加工中的檔案 詢問後做動作
                            MessageBoxResult messageBoxResult = WinUIMessageBox.Show(null,
                                $"復原的素材有包含正在加工中的素材，請確認是否要移除加工中的素材",
                                "通知",
                                MessageBoxButton.YesNoCancel,
                                MessageBoxImage.Warning,
                                MessageBoxResult.None,
                                MessageBoxOptions.ServiceNotification,
                                FloatingMode.Adorner);

                            if (messageBoxResult == MessageBoxResult.No)
                                MDV_SelectedItems.RemoveAll(e => (e.PositionEnum == PositionStatusEnum.加工中));

                            if (messageBoxResult == MessageBoxResult.Cancel)
                                return;
                        }

                        if (MDV_SelectedItems != null)
                        {
                            Task.Run(() =>
                            {
                                DevExpress.Xpf.Core.SplashScreenManager REProcessingScreenWin = DevExpress.Xpf.Core.SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });
                                REProcessingScreenWin.ViewModel.Status = $"正在註銷伺服器的零件...";
                                REProcessingScreenWin.Show();

                                if(MDV_SelectedItems.Count !=0)
                                    ClearAppServerPairWorkList(MDV_SelectedItems.Select(x => (x.MaterialNumber)).ToList());

                                Thread.Sleep(3000); //延遲清理
                                foreach (var Fu in MDV_SelectedItems)
                                {
                                    var Xindex = Finish_UndoneDataViews.FindIndex(x => x == Fu);
                                    if (Xindex != -1)
                                    {
                                        REProcessingScreenWin.ViewModel.Status = $"正在註銷機台端的加工資料{Fu.MaterialNumber}...";
                                        AddOperatingLog(LogSourceEnum.Machine, $"正在註銷機台端的加工資料{Fu.MaterialNumber}...");
                                        ClearMonitorWorkList(Xindex);
                                    }
                                }

                                REProcessingScreenWin.Close();

                            });
                        }


                    }
                    catch (Exception ex)
                    {
                        AddOperatingLog(LogSourceEnum.Machine, $"{ex.Message}");
                    }
                });
             }
        }


                #endregion


        private void VM_AssemblyPart(devDept.Eyeshot.Model model, string materialNumber)
        {
            ObSettingVM obvm = new ObSettingVM();
            //model.Clear();
            STDSerialization ser = new STDSerialization(); //序列化處理器
            ObservableCollection<MaterialDataView> materialDataViews = ser.GetMaterialDataView(); //序列化列表
            int index = materialDataViews.FindIndex(el => el.MaterialNumber == materialNumber);//序列化的列表索引
            MaterialDataView material = materialDataViews[index];
            ObservableCollection<SteelPart> parts = ser.GetPart(material.Profile.GetHashCode().ToString());//零件列表
            NcTempList ncTemps = ser.GetNcTempList(); //尚未實體化的nc檔案
            var _ = material.Parts.Select(x => x.PartNumber); //選擇要使用的零件編號
            var guid = (from el in ncTemps
                        where _.ToList().Contains(el.SteelAttr.PartNumber)
                        select el.SteelAttr.GUID.ToString()).ToList();//選擇使用的NC文件
            //產生nc檔案圖檔
            for (int i = 0; i < guid.Count; i++)
            {
                model.LoadNcToModel(guid[i], ObSettingVM.allowType);
            }


            var place = new List<(double Start, double End, bool IsCut, string Number)>();//放置位置參數
            place.Add((Start: 0, End: material.StartCut, IsCut: true, Number: "")); //素材起始切割物件
            Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
            for (int i = 0; i < material.Parts.Count; i++)
            {
                int partIndex = parts.FindIndex(el => el.Number == material.Parts[i].PartNumber); //回傳要使用的陣列位置
                if (partIndex == -1)
                {
                    // 未找到對應零件編號
                    string tmp = material.Parts[i].PartNumber;
                    WinUIMessageBox.Show(null,
                    $"未找到對應零件編號" + tmp,
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                    return;

                    //throw new Exception($"在 ObservableCollection<SteelPart> 找不到 {material.Parts[i].PartNumber}");
                }
                else
                {
                    double startCurrent = place[place.Count - 1].End,//當前物件放置起始點的座標
                                  endCurrent = startCurrent + parts[partIndex].Length;//當前物件放置結束點的座標
                    place.Add((Start: startCurrent, End: endCurrent, IsCut: false, Number: parts[partIndex].Number));
                    Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
                    //計算切割物件
                    double startCut = place[place.Count - 1].End, //當前切割物件放置起始點的座標
                                  endCut;//當前切割物件放置結束點的座標
                    if (i + 1 >= material.Parts.Count) //下一次迴圈結束
                    {
                        //endCut = material.LengthStr + material.StartCut + material.EndCut;//當前切割物件放置結束點的座標
                        endCut = material.LengthStr;// - material.StartCut - material.EndCut;//當前切割物件放置結束點的座標
                    }
                    else //下一次迴圈尚未結束
                    {
                        endCut = startCut + material.Cut;//當前切割物件放置結束點的座標
                    }
                    place.Add((Start: startCut, End: endCut, IsCut: true, Number: "")); //素材零件位置
                    Debug.WriteLine($"Start = {place[place.Count - 1].Start}, End : {place[place.Count - 1].End}, IsCut : {place[place.Count - 1].IsCut}");//除錯工具
                }
            }
            EntityList entities = new EntityList();
            for (int i = 0; i < place.Count; i++)
            {
                if (place.Count == 1) // count=1表素材沒有零件組成,只有(前端切除)程序紀錄
                {
                    return;
                }


                if (place[i].IsCut) //如果是切割物件
                {
                    Entity cut1 = VM_DrawCutMesh(parts[0], model, place[i].Start, place[i].End, "Cut");
                    if (cut1 != null)
                    {
                        entities.Add(cut1);
                    }

                    continue;
                }
                int placeIndex = place.FindIndex(el => el.Number == place[i].Number); //如果有重複的編號只會回傳第一個，以這個下去做比較。
                if (placeIndex != i) //如果 i != 第一次出現的 index 代表需要使用複製
                {
                    EntityList ent = new EntityList();
                    entities.
                        Where(el => el.GroupIndex == placeIndex).
                        ForEach(el =>
                        {
                            Entity copy = (Entity)el.Clone(); //複製物件
                            copy.GroupIndex = i;
                            copy.Translate(place[i].Start - place[placeIndex].Start, 0);
                            ent.Add(copy);
                        });
                    entities.AddRange(ent);
                }
                else
                {
                    int partIndex = parts.FindIndex(el => el.Number == place[i].Number);
                    if (parts[partIndex].GUID.ToString() != "") //如果圖面檔案
                    {
                        var DataName = parts[partIndex].GUID.ToString();
                        ReadFile file = ser.ReadPartModel(DataName); //讀取檔案內容
                        if (file == null)
                        {
                            var Path = ApplicationVM.DirectoryDevPart();
                            var _path = string.Empty;
                            foreach (var p in Path)
                            {
                                switch (p)
                                {
                                    case '/':
                                        break;

                                    default:
                                        _path += p;
                                        break;
                                }
                            }

                            WinUIMessageBox.Show(null,
                                $"Dev_Part資料:{_path}\\{DataName}.dm\r\n讀取失敗",
                                "通知",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation,
                                MessageBoxResult.None,
                                MessageBoxOptions.None,
                                 FloatingMode.Window);
                            return;
                        }
                        file.DoWork();
                        file.AddToScene(model);
                        Entity _entity = null;
                        SteelAttr _steelAttr = null;
                        model.Entities.ForEach(el =>
                        {
                            if (el.GetType() != typeof(LinearDim))
                            {
                                model.Blocks[((BlockReference)el).BlockName].Entities.ForEach(entity =>
                                {
                                    if (entity.EntityData is SteelAttr steelAttr)
                                    {
                                        _entity = entity;
                                        _steelAttr = steelAttr;
                                    }
                                });
                                el.GroupIndex = i;
                                el.Translate(place[i].Start, 0);
                                el.Selectable = false;
                                entities.Add(el);//加入到暫存列表
                            }
                        });
                        //Func<List<Point3D>, double> minFunc = (point3d) => point3d.Min(e => e.X);
                        //Func<List<Point3D>, double> maxFunc = (point3d) => point3d.Max(e => e.X);
                        //Transformation transformation = new Transformation(new Point3D(0, _steelAttr.H / 2, _steelAttr.W), Vector3D.AxisX, new Vector3D(0, 0, 1), new Vector3D(0, -1, 0));

                        //var minTopFlip = Flip((Entity)_entity.Clone(), minFunc, _steelAttr, -100, out double minTopAngle1, out double minTopAngle2, out double topMinX);
                        //var maxTopFlip = Flip((Entity)_entity.Clone(), maxFunc, _steelAttr, 100, out double maxTopAngle1, out double maxTopAngle2, out double topMaxX);
                        //var minBackFlip = Flip((Entity)_entity.Clone(), minFunc, _steelAttr, -100, out double minBackAngle1, out double minBackAngle2, out double backMinX, transformation);
                        //var maxBackFlip = Flip((Entity)_entity.Clone(), maxFunc, _steelAttr, 100, out double maxBackAngle1, out double maxBackAngle2, out double backMaxX, transformation);
                        //var minFrontFlip = Flip((Entity)_entity.Clone(), minFunc, _steelAttr, -100, out double minFrontAngle1, out double minFrontAngle2, out double frontMinX, transformation);
                        //var maxFrontFlip = Flip((Entity)_entity.Clone(), maxFunc, _steelAttr, 100, out double maxFrontAngle1, out double maxFrontAngle2, out double frontMaxX, transformation);
                        //if ((minBackFlip && minFrontFlip) || (maxBackFlip && maxFrontFlip))
                        //{
                        //    if (minTopFlip || maxTopFlip)
                        //    {
                        //        _steelAttr.StartAngle = minTopAngle1 == 0 ? minTopAngle2 : minTopAngle1;
                        //        _steelAttr.EndAngle = maxTopAngle1 == 0 ? maxTopAngle2 : maxTopAngle1;
                        //        if (_steelAttr.StartAngle > 90 || _steelAttr.EndAngle > 90)
                        //        {
                        //            Rotate(entities.Where(el => el.GroupIndex == i), (_entity.BoxMin + _entity.BoxMax) / 2);
                        //        }
                        //    }
                        //    else if (backMinX < frontMinX)
                        //        _steelAttr.StartAngle = minBackAngle1 == 0 ? minBackAngle2 : minBackAngle1;
                        //    else
                        //        _steelAttr.StartAngle = minFrontAngle1 == 0 ? minFrontAngle2 : minFrontAngle1;

                        //    if (backMaxX < frontMaxX)
                        //    {
                        //        _steelAttr.EndAngle = maxBackAngle1 == 0 ? maxBackAngle2 : maxBackAngle1;
                        //    }
                        //    else
                        //    {
                        //        _steelAttr.EndAngle = maxFrontAngle1 == 0 ? maxFrontAngle2 : maxFrontAngle1;
                        //    }
                        //}
                        //else if (minBackFlip || maxBackFlip)
                        //{
                        //    _steelAttr.StartAngle = minBackAngle1 == 0 ? minBackAngle2 : minBackAngle1;
                        //    _steelAttr.EndAngle = maxBackAngle1 == 0 ? maxBackAngle2 : maxBackAngle1;
                        //}
                        //else if (minFrontFlip || maxFrontFlip)
                        //{
                        //    _steelAttr.StartAngle = minFrontAngle1 == 0 ? minFrontAngle2 : minFrontAngle1;
                        //    _steelAttr.EndAngle = maxFrontAngle1 == 0 ? maxFrontAngle2 : maxFrontAngle1;
                        //}
                        //else if (minTopFlip || maxTopFlip)
                        //{
                        //    _steelAttr.StartAngle = minTopAngle1 == 0 ? minTopAngle2 : minTopAngle1;
                        //    _steelAttr.EndAngle = maxTopAngle1 == 0 ? maxTopAngle2 : maxTopAngle1;
                        //    if (_steelAttr.StartAngle > 90 || _steelAttr.EndAngle > 90)
                        //    {
                        //        Rotate(entities.Where(el => el.GroupIndex == i), (_entity.BoxMin + _entity.BoxMax) / 2);
                        //    }
                        //}
                    }
                    else //如果沒有 NC 檔案
                    {
                        SteelAttr steelAttr = new SteelAttr(parts[partIndex]); //產生物件設定檔
                        steelAttr.GUID = parts[partIndex].GUID = Guid.NewGuid(); //賦予新的guid
                        Steel3DBlock steel = Steel3DBlock.AddSteel(steelAttr, model, out BlockReference blockReference); //加入鋼構物件到 Model
                        blockReference.Translate(place[i].Start, 0);//移動目標
                        blockReference.Selectable = true;
                        entities.Add(blockReference); //加入到暫存列表
                        ser.SetPart(material.Profile.GetHashCode().ToString(), new ObservableCollection<object>(parts));//存取
                        ser.SetPartModel(steelAttr.GUID.ToString(), model);//儲存模型
                        ObservableCollection<DataCorrespond> dataCorrespond = ser.GetDataCorrespond();//零件清單
                        dataCorrespond.Add(new DataCorrespond()
                        {
                            Number = steelAttr.PartNumber,
                            DataName = steelAttr.GUID.ToString(),
                            Profile = steelAttr.Profile,
                            Type = steelAttr.Type,
                            TP = false,

                            // 2022/09/08 彥谷
                            oPoint = steelAttr.oPoint.ToArray(),
                            vPoint = steelAttr.vPoint.ToArray(),
                            uPoint = steelAttr.uPoint.ToArray(),
                        });
                        ser.SetDataCorrespond(dataCorrespond);//加入到製品零件清單
                    }
                }
            }
            //model.AssemblySelectionMode = devDept.Eyeshot.Environment.assemblySelectionType.Leaf;
            //model.Refresh();
            model.Entities.Clear();
            SteelAttr attr = new SteelAttr(parts[0]);
            attr.Length = material.LengthStr;
            Mesh cut = Steel3DBlock.GetProfile(attr);
            Solid solid = cut.ConvertToSolid();
            List<Solid> resultSolid = new List<Solid>();
            //foreach (BlockReference item in entities)
            //{

            //    if (item.Attributes.ContainsKey("Steel"))
            //    {
            //        Mesh main = ((Mesh)model.Blocks[item.BlockName].Entities[0]);
            //        main.Weld();
            //        Solid[] solids = Solid.Difference(solid, ((Mesh)model.Blocks[item.BlockName].Entities[0]).ConvertToSolid());
            //        if (solids.Length > 0)
            //        {
            //            resultSolid.Add(solids[0]);
            //            if (solids.Length >= 2)
            //            {
            //                solid = solids[1];
            //            }
            //        }
            //    }


            //}
            //EntityList result =  entities.Where(el => !((BlockReference)el).Attributes.ContainsKey("Cut")).ToList();
            model.Entities.AddRange(entities);
            model.Entities.AddRange(resultSolid);
            ser.SetMaterialModel(materialNumber, model); //儲存素材
        }


        private Entity VM_DrawCutMesh(SteelPart part, devDept.Eyeshot.Model model, double end, double start, string dic)
        {
            SteelAttr steelAttr = new SteelAttr(part);
            steelAttr.Length = end - start;
            if (steelAttr.Length == 0)
            {
                return null;
            }
            Steel3DBlock.AddSteel(steelAttr, model, out BlockReference result, dic);
            model.Blocks[steelAttr.GUID.ToString()].Entities[0].Color = System.Drawing.ColorTranslator.FromHtml(WPFSTD105.Properties.SofSetting.Default.Surplus);
            result.GroupIndex = int.MaxValue;
            result.Translate(start, 0);
            return result;
        }

        private bool TaskBoolean = true;

        /// <summary>
        /// 寫入加工參數到 Codesys <see cref="GD_STD.Phone.MonitorWork"/>
        /// </summary>
        /// <returns></returns>
        void WriteCodesys()
        {
            const string ProcessingScreenWinPrefix = "寫入Codesys";
            DevExpress.Xpf.Core.SplashScreenManager ProcessingScreenWin = DevExpress.Xpf.Core.SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });
            ProcessingScreenWin.ViewModel.Status = $"{ProcessingScreenWinPrefix}...";
            ProcessingScreenWin.Show();
            //await Task.Yield();
            //設定 Codesys MonitorWork 專案訊息
            ProcessingScreenWin.ViewModel.Status = $"{ProcessingScreenWinPrefix} - 專案訊息";
            using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
            using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
            {
                Type type = typeof(MonitorWork);
                long indexOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.Index)).ToInt64(); //index 偏移量
                long enOccupyOffset = Marshal.OffsetOf(type, nameof(MonitorWork.EntranceOccupy)).ToInt64(); //入口移動料架占用長度記憶體起始位置偏移位置
                long exOccupy1Offset = Marshal.OffsetOf(type, nameof(MonitorWork.ExportOccupy1)).ToInt64(); //出口移動料架占用長度 (1) 記憶體起始位置偏移位置
                long exOccupy2Offset = Marshal.OffsetOf(type, nameof(MonitorWork.ExportOccupy2)).ToInt64(); //出口移動料架占用長度 (2) 記憶體起始位置偏移位置
                long currentOffset = Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Current)).ToInt64();//當前值偏移量
                double enOccupy = 0;//入口移動料架占用長度
                double exOccupy1 = 0;//出口移動料架占用長度 (1) 
                double exOccupy2 = 0;//出口移動料架占用長度 (2)

                WorkOther workOther = null; //其他備份檔
                short current = -1;
                short[] index = new short[type.ArrayLength(nameof(MonitorWork.Index))] //初始化 index
                                                                        .Select(el => el = -1)
                                                                        .ToArray();
                if (File.Exists(ApplicationVM.WorkMaterialOtherBackup())) //如果有其他資訊備份檔
                {
                    STDSerialization ser = new STDSerialization();
                    workOther = ser.GetWorkMaterialOtherBackup();
                    if (workOther != null)
                    {
                        current = workOther.Current;
                        enOccupy = workOther.EntranceOccupy;
                        exOccupy1 = workOther.ExportOccupy1;
                        exOccupy2 = workOther.ExportOccupy2;
                    }
                }
                if (File.Exists(ApplicationVM.WorkMaterialIndexBackup())) //如果有 index 備份檔
                {
                    STDSerialization ser = new STDSerialization();
                    short[] dataIndex = ser.GetWorkMaterialIndexBackup();
                    if (dataIndex.Length != 0)
                    {
                        if (ApplicationViewModel.ProjectName == read.GetProjectName()) //如果相同專案名稱
                        {
                            Array.Copy(dataIndex, index, dataIndex.Length); //複製備份檔的 index 到要發送的 index
                        }
                        else //如果不同專案名稱
                        {
                            Array.Copy(dataIndex, index, current == -1 ? 0 : current + 1); //複製備份檔的 index 到要發送的 index
                        }
                    }
                }

                if (ApplicationViewModel.ProjectName != read.GetProjectName()) //如果相同專案
                {
                    byte[] project = ApplicationViewModel.ProjectName.ToByteArray(); //目前專案名稱
                    write.SetMonitorWorkOffset(project, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.ProjectName)).ToInt64()); //寫入專案名稱
                }

                MCurrent = current;

                ProcessingScreenWin.ViewModel.Status = $"寫入Current{current}";

                write.SetMonitorWorkOffset(current.ToByteArray(), currentOffset);//寫入Current
                write.SetMonitorWorkOffset(enOccupy.ToByteArray(), enOccupyOffset); //寫入入口料架占用長度
                write.SetMonitorWorkOffset(exOccupy1.ToByteArray(), exOccupy1Offset);//寫入出口料架占用長度 (1) 
                write.SetMonitorWorkOffset(exOccupy2.ToByteArray(), exOccupy2Offset);//寫入出口料架占用長度 (2) 
                write.SetMonitorWorkOffset(Convert.ToInt16(Finish_UndoneDataViews.Count).ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Count)).ToInt64()); //寫入準備加工的陣列位置
                write.SetMonitorWorkOffset(index.ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置

                if (index.Count() > 0)
                {
                    var workstring = "";
                    index.ForEach(x => workstring += (x.ToString() + ","));
                    workstring = workstring.Trim(',');
                    ProcessingScreenWin.ViewModel.Status = $"寫入加工陣列{index}";
                }
            }


            ProcessingScreenWin.ViewModel.Status = $"{ProcessingScreenWinPrefix} - 專案清單";
            ProcessingScreenWin.ViewModel.IsIndeterminate= false;
            int workIndex = 0;
            foreach (var view in Finish_UndoneDataViews)
            {
                #region 目前偏移量
                long cWork = _WorkOffset + _WorkSize * workIndex;
                long cMaterialNumber = cWork + _MaterialNumberOffset;
                long cProfile = cWork + _ProfileOffset;
                long cPartNumber = cWork + _PartNumberOffset;
                long cLength = cWork + _LengthOffset;
                long cMaterial = cWork + _MaterialOffset;
                long cAssemblyNumber = cWork + _AssemblyNumberOffset;
                long cH = cWork + _hOffset;
                long cW = cWork + _wOffset;
                long ct1 = cWork + _t1Offset;
                long ct2 = cWork + _t2Offset;
                long cGuid = cWork + _GuidOffset;
                long cObjectType = cWork + _ObjectTypeOffset;
                #endregion
                STDSerialization ser = new STDSerialization();
                var SteelPartCollection = ser.GetPart($"{view.Profile.GetHashCode()}");
                if (SteelPartCollection != null && view.Parts.Count != 0)
                {
                    var steelPart = SteelPartCollection[0];
                    using (Memor.WriteMemorClient Write = new Memor.WriteMemorClient())
                    {
                        ProcessingScreenWin.ViewModel.Status = $"{ProcessingScreenWinPrefix} - 寫入素材編號{view.MaterialNumber} ({workIndex+1}/{Finish_UndoneDataViews.Count})";
                        ProcessingScreenWin.ViewModel.Progress = ((double)(workIndex))*100 / ((double)Finish_UndoneDataViews.Count);

                        Write.SetMonitorWorkOffset(view.MaterialNumber.ToByteArray(), cMaterialNumber);//寫入素材編號
                        Write.SetMonitorWorkOffset(Encoding.ASCII.GetBytes(view.Profile), cProfile);//寫入斷面規格
                        Write.SetMonitorWorkOffset(view.Parts.Select(el => el.PartNumber).Aggregate((str1, str2) => $"{str1},{str2}").ToByteArray(), cPartNumber);//寫入零件編號
                        Write.SetMonitorWorkOffset(view.LengthStr.ToByteArray(), cLength);//寫入素材長度
                        Write.SetMonitorWorkOffset(Encoding.ASCII.GetBytes(view.Material == null ? "0" : view.Material), (_WorkOffset + _WorkSize * workIndex) + _MaterialOffset);//寫入材質
                        //溢位?
                        //Write.SetMonitorWorkOffset(view.Parts.Select(el => el.AssemblyNumber).Aggregate((str1, str2) => $"{str1},{str2}").ToByteArray(), cAssemblyNumber);//寫入構件編號
                        Write.SetMonitorWorkOffset(steelPart.H.ToByteArray(), cH);//寫入高度
                        Write.SetMonitorWorkOffset(steelPart.W.ToByteArray(), cW);//寫入寬度
                        Write.SetMonitorWorkOffset(steelPart.t1.ToByteArray(), ct1);//寫入腹板厚度
                        Write.SetMonitorWorkOffset(steelPart.t2.ToByteArray(), ct2);//寫入翼板厚度
                        Write.SetMonitorWorkOffset(Encoding.ASCII.GetBytes(view.MaterialNumber), cGuid);//寫入圖面 GUID

                        PROFILE_TYPE ObjectPROFILE_TYPE = PROFILE_TYPE.H;

                        switch (view.ObjectType)
                        {
                            case OBJECT_TYPE.RH:
                            case OBJECT_TYPE.BH:
                            case OBJECT_TYPE.H:
                                ObjectPROFILE_TYPE = PROFILE_TYPE.H;
                                break;
                            case OBJECT_TYPE.LB:
                            case OBJECT_TYPE.CH:
                                ObjectPROFILE_TYPE = PROFILE_TYPE.U;
                                break;
                            case OBJECT_TYPE.BOX:
                            case OBJECT_TYPE.TUBE:
                                ObjectPROFILE_TYPE = PROFILE_TYPE.BOX;
                                break;
                            case OBJECT_TYPE.L:
                                ObjectPROFILE_TYPE = PROFILE_TYPE.L; 
                                break;
                            case OBJECT_TYPE.PLATE:
                                ObjectPROFILE_TYPE = PROFILE_TYPE.PLATE;
                                break;
                            default:
                                AddOperatingLog(LogSourceEnum.Init, $"斷面規格{view.ObjectType}無法轉換成OBJECT_TYPE", true);
                                break;
                        }

                        Write.SetMonitorWorkOffset(((int)ObjectPROFILE_TYPE).ToByteArray(), cObjectType);//寫入型鋼類型

                        var a = Encoding.ASCII.GetBytes(view.Profile);
                        var p1 = (view.Parts.Select(el => el.AssemblyNumber).Aggregate((str1, str2) => $"{str1},{str2}"));
                        var p2 = (view.Parts.Select(el => el.AssemblyNumber).Aggregate((str1, str2) => $"{str1},{str2}").ToByteArray());

                        ProcessingScreenWin.ViewModel.Progress = ((double)(workIndex + 1))*100 / ((double)Finish_UndoneDataViews.Count);
                    }
                }
                workIndex++;
            }
            ProcessingScreenWin.ViewModel.IsIndeterminate = true;
            ProcessingScreenWin.ViewModel.Status = "寫入Codesys資料 - ";
            ProcessingScreenWin.Close();
        }
        /// <summary>
        /// 發送軸向工作
        /// </summary>
        /// <param name="boltsCountOffset">螺栓數量記憶體偏移量</param>
        /// <param name="drOffset">鑽孔陣列記憶體偏移量</param>
        /// <param name="drills">鑽孔資訊列表</param>
        /// <param name="steelPart">零件資訊</param>
        /// <param name="cWork">目前陣列記憶體偏移量</param>
        /// <param name="cutPointX">切割點 X 座標</param>
        /// <param name="h">寬度</param>
        private void SendDrillToMachine(long boltsCountOffset, long drOffset, List<Drill> drills, SteelPart steelPart, long cWork, double h, double length, double[] cutPointX = null)
        {
            List<Drill> dList = new List<Drill>();
            foreach(var el in drills)
            {
                if (dList.FindIndex(e => el.X == e.X && e.Y == el.Y) == -1)
                {
                    dList.Add(el);
                }
            }

            cutPointX?.ForEach(el =>
            {
                if (el > 0 && el < length)
                {
                    dList.Add(new Drill { X = el, Y = h / 3 * 1, AXIS_MODE = AXIS_MODE.POINT });
                    dList.Add(new Drill { X = el, Y = h / 3 * 2, AXIS_MODE = AXIS_MODE.POINT });
                }
            });
            long cBoltsL = cWork + boltsCountOffset;
            long cDrillL = cWork + drOffset;
            Drill[] drillArray = DrillSort(dList).ToArray();

            using (Memor.WriteMemorClient Write = new Memor.WriteMemorClient())
            {
                WriteCodesysMemor.SetMonitorWorkOffset(Convert.ToUInt16(drillArray.Length).ToByteArray(), cBoltsL);//軸加工陣列數量
                WriteCodesysMemor.SetMonitorWorkOffset(drillArray.ToByteArray(), cDrillL);//軸加工陣列
            }
        }

        private List<Drill> DrillSort(List<Drill> list)
        {
            bool reverse = false; //反轉
            List<Drill> result = new List<Drill>();

            foreach (var item in list.GroupBy(el => el.X).OrderBy(el => el.Key))
            {
                List<Drill> _ = item.ToList();
                _.Sort(Compare);
                if (reverse)
                {
                    _.Reverse();
                }
                result.AddRange(_);
                reverse = !reverse;
            }
            return result;
        }



        /// <summary>
        /// 發送加工訊息
        /// </summary>
        private bool SendDrill(int index , out bool isPinMode)
        {
            isPinMode = false;
            _CreateDMFileTask?.Wait(); //等待 Task CreateFile 完成 link:ProcessingMonitorVM.cs:CreateFile()
            try
            {
                var view = Finish_UndoneDataViews[index];
                if (File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{view.MaterialNumber}.dm"))
                {
                    if (!MachineDrillkeyValueDict.ContainsKey(view.MaterialNumber))
                    {
                        MachineDrillkeyValueDict[view.MaterialNumber] = GenerateMachiningDataPairs(view);
                    }
                   // var keyValuePairs = MachineDrillkeyValueDict[view.MaterialNumber];
                    //實際加工的資料 
                    var ActualDrillDict = new Dictionary<FACE, List<Drill>>();
                    //使用判斷式只保留要加工的孔位/加工方法
                    if (AllDrillBoltsDict.TryGetValue(Finish_UndoneDataViews[index].MaterialNumber, out var DrillBoltC))
                    {
                        foreach (var el in DrillBoltC)
                        {
                            isPinMode = el.Value.PinTestMode;

                            foreach (var DB in el.Value.DrillBoltList)
                            {
                                //要加入的加工孔位
                                if (DB.DrillWork)
                                {
                                    if (!ActualDrillDict.ContainsKey(el.Key))
                                        ActualDrillDict[el.Key] = new List<Drill>();
                                    try
                                    {
                                        //以db的原始孔位做比較
                                        var Addrange = MachineDrillkeyValueDict[view.MaterialNumber][el.Key].FindAll(x => (x.Dia == DB.Origin_DrillHoleDiameter&& x.AXIS_MODE == DB.Origin_WorkAXIS_MODE)).ToArray();

                                        for (int i = 0; i < Addrange.Count(); i++)
                                        {
                                            //如果有更改過孔位
                                            //保險起見 事實上DrillHoleDiameter已有切換孔的功能
                                            if (DB.DrillHoleDiameterIsChangeBool)
                                                Addrange[i].Dia = DB.DrillHoleDiameter;
                                            //保險起見 事實上WorkAXIS_modeIsChanged已有切換的功能
                                            if (DB.WorkAXIS_modeIsChanged)
                                                Addrange[i].AXIS_MODE = DB.Actual_WorkAXIS_MODE;
                                        }

                                        ActualDrillDict[el.Key].AddRange(Addrange);

                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ActualDrillDict = new Dictionary<FACE, List<Drill>>(MachineDrillkeyValueDict[view.MaterialNumber]);
                    }

                    STDSerialization ser = new STDSerialization();
                    var steelPart = ser.GetPart($"{view.Profile.GetHashCode()}")[0];


                    //把加工種類狀態記錄進表格
                    long cWork = _WorkOffset + _WorkSize * index;
                    if (ActualDrillDict.ContainsKey(FACE.FRONT))
                        SendDrillToMachine(_BoltsCountLOffset, _DrLOffset, ActualDrillDict[FACE.FRONT], steelPart, cWork, steelPart.W, view.LengthStr, null);
                    else
                        SendDrillToMachine(_BoltsCountLOffset, _DrLOffset, new List<Drill>(), steelPart, cWork, steelPart.W, view.LengthStr, null);

                    if (ActualDrillDict.ContainsKey(FACE.TOP))
                        SendDrillToMachine(_BoltsCountMOffset, _DrMOffset, ActualDrillDict[FACE.TOP], steelPart, cWork, steelPart.H, view.LengthStr, null);
                    else
                        SendDrillToMachine(_BoltsCountLOffset, _DrMOffset, new List<Drill>(), steelPart, cWork, steelPart.W,  view.LengthStr, null);

                    if (ActualDrillDict.ContainsKey(FACE.BACK))
                        SendDrillToMachine(_BoltsCountROffset, _DrROffset, ActualDrillDict[FACE.BACK], steelPart, cWork, steelPart.W, view.LengthStr, null);
                    else
                        SendDrillToMachine(_BoltsCountLOffset, _DrROffset, new List<Drill>(), steelPart, cWork, steelPart.W, view.LengthStr, null);
                }
                else
                {
                    AddOperatingLog(LogSourceEnum.Software, $"找不到{view.MaterialNumber}.dm", true);
                }
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }


        /// <summary>
        /// 排序 y 向鑽孔
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int Compare(Drill a, Drill b)
        {
            if (a.Y < b.Y)
            {
                return 1;
            }
            else if (a.Y > b.Y)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// <see cref="devDept.Eyeshot.Entities.Entity"/> 轉換 <see cref="GD_STD.Drill"/>
        /// </summary>
        /// <param name="IsTrialProcessing">試加工模式下會將孔轉換成點 並丟棄其他加工方法</param>
        /// <param name="entities">3D 模型物件列表</param>
        private List<Drill> BoltAsDrill(Entity[] entities, Transformation transformation = null)
        {
            if (!entities.Any(el => el.EntityData is BoltAttr))
                throw new Exception("entities.EntityData 有查詢到非 BoltAttr 的類型。");

            var drills = new Drill[entities.Length];
            Parallel.For(0, entities.Length, i =>
            {
                BoltAttr attr = (BoltAttr)entities[i].EntityData;
                Utility.ComputeBoundingBox(transformation, entities[i].Vertices, out Point3D boxMin, out Point3D boxMax);
                Point3D center = (boxMin + boxMax) / 2;

                switch (attr.Face)
                {
                    case GD_STD.Enum.FACE.TOP:
                        drills[i].X = center.X;
                        drills[i].Y = center.Y;
                        break;
                    case GD_STD.Enum.FACE.FRONT:
                    case GD_STD.Enum.FACE.BACK:
                        drills[i].X = center.X;
                        drills[i].Y = center.Z;
                        break;
                    default:
                        break;
                }
                drills[i].Dia = attr.Dia;
                drills[i].AXIS_MODE = attr.Mode;
            });

            //只保留特定半徑之功能(只看刀徑 不管加工類型)
            if (false)
            {
                var DrillList = new List<Drill>();

                foreach (var el in drills)
                {
                    //之後改成指定刀徑

                    //鑽頭可對應鑽孔 打點等功能
                    if ((el.AXIS_MODE == AXIS_MODE.PIERCE || el.AXIS_MODE == AXIS_MODE.POINT) && el.Dia == 10)
                    {
                        DrillList.Add(el);
                    }
                    else
                    {

                    }
                }
                drills = DrillList.ToArray();
            }


            //將孔轉換成點 功能
            if (DrillPin_Mode_RadioButtonIsEnable)
            {
                var DrillList = new List<Drill>();

                //foreach(var el in drills)
                for (int i =0; i < drills.Length;i++)
                {
                    if (drills[i].AXIS_MODE == AXIS_MODE.PIERCE)
                    {
                        drills[i].AXIS_MODE = AXIS_MODE.POINT;
                    }

                    if (drills[i].AXIS_MODE == AXIS_MODE.POINT)
                    {
                        DrillList.Add(drills[i]);
                    }
                }
                //將孔加工轉換成點 並丟棄其他加工方式
                return DrillList;
            }
            else
            {
                return drills.ToList();
            }
        }










        /// <summary>
        /// 產生 <see cref="MaterialDataView"/> 所有dm檔
        /// </summary>
         private async void CreateDMFile(WPFSTD105.ModelExt _Model)
        {

            STDSerialization ser = new STDSerialization();

            DevExpress.Xpf.Core.SplashScreenManager ProcessingScreenWin = DevExpress.Xpf.Core.SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });
            ProcessingScreenWin.ViewModel.Status = "正在產生缺少的素材3D視圖";
            ProcessingScreenWin.Show();

            List<string> NoneDmFileList = new List<string>();
            foreach (var el in Finish_UndoneDataViews)
                if (!File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{el.MaterialNumber}.dm"))
                    NoneDmFileList.Add(el.MaterialNumber);

            int ProcessCount = 1;
            foreach (var el_MaterialNumber in NoneDmFileList) //產生素材3D視圖
            {
                if (!File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{el_MaterialNumber}.dm"))
                {
                    _SynchronizationContext.Send(t =>
                    {
                        ProcessingScreenWin.ViewModel.IsIndeterminate = false;
                        ProcessingScreenWin.ViewModel.Status = $"產生{el_MaterialNumber}.dm中 ..{ProcessCount}/{NoneDmFileList.Count()}";
                        ProcessingScreenWin.ViewModel.Progress = (ProcessCount * 100) / NoneDmFileList.Count;
                        int RetryCount = 0;
                        while (RetryCount < 10)
                        {
                            try
                            {
                                _Model.Clear();
                                _Model.AssemblyPart(el_MaterialNumber);

                                ser.SetMaterialModel(el_MaterialNumber, _Model);//儲存 3d 視圖
                                break;
                            }
                            catch (Exception ex)
                            {
                                RetryCount++;
                            }
                        }


                        ProcessCount++;

                    }, null);
                }
            }


            ProcessingScreenWin.ViewModel.IsIndeterminate = true;
            ProcessingScreenWin.Close();

            _SynchronizationContext.Send(t => _Model.Clear(), null);
            await Task.Yield();
            ser.SetMaterialDataView(Finish_UndoneDataViews);
        }




        /// <summary>
        /// 執行與釋放 (Free)、釋放 (Release) 或重設 Unmanaged 資源相關聯之應用程式定義的工作。
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_DsposedValue)
            {
                if (disposing)
                {
                    if (_SerializationThread != null)
                    {
                        _SerializationThread.Abort();
                    }
                    if (_HostThread != null)
                    {
                        _HostThread.Abort();
                    }
                    _CreateDMFileTask?.Dispose();
                    _WriteCodesysTask?.Dispose();

                    TaskBoolean = false;
                }
                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                _DsposedValue = true;
            }
        }
        /// <summary>
        /// 顯示/隱藏3D立體圖
        /// </summary>
        public ICommand Display3DViewerCommand { get; set; }
        private WPFBase.RelayCommand Display3DViewer()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (ThreeDimensionalDisplayControl)
                    ThreeDimensionalDisplayControl = false;
                else
                    ThreeDimensionalDisplayControl = true;
            });
        }


        #region app server 溝通按鈕

        /// <summary>
        /// 切換到橫移料架-台車
        /// </summary>
        public ICommand ChangeTransportModeCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    HintStep4 = false;
                    Transport_RadioButtonIsChecked = false;

                    ProgressBar_Visible_Transport_Visibility = true;
                    Transport_RadioButtonIsEnable = false;
                    Transport_by_Continue_RadioButtonIsEnable = false;
                    Transport_by_Hand_RadioButtonIsEnable = false;

                    Task.Run(() =>
                    {
                        Thread.Sleep(100);
                        try
                        {
                            if (GD_STD.Properties.Optional.Default.HandAuto)
                            {
                                STDSerialization ser = new STDSerialization();
                                FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();//讀取記憶體

                                if (!optionSettings.HandAuto)
                                {
                                    AddOperatingLog(LogSourceEnum.Software, "手臂切換回自動模式");
                                    optionSettings.HandAuto = true; //修改參數
                                    ser.SetOptionSettings(optionSettings);//寫入記憶體
                                    Thread.Sleep(500);
                                    MecOptional mecOptional = JsonConvert.DeserializeObject<MecOptional>(optionSettings.ToString());
                                    WriteCodesysMemor.SetMecOptional(mecOptional);//寫入記憶體
                                } 
                            }

                        }
                        catch (Exception Ex)
                        {
                            AddOperatingLog(LogSourceEnum.Software, "手臂切換為自動模式失敗", true);
                            AddOperatingLog(LogSourceEnum.Software, Ex.Message, true);
                        }



                        try
                        {
                            //台車
                            ushort[] reply = null;
                            using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                            using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                            {
                                Type type = typeof(MonitorWork);
                                write.SetMonitorWorkOffset(new byte[1] { 1 }, Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.Move)).ToInt64());//呼叫台車
                                reply = read.GetInstantMessage();
                            }
                            var _ = reply.Where(el => el != 0); //Codesys 回復訊息
                            if (_.Count() > 0)
                            {
                                char[] chars = _.Select(el => Convert.ToChar(el)).ToArray(); //轉換字元
                                string str = new string(chars);//轉換文字
                                                               //System.Windows.MessageBox.Show($"{str}", "通知", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information, System.Windows.MessageBoxResult.None, System.Windows.MessageBoxOptions.ServiceNotification);
                                ShowMessageBox(obj as UIElement, str);
                            }

                            AddOperatingLog(LogSourceEnum.Software, "切換到橫移料架");
                            //失敗時跳出提示 成功時則繼續執行
                            Transport_RadioButtonIsChecked = true;
                            HintStep4 = true;
                        }
                        catch (Exception ex)
                        {
                            if (obj is UIElement)
                            {
                                ShowMessageBox(obj as UIElement, "無法切換到橫移料架，請重試一次");
                            }
                            AddOperatingLog(LogSourceEnum.Software, "切換到橫移料架失敗", true);
                            AddOperatingLog(LogSourceEnum.Software, ex.Message, true);
                        }
                        ProgressBar_Visible_Transport_Visibility = false;
                        Transport_RadioButtonIsEnable = true;
                        Transport_by_Continue_RadioButtonIsEnable = true;
                        Transport_by_Hand_RadioButtonIsEnable = true;

                    });
                });
            }
        }
        /// <summary>
        /// 新續接命令
        /// </summary>
        public ICommand ChangeTransportMode_To_Continue_Command
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    HintStep4 = false;
                    ProgressBar_Visible_Transport_by_Continue_Visibility = true;
                    Transport_by_Continue_RadioButtonIsChecked = false;
                    Transport_RadioButtonIsEnable = false;
                    Transport_by_Continue_RadioButtonIsEnable = false;
                    Transport_by_Hand_RadioButtonIsEnable = false;

                    Task.Run(() =>
                    {
                        //計時器 單位為毫秒
                        Thread.Sleep(100);
                        //對機台下命令 並回傳是否成功，當成功時IsChecked=true 否則IsChecked = False
                        //等待sleep完成

                        try
                        {
                            if (GD_STD.Properties.Optional.Default.HandAuto)
                            {
                                STDSerialization ser = new STDSerialization();
                                //FluentAPI.MecSetting mecSetting = ser.GetMecSetting();
                                FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();//讀取記憶體

                                if (!optionSettings.HandAuto)
                                {
                                    AddOperatingLog(LogSourceEnum.Software, "手臂切換回自動模式");
                                    optionSettings.HandAuto = true; //修改參數
                                    ser.SetOptionSettings(optionSettings);//寫入記憶體
                                    Thread.Sleep(500);
                                    MecOptional mecOptional = JsonConvert.DeserializeObject<MecOptional>(optionSettings.ToString());
                                    WriteCodesysMemor.SetMecOptional(mecOptional);//寫入記憶體
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            AddOperatingLog(LogSourceEnum.Software, "手臂切換為自動模式失敗", true);
                            AddOperatingLog(LogSourceEnum.Software, Ex.Message, true);
                        }


                        //失敗時跳出提示 成功時則繼續執行
                        try
                        {
                            using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                            {
                                write.SetMonitorWorkOffset(new byte[1] { 1 }, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.ContinueWork)).ToInt64()); //寫入準備加工的陣列位置
                            }
                            AddOperatingLog(LogSourceEnum.Software, "切換到續接");

                            Transport_by_Continue_RadioButtonIsChecked = true;
                        }
                        catch (Exception ex)
                        {
                            if (obj is UIElement)
                            {
                                ShowMessageBox(obj as UIElement, "無法切換到續接");
                            }
                            AddOperatingLog(LogSourceEnum.Software, "切換到到續接失敗", true);
                            AddOperatingLog(LogSourceEnum.Software, ex.Message, true);
                        }


                        ProgressBar_Visible_Transport_by_Continue_Visibility = false;
                        Transport_RadioButtonIsEnable = true;
                        Transport_by_Continue_RadioButtonIsEnable = true;
                        Transport_by_Hand_RadioButtonIsEnable = true;

                        HintStep4 = true;
                    });


                });
            }
        }

        /// <summary>
        /// 切換到手動模式
        /// </summary>
        public ICommand ChangeTransportMode_To_Hand_Command
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    HintStep4 = false;
                    Transport_by_Hand_RadioButtonIsChecked = false;
                    ProgressBar_Visible_Transport_by_Hand_Visibility = true;

                    Transport_RadioButtonIsEnable = false;
                    Transport_by_Continue_RadioButtonIsEnable = false;
                    Transport_by_Hand_RadioButtonIsEnable = false;

                    Task.Run(() =>
                    {
                        //計時器 單位為毫秒
                        Thread.Sleep(500);
                        //對機台下命令 並回傳是否成功，當成功時IsChecked=true 否則IsChecked = False
                        try
                        {
                            if (GD_STD.Properties.Optional.Default.HandAuto)
                            {
                                STDSerialization ser = new STDSerialization();
                                FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();//讀取記憶體
                                                                                                  //打開選配->自動模式->按鈕取消底線

                                optionSettings.HandAuto = !optionSettings.HandAuto;
                                if (optionSettings.HandAuto)
                                {
                                    //改為開啟自動
                                    HintStep4 = false;
                                    Transport_by_Hand_RadioButtonIsChecked = false;
                                    AddOperatingLog(LogSourceEnum.Software, "手臂切換回自動模式，");
                                    AddOperatingLog(LogSourceEnum.Software, "需再選擇台車或續接模式");
                                }
                                else
                                {
                                    //改為關閉自動
                                    HintStep4 = true;
                                    Transport_by_Hand_RadioButtonIsChecked = true;
                                    AddOperatingLog(LogSourceEnum.Software, "手臂切換到手動模式");
                                    //插單
                                }
                                ser.SetOptionSettings(optionSettings);//寫入記錄
                                MecOptional mecOptional = JsonConvert.DeserializeObject<MecOptional>(optionSettings.ToString());
                                WriteCodesysMemor.SetMecOptional(mecOptional);//寫入記憶體
                            }
                            else
                            {
                                if (obj is UIElement)
                                {
                                    ShowMessageBox(obj as UIElement, "本機台無此選配，手臂無法切換到手動模式");
                                }
                                AddOperatingLog(LogSourceEnum.Software, "手臂切換到手動模式失敗", true);
                            }

                        }
                        catch (Exception ex)
                        {
                            if (obj is UIElement)
                            {
                                ShowMessageBox(obj as UIElement, "無法切換手臂功能");
                            }
                            AddOperatingLog(LogSourceEnum.Software, "切換手臂功能失敗", true);
                            AddOperatingLog(LogSourceEnum.Software, ex.Message, true);
                        }
                        ProgressBar_Visible_Transport_by_Hand_Visibility = false;
                        Transport_RadioButtonIsEnable = true;
                        Transport_by_Continue_RadioButtonIsEnable = true;
                        Transport_by_Hand_RadioButtonIsEnable = true;

                    });


                });
            }
        }

        private void ShowMessageBox(UIElement _element, string Message)
        {
            if (_element != null)
            {
                _element.Dispatcher.BeginInvoke(new Action(delegate
                {
                    Thread.Sleep(100);
                    WinUIMessageBox.Show(null,
                        $"{Message}",
                        $"通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        FloatingMode.Adorner);
                }));
            }
            else
                Debugger.Break();
        }


        private void ClearPButtonModeValue(ref GD_STD.PanelButton PButton)
        {
            PButton.ClampDown = false;
            PButton.SideClamp = false;
            PButton.EntranceRack = false;
            PButton.Hand = false;
            PButton.DrillWarehouse = false;
            PButton.Volume = false;
            PButton.MainAxisMode = false;
        }

        //註解燈號
        public bool HintStep1 { get; set; } = false;
        public bool HintStep2 { get; set; } = false;
        public bool HintStep3 { get; set; } = false;
        public bool HintStep4 { get; set; } = false;



        public Visibility SelectedMaterial_Info_Button_Visibility { get; set; } 


        public bool DrillHole_Mode_RadioButtonIsEnable { get; set; } 
        public bool DrillPin_Mode_RadioButtonIsEnable { get; set; }


        private bool _transportGridIsEnable = false;
        /// <summary>
        /// 最上層 當有物料是配對完成狀態才可用
        /// </summary>
        public bool TransportGridIsEnable { get { return _transportGridIsEnable; } set { _transportGridIsEnable = value; OnPropertyChanged("TransportGridIsEnable"); } } 

        public bool Transport_RadioButtonIsEnable { get; set; } = true;
        public bool Transport_by_Continue_RadioButtonIsEnable { get; set; } = true;
        private bool _transport_by_hand_RadioButtonIsEnable = true;
        public bool Transport_by_Hand_RadioButtonIsEnable
        {
            get
            {
                //查詢選配，有選配才能按此按鈕，否則反灰
                if (GD_STD.Properties.Optional.Default.HandAuto)
                {
                    return _transport_by_hand_RadioButtonIsEnable;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                _transport_by_hand_RadioButtonIsEnable = value;
            }
        }

        public bool Transport_RadioButtonIsChecked { get; set; }
        public bool Transport_by_Continue_RadioButtonIsChecked { get; set; }
        public bool Transport_by_Hand_RadioButtonIsChecked { get; set; }

        public bool ProgressBar_Visible_Transport_Visibility { get; set; } = false;

        public bool ProgressBar_Visible_Transport_by_Continue_Visibility { get; set; } = false;

        public bool ProgressBar_Visible_Transport_by_Hand_Visibility { get; set; } = false;


        public ICommand DrillHole_ModeCommand
        {
            get
            {
                return new WPFBase.RelayCommand(() =>
                {
                    AddOperatingLog(LogSourceEnum.Software, "切換到一般加工模式");
                    MachiningCombinational_DrillBoltsItemSource = GetDrillBoltsItemCollection(false,Finish_UndoneDataViews_SelectedItem);
                    HintStep1 = true;
                    //如果切換時有已排程但未加工的零件->清理掉狀態
                    //若目前已選取的為等待配對資料 改變其鑽孔/打點狀態

                    //MachiningCombinational_DrillBoltsItemSource.ForEach(el => el.Value.PinTestMode = false);
                    ClearPairedMachineData(Finish_UndoneDataViews_SelectedItem);
                });
            }
        }

        public ICommand DrillPin_ModeCommand
        {
            get => new WPFBase.RelayCommand(() =>
            {
                AddOperatingLog(LogSourceEnum.Software, "切換到測試打孔模式");
                MachiningCombinational_DrillBoltsItemSource = GetDrillBoltsItemCollection(true,Finish_UndoneDataViews_SelectedItem);
                //若目前已選取的為等待配對資料 改變其鑽孔/打點狀態
                //MachiningCombinational_DrillBoltsItemSource.ForEach(el => el.Value.PinTestMode = true);

                //如果切換時有已排程但未加工的零件->清理掉狀態並重新上傳
                ClearPairedMachineData(Finish_UndoneDataViews_SelectedItem);
                //如果已在電腦模式 不跑這條
                if (!Input_by_Computer_RadioButtonIsChecked)
                    set_Input_by_Computer();
            });
        }
        /// <summary>
        /// 點選電腦模式
        /// </summary>
        public ICommand Set_Input_by_Computer_Command { get => new WPFBase.RelayCommand(() => set_Input_by_Computer()); }

        private void set_Input_by_Computer()
        {
            HintStep1 = true;
            HintStep2 = false;
            try
            {
                Input_by_Computer_RadioButtonIsChecked = false;
                ProgressBar_Visible_Input_by_Computer = true;
                Task.Run(() =>
                {
                    Input_by_SmartPhone_RadioButtonVMEnable = false;
                    Input_by_Computer_RadioButtonVMEnable = false;

                    GD_STD.Phone.Operating operating;
                    using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                    {
                        operating = read.GetOperating();
                    }
                    operating.Satus = PHONE_SATUS.REFUSE;
                    CodesysIIS.WriteCodesysMemor.SetPhoneOperating(operating);
                    //關閉連線

                    //var Result = MachineAndPhoneAPI.AppServerCommunicate.SetMachineenableAppPairing();

                    //不管成功或失敗 都使按鈕恢復可用
                    Input_by_SmartPhone_RadioButtonVMEnable = true;
                    Input_by_Computer_RadioButtonVMEnable = true;

                    ProgressBar_Visible_Input_by_Computer = false;

                    AddOperatingLog(LogSourceEnum.Software, "切換到機台模式");
                    Input_by_Computer_RadioButtonIsChecked = true;

                    //TourTaskStart();
                    //TourTaskBoolean = false;
                    HintStep2 = true;
                });
            }
            catch
            {

            }
        }





        public ICommand Set_Input_by_SmartPhone_Command
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    HintStep2 = false;
                    Input_by_SmartPhone_RadioButtonIsChecked = false;
                    ProgressBar_Visible_Input_by_SmartPhone = true;
                    Task.Run(() =>
                    {
                        Input_by_SmartPhone_RadioButtonVMEnable = false;
                        Input_by_Computer_RadioButtonVMEnable = false;


                        var Result = MachineAndPhoneAPI.AppServerCommunicate.SetPhoneEnableAppPairing();

                        //不管成功或失敗 都使按鈕恢復可用
                        Input_by_SmartPhone_RadioButtonVMEnable = true;
                        Input_by_Computer_RadioButtonVMEnable = true;

                        ProgressBar_Visible_Input_by_SmartPhone = false;
                        if (Result)
                        {
                            AddOperatingLog(LogSourceEnum.Software, "切換到手機模式");
                            Input_by_SmartPhone_RadioButtonIsChecked = true;

                            HintStep2 = true;
                        }
                        else
                        {
                            if (obj is UIElement)
                            {
                                ShowMessageBox(obj as UIElement, "無法切換到手機模式，需檢查伺服器連線是否正常");
                            }
                            AddOperatingLog(LogSourceEnum.Software, "無法切換到手機模式", true);
                        }


                    });


                });
            }
        }



        public bool Input_by_Computer_RadioButtonIsChecked { get; set; } = false;
        private bool _input_by_SmartPhone_RadioButtonIsChecked = false;
        public bool Input_by_SmartPhone_RadioButtonIsChecked
        {
            get
            {
                return _input_by_SmartPhone_RadioButtonIsChecked;
            }
            set
            {
                _input_by_SmartPhone_RadioButtonIsChecked = value;
            }
        }






        /// <summary>
        /// 使用多重綁定時當Input_by_SmartPhoneVMEnable=false時 Enable 直接為false，其餘狀況依照聯集結果而定
        /// </summary>
        public bool Input_by_SmartPhone_RadioButtonVMEnable { get; set; } = true;
        /// <summary>
        /// 使用多重綁定時當Input_by_Computer_RadioButtonVMEnable=false時 Enable 直接為false，其餘狀況依照聯集結果而定
        /// </summary>
        public bool Input_by_Computer_RadioButtonVMEnable { get; set; } = true;

        public bool ProgressBar_Visible_Input_by_SmartPhone { get; set; } = false;

        public bool ProgressBar_Visible_Input_by_Computer { get; set; } = false;


        //啟用輪巡
        //Task TourTask;
        //bool TourTaskBoolean = false;
        /*private void TourTaskStart()
        {
            try
            {
                if (TourTask != null)
                {
                    //正在執行中改為不打斷
                    //TourTask.Wait();
                    if (TourTask.Status == TaskStatus.Running)
                    {
                        return;
                        // TourTask.Dispose();
                    }
                }
            }
            catch
            {

            }
            //開始輪巡

        }*/
        /// <summary>
        /// 將app配對寫入機台
        /// </summary>
        private void WriteTourData()
        {
            var GetAppDataBool = MachineAndPhoneAPI.AppServerCommunicate.GetAppPairingData(MachineAndPhoneAPI.AppServerCommunicate.RegeditMaterialSource.PC, out var PC_PairingData);
            var GetPCDataBool = MachineAndPhoneAPI.AppServerCommunicate.GetAppPairingData(MachineAndPhoneAPI.AppServerCommunicate.RegeditMaterialSource.APP, out var APP_PairingData);
            if (GetAppDataBool || GetPCDataBool)
            {
                var PairingDataDict = new Dictionary<MachineAndPhoneAPI.AppServerCommunicate.RegeditMaterialSource,List<MachineAndPhoneAPI.Models.Assemblyinfo.Data>>();
            
                if (PC_PairingData.data != null)
                {
                    PairingDataDict.Add(MachineAndPhoneAPI.AppServerCommunicate.RegeditMaterialSource.PC, PC_PairingData.data);
                }
                if (APP_PairingData.data != null)
                {
                    PairingDataDict.Add(MachineAndPhoneAPI.AppServerCommunicate.RegeditMaterialSource.APP,APP_PairingData.data);
                }

                var TourTaskList = new List<Task>();
                foreach (var PairingDataKeyValue in PairingDataDict)
                {
                    foreach (var EachPair in PairingDataKeyValue.Value)
                    {
                        var Index = Finish_UndoneDataViews.ToList().FindIndex(x => (x.MaterialNumber == EachPair.materialNumber));
                        if (Index != -1)
                        {
                            Func<object, bool> action = (object obj) =>
                            {
                                int MaterialIndex = (int)obj;
                                //Finish_UndoneDataViews[MaterialIndex].PositionEnum == PositionStatusEnum.等待配對
                                if (Finish_UndoneDataViews[MaterialIndex].PositionEnum == PositionStatusEnum.已配對)
                                {
                                    //軟體配對 
                                    //區別手機與機台配料->如果id中有包含專案名稱->機台的 沒有的話則是手機
                                    if (PairingDataKeyValue.Key is MachineAndPhoneAPI.AppServerCommunicate.RegeditMaterialSource.PC)
                                    {
                                        Finish_UndoneDataViews[MaterialIndex].PositionEnum = PositionStatusEnum.從軟體配對;
                                    }

                                    if (PairingDataKeyValue.Key is MachineAndPhoneAPI.AppServerCommunicate.RegeditMaterialSource.APP)
                                    {
                                        Finish_UndoneDataViews[MaterialIndex].PositionEnum = PositionStatusEnum.從手機配對;
                                    }
                                    AddOperatingLog(LogSourceEnum.Phone, $"素材編號：{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}-{PairingDataKeyValue.Key}", false);
                                }
                                return true;
                            };
                            TourTaskList.Add(Task<bool>.Factory.StartNew(action, Index));
                        }
                    }
                }
                Task.WaitAll(TourTaskList.ToArray()); 
                RefreshScheduleGridC();
            }

        }





        #endregion









        /// <summary>
        /// 解構式
        /// </summary>
        ~ProcessingMonitor_MachineVM()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: false);
        }
        /// <inheritdoc/>  
        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public void SetSerializationInit(WPFSTD105.ModelExt _Model)
        {
            //await Task.Yield();
            int errorCount = 0;
            while (errorCount < 50)
            {
                try
                {
                    if (errorCount == 0)
                    {
                        NewSerializationThread();
                        NewHostThread();
                    }

                    //int synIndex = 0;
                    if (ApplicationViewModel.PanelButton.Key == KEY_HOLE.MANUAL) //如果沒有在自動狀況下
                    {
                        if (errorCount == 0) //如果沒有發送失敗
                        {
                            var _Cts = new CancellationTokenSource();

                            _CreateDMFileTask = Task.Run(() => { CreateDMFile(_Model); }, _Cts.Token);
                            _WriteCodesysTask = Task.Run(() => { WriteCodesys(); }, _Cts.Token);
                        }

                        //如有備份檔就寫回給 Codesys
                        for (int i = 0; i < Finish_UndoneDataViews.Count; i++)
                        {
                            //葉:需要比對衝突
                            STDSerialization ser = new STDSerialization();
                            
                            WorkMaterial? work = ser.GetWorkMaterialBackup(Finish_UndoneDataViews[i].MaterialNumber);
                            if (work != null)
                            {
                                long workOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.WorkMaterial)).ToInt64();
                                int workSize = Marshal.SizeOf(typeof(WorkMaterial));
                                if (work.Value.AssemblyNumber != null && work.Value.MaterialNumber != null)
                                {
                                    if (work.Value.Position == -2 ) //如果是完成的狀態
                                    {
                                        Finish_UndoneDataViews[i].Schedule = 100;
                                        Finish_UndoneDataViews[i].PositionEnum = PositionStatusEnum.完成;       //"完成";
                                        _Finish.Add(Convert.ToInt16(i)); //加入到完成列表
                                    }
                                    else
                                    {
                                        WriteCodesysMemor.SetMonitorWorkOffset(work.Value.ToByteArray(), workOffset + (workSize * i)); //發送加工陣列

                                    }
                                    _SendIndex.Add(Convert.ToInt16(i));
                                    // synIndex = i;
                                }
                            }
                        }

                        foreach (var FNDV in Finish_UndoneDataViews)
                        {
                            if (FNDV.PositionEnum != PositionStatusEnum.完成)
                            {
                                if ((FNDV.ObjectType == GD_STD.Enum.OBJECT_TYPE.CH || FNDV.ObjectType == GD_STD.Enum.OBJECT_TYPE.LB) && FNDV.LengthStr > 9050)
                                    FNDV.PositionEnum = PositionStatusEnum.不可配對;
                            }
                        }
                    }   //如果是在自動狀況下
                    else if (ApplicationViewModel.PanelButton.Key == KEY_HOLE.AUTO) //如果有在自動狀況下
                    {
                        using (Memor.ReadMemorClient client = new Memor.ReadMemorClient())
                        {
                            //同步列表
                            for (int i = 0; i < Finish_UndoneDataViews.Count; i++)
                            {
                                _WorkMaterials[i] = client.GetWorkMaterial(Convert.ToUInt16(i));
                                if (_WorkMaterials[i].BoltsCountL != 0 || _WorkMaterials[i].BoltsCountR != 0 || _WorkMaterials[i].IndexBoltsM != 0)
                                {
                                    _SendIndex.Add(Convert.ToInt16(i));
                                }
                              
                            }
                        }
                    }

                    using (Memor.WriteMemorClient Write = new Memor.WriteMemorClient())
                    {
                        long ImportProjectOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.ImportProject)).ToInt64();
                        Write.SetMonitorWorkOffset(new byte[1] { 1 }, ImportProjectOffset);//寫入匯入專案完成
                    }

                    break;
                }
                catch (Exception ex)
                {
                    if (errorCount < 20) //如果同步失敗 20 次
                    {
                        log4net.LogManager.GetLogger("同步失敗").Debug($"同步失敗");
                        ;
                    }
                    else
                    {
                        // throw new Exception("同步失敗超過 20 次");
                        AddOperatingLog(LogSourceEnum.Init, "無法同步", true);
                        AddOperatingLog(LogSourceEnum.Init, ex.Message, true);
                        AddOperatingLog(LogSourceEnum.Init, "監聽執行序停止", true);
                        return;
                    }
                }
            }

            _SerializationThread.Start();
            _HostThread.Start();

        }

        /// <summary>
        /// 持續序列化
        /// </summary>
        private Thread _SerializationThread;
        /// <summary>
        /// 持續監看Host狀態
        /// </summary>
        private Thread _HostThread;
        /// <summary>
        /// 產生新的序列化執行續
        /// </summary>
        private void NewSerializationThread()
        {
            if (_SerializationThread != null)
            {
                _SerializationThread.Abort();
            }

            _SerializationThread = new Thread(new ThreadStart(() =>
            {
                _WriteCodesysTask?.Wait();
                int count = 0;
                while (true)
                {
                    try
                    {
                        //要有點選才序列化

                        ContinuedSerialization();
                        
                        count = 0;
                    }
                    catch (Exception ex)
                    {
                        if (count < 30)
                        {
                            Debug.WriteLine($"序列化失敗第 {count} 次 ...");
                        }
                        else
                        {
                            log4net.LogManager.GetLogger("同步失敗").Debug($"同步失敗");
                            //Debugger.Break();
                            //throw new Exception("伺服器無法連線 ....");
                        }
                    }
                }
            }));
            _SerializationThread.IsBackground = true;

        }
        /// <summary>
        /// 產生新的序列化執行續
        /// </summary>
        private void NewHostThread()
        {
            if (_HostThread != null)
            {
                _HostThread.Abort();
            }
            _HostThread = new Thread(new ThreadStart(() =>
            {
                _WriteCodesysTask?.Wait();
                int count = 0;
                while (true)
                {
                    try
                    {
                        ContinuedHost();
                        count = 0;
                    }
                    catch (Exception ex)
                    {
                        if (count < 30)
                        {
                            Debug.WriteLine($"Host讀取失敗第 {count} 次 ...");
                        }
                        else
                        {
                            log4net.LogManager.GetLogger("Host同步失敗").Debug($"同步失敗");
                        }
                    }
                }
            }));
            _HostThread.IsBackground = true;
        }

        private bool IsSerializing = false;
        /// <summary>
        /// 持續序列化
        /// </summary>
        private void ContinuedSerialization()
        {
            IsSerializing = true;
            STDSerialization ser = new STDSerialization();
            short[] indexArray = null;
            WorkOther workOther = null;
            Host host;
            using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
            {
                indexArray = read.GetIndex();
                workOther = read.GetWorkOther();
                host = read.GetHost();
            }

            if (MCurrent != workOther.Current)
            {
                if (workOther.Current != -1)
                    AddOperatingLog(LogSourceEnum.Machine, $"切換加工索引到：{workOther.Current}");
                else
                    AddOperatingLog(LogSourceEnum.Machine, $"目前無等待加工之索引");
            }
            MCurrent = workOther.Current;

            ser.SetWorkMaterialOtherBackup(workOther);
            ser.SetWorkMaterialIndexBackup(indexArray); //備份 indexArray
            //解除配對時需把工作陣列全清除
            var noInfo = indexArray.Except(_SendIndex).ToArray(); //查詢尚未發送加工孔位的 index 
            for (int i = 0; i < noInfo.Length; i++) //找出沒發送過的工作陣列
            {
                //AddOperatingLog(LogSourceEnum.Machine, $"發送加工index：{noInfo[i]}");
                AddOperatingLog(LogSourceEnum.Machine, $"準備發送排版編號{Finish_UndoneDataViews[noInfo[i]].MaterialNumber}", false);
                //發送
                if (SendDrill(noInfo[i], out var IsPinMode))
                {
                    AddOperatingLog(LogSourceEnum.Machine, $"排版編號{Finish_UndoneDataViews[noInfo[i]].MaterialNumber}加工訊息發送成功", false);
                    Finish_UndoneDataViews[noInfo[i]].MachiningTypeMode = IsPinMode? MachiningType.PinTest : MachiningType.NormalMaching;

                    //把更新後的MachiningTypeMode寫入資料表中
                    ser.SetMaterialDataView(Finish_UndoneDataViews);
                    _SendIndex.Add(noInfo[i]); //存取已經發送過的列表
                }
                else
                {
                    AddOperatingLog(LogSourceEnum.Machine, $"排版編號{Finish_UndoneDataViews[noInfo[i]].MaterialNumber}加工訊息發送失敗", true);
                }
            }
            //_SendIndex.AddRange(noInfo); //存取已經發送過的列表
            //SerializationValue機台內有資料的都會在此陣列中
            List<short> SerializationValue = new List<short>(indexArray);
            List<short> delete = _LastTime.Except(SerializationValue).ToList(); //找出上次有序列化的文件
            SerializationValue.AddRange(delete);

            Serialization(SerializationValue);
            _LastTime = indexArray.ToArray();

            //刷新初始化
            for (int i = 0; i < Finish_UndoneDataViews.Count; i++)
            {
                if (SerializationValue.ToList().Exists(x => x == i))
                    continue;
                else
                {
                    if (Finish_UndoneDataViews[i].PositionEnum == PositionStatusEnum.初始化)
                    {
                        Finish_UndoneDataViews[i].PositionEnum = PositionStatusEnum.等待配對;
                        Finish_UndoneDataViews[i].EX_EN_count = "";
                    }
                    else if (Finish_UndoneDataViews[i].PositionEnum == PositionStatusEnum.未取得狀態)
                    {
                        Finish_UndoneDataViews[i].PositionEnum = PositionStatusEnum.等待配對;
                    }
                    //找出表格中是已配對 / 手機配對 / 軟體配對 但機台端沒有資料的->代表資料有誤或是被移除

                    else if (Finish_UndoneDataViews[i].PositionEnum == PositionStatusEnum.從軟體配對 ||
                        Finish_UndoneDataViews[i].PositionEnum == PositionStatusEnum.從手機配對 ||
                        Finish_UndoneDataViews[i].PositionEnum == PositionStatusEnum.已配對)
                    {
                        Finish_UndoneDataViews[i].PositionEnum = PositionStatusEnum.等待配對;
                    }
                }
            }
         
            RefreshScheduleGridC();

            IsSerializing = false;
            Thread.Sleep(1000); //等待 2 秒後執行
        }


        /// <summary>
        /// 要序列化的值
        /// </summary>
        //private List<short> SerializationValue { get; set; }
        /// <summary>
        /// 上一次的 Index
        /// </summary>
        private short[] _LastTime { get; set; } = new short[0];

        /// <summary>
        /// 持續監看 Host
        /// </summary>
        /// <param name="count"></param>
        private void ContinuedHost()
        {
            Thread.Sleep(1333);//等待 1.333 秒後執行
            Host host;
            WorkOther workOther = null;
            short[] index = new short[0];
            using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
            {
                host = read.GetHost();
                if (host.Analysis)
                {
                    workOther = read.GetWorkOther();
                    index = read.GetIndex();
                }
            }
            if (workOther != null)
            {
                WorkMaterial cWork = _WorkMaterials[index[workOther.Current]];
                var _ = cWork.DrLeft //找尋是否有不匹配的刀具
                                                                        .Where(el => el.NoMatch)
                                                                        .Union(cWork.DrRight
                                                                                                    .Where(el => el.NoMatch))
                                                                        .Union(cWork.DrMiddle
                                                                                                    .Where(el => el.NoMatch))
                                                                        .ToList();
                if (_.Count() == 0) //如果沒有不匹配的物件
                {
                    host.Comply = true;
                }
                else
                {
                    //葉:頁面好惹
                    //var _ = System.Windows.MessageBox.Show("有匹配不到刀具的孔位，是否略過", "通知", System.Windows.MessageBoxButton.YesNo);
                    WinUIDialogWindow winuidialog = new WinUIDialogWindow("Information", MessageBoxButton.YesNo)
                    {
                        Title = "通知",
                        Content = new TextBlock() { Text = "有匹配不到刀具的孔位，是否略過？" }
                    };
                    if (winuidialog.DialogResult == false)
                    {
                        //顯示孔位匹配頁面
                        WPFBase.AskingDrillMatch windows = new WPFBase.AskingDrillMatch();
                        windows.ShowDialog();
                    }
                    else
                    {
                        //葉:略過孔位要做啥事？
                    }
                }
                using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                {
                    write.SetHost(host);
                }
            }
        }

        /// <summary>
        /// 序列化加工資訊
        /// </summary>
        /// <param name="index">序列化的陣列位置</param>
        private void Serialization(List<short> index)
        {
            if (index.Count == 0)
            {
                HintStep3 = false;
                TransportGridIsEnable = false;
                return;
            }
            int exCount = 1, //出口數量
                enCount = 1; //入口數量
                             //_SynchronizationContext.Send(t =>
            try
            {
                using (Memor.ReadMemorClient client = new Memor.ReadMemorClient())
                {
                    try
                    {
                        var serIndex = index.Except(_Finish); //差集未完成的陣列數值
                        foreach (var value in serIndex)
                        {
                            _WorkMaterials[value] = client.GetWorkMaterial(Convert.ToUInt16(value));

                            STDSerialization ser = new STDSerialization();
                            ser.SetWorkMaterialBackup(_WorkMaterials[value]);
                            //string number = _WorkMaterials[value].MaterialNumber.Where(el => el != 0).Select(el => Convert.ToChar(el).ToString()).Aggregate((str1, str2) => str1 + str2);
                            //int MIndex = Finish_UndoneDataViews.ToList().FindIndex(el => el.MaterialNumber == number);

                                Finish_UndoneDataViews[value].EX_EN_count = "";

                                if (_WorkMaterials[value].Position == -2) //如果已經完成
                                {
                                    Finish_UndoneDataViews[value].Schedule = 100;
                                    Finish_UndoneDataViews[value].Finish = true;
                                    Finish_UndoneDataViews[value].PositionEnum = PositionStatusEnum.完成;
                                    _Finish.Add(value); //加入到完成列表

                                    if (Finish_UndoneDataViews[value].MachiningEndTime == null)
                                        Finish_UndoneDataViews[value].MachiningEndTime = DateTime.Now;
                                }
                                else if (_WorkMaterials[value].Position == -1)
                                {
                                    Finish_UndoneDataViews[value].Schedule = _WorkMaterials[value].Finish;
                                    Finish_UndoneDataViews[value].PositionEnum = PositionStatusEnum.加工中;

                                    if (Finish_UndoneDataViews[value].MachiningStartTime == null)
                                        Finish_UndoneDataViews[value].MachiningStartTime = DateTime.Now;
                                }
                                else if (_WorkMaterials[value].Position == 0)
                                {
                                    if (Finish_UndoneDataViews[value].PositionEnum != PositionStatusEnum.從軟體配對 &&
                                        Finish_UndoneDataViews[value].PositionEnum != PositionStatusEnum.從手機配對 &&
                                        Finish_UndoneDataViews[value].PositionEnum != PositionStatusEnum.已配對)
                                    {
                                        Finish_UndoneDataViews[value].PositionEnum = PositionStatusEnum.已配對;
                                    }
                                    //手機或機台狀態靠輪巡去修改
                                }
                                else
                                {
                                    if (_WorkMaterials[value].IsExport) //出口處
                                    {
                                        //以防完成時沒加到
                                        if (Finish_UndoneDataViews[value].MachiningEndTime == null)
                                            Finish_UndoneDataViews[value].MachiningEndTime = DateTime.Now;

                                        Finish_UndoneDataViews[value].Schedule = _WorkMaterials[value].Finish;
                                        Finish_UndoneDataViews[value].PositionEnum = PositionStatusEnum.等待出料;//$"等待(出)-{exCount}";

                                        Finish_UndoneDataViews[value].EX_EN_count = $"-{exCount.ToString()}";//$"等待(出)-{exCount}";
                                        exCount++;
                                    }
                                    else
                                    {
                                        Finish_UndoneDataViews[value].MachiningStartTime = null;
                                        Finish_UndoneDataViews[value].MachiningEndTime = null;

                                        Finish_UndoneDataViews[value].PositionEnum = PositionStatusEnum.等待入料;//$"等待(入)-{enCount}";
                                        Finish_UndoneDataViews[value].EX_EN_count = $"-{enCount.ToString()}";//$"等待(入)-{enCount}";
                                        enCount++;
                                    }
                                }
                            
                        }



                        if (_WorkMaterials.FindIndex(x => x.Position == 0 || x.Position == -1) != -1)
                        {
                            HintStep3 = true;
                            TransportGridIsEnable = true;
                        }
                        else
                        {
                            HintStep3 = false;
                            TransportGridIsEnable = false;
                        }
                        RefreshScheduleGridC();
                    }
                    catch (Exception ex)
                    {
                        AddOperatingLog(LogSourceEnum.Software, ex.Message, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
        }

        private void ClearPairedMachineData(MaterialDataView _Mdataview)
        {
            //找出list
            var PositionStatusEnumArray = new List<PositionStatusEnum> { PositionStatusEnum.從軟體配對, PositionStatusEnum.從手機配對, PositionStatusEnum.已配對 };

            if (_Mdataview != null)
            {
                if (PositionStatusEnumArray.Exists(x => x == _Mdataview.PositionEnum))
                {
                    var Xindex = Finish_UndoneDataViews.FindIndex(x => x == _Mdataview);
                    if (Xindex != -1)
                    {
                        if (_SendIndex.Exists(y => y == Xindex))
                        {
                            AddOperatingLog(LogSourceEnum.Machine, $"素材編號{_Mdataview.MaterialNumber}的加工資料已上傳到機台");
                            _SendIndex.RemoveAll(z => z == Xindex);
                            Finish_UndoneDataViews[Xindex].MachiningTypeMode = MachiningType.Unknown;
                            AddOperatingLog(LogSourceEnum.Machine, $"準備重新上傳素材編號{_Mdataview.MaterialNumber}的加工資料");
                        }
                    }
                }
            }
            //當_finish_UndoneDataViews內的PositionEnum符合PositionStatusEnumArray中的任意一項時，將值取出
            /* var FUDV_List = Finish_UndoneDataViews.ToList().FindAll(x => PositionStatusEnumArray.Exists(y => y == x.PositionEnum));
             foreach (var Fu in FUDV_List)
             {
                 var Xindex = Finish_UndoneDataViews.FindIndex(x => x == Fu);
                 if (Xindex != -1)
                 {
                     AddOperatingLog(LogSourceEnum.Machine, $"準備重新上傳素材編號{Fu.MaterialNumber}的加工資料");
                     var dataViewIndex = Finish_UndoneDataViews.FindIndex(x=>x.MaterialNumber == Fu.MaterialNumber);
                     if (dataViewIndex != -1)
                     {
                         _Finish.RemoveAll(x => x == dataViewIndex);
                         _SendIndex.RemoveAll(x => x == dataViewIndex);
                     }
                 }
             }*/
        }

        private bool ClearAppServerPairWorkList(List<string> MaterialList)
        {
            var ApiReturn = MachineAndPhoneAPI.AppServerCommunicate.UnregisterAssembly(ApplicationViewModel.ProjectName, MaterialList, out var result);
            if (ApiReturn)
            {
                if (result.errorCode == 0)
                {
                    //會回傳註銷成功的id
                    //比較送出的列表和返還的列表是否有不同
                    if (result.data != null)
                    {
                        foreach (var el in result.data)
                        {
                            AddOperatingLog(LogSourceEnum.Phone, $"素材編號{el}的伺服器資料成功註銷");
                        }
                    }
                    /*foreach (var el in MaterialList.Except(result.data))
                    {
                        AddOperatingLog(LogSourceEnum.Phone, $"素材編號{el}無法註銷", true);
                    }*/
                }
                else
                {
                    AddOperatingLog(LogSourceEnum.Phone, $"註銷命令失敗");
                    AddOperatingLog(LogSourceEnum.Phone, $"ErrorCode = {result.errorCode}");
                }
            }
            else
            {
                AddOperatingLog(LogSourceEnum.Phone, $"通訊失敗 伺服器無法執行註銷命令");
            }
            return ApiReturn;
        }



        /// <summary>
        /// 清除機台端已存在的加工陣列index (不管他是否有加工完成)
        /// </summary>
        /// <param name="dataViewIndex"></param>
        private void ClearMonitorWorkList(int dataViewIndex)
        {
            ClearMonitorWorkList(Convert.ToInt16(dataViewIndex));
        }

        /// <summary>
        /// 清除機台端已存在的加工陣列index (不管他是否有加工完成)
        /// </summary>
        /// <param name="dataViewIndex"></param>
        private void ClearMonitorWorkList(short dataViewIndex)
        {
            Task.Run(() =>
            {
                //不要在序列化中途清除加工陣列
                while (IsSerializing)
                    Thread.Sleep(1000);

                short[] index;
                using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                {
                    index = read.GetIndex();
                }

                while (true)
                {
                    int iIndex = index.FindIndex(x => x == dataViewIndex);
                    if (iIndex != -1)
                        index[iIndex] = -1;
                    else
                        break;
                }

                var writeByte = index.ToByteArray();
                using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                    write.SetMonitorWorkOffset(writeByte, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列

                //將加工列表的資料清除
                _WorkMaterials[dataViewIndex].Finish = 0;
                _WorkMaterials[dataViewIndex].IsExport = false;
                _WorkMaterials[dataViewIndex].Position = 0;
                long cWork = _WorkOffset + (_WorkSize * dataViewIndex);
                using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                {
                    WriteCodesysMemor.SetMonitorWorkOffset(_WorkMaterials[dataViewIndex].ToByteArray(), cWork); //發送加工陣列
                }

                _Finish.RemoveAll(x => x == dataViewIndex);
                _SendIndex.RemoveAll(x => x == dataViewIndex);

                //復原加工資料
                Finish_UndoneDataViews[dataViewIndex].PositionEnum = PositionStatusEnum.未取得狀態;
                Finish_UndoneDataViews[dataViewIndex].MachiningTypeMode = MachiningType.Unknown;
                Finish_UndoneDataViews[dataViewIndex].Schedule = 0;
                Finish_UndoneDataViews[dataViewIndex].Finish = false;
                Finish_UndoneDataViews[dataViewIndex].MachiningStartTime = null;
                Finish_UndoneDataViews[dataViewIndex].MachiningEndTime = null;

                STDSerialization ser = new STDSerialization();
                ser.SetMaterialDataView(Finish_UndoneDataViews);
                ser.SetWorkMaterialBackup(_WorkMaterials[dataViewIndex]);
                //寫入加工陣列

                //寫入備份
                ser.SetWorkMaterialIndexBackup(index);

                //如果有備份檔 把紀錄刪掉
                ser.DeleteWorkMaterialBackup(Finish_UndoneDataViews[dataViewIndex].MaterialNumber);
                RefreshScheduleGridC();




            });
        }

        private void RefreshScheduleGridC()
        {
            if (ScheduleGridC != null)
            {
                ScheduleGridC.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        ScheduleGridC.ItemsSource = new string[Finish_UndoneDataViews.Count];
                        ScheduleGridC.ItemsSource = Finish_UndoneDataViews;
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }
        private void RefreshLogGridC()
        {
            if (LogGridC != null)
            {
                LogGridC.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        LogGridC.ItemsSource = new string[LogDataList.Count];
                        LogGridC.ItemsSource = LogDataList;
                    }
                    catch (Exception ex)
                    {

                    }

                    if (!LogGridC.IsMouseOver)
                    {
                        var TB = GetWpfLogicalChildClass.GetLogicalChildCollection<TableView>(LogGridC);
                        TB.ForEach(el => el.TopRowIndex = LogDataList.Count);
                    }

                });
            }
        }



    }


    
}

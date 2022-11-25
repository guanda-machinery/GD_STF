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

//using DevExpress.Utils.Extensions;
namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 加工監控VM
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ProcessingMonitor_MachineVM : WPFBase.BaseViewModel, IDisposable/*, MonitorDuplex.IMonitorDuplexCallback*/
    {
        #region 公開屬性
        /// <summary>
        /// 控制3D頁面顯示
        /// </summary>
        public bool ThreeDimensionalDisplayControl { get; set; } = true;
        /// <summary>
        /// 當前值
        /// </summary>
        public int Current { get; set; } = -1;

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
            //若選擇已完成零件則顯示該零件已完成
            if (Input_by_Computer_RadioButtonIsChecked)
            {
                //這裡要放歷程記錄 -> 加入list                        ClearPButtonModeValue(ref PButton);
                if (args.Item is MaterialDataView)
                {
                    var argsMaterial = args.Item as MaterialDataView;
                    if (argsMaterial.Position == WaitBePairString)
                    {
                        var Result = MachineAndPhoneAPI.AppServerCommunicate.SetRegisterAssembly(
                            ApplicationViewModel.ProjectName, argsMaterial.MaterialNumber, argsMaterial.Material, argsMaterial.Profile, argsMaterial.LengthStr, out var RegisterResult);

                        if (Result)
                        {
                            //error202->Material not found 
                            if (RegisterResult.data != null)
                            {
                                //一次只會加入一筆資料 所以回傳資料list行直接取取[0]
                                if (RegisterResult.data[0].errorCode == 0)
                                {
                                    foreach (var Data in Finish_UndoneDataViews)
                                    {
                                        if (Data.MaterialNumber == argsMaterial.MaterialNumber)
                                        {
                                            Data.Position = "軟體配對";
                                            break;
                                        }
                                    }
                                    AddOperatingLog(LogSourceEnum.Machine, $"加入素材編號：{argsMaterial.MaterialNumber}成功", false);
                                    WinUIMessageBox.Show(null,
                                        $"加入素材編號：{argsMaterial.MaterialNumber}成功",
                                        $"通知",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Exclamation,
                                        MessageBoxResult.None,
                                        MessageBoxOptions.None,
                                        FloatingMode.Adorner);
                                }
                                else
                                {
                                    AddOperatingLog(LogSourceEnum.Machine, $"加入素材編號：{argsMaterial.MaterialNumber}失敗", true);
                                    Thread.Sleep(100);
                                    AddOperatingLog(LogSourceEnum.Machine, $"錯誤資訊：{RegisterResult.data[0].errorCode},{RegisterResult.data[0].errorMessage}", true);
                                }

                            }
                            else
                            {
                                WinUIMessageBox.Show(null,
                                    $"與伺服器溝通成功，但加入素材編號：{argsMaterial.MaterialNumber}失敗",
                                    $"通知",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation,
                                    MessageBoxResult.None,
                                    MessageBoxOptions.None,
                                    FloatingMode.Adorner);
                                AddOperatingLog(LogSourceEnum.Machine, $"與伺服器溝通成功，但加入素材編號：{argsMaterial.MaterialNumber}失敗", false);
                            }

                        }
                        else
                        {
                            WinUIMessageBox.Show(null,
                            $"素材加入失敗",
                            $"通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                            FloatingMode.Adorner);
                            AddOperatingLog(LogSourceEnum.Machine, "加入素材失敗！", true);
                        }
                    }
                    else
                    {
                        //AddOperatingLog(LogSourceEnum.Machine, $"素材編號：{argsMaterial.MaterialNumber}之狀態不可配對");
                    }
                }
            }
        }

        private const string FinishString = "完成";
        private const string MachiningString = "加工中";
        private const string WaitBePairString = "等待配對";


        //private ObservableCollection<MaterialDataView> _undoneDataView = new ObservableCollection<MaterialDataView>();
        /// <summary>
        /// 未加工的清單
        /// </summary>
        //public ObservableCollection<MaterialDataView> UndoneDataView { get { return _undoneDataView; } } 
        /// <summary>
        /// 配料清單
        /// </summary>
        //public ObservableCollection<MaterialDataView> DataViews { get; set; }

        //private ObservableCollection<MaterialDataView> _finishDataViews = new ObservableCollection<MaterialDataView>();
        /// <summary>
        /// 已完成的清單
        /// </summary>
        //public ObservableCollection<MaterialDataView> FinishDataViews { get { return _finishDataViews; } }



        private ObservableCollection<MaterialDataView> _finish_UndoneDataViews = new ObservableCollection<MaterialDataView>();
        /// <summary>
        /// 未加工-已完成合併清單
        /// </summary>
        public ObservableCollection<MaterialDataView> Finish_UndoneDataViews
        {
            get
            {
                //將資料表重新排列
                var FUDataList = _finish_UndoneDataViews.ToList();
                /*FUDataList.Sort((x, y) =>
                {
                    return x.Position.CompareTo(y.Position);
                });     */
                //_finish_UndoneDataViews
                return new ObservableCollection<MaterialDataView>(FUDataList);
            }
            set
            {

                //_undoneDataView = new ObservableCollection<MaterialDataView>(value.ToList().FindAll(x => (x.Position != FinishString)));
                //_finishDataViews = new ObservableCollection<MaterialDataView>(value.ToList().FindAll(x => (x.Position == FinishString)));
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
                var AssemblyNumberList = new List<string>();
                foreach (var M_part in value.Parts)
                {
                    AssemblyNumberList.Add(M_part.AssemblyNumber);
                }
                MachiningCombinational_DrillBoltsItemSource = GetDrillBoltsItemCollection(AssemblyNumberList);


                _finish_UndoneDataViews_SelectedItem = value;

                AddOperatingLog(LogSourceEnum.Software, $"已選擇素材編號：{_finish_UndoneDataViews_SelectedItem.MaterialNumber}");
            }
        }




        public List<OperatingLog> LogDataList { get; set; } = new List<OperatingLog>();
        private void AddOperatingLog(LogSourceEnum LogSourceEnum, string _Logstring, bool _IsAlert = false)
        {
            //當歷程記錄AddOperatingLog與ShowMessageBox顯示同時使用時，需先執行ShowMessageBox，
            //否則會發生歷程記錄無法自動捲動之情況
            string source = "unknown";
            if (LogSourceEnum == LogSourceEnum.Init)
            {
                source = "初始";
            }
            if (LogSourceEnum == LogSourceEnum.Phone)
            {
                source = "手機";
            }
            if (LogSourceEnum == LogSourceEnum.Machine)
            {
                source = "機台";
            }
            if (LogSourceEnum == LogSourceEnum.Software)
            {
                source = "操作";
            }

            LogDataList.Add(new OperatingLog { LogString = _Logstring, LogSource = source, LogDatetime = DateTime.Now, IsAlert = _IsAlert });
            var TempList = LogDataList.ToList();
            LogDataList.Clear();
            LogDataList = TempList;
            //需加入自動捲動功能
        }



        /// <summary>
        /// 素材(多個零件)加工孔位表
        /// </summary>
        public ObservableCollection<DrillBolts> MachiningCombinational_DrillBoltsItemSource { get; set; }
        /// <summary>
        /// 單一零件加工孔位表
        /// </summary>
        public ObservableCollection<DrillBolts> MachiningDetail_DrillBoltsItemSource { get; set; }



        private ObservableCollection<DrillBolts> GetDrillBoltsItemCollection(List<string> NumberList)
        {
            var PartsDataViews = WPFSTD105.ViewModel.ObSettingVM.GetData(false);
            //var PartsDataViews = new ObservableCollection<WPFSTD105.ViewModel.ProductSettingsPageViewModel>(WPFSTD105.ViewModel.ObSettingVM.GetData(false)).ToList();
            //選擇單一素材
            //取得專案內所有資料 並找出符合本零件的dataname(dm名) 
            //有可能會有複數個(選擇素材/單一零件的差別)
            var PartsList = new List<ProductSettingsPageViewModel>();
            foreach (var AssemblyNum in NumberList)
            {
                PartsList.AddRange(PartsDataViews.FindAll(x => x.AssemblyNumber == AssemblyNum));
            }

            var DrillBoltsListInfo = new List<DrillBolts>();
            foreach (var _part in PartsList)
            {
                GetMaterialdataDrillBoltsInfo(_part.DataName, out var MaterialDrillBolts);
                //複數零件時須將相同之加工資料合併
                //會將不同零件上但孔徑相同的孔數量做加總
                foreach (var MDrill in MaterialDrillBolts)
                {
                    if (DrillBoltsListInfo.Exists(x => (x.WorkType == MDrill.WorkType && x.Face == MDrill.Face && x.DrillHoleDiameter == MDrill.DrillHoleDiameter)))
                    {
                        DrillBoltsListInfo.Find(x => (x.WorkType == MDrill.WorkType && x.Face == MDrill.Face && x.DrillHoleDiameter == MDrill.DrillHoleDiameter)).DrillHoleCount += MDrill.DrillHoleCount;
                    }
                    else
                    {
                        DrillBoltsListInfo.Add(MDrill);
                    }
                }
            }
            return new ObservableCollection<DrillBolts>(DrillBoltsListInfo);
        }




        /// <summary>
        /// 給定DataName 回傳該零件所有的孔位及位置
        /// </summary>
        /// <param name="DataName"></param>
        /// <param name="DrillBoltsList"></param>
        /// <returns></returns>
        private bool GetMaterialdataDrillBoltsInfo(string DataName, out List<DrillBolts> DrillBoltsList)
        {
            DrillBoltsList = new List<DrillBolts>();
            devDept.Eyeshot.Model _BufferModel = new devDept.Eyeshot.Model();
            _BufferModel.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            _BufferModel.InitializeViewports();
            _BufferModel.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());

            var ser = new STDSerialization();
            var readFile = ser.ReadPartModel(DataName); //讀取檔案內容
            if (readFile == null)
            {
                return false;
                //continue;
            }

            readFile.DoWork();//開始工作
            readFile.AddToScene(_BufferModel);//將讀取完的檔案放入到模型
            if (_BufferModel.Entities[_BufferModel.Entities.Count - 1].EntityData is null)
            {
                return false;
                //continue;
            }
            //ViewModel.WriteSteelAttr((SteelAttr)model.Entities[model.Entities.Count - 1].EntityData);//寫入到設定檔內
            //WriteSteelAttr((SteelAttr)_BufferModel.Blocks[1].Entities[0].EntityData);//寫入到設定檔內
            //ViewModel.GetSteelAttr();
            _BufferModel.Blocks[1] = new Steel3DBlock((Mesh)_BufferModel.Blocks[1].Entities[0]);//改變讀取到的圖塊變成自訂義格式(零件)

            for (int i = 0; i < _BufferModel.Entities.Count; i++)//逐步展開 3d 模型實體
            {
                if (_BufferModel.Entities[i].EntityData is GroupBoltsAttr boltsAttr) //是螺栓
                {
                    BlockReference blockReference = (BlockReference)_BufferModel.Entities[i]; //取得參考圖塊
                    var BoltsAttr = blockReference.EntityData as GroupBoltsAttr;
                    //BoltsList.Add(BoltsAttr);

                    //如果直徑和面已經存在 則加入舊的 否則建立新的資料
                    if (DrillBoltsList.Exists(x => (x.DrillHoleDiameter == BoltsAttr.Dia && x.Face == BoltsAttr.Face)))
                    {
                        DrillBoltsList.Find(x => x.DrillHoleDiameter == BoltsAttr.Dia).DrillHoleCount += BoltsAttr.Count;
                    }
                    else
                    {
                        DrillBoltsList.Add(new DrillBolts { DrillHoleDiameter = BoltsAttr.Dia, DrillHoleCount = BoltsAttr.Count, Face = BoltsAttr.Face });
                    }
                }
            }

            return true;
        }







        /// <summary>
        /// 未加工-已完成清單-細項
        /// </summary>
        public ObservableCollection<MaterialPartDetail> Finish_UndoneDataViewsDetail
        {
            get
            {
                var _MPartDetail = new ObservableCollection<MaterialPartDetail>();
                Finish_UndoneDataViews.ForEach(FUdataviews =>
                {
                    if (FUdataviews != null)
                    {
                        if (FUdataviews.Parts.Count != 0)
                        {
                            foreach (var FUPart in FUdataviews.Parts)
                            {

                                //加入素材<->零件 先素材再零件    
                                _MPartDetail.Add(new MaterialPartDetail()
                                {
                                    MaterialNumber = FUdataviews.MaterialNumber,//排版編號
                                    Profile = FUdataviews.Profile,//斷面規格
                                    Material = FUdataviews.Material,//材質
                                    AssemblyNumber = FUPart.AssemblyNumber,
                                    PartNumber = FUPart.PartNumber,
                                    Length = FUPart.Length,
                                    Position = FUdataviews.Position,
                                });
                            }
                        }
                    }
                });

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
                MachiningDetail_DrillBoltsItemSource = GetDrillBoltsItemCollection(new List<string>() { value.AssemblyNumber });
                _finish_UndoneDataViewsDetail_SelectedItem = value;
            }
        }

        /// <summary>
        /// 展開素材內的零件所使用的資料表
        /// </summary>
        public struct MaterialPartDetail
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
            public string Position { get; set; }
        }






        public GridControl Combinational_List_GridControl { get; set; }





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
                _Model.Clear();
                STDSerialization ser = new STDSerialization();
                _SelectedItem = value;
                ReadFile file = ser.ReadMaterialModel(value.MaterialNumber);
                file.DoWork();
                file.AddToScene(_Model);
                _Model.Refresh();
                _Model.ZoomFit();//設置道適合的視口
                _Model.Invalidate();
            }
        }

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
        /// <summary>
        /// 持續序列化
        /// </summary>
        //private Thread _SerializationThread;
        /// <summary>
        /// 持續監看Host狀態
        /// </summary>
        //private Thread _HostThread;
        private devDept.Eyeshot.Model _Model { get; set; }
        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private MaterialDataView _SelectedItem;
        private bool _DsposedValue;
        private STDSerialization _Ser = new STDSerialization();
        private Task _CreateFileTask;
        private Task _WriteCodesysTask;
        private CancellationTokenSource _Cts = new CancellationTokenSource();
        private CancellationToken _Token;
        private SynchronizationContext _SynchronizationContext;
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
        private devDept.Eyeshot.Model _BufferModel { get; set; }
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
        private long DrMOffset = Marshal.OffsetOf(typeof(WorkMaterial), nameof(WorkMaterial.DrMiddle)).ToInt64();
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
                    {
                        write.SetMonitorWorkOffset(new byte[1] { 1 }, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.ContinueWork)).ToInt64()); //寫入準備加工的陣列位置
                    }
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
        /// 插單命令
        /// </summary>
        public WPFBase.RelayParameterizedCommand InsertCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(el =>
                {
                    using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                    using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                    {
                        var index = read.GetIndex().ToList();
                        var workOther = read.GetWorkOther();
                        var workIndex = index[workOther.Current + 1];
                        long cWork = _WorkOffset + _WorkSize * workIndex;
                        long cInsert = cWork + Marshal.OffsetOf<WorkMaterial>(nameof(WorkMaterial.Insert)).ToInt64();
                        index.Insert(workOther.Current + 1, Convert.ToInt16(Finish_UndoneDataViews.IndexOf((MaterialDataView)el)));
                        write.SetMonitorWorkOffset(new byte[] { 1 }, cInsert); //寫入準備加工的陣列位置
                        write.SetMonitorWorkOffset(index.ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置
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
                    short[] index;
                    using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                    {
                        index = read.GetIndex();
                    }
                    MaterialDataView dataView = (MaterialDataView)el;
                    int selected = Finish_UndoneDataViews.IndexOf(dataView);
                    short[] value = new short[index.Length + 1];
                    Array.Copy(index, value, index.Length);
                    value[value.Length - 1] = Convert.ToInt16(selected);
                    long indexOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.Index)).ToInt64(); //index 偏移量
                    byte[] writeByte = value.ToByteArray();
                    using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                        write.SetMonitorWorkOffset(writeByte, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置
                });
            }
        }
        public WPFBase.RelayParameterizedCommand FinishCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(el =>
                {
                    short[] index;
                    using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                    {
                        index = read.GetIndex();

                    }
                    //MaterialDataView dataView = (MaterialDataView)el;
                    MaterialDataView dataView = (MaterialDataView)el;
                    int selected = Finish_UndoneDataViews.IndexOf(dataView);
                    _WorkMaterials[selected].Finish = 100;
                    _WorkMaterials[selected].IsExport = true;
                    _WorkMaterials[selected].Position = -2;
                    long cWork = _WorkOffset + (_WorkSize * selected);
                    using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                    {

                        WriteCodesysMemor.SetMonitorWorkOffset(_WorkMaterials[selected].ToByteArray(), cWork); //發送加工陣列
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
                    short[] index;
                    using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                    {
                        index = read.GetIndex();
                    }
                    MaterialDataView dataView = SelectedItem;
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

                });
            }
        }
        #endregion

        /// <summary>
        /// 建構式
        /// </summary>
        public ProcessingMonitor_MachineVM()
        {
            Display3DViewerCommand = Display3DViewer();
            Finish_UndoneDataViews = _Ser.GetMaterialDataView();
            //UndoneDataView = new ObservableCollection<MaterialDataView>(DataViews);
            //Finish_UndoneDataViews = new ObservableCollection<MaterialDataView>(DataViews);
            _SynchronizationContext = SynchronizationContext.Current;
            _WorkMaterials = new WorkMaterial[Finish_UndoneDataViews.Count];

            _BufferModel = new devDept.Eyeshot.Model();
            _BufferModel.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            _BufferModel.InitializeViewports();
            _BufferModel.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());
            
            /*SplashScreenManager manager = SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });
            manager.Show();
            try
            {

                manager.ViewModel.Status = "正在生成dm檔";
                manager.ViewModel.IsIndeterminate = false;
                manager.ViewModel.Progress = 0;
                //生成dm檔
                int DataForeachCount = 0;
                DataViews.ForEach(el => //產生素材3D視圖
                {
                    try
                    {
                        manager.ViewModel.Progress = (DataForeachCount * 100) / DataViews.Count();
                        manager.ViewModel.Status = $"正在生成素材編號:{el.MaterialNumber}的dm檔 {DataForeachCount + 1}/{DataViews.Count()}";

                        var _model = new devDept.Eyeshot.Model();    
                        _model.DataContext = new ObSettingVM();
                        _model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT"); 
                        _model.InitializeViewports();
                        _model.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());
                        VM_AssemblyPart(_model, el.MaterialNumber);
                       // _model.Viewports.Add(new devDept.Eyeshot.Viewport());
                        //_model.AssemblyPart(el.MaterialNumber);

                        manager.ViewModel.Progress = (DataForeachCount * 100) / DataViews.Count();
                    }
                    catch (Exception ex)
                    {
                        AddOperatingLog(LogSourceEnum.Init, "DM生成初始化失敗", true);
                        AddOperatingLog(LogSourceEnum.Init, ex.Message, true);
                    }
                    DataForeachCount++;
                });
                _Ser.SetMaterialDataView(DataViews);
                manager.ViewModel.Status = "dm檔生成完成";
                manager.ViewModel.Progress = 100;
                manager.ViewModel.IsIndeterminate = true;
            }
            catch (Exception ex)
            {
                AddOperatingLog(LogSourceEnum.Init, ex.Message, true);
            }
            
            manager.Close();    */


            //清單初始化
            try
            {
                Finish_UndoneDataViews.ForEach(el => el.Position = WaitBePairString);
                SyncCodesysData();
            }
            catch(Exception ex)
            {
                AddOperatingLog(LogSourceEnum.Init, "清單初始化失敗", true);
                AddOperatingLog(LogSourceEnum.Init, ex.Message, true);
            }


            //啟動一執行序持續掃描是否有錯誤訊息

            ReadErrorInfoTask = Task.Factory.StartNew(() =>
            {
                string Local_ErrorInfo = string.Empty;
                while (ReadTaskBoolean)
                {
                    try
                    {
                        if (ViewLocator.ApplicationViewModel.ErrorInfo != null)
                        {
                            //如果error持續存在，則只加入一次，且當ErrorInfo被清空時重置狀態
                            if (Local_ErrorInfo != ViewLocator.ApplicationViewModel.ErrorInfo)
                            {
                                Local_ErrorInfo = ViewLocator.ApplicationViewModel.ErrorInfo;
                                AddOperatingLog(LogSourceEnum.Machine, ViewLocator.ApplicationViewModel.ErrorInfo, true);
                            }
                        }
                        else
                        {
                            Local_ErrorInfo = string.Empty;
                        }
                        Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });

            //啟動一執行序掃描是否有手機配對
            Task.Run(() =>
            {
                try { 
                WriteTourData();
                    if (MachineAndPhoneAPI.AppServerCommunicate.GetEnableAppPairing(out var Result))
                    {
                        if (Result)
                        {
                            //若為手機連線模式
                            Input_by_SmartPhone_RadioButtonIsChecked = true;
                            Input_by_Computer_RadioButtonIsChecked = false;
                            TourTaskStart();
                        }
                        else
                        {
                            Input_by_Computer_RadioButtonIsChecked = true;
                            Input_by_SmartPhone_RadioButtonIsChecked = false;
                        }
                    }
                    else
                    {
                        AddOperatingLog(LogSourceEnum.Software, "取得手機連線模式失敗", false);

                    }
                }
                catch(Exception ex)
                {
                    AddOperatingLog(LogSourceEnum.Init, "取得手機連線模式失敗", true);
                    AddOperatingLog(LogSourceEnum.Init, ex.Message, true);
                }
            });

            //寫入加工參數

            /*Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        _WriteCodesysTask = Task.Factory.StartNew(WriteCodesys, _Cts.Token);
                        _WriteCodesysTask.Wait();
                        SyncCodesysData();
                      
                    }
                    catch(Exception ex)
                    {
                        AddOperatingLog(LogSourceEnum.Software, "寫入加工參數失敗", true);
                        AddOperatingLog(LogSourceEnum.Software, ex.Message, true);
                    }
                    Thread.Sleep(2000);
                }
            });               */

            //持續序列化
            /*
            NewSerializationTask = Task.Factory.StartNew(() =>
            {
                _WriteCodesysTask?.Wait();
                short[] _LastTime = new short[0];
                while (true)
                {
                    Thread.Sleep(10);
                    int count = 0;
                    try
                    {
                        short[] index = null;
                        WorkOther workOther = null;
                        Host host;
                        using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                        {
                            index = read.GetIndex();
                            workOther = read.GetWorkOther();
                            host = read.GetHost();
                        }
                        Current = workOther.Current;

                        _Ser.SetWorkMaterialOtherBackup(workOther);
                        _Ser.SetWorkMaterialIndexBackup(index); //備份 Index
                        var noInfo = index.Except(_SendIndex).ToArray(); //查詢尚未發送加工孔位的 index 
                        for (int i = 0; i < noInfo.Length; i++) //找出沒發送過的工作陣列
                        {
                            SendDrill(noInfo[i]); //發送
                        }
                        _SendIndex.AddRange(noInfo); //存取已經發送過的列表
                        List<short> SerializationValue = new List<short>(index);
                        List<short> delete = _LastTime.Except(SerializationValue).ToList(); //找出上次有序列化的文件
                        SerializationValue.AddRange(delete);
                        //_Ser.SetWorkMaterialIndexBackup(index);

                        var serIndexList = SerializationValue.Except(_Finish); //差集未完成的陣列數值

                        var taskList = new List<Task>();

                        foreach (var serIndex in serIndexList)
                        {
                            //序列化
                            taskList.Add(Task.Run(() =>
                            {
                                Serialization(serIndex);
                            }));
                        }
                        Task.WaitAll(taskList.ToArray());

                        _LastTime = index.ToArray();
                     
                    }
                    catch (Exception ex)
                    {
                        count++;
                        if (count < 30)
                        {
                            //AddOperatingLog(LogSourceEnum.Software, $"序列化執行續讀取失敗第 {count} 次 ...");
                        }
                        else
                        {
                            AddOperatingLog(LogSourceEnum.Software, "序列化執行續同步失敗，正在重試");
                            log4net.LogManager.GetLogger("同步失敗").Debug($"同步失敗");
                            // throw new Exception("伺服器無法連線 ....");
                        }
                    }

                    Thread.Sleep(2000); //等待 2 秒後執行

                }
            });

            HostTask = Task.Factory.StartNew(() =>
            {
                _WriteCodesysTask?.Wait();
                while (true)
                {
                    try
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
                    catch (Exception ex)
                    {

                    }
                }
            });
                   */


        }


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
                    FloatingMode.Popup);
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
                                FloatingMode.Popup);
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







        /*
        private void Serialization(short serIndex)
        {
            int exCount = 1, //出口數量
                enCount = 1; //入口數量
            using (Memor.ReadMemorClient client = new Memor.ReadMemorClient())
            {
                //_SynchronizationContext.Send(t =>
                //{
                try
                {
                    //ushort q = Convert.ToUInt16(serIndex);
                    _WorkMaterials[serIndex] = client.GetWorkMaterial((ushort)serIndex);
                    _Ser.SetWorkMaterialBackup(_WorkMaterials[serIndex]);
                    string number = _WorkMaterials[serIndex].MaterialNumber
                                                                                                .Where(el => el != 0)
                                                                                                .Select(el => Convert.ToChar(el).ToString())
                                                                                                .Aggregate((str1, str2) => str1 + str2);

                    if (!Finish_UndoneDataViews.ToList().Exists(el => el.MaterialNumber == number))
                    {
                        Finish_UndoneDataViews.Add(DataViews[serIndex]);
                    }
                    var Index = Finish_UndoneDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                    if (Index == -1)
                    {
                        //找不到編號零件
                        //Debugger.Break();
                        return;
                    }

                    if (_WorkMaterials[serIndex].Position == -2) //如果已經完成
                    {

                        //FinishDataViews.ToList().existed(el => el.MaterialNumber == number);
                        //if (finishIndex == -1)
                        //{
                        //    FinishDataViews.Add(DataViews[value]);
                        //}
                        //int finishIndex = FinishDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                        //FinishDataViews[finishIndex].Finish = true;
                        //FinishDataViews[finishIndex].Position = FinishString;
                        Finish_UndoneDataViews[Index].Finish = true;
                        Finish_UndoneDataViews[Index].Position = FinishString;

                        //DependencyObject rowState = FinishGrid.GetRowState(finishIndex, false);//找該Index的RowStat
                        //rowState.SetValue(RowPropertyHelper.BackgroundProperty, Brushes.Lime);//給予背景顏色
                        //rowState.SetValue(RowPropertyHelper.ForegroundProperty, Brushes.LightGray);//
                        //FinishGrid.RefreshRow(finishIndex);
                        //UndoneDataView.Remove(DataViews[value]);
                        _Finish.Add(serIndex); //加入到完成列表
                    }
                    else if (_WorkMaterials[serIndex].Position == -1) //如果已經完成
                    {
                        // int arrayIndex = UndoneDataView.ToList().FindIndex(el => el.MaterialNumber == number);
                        //int arrayIndex = Finish_UndoneDataViews.ToList().FindIndex(el => el.MaterialNumber == number);

                        //int count = _WorkMaterials[value].BoltsCountL + _WorkMaterials[value].BoltsCountR + _WorkMaterials[value].BoltsCountM;
                        //int finishCount = _WorkMaterials[value].DrMiddle //完成的數量
                        //                                                                        .Union(_WorkMaterials[value].DrLeft)
                        //                                                                        .Union(_WorkMaterials[value].DrRight)
                        //                                                                        .Where(el => el.Finish)
                        //                                                                        .Count();
                        //UndoneDataView[arrayIndex].Schedule =Convert.ToDouble(finishCount) /Convert.ToDouble(count) * 100d; //完成趴數
                        //UndoneDataView[arrayIndex].Schedule =  _WorkMaterials[value].Finish;
                        //UndoneDataView[arrayIndex].Position = MachiningString;
                        Finish_UndoneDataViews[Index].Schedule = _WorkMaterials[serIndex].Finish;
                        Finish_UndoneDataViews[Index].Position = MachiningString;

                        //DependencyObject rowState = UndoneGrid.GetRowState(arrayIndex, false);//找該Index的RowStat
                        //rowState.SetValue(RowPropertyHelper.BackgroundProperty, Brushes.Lime);//給予背景顏色
                        //rowState.SetValue(RowPropertyHelper.ForegroundProperty, Brushes.LightGray);//給予字體顏色

                    }
                    else if (_WorkMaterials[serIndex].Position == 0)
                    {
                        //FinishDataViews.Remove(DataViews[value]);
                        //int arrayIndex = UndoneDataView.ToList().FindIndex(el => el.MaterialNumber == number);

                       // if (arrayIndex == -1) //如果沒有在列表內
                       // {
                       //     UndoneDataView.Add(DataViews[value]);
                      //      UndoneDataView[UndoneDataView.Count].Position = WaitBePairString;
                       // }
                     //   else
                      //  {
                      //      int undoneIndex = UndoneDataView.ToList().FindIndex(el => el.MaterialNumber == number);
                      //      UndoneDataView[undoneIndex].Position = $WaitBePairString;
                     //       UndoneGrid.RefreshRow(undoneIndex);
                      //  }
                        //Finish_UndoneDataViews[Index].Position = WaitBePairString;
                    }
                    else
                    {
                        if (_WorkMaterials[serIndex].IsExport) //如果再出口處
                        {
                            //UndoneDataView.Remove(DataViews[value]);
                            //int finishIndex = FinishDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                          //  if (finishIndex == -1) //如果找不到物件 
                          //  {
                          //      FinishDataViews.Add(DataViews[value]);
                          //      finishIndex = FinishDataViews.Count -1;
                        //    }
                        //    FinishDataViews[finishIndex].Position = $"等待(出)-{enCount}";
                        //    FinishGrid.RefreshRow(finishIndex);
                        //    enCount++;
                            Finish_UndoneDataViews[Index].Position = $"等待(出)-{enCount}";
                        }
                        else
                        {
                            Finish_UndoneDataViews[Index].Position = $"等待(入)-{exCount}";
                        }
                        exCount++;
                    }

                    //舊介面用
                    try
                    {
                        if (UndoneGrid != null)
                        {
                            if (UndoneGrid.ItemsSource != null)
                            {
                                var arrayIndex = (UndoneGrid.ItemsSource as ObservableCollection<MaterialPartDetail>).ToList().FindIndex(el => el.MaterialNumber == number);
                                if (arrayIndex != -1)
                                {
                                    UndoneGrid.RefreshRow(arrayIndex);
                                }
                            }
                        }
                        if (FinishGrid != null)
                        {
                            if (FinishGrid.ItemsSource != null)
                            {
                                var arrayIndex = (FinishGrid.ItemsSource as ObservableCollection<MaterialPartDetail>).ToList().FindIndex(el => el.MaterialNumber == number);
                                if (arrayIndex != -1)
                                {
                                    FinishGrid.RefreshRow(arrayIndex);
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                catch (Exception ex)
                {
                    AddOperatingLog(LogSourceEnum.Software, "序列化問題", true);
                    AddOperatingLog(LogSourceEnum.Software, ex.Message, true);
                }
                // }, null);
            }
        }          */




        private bool ReadTaskBoolean = true;
        private Task ReadErrorInfoTask;
        private Task NewSerializationTask;
        private Task HostTask;



        /// <summary>
        /// 寫入加工參數到 Codesys <see cref="GD_STD.Phone.MonitorWork"/>
        /// </summary>
        /// <returns></returns>
        public void WriteCodesys()
        {
            //await Task.Yield();
            //設定 Codesys MonitorWork 專案訊息
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
                    workOther = _Ser.GetWorkMaterialOtherBackup();
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
                    short[] dataIndex = _Ser.GetWorkMaterialIndexBackup();

                    if (ApplicationViewModel.ProjectName == read.GetProjectName()) //如果相同專案名稱
                    {
                        Array.Copy(dataIndex, index, dataIndex.Length); //複製備份檔的 index 到要發送的 index
                    }
                    else //如果不同專案名稱
                    {
                        if (dataIndex.Length != 0)
                            Array.Copy(dataIndex, index, current == -1 ? 0 : current + 1); //複製備份檔的 index 到要發送的 index

                    }
                }

                if (ApplicationViewModel.ProjectName != read.GetProjectName()) //如果相同專案
                {
                    byte[] project = ApplicationViewModel.ProjectName.ToByteArray(); //目前專案名稱
                    write.SetMonitorWorkOffset(project, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.ProjectName)).ToInt64()); //寫入專案名稱
                }
                Current = current;
                write.SetMonitorWorkOffset(current.ToByteArray(), currentOffset);//寫入Current
                write.SetMonitorWorkOffset(enOccupy.ToByteArray(), enOccupyOffset); //寫入入口料架占用長度
                write.SetMonitorWorkOffset(exOccupy1.ToByteArray(), exOccupy1Offset);//寫入出口料架占用長度 (1) 
                write.SetMonitorWorkOffset(exOccupy2.ToByteArray(), exOccupy2Offset);//寫入出口料架占用長度 (2) 
                write.SetMonitorWorkOffset(Convert.ToInt16(Finish_UndoneDataViews.Count).ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Count)).ToInt64()); //寫入準備加工的陣列位置
                write.SetMonitorWorkOffset(index.ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置
            }

            int workIndex = 0;
            Finish_UndoneDataViews.ForEach(view =>
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
                #endregion

                var SteelPartCollection = _Ser.GetPart($"{view.Profile.GetHashCode()}");
                if (SteelPartCollection != null && view.Parts.Count != 0)
                {
                    var steelPart = SteelPartCollection[0];
                    using (Memor.WriteMemorClient Write = new Memor.WriteMemorClient())
                    {
                        Write.SetMonitorWorkOffset(view.MaterialNumber.ToByteArray(), cMaterialNumber);//寫入素材編號
                        Write.SetMonitorWorkOffset(Encoding.ASCII.GetBytes(view.Profile), cProfile);//寫入斷面規格
                        Write.SetMonitorWorkOffset(view.Parts
                            .Select(el => el.PartNumber).Aggregate((str1, str2) => $"{str1},{str2}").ToByteArray(), cPartNumber);//寫入零件編號
                        Write.SetMonitorWorkOffset(view.LengthStr.ToByteArray(), cLength);//寫入素材長度
                        Write.SetMonitorWorkOffset(Encoding.ASCII.GetBytes(view.Material == null ? "0" : view.Material), (_WorkOffset + _WorkSize * workIndex) + _MaterialOffset);//寫入材質
                        Write.SetMonitorWorkOffset(view.Parts
                            .Select(el => el.AssemblyNumber).Aggregate((str1, str2) => $"{str1},{str2}").ToByteArray(), cAssemblyNumber);//寫入構件編號
                        Write.SetMonitorWorkOffset(steelPart.H.ToByteArray(), cH);//寫入高度
                        Write.SetMonitorWorkOffset(steelPart.W.ToByteArray(), cW);//寫入寬度
                        Write.SetMonitorWorkOffset(steelPart.t1.ToByteArray(), ct1);//寫入腹板厚度
                        Write.SetMonitorWorkOffset(steelPart.t2.ToByteArray(), ct2);//寫入翼板厚度
                        Write.SetMonitorWorkOffset(Encoding.ASCII.GetBytes(view.MaterialNumber), cGuid);//寫入圖面 GUID
                    }
                }
                workIndex++;
            });
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
        private void SendDrill(long boltsCountOffset, long drOffset, List<Drill> drills, SteelPart steelPart, long cWork, double h, double length, double[] cutPointX = null)
        {
            List<Drill> dList = new List<Drill>();
            drills.ForEach(el =>
            {
                if (dList.FindIndex(e => el.X == e.X && e.Y == el.Y) == -1)
                {
                    dList.Add(el);
                }
            });
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
                WriteCodesysMemor.SetMonitorWorkOffset(Convert.ToUInt16(drillArray.Length).ToByteArray(), cBoltsL);//左軸加工陣列數量
                WriteCodesysMemor.SetMonitorWorkOffset(drillArray.ToByteArray(), cDrillL);//左軸加工陣列
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

        private void SendDrill(int index)
        {
            _CreateFileTask?.Wait(); //等待 Task CreateFile 完成 link:ProcessingMonitorVM.cs:CreateFile()
                                     //_SynchronizationContext.Wait();
            _SynchronizationContext.Send(t =>
            {
                _BufferModel.Clear();
                //List<double> cutPointX = new List<double>();
                //產生加工陣列
                var view = Finish_UndoneDataViews[index];
                Dictionary<FACE, List<Drill>> keyValuePairs = new Dictionary<FACE, List<Drill>>();
                if (File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{view.MaterialNumber}.dm"))
                {
                    ReadFile readFile = _Ser.ReadMaterialModel(view.MaterialNumber);
                    //_SynchronizationContext.Send(t =>
                    //{
                    _BufferModel.Clear();
                    readFile.DoWork();
                    readFile.AddToScene(_BufferModel);
                    //}, null);
                    FACE face = FACE.TOP;
                    var steelPart = _Ser.GetPart($"{view.Profile.GetHashCode()}")[0];
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

                            _BufferModel.Blocks[blockReference.BlockName].Entities.ForEach(steel =>
                            {

                                //Point3D[] oCut = steel.Vertices.Select(el =>  )
                            });
                        }
                    });

                    long cWork = _WorkOffset + _WorkSize * index;

                    //葉:計算切割點先頂著用，等待修正
                    #region 計算切割點
                    //double[] cutPointX = new double[view.Parts.Count +1];
                    //double total = view.StartCut;
                    //cutPointX[0] =total -(view.Cut*0.5);
                    //for (int i = 0; i < view.Parts.Count; i++)
                    //{

                    //}
                    #endregion


                    if (keyValuePairs.ContainsKey(FACE.FRONT))
                    {
                        SendDrill(_BoltsCountLOffset, _DrLOffset, keyValuePairs[FACE.FRONT], steelPart, cWork, steelPart.W, Finish_UndoneDataViews[index].LengthStr, null);
                    }
                    if (keyValuePairs.ContainsKey(FACE.TOP))
                    {
                        SendDrill(_BoltsCountMOffset, DrMOffset, keyValuePairs[FACE.TOP], steelPart, cWork, steelPart.H, Finish_UndoneDataViews[index].LengthStr, null);
                    }
                    if (keyValuePairs.ContainsKey(FACE.BACK))
                    {
                        SendDrill(_BoltsCountROffset, _DrROffset, keyValuePairs[FACE.BACK], steelPart, cWork, steelPart.W, Finish_UndoneDataViews[index].LengthStr, null);
                    }
                }
            }, null);
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
        /// <param name="entities">3D 模型物件列表</param>
        private static Drill[] BoltAsDrill(Entity[] entities, Transformation transformation = null)
        {

            if (!entities.Any(el => el.EntityData is BoltAttr))
                throw new Exception("entities.EntityData 有查詢到非 BoltAttr 的類型。");


            Drill[] drills = new Drill[entities.Length];
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
            return drills;
        }




        /// <summary>
        /// 比對衝突 並將已完成的加入到完成列表
        /// </summary>
        private void SyncCodesysData()
        {
            //int synIndex = 0;
            if (ApplicationViewModel.PanelButton.Key != KEY_HOLE.AUTO) //如果沒有在自動狀況下
            {
                //如有備份檔就寫回給 Codesys
                for (int i = 0; i < Finish_UndoneDataViews.Count; i++)
                {
                    //葉:需要比對衝突
                    WorkMaterial? work = _Ser.GetWorkMaterialBackup(Finish_UndoneDataViews[i].MaterialNumber);
                    if (work != null)
                    {
                        long workOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.WorkMaterial)).ToInt64();
                        int workSize = Marshal.SizeOf(typeof(WorkMaterial));
                        if (work.Value.AssemblyNumber != null && work.Value.MaterialNumber != null)
                        {
                            WriteCodesysMemor.SetMonitorWorkOffset(work.Value.ToByteArray(), workOffset + (workSize * i)); //發送加工陣列
                            _SendIndex.Add(Convert.ToInt16(i));
                            if (work.Value.Position == -2) //如果是完成的狀態
                            {
                                //FinishDataViews.Add(DataViews[i]);
                                //FinishDataViews[FinishDataViews.Count - 1].Position = FinishString;
                                //UndoneDataView.Remove(DataViews[i]);
                                Finish_UndoneDataViews[i].Position = FinishString;
                                _Finish.Add(Convert.ToInt16(i)); //加入到完成列表
                            }
                            //synIndex = i;
                        }
                    }
                }
                //UndoneDataView.ForEach(el => el.Position = WaitBePairString)
            }   
            else //如果有在自動狀況下
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
                        //synIndex = i;
                    }
                }
            }

            using (Memor.WriteMemorClient Write = new Memor.WriteMemorClient())
            {
                long ImportProjectOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.ImportProject)).ToInt64();
                Write.SetMonitorWorkOffset(new byte[1] { 1 }, ImportProjectOffset);//寫入匯入專案完成

                //AddOperatingLog(LogSourceEnum.Software, "寫入匯入專案完成");
            }


        }















        /// <summary>
        /// 產生 <see cref="MaterialDataView"/> 的3D檔案
        /// </summary>
        public async void CreateFile()
        {
            Finish_UndoneDataViews.ForEach(el => //產生素材3D視圖
            {
                if (!File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{el.MaterialNumber}.dm"))
                {

                    _SynchronizationContext.Send(t =>
                    {
                        _Model.Clear();
                        _Model.AssemblyPart(el.MaterialNumber);

                        _Ser.SetMaterialModel(el.MaterialNumber, _Model);//儲存 3d 視圖
                    }, null);
                }
            });
            _SynchronizationContext.Send(t => _Model.Clear(), null);
            await Task.Yield();
            _Ser.SetMaterialDataView(Finish_UndoneDataViews);
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
                    _CreateFileTask?.Dispose();
                    _WriteCodesysTask?.Dispose();
                    if (_BufferModel != null)
                    {
                        try
                        {
                            _BufferModel.Dispose();
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    ReadTaskBoolean = false;

                    try
                    {
                        if (ReadErrorInfoTask != null)
                        {
                            ReadErrorInfoTask.Dispose();
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (NewSerializationTask != null)
                        {
                            NewSerializationTask.Dispose();
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (HostTask != null)
                        {
                            HostTask.Dispose();
                        }
                    }
                    catch
                    {

                    }
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
        /// 切換到橫移料架
        /// </summary>
        public ICommand ChangeTransportModeCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    Transport_RadioButtonIsChecked = false;

                    ProgressBar_Visible_Transport_Visibility = true;
                    Transport_RadioButtonIsEnable = false;
                    Transport_by_Continue_RadioButtonIsEnable = false;
                    Transport_by_Hand_RadioButtonIsEnable = false;

                    Task.Run(() =>
                    {


                        Thread.Sleep(500);
                        try
                        {
                            if (GD_STD.Properties.Optional.Default.HandAuto)
                            {
                                STDSerialization ser = new STDSerialization();
                                //FluentAPI.MecSetting mecSetting = ser.GetMecSetting();
                                FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();//讀取記憶體
                                optionSettings.HandAuto = false; //修改參數
                                ser.SetOptionSettings(optionSettings);//寫入記憶體
                            }



                            //台車

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
                                ShowMessageBox(obj as UIElement, str);
                            }

                            AddOperatingLog(LogSourceEnum.Software, "切換到橫移料架");
                            //失敗時跳出提示 成功時則繼續執行
                            Transport_RadioButtonIsChecked = true;
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
                    ProgressBar_Visible_Transport_by_Continue_Visibility = true;
                    Transport_by_Continue_RadioButtonIsChecked = false;
                    Transport_RadioButtonIsEnable = false;
                    Transport_by_Continue_RadioButtonIsEnable = false;
                    Transport_by_Hand_RadioButtonIsEnable = false;

                    Task.Run(() =>
                    {
                        //計時器 單位為毫秒
                        Thread.Sleep(500);
                        //對機台下命令 並回傳是否成功，當成功時IsChecked=true 否則IsChecked = False
                        //等待sleep完成




                        //失敗時跳出提示 成功時則繼續執行
                        try
                        {
                            if (GD_STD.Properties.Optional.Default.HandAuto)
                            {
                                STDSerialization ser = new STDSerialization();
                                //FluentAPI.MecSetting mecSetting = ser.GetMecSetting();
                                FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();//讀取記憶體
                                optionSettings.HandAuto = false; //修改參數
                                ser.SetOptionSettings(optionSettings);//寫入記憶體
                            }





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
                                //FluentAPI.MecSetting mecSetting = ser.GetMecSetting();
                                FluentAPI.OptionSettings optionSettings = ser.GetOptionSettings();//讀取記憶體
                                optionSettings.HandAuto = !optionSettings.HandAuto; //修改參數
                                ser.SetOptionSettings(optionSettings);//寫入記憶體

                                //打開選配->自動模式->按鈕取消底線
                                if (optionSettings.HandAuto)
                                {
                                    Transport_by_Hand_RadioButtonIsChecked = false;
                                    AddOperatingLog(LogSourceEnum.Software, "手臂切換到自動模式");
                                }
                                else
                                {
                                    Transport_by_Hand_RadioButtonIsChecked = true;
                                    AddOperatingLog(LogSourceEnum.Software, "手臂切換到手動模式");
                                }
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
                return _transport_by_hand_RadioButtonIsEnable;
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


        public ICommand Set_Input_by_Computer_Command
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    try
                    {
                        Input_by_Computer_RadioButtonIsChecked = false;
                        ProgressBar_Visible_Input_by_Computer = true;
                        Task.Run(() =>
                        {
                            Input_by_SmartPhone_RadioButtonVMEnable = false;
                            Input_by_Computer_RadioButtonVMEnable = false;


                            var Result = MachineAndPhoneAPI.AppServerCommunicate.SetMachineenableAppPairing();

                            //不管成功或失敗 都使按鈕恢復可用
                            Input_by_SmartPhone_RadioButtonVMEnable = true;
                            Input_by_Computer_RadioButtonVMEnable = true;

                            ProgressBar_Visible_Input_by_Computer = false;
                            if (Result)
                            {
                                AddOperatingLog(LogSourceEnum.Software, "切換到機台模式");
                                Input_by_Computer_RadioButtonIsChecked = true;
                                TourTaskBoolean = false;
                            }
                            else
                            {
                                if (obj is UIElement)
                                {
                                    ShowMessageBox(obj as UIElement, "無法切換到機台模式，需檢查伺服器連線是否正常");
                                }
                                AddOperatingLog(LogSourceEnum.Software, "無法切換到機台模式", true);
                            }

                        });
                    }
                    catch
                    {

                    }
                });
            }
        }

        public ICommand Set_Input_by_SmartPhone_Command
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {

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

                            TourTaskStart();
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
        Task TourTask;
        bool TourTaskBoolean = false;
        private void TourTaskStart()
        {
            try
            {
                TourTaskBoolean = false;
                if (TourTask != null)
                {
                    TourTask.Wait();
                    if (TourTask.Status == TaskStatus.Running)
                    {
                        TourTask.Dispose();
                    }
                }
            }
            catch
            {

            }
            //開始輪巡
            TourTask = new Task(() =>
            {
                TourTaskBoolean = true;
                AddOperatingLog(LogSourceEnum.Software, "啟用輪巡", false);
                while (TourTaskBoolean)
                {
                    try
                    {
                        if (MachineAndPhoneAPI.AppServerCommunicate.GetEnableAppPairing(out var Result))
                        {
                            if (Result)
                            {
                                //若為手機連線模式
                                Input_by_SmartPhone_RadioButtonIsChecked = true;
                                Input_by_Computer_RadioButtonIsChecked = false;
                            }
                            else
                            {
                                Input_by_Computer_RadioButtonIsChecked = true;
                                Input_by_SmartPhone_RadioButtonIsChecked = false;
                            }
                        }
                        else
                        {
                            AddOperatingLog(LogSourceEnum.Software, "取得手機連線模式失敗", false);
                        }

                        WriteTourData();

                        for (int i = 0; i < 5000; i++)
                        {
                            Thread.Sleep(1);
                            if (!TourTaskBoolean)
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                AddOperatingLog(LogSourceEnum.Software, "停用輪巡", false);
            });
            TourTask.Start();
        }
        /// <summary>
        /// 將手機配對寫入機台
        /// </summary>
        private void WriteTourData()
        {
            if (MachineAndPhoneAPI.AppServerCommunicate.GetAppPairingData(out var PairingData))
            {
                foreach (var EachPair in PairingData.data)
                {
                    var MaterialIndex = Finish_UndoneDataViews.ToList().FindIndex(x => (x.MaterialNumber == EachPair.materialNumber));
                    if (MaterialIndex != -1)
                    {
                        if (Finish_UndoneDataViews[MaterialIndex].Position == WaitBePairString)
                        {
                            //軟體配對 
                            //區別手機與機台配料->如果id中有包含專案名稱->機台的 沒有的話則是手機
                            var ProjectNameBase64 = MachineAndPhoneAPI.AppServerCommunicate.StringToBase64Converter(ApplicationViewModel.ProjectName);
                            if (EachPair.id.Contains(ProjectNameBase64))
                            {
                                Finish_UndoneDataViews[MaterialIndex].Position = "手機配對";
                                AddOperatingLog(LogSourceEnum.Phone, $"素材編號：{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}從軟體配料", false);
                            }
                            else
                            {
                                //手機配對
                                Finish_UndoneDataViews[MaterialIndex].Position = "手機配對";
                                AddOperatingLog(LogSourceEnum.Phone, $"素材編號：{Finish_UndoneDataViews[MaterialIndex].MaterialNumber}從手機配料", false);
                            }
                        }

                    }
                }
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


        public async void SetModel(devDept.Eyeshot.Model mdoel, int synIndex = 0, int errorCount = 0)
        {
            try
            {
                if (errorCount == 0) //如果沒有發送失敗
                {
                    _Model = mdoel;
                    _Cts = new CancellationTokenSource();
                    _Token = _Cts.Token;

                    NewSerializationThread();
                    NewHostThread();
                }

                await Task.Yield();
                if (ApplicationViewModel.PanelButton.Key != KEY_HOLE.AUTO) //如果沒有在自動狀況下
                {
                    if (errorCount == 0) //如果沒有發送失敗
                    {
                        _CreateFileTask = Task.Factory.StartNew(CreateFile, _Token);
                        _WriteCodesysTask = Task.Factory.StartNew(WriteCodesys, _Token);
                    }

                    //如有備份檔就寫回給 Codesys
                    for (int i = synIndex; i < Finish_UndoneDataViews.Count; i++)
                    {
                        //葉:需要比對衝突
                        WorkMaterial? work = _Ser.GetWorkMaterialBackup(Finish_UndoneDataViews[i].MaterialNumber);
                        if (work != null)
                        {
                            long workOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.WorkMaterial)).ToInt64();
                            int workSize = Marshal.SizeOf(typeof(WorkMaterial));
                            if (work.Value.AssemblyNumber != null && work.Value.MaterialNumber != null)
                            {
                                WriteCodesysMemor.SetMonitorWorkOffset(work.Value.ToByteArray(), workOffset + (workSize * i)); //發送加工陣列
                                _SendIndex.Add(Convert.ToInt16(i));
                                if (work.Value.Position == -2) //如果是完成的狀態
                                {
                                    //FinishDataViews.Add(Finish_UndoneDataViews[i]);
                                    //FinishDataViews[FinishDataViews.Count - 1].Position = "完成";
                                    //UndoneDataView.Remove(Finish_UndoneDataViews[i]);
                                    Finish_UndoneDataViews[i].Position = FinishString;       //"完成";
                                    _Finish.Add(Convert.ToInt16(i)); //加入到完成列表
                                }
                                synIndex = i;
                            }
                        }
                    }

                    //UndoneDataView.ForEach(el => el.Position = WaitBePairString);
                    foreach (var el in Finish_UndoneDataViews)
                    {        
                        if(el.Position != FinishString)        
                            el.Position = WaitBePairString;
                    }


                }   //如果是在自動狀況下
                else //如果有在自動狀況下
                {
                    using (Memor.ReadMemorClient client = new Memor.ReadMemorClient())
                    {
                        //同步列表
                        for (int i = synIndex; i < Finish_UndoneDataViews.Count; i++)
                        {
                            _WorkMaterials[i] = client.GetWorkMaterial(Convert.ToUInt16(i));
                            if (_WorkMaterials[i].BoltsCountL != 0 || _WorkMaterials[i].BoltsCountR != 0 || _WorkMaterials[i].IndexBoltsM != 0)
                            {
                                _SendIndex.Add(Convert.ToInt16(i));
                            }
                            synIndex = i;
                        }
                    }
                }

                using (Memor.WriteMemorClient Write = new Memor.WriteMemorClient())
                {
                    long ImportProjectOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.ImportProject)).ToInt64();
                    Write.SetMonitorWorkOffset(new byte[1] { 1 }, ImportProjectOffset);//寫入匯入專案完成
                }
            }
            catch (Exception ex)
            {
                if (errorCount < 20) //如果同步失敗 20 次
                {
                    log4net.LogManager.GetLogger("同步失敗").Debug($"同步失敗");
                    SetModel(mdoel, synIndex = 0, errorCount + 1); //繼續同步
                    return;
                }
                else
                {
                    throw new Exception("同步失敗 20 次");
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
                while (true)
                {
                    ContinuedSerialization();
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
                while (true)
                {
                    ContinuedHost();
                }
            }));
            _HostThread.IsBackground = true;
        }



        /// <summary>
        /// 持續序列化
        /// </summary>
        private void ContinuedSerialization(int count = 0)
        {
            try
            {
                short[] index = null;
                WorkOther workOther = null;
                Host host;
                using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                {
                    index = read.GetIndex();
                    workOther = read.GetWorkOther();
                    host = read.GetHost();
                }
                Current = workOther.Current;

                _Ser.SetWorkMaterialOtherBackup(workOther);
                _Ser.SetWorkMaterialIndexBackup(index); //備份 Index
                var noInfo = index.Except(_SendIndex).ToArray(); //查詢尚未發送加工孔位的 index 
                for (int i = 0; i < noInfo.Length; i++) //找出沒發送過的工作陣列
                {
                    SendDrill(noInfo[i]); //發送
                }
                _SendIndex.AddRange(noInfo); //存取已經發送過的列表
                List<short> value = new List<short>(index);
                List<short> delete = _LastTime.Except(value).ToList(); //找出上次有序列化的文件
                value.AddRange(delete);
                SerializationValue = value;
                //_Ser.SetWorkMaterialIndexBackup(index);
                Serialization(SerializationValue);
                _LastTime = index.ToArray();
                Thread.Sleep(2000); //等待 1 秒後執行
            }
            catch (Exception ex)
            {
                if (count < 30)
                {
                    ContinuedSerialization(count + 1);
                    Debug.WriteLine($"讀取失敗第 {count} 次 ...");
                }
                else
                {
                    log4net.LogManager.GetLogger("同步失敗").Debug($"同步失敗");
                    Debugger.Break();
                    //throw new Exception("伺服器無法連線 ....");
                }
            }

        }


        /// <summary>
        /// 要序列化的值
        /// </summary>
        private List<short> SerializationValue { get; set; }
        /// <summary>
        /// 上一次的 Index
        /// </summary>
        private short[] _LastTime { get; set; } = new short[0];

        /// <summary>
        /// 持續監看 Host
        /// </summary>
        /// <param name="count"></param>
        private void ContinuedHost(int count = 0)
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
        public void Serialization(List<short> index)
        {
            try
            {
                if (index.Count == 0)
                {
                    return;
                }
                int exCount = 1, //出口數量
                    enCount = 1; //入口數量
                using (Memor.ReadMemorClient client = new Memor.ReadMemorClient())
                {
                    _SynchronizationContext.Send(t =>
                    {
                        var serIndex = index.Except(_Finish); //差集未完成的陣列數值
                        foreach (var value in serIndex)
                        {
                            ushort q = Convert.ToUInt16(value);
                            _WorkMaterials[value] = client.GetWorkMaterial(q);
                            _Ser.SetWorkMaterialBackup(_WorkMaterials[value]);
                            string number = _WorkMaterials[value].MaterialNumber
                                                                                                        .Where(el => el != 0)
                                                                                                        .Select(el => Convert.ToChar(el).ToString())
                                                                                                        .Aggregate((str1, str2) => str1 + str2);

                            if (_WorkMaterials[value].Position == -2) //如果已經完成
                            {
                                int finishIndex = Finish_UndoneDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                                if (finishIndex == -1)
                                {     
                                    Finish_UndoneDataViews[finishIndex].Finish = true;    
                                    Finish_UndoneDataViews[finishIndex].Position = "完成";
                                }

                                //DependencyObject rowState = FinishGrid.GetRowState(finishIndex, false);//找該Index的RowStat
                                //rowState.SetValue(RowPropertyHelper.BackgroundProperty, Brushes.Lime);//給予背景顏色
                                //rowState.SetValue(RowPropertyHelper.ForegroundProperty, Brushes.LightGray);//給予字體顏色
                                Combinational_List_GridControl.RefreshRow(finishIndex);
                                //UndoneDataView.Remove(Finish_UndoneDataViews[value]);
                                _Finish.Add(value); //加入到完成列表
                            }
                            else if (_WorkMaterials[value].Position == -1) //如果已經完成
                            {
                                int arrayIndex = Finish_UndoneDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                                //int count = _WorkMaterials[value].BoltsCountL + _WorkMaterials[value].BoltsCountR + _WorkMaterials[value].BoltsCountM;
                                //int finishCount = _WorkMaterials[value].DrMiddle //完成的數量
                                //                                                                        .Union(_WorkMaterials[value].DrLeft)
                                //                                                                        .Union(_WorkMaterials[value].DrRight)
                                //                                                                        .Where(el => el.Finish)
                                //                                                                        .Count();
                                //UndoneDataView[arrayIndex].Schedule =Convert.ToDouble(finishCount) /Convert.ToDouble(count) * 100d; //完成趴數
                                Finish_UndoneDataViews[arrayIndex].Schedule = _WorkMaterials[value].Finish;
                                Finish_UndoneDataViews[arrayIndex].Position = "加工中";

                                //DependencyObject rowState = UndoneGrid.GetRowState(arrayIndex, false);//找該Index的RowStat
                                //rowState.SetValue(RowPropertyHelper.BackgroundProperty, Brushes.Lime);//給予背景顏色
                                //rowState.SetValue(RowPropertyHelper.ForegroundProperty, Brushes.LightGray);//給予字體顏色
                                Combinational_List_GridControl.RefreshRow(arrayIndex);
                            }
                            else if (_WorkMaterials[value].Position == 0)
                            {
                                //FinishDataViews.Remove(Finish_UndoneDataViews[value]);
                                int arrayIndex = Finish_UndoneDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                                if (arrayIndex != -1) //如果在列表內
                                {
                                    //int undoneIndex = UndoneDataView.ToList().FindIndex(el => el.MaterialNumber == number);
                                    Finish_UndoneDataViews[arrayIndex].Position = $"等待配對";
                                    Combinational_List_GridControl.RefreshRow(arrayIndex);
                                }
                            }
                            else
                            {
                                if (_WorkMaterials[value].IsExport) //如果再出口處
                                {
                                    //UndoneDataView.Remove(Finish_UndoneDataViews[value]);
                                    int finishIndex = Finish_UndoneDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                                    if (finishIndex != -1)
                                    {
                                        Finish_UndoneDataViews[finishIndex].Position = $"等待(出)-{enCount}";
                                        Combinational_List_GridControl.RefreshRow(finishIndex);
                                    }
                                    enCount++;
                                }
                                else
                                {
                                    //FinishDataViews.Remove(Finish_UndoneDataViews[value]);
                                    int undoneIndex = Finish_UndoneDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                                    if (undoneIndex != -1) //如果找不到物件 
                                    {
                                        Finish_UndoneDataViews[undoneIndex].Position = $"等待(入)-{exCount}";
                                        Combinational_List_GridControl.RefreshRow(undoneIndex);
                                    }
                                    exCount++;
                                }

                            }
                        }
                    }, null);
                }
            }
            catch (Exception)
            {
                Debugger.Break();
            }
        }


    }


}

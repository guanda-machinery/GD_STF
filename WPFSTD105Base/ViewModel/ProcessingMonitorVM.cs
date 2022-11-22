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

//using DevExpress.Utils.Extensions;
namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 加工監控VM
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ProcessingMonitorVM : WPFBase.BaseViewModel, IDisposable/*, MonitorDuplex.IMonitorDuplexCallback*/
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
        /// <summary>
        /// 未加工的清單
        /// </summary>
        public ObservableCollection<MaterialDataView> UndoneDataView { get; set; }
        /// <summary>
        /// 配料清單
        /// </summary>
        public ObservableCollection<MaterialDataView> DataViews { get; set; }
        /// <summary>
        /// 已完成的清單
        /// </summary>
        public ObservableCollection<MaterialDataView> FinishDataViews { get; set; } = new ObservableCollection<MaterialDataView>();

        /// <summary>
        /// 未加工-已完成合併清單
        /// </summary>
        public ObservableCollection<MaterialDataView> Finish_UndoneDataViews
        {
            get
            {
                var DViews = new List<MaterialDataView>();
                DViews.AddRange(FinishDataViews);
                DViews.AddRange(UndoneDataView);

                return new ObservableCollection<MaterialDataView>(DViews); ;
            }
        }

        public MaterialDataView Finish_UndoneDataViews_Selected { get; set; }




        /// <summary>
        /// 未加工的控制項
        /// </summary>
        public GridControl UndoneGrid { get; set; }
        /// <summary>
        /// 加工完成的控制項
        /// </summary>
        public GridControl FinishGrid { get; set; }
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

        private bool _MachiningCombinationl_List_GridControl_IsSelected = false;
        /// <summary>
        /// 已選擇MachiningCombinationl_List_GridControl
        /// </summary>
        public bool MachiningCombinationl_List_GridControl_IsSelected
        {
            get
            {
                return _MachiningCombinationl_List_GridControl_IsSelected;
            }
            set
            {
                if (_MachiningCombinationl_List_GridControl_IsSelected != value)
                {
                    _MachiningCombinationl_List_GridControl_IsSelected = value;
                    //↓可即時反應
                    OnPropertyChanged("MachiningCombinationl_List_GridControl_IsSelected");
                }
            }
        }







        #endregion







        #region 私有屬性
        /// <summary>
        /// 持續序列化
        /// </summary>
        private Thread _SerializationThread;
        /// <summary>
        /// 持續監看Host狀態
        /// </summary>
        private Thread _HostThread;
        private devDept.Eyeshot.Model _Model { get; set; }
        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private MaterialDataView _SelectedItem;
        private bool _DsposedValue;
        private STDSerialization _Ser = new STDSerialization();
        private Task _CreateFileTask;
        private Task _WriteCodesysTask;
        private CancellationTokenSource _Cts;
        private CancellationToken _Token;
        private SynchronizationContext _SynchronizationContext;
        /// <summary>
        /// 完成的 Index
        /// </summary>
        private List<short> _Finish { get; set; } = new List<short>();
        /// <summary>
        /// 上一次的 Index
        /// </summary>
        private short[] _LastTime { get; set; } = new short[0];
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
        public WPFBase.RelayCommand ContinueCommand { get; set; }
        /// <summary>
        /// 續接專案
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayCommand Continue()
        {
            return new WPFBase.RelayCommand(() =>
            {
                using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                {
                    write.SetMonitorWorkOffset(new byte[1] { 1 }, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.ContinueWork)).ToInt64()); //寫入準備加工的陣列位置
                }
            });
        }
        /// <summary>
        /// 呼叫台車命令
        /// </summary>
        public WPFBase.RelayCommand CallCarCommand { get; set; }
        /// <summary>
        /// 呼叫台車
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayCommand CallCar()
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
                        FloatingMode.Popup);
                }
            });
        }


        /// <summary>
        /// 插單命令
        /// </summary>
        public WPFBase.RelayParameterizedCommand InsertCommand { get; set; }
        /// <summary>
        /// 插單
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayParameterizedCommand Insert()
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
                    index.Insert(workOther.Current + 1, Convert.ToInt16(DataViews.IndexOf((MaterialDataView)el)));
                    write.SetMonitorWorkOffset(new byte[] { 1 }, cInsert); //寫入準備加工的陣列位置
                    write.SetMonitorWorkOffset(index.ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置
                }
            });
        }

        /// <summary>
        /// 加入命令
        /// </summary>
        public WPFBase.RelayParameterizedCommand AddCommand { get; set; }
        /// <summary>
        /// 加入
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayParameterizedCommand Add()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                short[] index;
                using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                {
                    index = read.GetIndex();
                }
                MaterialDataView dataView = (MaterialDataView)el;
                int selected = DataViews.IndexOf(dataView);
                short[] value = new short[index.Length + 1];
                Array.Copy(index, value, index.Length);
                value[value.Length - 1] = Convert.ToInt16(selected);
                long indexOffset = Marshal.OffsetOf(typeof(MonitorWork), nameof(MonitorWork.Index)).ToInt64(); //index 偏移量
                byte[] writeByte = value.ToByteArray();
                using (Memor.WriteMemorClient write = new Memor.WriteMemorClient())
                    write.SetMonitorWorkOffset(writeByte, Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置
            });
        }
        public WPFBase.RelayParameterizedCommand FinishCommand { get; set; }

        public WPFBase.RelayParameterizedCommand Finish()
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
                int selected = DataViews.IndexOf(dataView);
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
        public WPFBase.RelayParameterizedCommand DeleteCommand { get; set; }

        public WPFBase.RelayParameterizedCommand Delete()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                short[] index;
                using (Memor.ReadMemorClient read = new Memor.ReadMemorClient())
                {
                    index = read.GetIndex();
                }
                MaterialDataView dataView = SelectedItem;
                int selected = DataViews.IndexOf(dataView);
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
        #endregion

        /// <summary>
        /// 建構式
        /// </summary>
        public ProcessingMonitorVM()
        {
            Display3DViewerCommand = Display3DViewer();
            DataViews = _Ser.GetMaterialDataView();
            UndoneDataView = new ObservableCollection<MaterialDataView>(DataViews);
            _SynchronizationContext = SynchronizationContext.Current;
            _WorkMaterials = new WorkMaterial[DataViews.Count];
            CallCarCommand = CallCar();
            AddCommand = Add();
            _BufferModel = new devDept.Eyeshot.Model();
            _BufferModel.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            _BufferModel.InitializeViewports();
            _BufferModel.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());
            InsertCommand = Insert();
            FinishCommand = Finish();
            ContinueCommand = Continue();


            int synIndex = 0;
            if (ApplicationViewModel.PanelButton.Key != KEY_HOLE.AUTO) //如果沒有在自動狀況下
            {

                //如有備份檔就寫回給 Codesys

                for (int i = synIndex; i < DataViews.Count; i++)
                {
                    //葉:需要比對衝突
                    WorkMaterial? work = _Ser.GetWorkMaterialBackup(DataViews[i].MaterialNumber);
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
                                FinishDataViews.Add(DataViews[i]);
                                FinishDataViews[FinishDataViews.Count - 1].Position = "完成";
                                UndoneDataView.Remove(DataViews[i]);
                                _Finish.Add(Convert.ToInt16(i)); //加入到完成列表
                            }
                            synIndex = i;
                        }
                    }
                }
                UndoneDataView.ForEach(el => el.Position = "等待配對");

            }   //如果是在自動狀況下
            else //如果有在自動狀況下
            {
                using (Memor.ReadMemorClient client = new Memor.ReadMemorClient())
                {
                    //同步列表
                    for (int i = synIndex; i < DataViews.Count; i++)
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

        /// <summary>
        /// 寫入加工參數到 Codesys <see cref="GD_STD.Phone.MonitorWork"/>
        /// </summary>
        /// <returns></returns>
        public /*async*/ void WriteCodesys()
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
                    current = workOther.Current;
                    enOccupy = workOther.EntranceOccupy;
                    exOccupy1 = workOther.ExportOccupy1;
                    exOccupy2 = workOther.ExportOccupy2;
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
                write.SetMonitorWorkOffset(Convert.ToInt16(DataViews.Count).ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Count)).ToInt64()); //寫入準備加工的陣列位置
                write.SetMonitorWorkOffset(index.ToByteArray(), Marshal.OffsetOf<MonitorWork>(nameof(MonitorWork.Index)).ToInt64()); //寫入準備加工的陣列位置
            }

            int workIndex = 0;
            DataViews.ForEach(view =>
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
                var steelPart = _Ser.GetPart($"{view.Profile.GetHashCode()}")[0];
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
                var view = DataViews[index];
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
                        SendDrill(_BoltsCountLOffset, _DrLOffset, keyValuePairs[FACE.FRONT], steelPart, cWork, steelPart.W, DataViews[index].LengthStr, null);
                    }
                    if (keyValuePairs.ContainsKey(FACE.TOP))
                    {
                        SendDrill(_BoltsCountMOffset, DrMOffset, keyValuePairs[FACE.TOP], steelPart, cWork, steelPart.H, DataViews[index].LengthStr, null);
                    }
                    if (keyValuePairs.ContainsKey(FACE.BACK))
                    {
                        SendDrill(_BoltsCountROffset, _DrROffset, keyValuePairs[FACE.BACK], steelPart, cWork, steelPart.W, DataViews[index].LengthStr, null);
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
        /// 存取模型
        /// </summary>
        /// <param name="mdoel"></param>
        /// <param name="synIndex">已經同步的 Index</param>
        /// <param name="errorCount">失敗數量</param>
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
                    for (int i = synIndex; i < DataViews.Count; i++)
                    {
                        //葉:需要比對衝突
                        WorkMaterial? work = _Ser.GetWorkMaterialBackup(DataViews[i].MaterialNumber);
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
                                    FinishDataViews.Add(DataViews[i]);
                                    FinishDataViews[FinishDataViews.Count - 1].Position = "完成";
                                    UndoneDataView.Remove(DataViews[i]);
                                    _Finish.Add(Convert.ToInt16(i)); //加入到完成列表
                                }
                                synIndex = i;
                            }
                        }
                    }
                    UndoneDataView.ForEach(el => el.Position = "等待配對");

                }   //如果是在自動狀況下
                else //如果有在自動狀況下
                {
                    using (Memor.ReadMemorClient client = new Memor.ReadMemorClient())
                    {
                        //同步列表
                        for (int i = synIndex; i < DataViews.Count; i++)
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
                    throw new Exception("伺服器無法連線 ....");
                }
            }

        }

        /// <summary>
        /// 產生 <see cref="MaterialDataView"/> 的3D檔案
        /// </summary>
        public async void CreateFile()
        {
            DataViews.ForEach(el => //產生素材3D視圖
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
            _Ser.SetMaterialDataView(DataViews);
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

        /// <summary>
        /// 解構式
        /// </summary>
        ~ProcessingMonitorVM()
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
        //Memor.ReadMemorClient client = new Memor.ReadMemorClient();
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
                                int finishIndex = FinishDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                                if (finishIndex == -1)
                                {
                                    FinishDataViews.Add(DataViews[value]);
                                    finishIndex = FinishDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                                }
                                FinishDataViews[finishIndex].Finish = true;
                                FinishDataViews[finishIndex].Position = "完成";

                                //DependencyObject rowState = FinishGrid.GetRowState(finishIndex, false);//找該Index的RowStat
                                //rowState.SetValue(RowPropertyHelper.BackgroundProperty, Brushes.Lime);//給予背景顏色
                                //rowState.SetValue(RowPropertyHelper.ForegroundProperty, Brushes.LightGray);//給予字體顏色
                                FinishGrid.RefreshRow(finishIndex);
                                UndoneDataView.Remove(DataViews[value]);
                                _Finish.Add(value); //加入到完成列表
                            }
                            else if (_WorkMaterials[value].Position == -1) //如果已經完成
                            {
                                int arrayIndex = UndoneDataView.ToList().FindIndex(el => el.MaterialNumber == number);
                                //int count = _WorkMaterials[value].BoltsCountL + _WorkMaterials[value].BoltsCountR + _WorkMaterials[value].BoltsCountM;
                                //int finishCount = _WorkMaterials[value].DrMiddle //完成的數量
                                //                                                                        .Union(_WorkMaterials[value].DrLeft)
                                //                                                                        .Union(_WorkMaterials[value].DrRight)
                                //                                                                        .Where(el => el.Finish)
                                //                                                                        .Count();
                                //UndoneDataView[arrayIndex].Schedule =Convert.ToDouble(finishCount) /Convert.ToDouble(count) * 100d; //完成趴數
                                UndoneDataView[arrayIndex].Schedule = _WorkMaterials[value].Finish;
                                UndoneDataView[arrayIndex].Position = "加工中";

                                //DependencyObject rowState = UndoneGrid.GetRowState(arrayIndex, false);//找該Index的RowStat
                                //rowState.SetValue(RowPropertyHelper.BackgroundProperty, Brushes.Lime);//給予背景顏色
                                //rowState.SetValue(RowPropertyHelper.ForegroundProperty, Brushes.LightGray);//給予字體顏色
                                UndoneGrid.RefreshRow(arrayIndex);
                            }
                            else if (_WorkMaterials[value].Position == 0)
                            {
                                FinishDataViews.Remove(DataViews[value]);
                                int arrayIndex = UndoneDataView.ToList().FindIndex(el => el.MaterialNumber == number);
                                if (arrayIndex == -1) //如果沒有在列表內
                                {
                                    UndoneDataView.Add(DataViews[value]);
                                    UndoneDataView[UndoneDataView.Count].Position = "等待配對";
                                }
                                else
                                {
                                    int undoneIndex = UndoneDataView.ToList().FindIndex(el => el.MaterialNumber == number);
                                    UndoneDataView[undoneIndex].Position = $"等待配對";
                                    UndoneGrid.RefreshRow(undoneIndex);
                                }
                            }
                            else
                            {
                                if (_WorkMaterials[value].IsExport) //如果再出口處
                                {
                                    UndoneDataView.Remove(DataViews[value]);
                                    int finishIndex = FinishDataViews.ToList().FindIndex(el => el.MaterialNumber == number);
                                    if (finishIndex == -1) //如果找不到物件 
                                    {
                                        FinishDataViews.Add(DataViews[value]);
                                        finishIndex = FinishDataViews.Count - 1;
                                    }
                                    FinishDataViews[finishIndex].Position = $"等待(出)-{enCount}";

                                    FinishGrid.RefreshRow(finishIndex);
                                    enCount++;
                                }
                                else
                                {

                                    FinishDataViews.Remove(DataViews[value]);
                                    int undoneIndex = UndoneDataView.ToList().FindIndex(el => el.MaterialNumber == number);
                                    if (undoneIndex == -1) //如果找不到物件 
                                    {
                                        UndoneDataView.Add(DataViews[value]);
                                        undoneIndex = UndoneDataView.Count - 1;
                                    }
                                    UndoneDataView[undoneIndex].Position = $"等待(入)-{exCount}";
                                    UndoneGrid.RefreshRow(undoneIndex);
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
        /// <summary>
        /// 要序列化的值
        /// </summary>
        private List<short> SerializationValue { get; set; }


    }

    /// <summary>
    /// RowStat屬性
    /// </summary>
    public class RowPropertyHelper
    {

        public static Brush GetBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BackgroundProperty);
        }

        public static void SetBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// 背景顏色
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(RowPropertyHelper), new PropertyMetadata(false));

        public static Brush GetForegroundProperty(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ForegroundProperty);
        }

        public static void SetForegroundProperty(DependencyObject obj, Brush value)
        {
            obj.SetValue(ForegroundProperty, value);
        }

        /// /// <summary>
        /// 文字顏色
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(RowPropertyHelper), new PropertyMetadata(false));

    }
}

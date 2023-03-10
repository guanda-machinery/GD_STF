using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBase = WPFWindowsBase;
using DevExpress.Xpf.Grid;
using System.Collections.ObjectModel;
using GD_STD.Data;
using static WPFSTD105.ViewLocator;
using DevExpress.Xpf.CodeView;
using DevExpress.Data.Extensions;
using GD_STD.Enum;
using DevExpress.Utils.Extensions;
using System.Threading;
using System.Windows;
using GD_STD.Data.MatchDI;
using System.Text.RegularExpressions;
using WPFSTD105.ViewModel;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;
using WPFSTD105.Model;
using System.Diagnostics;
using DevExpress.Mvvm.Native;
using static WPFSTD105.ViewModel.SettingParVM;

namespace WPFSTD105
{
    /// <summary>
    /// 抽象排版設定
    /// </summary>
    public abstract class AbsTypeSettingVM : WPFBase.BaseViewModel
    {
        //ObSettingVM obvm = new ObSettingVM();
        /// <summary>                             
        /// 控制排版結果是否顯示
        /// </summary>
        public bool ShowTypeResult { get; set; }
        /// <summary>
        /// 抽象排版設定
        /// </summary>
        public AbsTypeSettingVM()
        {
            DataViews = LoadDataViews();
            STDSerialization ser = new STDSerialization();
            MaterialDataViews = ser.GetMaterialDataView();
            AllSelectedGridCommand = AllSelectedGrid();// 選擇報表全部物件命令
            ReverseSelectedGridCommand = ReverseSelectedGrid();//反向選取命令
            UnselectSelectedGridCommand = UnselectSelectedGrid();//取消選取命令
            SaveMatchCommand = SaveMatch();

        }

        /// <summary>
        /// 載入DataViews
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<TypeSettingDataView> LoadDataViews()
        {
            var LoadedDataViews = new ObservableCollection<TypeSettingDataView>();
            STDSerialization ser = new STDSerialization();
            ObservableCollection<BomProperty> bomProperties = CommonViewModel.ProjectProperty.BomProperties; //報表屬性設定檔
            ObservableCollection<SteelAssembly> assemblies = ser.GetGZipAssemblies();//模型構件列表
            
            //20220824 蘇 新增icommand
            Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();//模型構件列表
            foreach (KeyValuePair<string, ObservableCollection<SteelPart>> eachPart in part)
            {
                ObservableCollection<SteelPart> buffer = eachPart.Value;
                //只將 BH RH L TUBE BOX CH H LB([)加入到列表內

                if (buffer != null && buffer.Count !=0)
                {
                    if (ObSettingVM.allowType.Contains(buffer[0].Type))
                    {
                        foreach (var item in buffer.Where(x => x.ExclamationMark == false || x.ExclamationMark is null)) //逐步展開零件
                        {
                            if (item.Father != null)
                            {
                                for (int i = 0; i < item.Father.Count; i++)  //逐步展開零件  id or match
                                {
                                    int index = assemblies.FindIndex(el => el.ID.Contains(item.Father[i])); //找出構件列表內是零件的 Father 位置
                                    if (index == -1) //找不到物件
                                    {
                                        //throw new Exception("index 不可以是 -1");
                                        continue;
                                    }

                                    int idIndex = assemblies[index].ID.IndexOf(item.Father[i]); //找出構件 id 所在的陣列位置
                                    TypeSettingDataView view = new TypeSettingDataView(item, assemblies[index], idIndex, i);
                                    view.SortCount = 0;
                                    int dataIndex = LoadedDataViews.ToList().IndexOf(view); //搜尋指定的物件
                                    if (dataIndex == -1) //如果找不到物件
                                    {
                                        LoadedDataViews.Add(view);
                                    }
                                    else
                                    {
                                        LoadedDataViews[dataIndex].Add(item, i);
                                    }
                                }
                            }
                            else
                            {
                                LoadedDataViews.Add(new TypeSettingDataView()
                                {
                                    Profile = item.Profile,
                                    Length = item.Length,
                                    AssemblyNumber = item.Number,
                                    PartNumber = item.Number,
                                    Material = item.Material,
                                    SortCount = 0,
                                    Match = item.Match,
                                    Weigth = item.UnitWeight
                                });
                            }
                        }
                    }
                }

            }
            return LoadedDataViews;
        }

    




        private GridControl _GridControl { get; set; }
        #region 命令
        /// <summary>
        /// 手動排版命令
        /// </summary>
        public ICommand ManualCommand { get; set; }

        /// <summary>
        /// 顯示零件圖命令
        /// </summary>
        public WPFBase.RelayParameterizedCommand PartDrawingCommand { get; set; }
        /// <summary>
        /// 選擇報表全部物件命令
        /// </summary>
        public ICommand AllSelectedGridCommand { get; set; }
        /// <summary>
        /// 反向選取命令
        /// </summary>
        public ICommand ReverseSelectedGridCommand { get; set; }
        /// <summary>
        /// 取消選取命令
        /// </summary>
        public ICommand UnselectSelectedGridCommand { get; set; }
        /// <summary>
        /// 顯示全部列表命令 (清除過濾器)
        /// </summary>
        public ICommand ClearFilterCommand { get; set; }
        /// <summary>
        /// 儲存配料結果
        /// </summary>
        public ICommand SaveMatchCommand { get; set; }

        #endregion

        #region 命令處理方法
        /// <summary>
        /// 存取配料結果
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayCommand SaveMatch()
        {
            return new WPFBase.RelayCommand(() =>
            {
                STDSerialization ser = new STDSerialization();
                ser.SetMaterialDataView(MaterialDataViews);
            });
        }
        /// <summary>
        /// 手動排版處理方法
        /// </summary>
        /// <returns></returns>
        protected abstract WPFBase.RelayCommand Manual();
        ///// <summary>
        ///// 自動排版處理方法
        ///// </summary>
        ///// <returns></returns>
        //protected abstract WPFBase.RelayCommand Auto();
        /// <summary>
        /// 選擇報表全部物件處理方法
        /// </summary>
        /// <returns></returns>
        protected WPFBase.RelayParameterizedCommand AllSelectedGrid()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                GridControl grid = (GridControl)el;
                grid.SelectAll();//全部資料行
                grid.EndSelection();//强制立即更新
            });
        }
        /// <summary>
        /// 反向選取處理方法
        /// </summary>
        /// <returns></returns>
        protected WPFBase.RelayParameterizedCommand ReverseSelectedGrid()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                GridControl grid = (GridControl)el;
                List<int> useRow = grid.GetSelectedRowHandles().ToList(); //用戶選擇的行數
                int row = grid.VisibleRowCount; //顯示的行數
                for (int i = 0; i < row; i++) //逐步選取資料行
                {
                    if (!useRow.Contains(i))//用戶沒有選擇
                    {
                        grid.SelectItem(i); //選擇資料行
                    }
                }
                grid.EndSelection();//强制立即更新
            });
        }
        /// <summary>
        /// 取消選取處理方法
        /// </summary>
        /// <returns></returns>
        protected WPFBase.RelayParameterizedCommand UnselectSelectedGrid()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                GridControl grid = (GridControl)el;
                grid.UnselectAll(); //取消選取
                grid.EndSelection();//强制立即更新
            });
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand((el) =>
                {
                    ObservableCollection<MaterialDataView> _materialDataViews = new ObservableCollection<MaterialDataView>(MaterialDataViews);
                    ObservableCollection<MaterialDataView> select = new ObservableCollection<MaterialDataView>((ObservableCollection<MaterialDataView>)el);
                    STDSerialization ser = new STDSerialization();

                    foreach (MaterialDataView view in (System.Collections.IList)select)
                    {
                        ObservableCollection<SteelPart> steelParts = ser.GetPart(view.Profile.GetHashCode().ToString());
                        foreach (var part in view.Parts)
                        {
                            int index = DataViews.FindIndex(e => e == part);
                            int m = DataViews[index].Match.FindLastIndex(e => e == false);
                            if (m != -1)
                                DataViews[index].Match[m] = true;
                            int steelIndex = steelParts.FindIndex(e => e.Number == part.PartNumber);
                            int partMatch = steelParts[steelIndex].Match.FindLastIndex(e => e == false);
                            if (partMatch != -1)
                                steelParts[steelIndex].Match[partMatch] = true;
                        }
                        ser.SetPart(view.Profile.GetHashCode().ToString(), new ObservableCollection<object>(steelParts));
                        _materialDataViews.Remove(view);
                        //DataViews.ForEach(e => e.SortCount = e.GetSortCount());
                        DataViews = new ObservableCollection<TypeSettingDataView>(DataViews.ToList());
                        MaterialDataViews = new ObservableCollection<MaterialDataView>(_materialDataViews);
                    }
                    ser.SetMaterialDataView(MaterialDataViews);
                });
            }
        }
        #endregion

        #region 私有屬性
        ///// <summary>
        ///// 建立零件列表內的集合
        ///// </summary>
        //private ObservableCollection<SteelPart> _SteelParts { get; set; } = new ObservableCollection<SteelPart>();
        #endregion

        #region 公開屬性
        ///// <summary>
        ///// 過濾斷面規格選擇器
        ///// </summary>
        //public ObservableCollection<FilterProfileType> FilterProfileTypes { get; set; } = new ObservableCollection<FilterProfileType>();
        ///// <summary>
        ///// 顯示在建立零件列表內的集合
        ///// </summary>
        //public ObservableCollection<SteelPart> SteelParts { get; set; } = new ObservableCollection<SteelPart>();
        // public ObservableCollection<MaterialDataView> SelectedMaterial { get; set; } = new ObservableCollection<MaterialDataView>();


        private ObservableCollection<TypeSettingDataView> dataViews = new ObservableCollection<TypeSettingDataView>();
        /// <summary>
        /// 報表視圖
        /// </summary>
        public ObservableCollection<TypeSettingDataView> DataViews { get { return dataViews; } set { dataViews = value; OnPropertyChanged(nameof(DataViews)); } } 
        public ObservableCollection<TypeSettingDataView> SelectedParts { get; set; } = new ObservableCollection<TypeSettingDataView>();



        private ObservableCollection<MaterialDataView> _materialDataViews = new ObservableCollection<MaterialDataView>();
        /// <summary>
        /// 素材組合列表
        /// </summary>
        public ObservableCollection<MaterialDataView> MaterialDataViews 
        { 
            get 
            { 
                return _materialDataViews; 
            }
            set
            { 
                _materialDataViews = value;
                OnPropertyChanged(nameof(MaterialDataViews)); 
            }
        }


        /// <summary>
        /// 加工完成進度百分比
        /// </summary>
        public double FinishPercentage { get; set; }
        /// <summary>
        /// 採購 / 未排版數量
        /// </summary>
        public int PurchaseCount { get; set; }
        /// <summary>
        /// 加工未排版數量
        /// </summary>
        public int WorkCount { get; set; }
        /// <summary>
        /// 加工 / 採購未排版數量
        /// </summary>
        public int WorkPurchaseCount { get; set; }
        /// <summary>
        /// 零件總數量
        /// </summary>
        public int PartTotal { get => PurchaseCount + WorkCount + WorkPurchaseCount; set { } }
        public MatchSetting MatchSetting { get; set; } = new MatchSetting()//配料設定檔
        {
            MainLengths =
            new List<double>()
                {
                     9000,
                     10000,
                     12000
                },
            SecondaryLengths = new List<double>()
                {
                    14000
                },
            Cut = 3,
            EndCut = 10,
            StartCut = 10,
            StartNumber = "RH",
            Price = 29.3f
        };

        




        /// <summary>
        /// GridControl之搜尋字串
        /// </summary>
        public string PartsSearchString { get; set; }


        /// <summary>
        /// 素材GridControl之搜尋字串
        /// </summary>
        public string MaterialGridSearchString { get; set; }



        #endregion
        #region 私有方法
        private string GetString(string str)
        {
            string result = str.Select(el => el >= 41 && el <= 122).ToString();
            return result;
        }



        /// <summary>
        /// 確定配料
        /// </summary>
        /// <returns></returns>
        public ICommand CancelCommand
        {
            get
            {
                return new WPFBase.RelayCommand(() =>
                {
                    LengthDodageControl = false;
                });
            }
        }


        /// <summary>
        /// 確定配料
        /// </summary>
        /// <returns></returns>
        public ICommand SureCommand
        {
            get
            {
                return new WPFBase.RelayCommand(() =>
                {
                    if (LengthDodageControl)
                    {
                        AutoMatchAsync();
                        _GridControl.Dispatcher.Invoke(() =>
                        {
                            _GridControl.RefreshData();
                        });

                        if (MaterialDataViews != null)
                        {
                            ShowTypeResult = true;
                        }
                        else
                        {
                            ShowTypeResult = false;
                        }
                        if (!ShowTypeResult)
                        {
                            ShowTypeResult = true; // 開啟辦公室軟體排版結果報表
                        }
                    }
                    LengthDodageControl = false;

                });
            }
        }

        /// <summary>
        /// 自動配料
        /// </summary>
        public ICommand GoCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(objArray =>
                {
                    //檢查數值 若最長零件比素材還長 中斷命令並跳出提示框

                    List<double> LengthList = new List<double>();

                    foreach (var MLength in MainLength.Split(' '))
                    {
                        if (double.TryParse(MLength, out var result))
                            LengthList.Add(result);
                    }
                    foreach (var SLength in SecondaryLength.Split(' '))
                    {
                        if (double.TryParse(SLength, out var result))
                            LengthList.Add(result);
                    }

                    if (!DataViews.ToList().Exists(x => (x.SortCount > 0)))
                    {
                        WinUIMessageBox.Show(null,
                            $"需先於左側表格預排零件才可進行素材分配",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                             FloatingMode.Window);
                        return;
                    }

                    if (DataViews.ToList().Exists(x => x.SortCount > 0 && x.Length > LengthList.Max()))
                    {
                        var MaData = DataViews.ToList().Find(x => x.SortCount > 0 && x.Length > LengthList.Max());
                        WinUIMessageBox.Show(null,
                            $"預排零件：構件編號「{MaData.AssemblyNumber}」的長度「{MaData.Length}」超過素材最大長度「{LengthList.Max()}」！需要更長的素材才能加工該零件",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                            FloatingMode.Window);
                        return;
                    }

                    if (DataViews.ToList().Exists(x => x.SortCount > 0 && x.Length > LengthList.Max()))
                    {
                        var MaDataList = DataViews.ToList().FindAll(x => x.SortCount > 0 && x.Length > LengthList.Max());
                        var NumberString = ""; 
                        var LengthString = "";
                        foreach (var MaData in MaDataList)
                        {
                            NumberString+=$"「{ MaData.AssemblyNumber}」";
                            LengthString += $"「{MaData.Length}";
                        }

                       var Result = WinUIMessageBox.Show(null,
                            $"預排零件：構件編號{NumberString}的長度{LengthString}於切割後會超過素材最大長度「{LengthList.Max()}」！需注意是否配置錯誤，按下Yes會繼續排版，按下No則會取消排版作業",
                            "通知",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                            FloatingMode.Window);
                      if(Result == MessageBoxResult.No || Result == MessageBoxResult.Cancel)
                        {
                            return;
                        }
                    }







                    AutoMatchAsyncV2();

                   /* foreach (var Data in DataViews)
                    {
                        Data.SortCount = 0;
                    }*/

                    //確保多重系結objArray為陣列，否則傳出例外
                    if (objArray.GetType().Equals(typeof(object[])))
                    {
                        foreach (var obj in (object[])objArray)
                        {
                            //確認type為GridControl才進行重新整理
                            if (obj.GetType().Equals(typeof(DevExpress.Xpf.Grid.GridControl)))
                            {
                                var GoCommandGridControl = (DevExpress.Xpf.Grid.GridControl)obj;
                                GoCommandGridControl.Dispatcher.Invoke(() =>
                                {
                                    GoCommandGridControl.RefreshData();
                                    var GridControlItem = GoCommandGridControl.ItemsSource as IEnumerable<object>;
                                    if (GridControlItem != null)
                                    {
                                        for (int i = 0; i < GridControlItem.Count(); i++)
                                            GoCommandGridControl.RefreshRow(i);
                                    }
                                });
                            }
                            else
                            {
                                Debugger.Break();
                            }
                        }
                    }
                    else
                    {
                        //throw new Exception("傳入參數須為多重系結");
                    }
                });
            }
        }


        /// <summary>
        /// 只配一根素材 只取MainLength不取SecondaryLength
        /// </summary>
        
        public ICommand GoOneMaterialCommand
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(objArray =>
                {
                    if (!DataViews.ToList().Exists(x => (x.SortCount > 0)))
                    {
                        WinUIMessageBox.Show(null,
                            $"需先預排零件才可進行素材分配",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                            FloatingMode.Window);
                        return;
                    }

                    if (DataViews.ToList().Exists(x => (x.SortCount > 0 && x.Length > _MainLengthMachine)))
                    {
                        var MaData = DataViews.ToList().Find(x => (x.SortCount > 0 && x.Length > _MainLengthMachine));
                        WinUIMessageBox.Show(null,
                            $"預排零件：構件編號「{MaData.AssemblyNumber}」的長度「{MaData.Length}」超過素材長度「{_MainLengthMachine}」！需更要更長的素材才能加工該零件",
                            "通知",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.None,
                            MessageBoxOptions.None,
                            FloatingMode.Window);
                        return;
                    }

                    MainLength = _MainLengthMachine.ToString();
                   SecondaryLength= MainLength;
                    AutoMatchAsyncV2();

                    foreach (var Data in DataViews)
                    {
                        Data.SortCount = 0;
                    }

                    //確保多重系結objArray為陣列，否則傳出例外
                    if (objArray.GetType().Equals(typeof(object[])))
                    {
                        foreach (var obj in (object[])objArray)
                        {
                            //確認type為GridControl才進行重新整理
                            if (obj.GetType().Equals(typeof(DevExpress.Xpf.Grid.GridControl)))
                            {
                                var GoCommandGridControl = (DevExpress.Xpf.Grid.GridControl)obj;
                                GoCommandGridControl.Dispatcher.Invoke(() =>
                                {
                                    GoCommandGridControl.RefreshData();
                                    var GridControlItem = GoCommandGridControl.ItemsSource as IEnumerable<object>;
                                    if (GridControlItem != null)
                                    {
                                        for (int i = 0; i < GridControlItem.Count(); i++)
                                            GoCommandGridControl.RefreshRow(i);
                                    }
                                });
                            }
                            else
                            {
                                Debugger.Break();
                            }
                        }
                    }
                    else
                    {
                        //throw new Exception("傳入參數須為多重系結");
                    }



                });
            }
        }

        


        /// <summary>
        /// 加入素材(單個)
        /// </summary>
        public ICommand AddMaterial
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    var PartGirdControl = (DevExpress.Xpf.Grid.GridControl)obj;
                    //只在有選擇的狀態下執行命令
                    if (PartGirdControl.SelectedItems.Count > 0)
                    {
                        foreach (GD_STD.Data.TypeSettingDataView PartGridColumn in PartGirdControl.SelectedItems)
                        {
                            //要是可預排的零件
                            if (PartGridColumn.Sortable)
                            {
                                //數量
                                var IDCount = PartGridColumn.ID.Count;
                                //已配對
                                //var MatchCount = PartGridColumn.Match.FindAll(x => (x == false)).Count;
                                var alreadyMatchCount = PartGridColumn.Match.FindAll(x => (x == false)).Count;
                                if (PartGridColumn.SortCount < IDCount - alreadyMatchCount)
                                    PartGridColumn.SortCount++;

                                var PGIndex = (PartGirdControl.ItemsSource as IEnumerable<GD_STD.Data.TypeSettingDataView>).ToList().FindIndex(x => x == PartGridColumn);
                                if (PGIndex != -1)
                                {
                                    PartGirdControl.Dispatcher.Invoke(() =>
                                    {
                                        PartGirdControl.RefreshRow(PGIndex);
                                    });
                                }
                            }
                        }
                        //需要重整才能更新data binding 

                        PartGirdControl.Dispatcher.Invoke(() =>
                        {
                            PartGirdControl.RefreshData();
                        });

                    }
                }
                );
            }
        }

        /// <summary>
        /// 減少素材(單個)
        /// </summary>
        public ICommand DeductMaterial
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    var PartGirdControl = (DevExpress.Xpf.Grid.GridControl)obj;
                    //只在有選擇的狀態下執行命令
                    if (PartGirdControl.SelectedItems.Count > 0)
                    {
                        foreach (GD_STD.Data.TypeSettingDataView PartGridColumn in PartGirdControl.SelectedItems)
                        {
                            //只在>=1的時候做加減
                            if (PartGridColumn.SortCount >= 1)
                                PartGridColumn.SortCount--;

                            //保險：避免出現負數的情況
                            if (PartGridColumn.SortCount < 0)
                                PartGridColumn.SortCount = 0;

                        }

                        //需要重整才能更新data binding 
                        PartGirdControl.Dispatcher.Invoke(() =>
                        {
                            PartGirdControl.RefreshData();
                        });
                    }
                }
                );
            }
        }


        /// <summary>
        /// 加入素材(全部)
        /// </summary>
        public ICommand AddAllMaterial
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    var PartGirdControl = (DevExpress.Xpf.Grid.GridControl)obj;
                    //只在有選擇的狀態下執行命令
                    if (PartGirdControl.SelectedItems.Count > 0)
                    {
                        foreach (GD_STD.Data.TypeSettingDataView PartGridColumn in PartGirdControl.SelectedItems)
                        {
                            if (PartGridColumn.Sortable)
                            {
                                //數量
                                var IDCount = PartGridColumn.ID.Count;
                                //已配對
                                var alreadyMatchCount = PartGridColumn.Match.FindAll(x => (x == false)).Count;
                                //var alreadyMatchCount = PartGridColumn.Match.FindAll(x => (x == false)).Count;

                                PartGridColumn.SortCount = IDCount - alreadyMatchCount;
                                //已配對
                                //  PartGridColumn.Match.FindAll(x => (x == true)).Count;

                                // DataViews[];
                                //取得構件編號，將資料寫入datagrid

                                //需要重整才能更新data binding 
                                PartGirdControl.Dispatcher.Invoke(() =>
                                {
                                    PartGirdControl.RefreshData();
                                });
                            }
                        }
                    }
                }
                );
            }
        }
        /// <summary>
        /// 加入所有存在
        /// </summary>
        public ICommand AddEveryMaterial
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    var PartGirdControl = (DevExpress.Xpf.Grid.GridControl)obj;
  
                    foreach (GD_STD.Data.TypeSettingDataView PartGridColumn in DataViews)
                    {
                        if (PartGridColumn.Sortable)
                        {
                            //數量
                            var IDCount = PartGridColumn.ID.Count;
                            //已配對
                            var alreadyMatchCount = PartGridColumn.Match.FindAll(x => (x == false)).Count;

                            PartGridColumn.SortCount = IDCount - alreadyMatchCount;
                            //已配對
                            //  PartGridColumn.Match.FindAll(x => (x == true)).Count;

                            // DataViews[];
                            //取得構件編號，將資料寫入datagrid

                            //需要重整才能更新data binding 
                            PartGirdControl.Dispatcher.Invoke(() =>
                            {
                                PartGirdControl.RefreshData();
                            });
                        }
                    }

                });
            }
        }

        /// <summary>
        /// 減少素材(全部)
        /// </summary>
        public ICommand DeductAllMaterial
        {
            get
            {
                return new WPFBase.RelayParameterizedCommand(obj =>
                {
                    var PartGirdControl = (DevExpress.Xpf.Grid.GridControl)obj;
                    //只在有選擇的狀態下執行命令
                    if (PartGirdControl.SelectedItems.Count > 0)
                    {
                        foreach (GD_STD.Data.TypeSettingDataView PartGridColumn in PartGirdControl.SelectedItems)
                        {
                            PartGridColumn.SortCount = 0;
                        }
                        //需要重整才能更新data binding 
                        PartGirdControl.Dispatcher.Invoke(() =>
                        {
                            PartGirdControl.RefreshData();
                        });
                    }
                }
                );
            }
        }

        /// <summary>
        /// 20220901 checkbox 全選-全不選command
        /// </summary>
        public ICommand SetAllCheckboxCheckedCommand
        {
            get
            {
                return new WPFWindowsBase.RelayParameterizedCommand(obj =>
                {
                    GetWpfLogicalChildClass.SetAllCheckBoxTrueOrFalse(obj);
                });
            }
        }



        public bool LengthDodageControl { get; set; } = false;


        private bool? _startNumberCheckboxBoolen= true;

        #region 排版參數設定
        public bool? StartNumberCheckboxBoolen
        {
            get
            {
                return _startNumberCheckboxBoolen;
            }
            set
            {
                _startNumberCheckboxBoolen = value;
                if (value is true)
                {
                    MatchSetting.StartNumber = PreCode + _startNumber;
                }
                else
                {
                    // 若無打勾，起始號碼 = 001
                    MatchSetting.StartNumber = "001";
                }
            }
        }

private string _preCode = "";
        /// <summary>
        /// 前置字串
        /// </summary>
        public string PreCode 
        {
            get
            {
                if (StartNumberCheckboxBoolen is true)
                    return _preCode;
                else
                    return "";
            }
            set
            {
                _preCode = value;
                StartNumberCheckboxBoolen = !string.IsNullOrEmpty(_preCode) || !string.IsNullOrEmpty(_startNumber);
            }
        } 
        /// <summary>
        /// 排版編號checkbox
        /// </summary>

        private string _startNumber;
        /// <summary>
        /// 起始編號
        /// </summary>
        public string StartNumber
        {
            get
            {
                if (StartNumberCheckboxBoolen is true)
                    return _startNumber;
                else
                    return "";
            }
            set
            {
                _startNumber = value;
                StartNumberCheckboxBoolen = !string.IsNullOrEmpty(_startNumber);
            }
        }


        const string MainLengthDefault = "9000 10000 12000";
        /// <summary>
        /// 預設長度checkbox
        /// </summary>
        public bool? MainLengthCheckboxBoolen { get; set; } = true;
        private string _mainLength = MainLengthDefault;
        /// <summary>
        /// 預設長度
        /// </summary>
        public string MainLength 
        {
            get
            {
                /*if (string.IsNullOrEmpty(_mainLength ))
                {
                    _mainLength = MainLengthDefault;
                    return _mainLength;
                }*/

                if (MainLengthCheckboxBoolen is true)
                {
                    return _mainLength;
                }
                else
                {
                    return MainLengthDefault;
                }
            }
            set
            {
                _mainLength = value;
                MainLengthCheckboxBoolen = _mainLength != MainLengthDefault;
            }
        }

        const double MainLengthMachineDefault = 9000;
        private double _MainLengthMachine = 0;
        /// <summary>
        /// 預設長度 機台端用 純設定用
        /// </summary>
        public double MainLengthMachine
        {
            get
            {
                SecondaryLength = "";
                if (_MainLengthMachine == 0)
                {
                    _MainLengthMachine = MainLengthMachineDefault;
                    return MainLengthMachineDefault;
                }
                if (MainLengthCheckboxBoolen is true)
                    return _MainLengthMachine;
                else
                    return MainLengthMachineDefault;
                
            }
            set
            {
                _MainLengthMachine = value;
                MainLengthCheckboxBoolen = _MainLengthMachine != MainLengthMachineDefault;
            }
        }



        const string SecondaryLengthDefault = "11000 13000 14000 15000";
        /// <summary>
        /// 次要條件checkbox
        /// </summary>
        public bool? SecondaryLengthCheckboxBoolen { get; set; } = true;
        private string _secondaryLength = SecondaryLengthDefault;
        /// <summary>
        /// 次要條件
        /// </summary>
        public string SecondaryLength
        {
            get
            {
              /*  if (string.IsNullOrEmpty(_secondaryLength))
                {
                    _secondaryLength = SecondaryLengthDefault;
                    return _secondaryLength;
                }*/

                if (SecondaryLengthCheckboxBoolen is true)
                {
                    return _secondaryLength;
                }
                else
                {
                    return SecondaryLengthDefault;
                }
            }
            set
            {
                _secondaryLength = value;
                SecondaryLengthCheckboxBoolen = _secondaryLength !=SecondaryLengthDefault;
            }
        }


        const double StartCutDefault = 10;
        /// <summary>
        /// 前端切除Checkbox
        /// </summary>
        public bool? StartCutCheckboxBoolen { get; set; } = true;
        private double _matchSettingStartCut = StartCutDefault;
        /// <summary>
        /// 前端切除Binding
        /// </summary>
        public double MatchSettingStartCut
        {
            get
            {
                if (StartCutCheckboxBoolen is true)
                {
                    MatchSetting.StartCut = _matchSettingStartCut;
                }
                else
                {
                    MatchSetting.StartCut = StartCutDefault;
                }

                return MatchSetting.StartCut;
            }
            set
            {
                _matchSettingStartCut = value;
                StartCutCheckboxBoolen = _matchSettingStartCut != StartCutDefault;
            }
        }



        const double EndCutDefault = 10;
        /// <summary>
        /// 後端切除Checkbox
        /// </summary>
        public bool? EndCutCheckboxBoolen { get; set; } = true;
        private double _matchSettingEndCut = EndCutDefault;
        /// <summary>
        /// 後端切除Binding
        /// </summary>
        public double MatchSettingEndCut
        {
            get
            {
                if (EndCutCheckboxBoolen is true)
                {
                    MatchSetting.EndCut = _matchSettingEndCut;
                }
                else
                {
                    MatchSetting.EndCut = EndCutDefault;
                }

                return MatchSetting.EndCut;
            }
            set
            {
                _matchSettingEndCut = value;
                EndCutCheckboxBoolen = _matchSettingEndCut != EndCutDefault;
            }
        }


        public bool? PhaseCheckboxBoolen { get; set; } = true;
        public bool? DismanteCheckboxBoolen { get; set; } = true;





        #endregion













        private void AutoMatchAsync()
        {
            // 2020.06.22  呂宗霖 新增IsNullOrEmpty條件
            MatchSetting.MainLengths = MainLength.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(el => Convert.ToDouble(el)).ToList();
            MatchSetting.SecondaryLengths = SecondaryLength.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(el => Convert.ToDouble(el)).ToList();
            //嵐:這邊我註解掉
            //Window.ShowDialog();
            //開啟長度配料視窗
            //OfficeViewModel.LengthDodageControl = true;
            unsafe
            {
                List<string> allowTypeStr = ObSettingVM.allowType.Select(x=>x.ToString()).ToList();
                string allowTypString = String.Join(",", allowTypeStr);
                STDSerialization ser = new STDSerialization();//序列化處理器
                List<string> profiles = ser.GetProfile().Where(el => 
                el.Contains(ObSettingVM.allowType.Select(x=>x.ToString())
                .ToArray()
                )).ToList();//模型有使用到的斷面規格
                string strNumber = "RH";
                var strings = MaterialDataViews.Where(el => el.MaterialNumber.Contains(strNumber));

                int startNumber = 1;
                if (strings.Count() != 0)
                {
                    List<int> value = new List<int>();
                    MatchCollection matches = Regex.Matches(strings.ElementAt(strings.Count()-1).MaterialNumber, @"[0-9]+");
                    string result = string.Empty;

                    for (int i = 0; i < matches.Count; i++)
                    {
                        startNumber = Convert.ToInt32(matches[i].Value) +1;
                    }
                }

                //startNumber = strings.Max(el => Convert.ToInt32(el));

                for (int i = 0; i < profiles.Count; i++)
                {
                    string partData = profiles[i].GetHashCode().ToString(); //資料名稱

                    ObservableCollection<SteelPart> steels = ser.GetPart(partData); //零件序列化檔案
                    if (steels == null)
                    {
                        continue;
                    }
                    List<SinglePart> listPart = new List<SinglePart>();//配料列表

                    foreach (var item in steels)//將物件全部變成單一個體
                    {
                        //where->將符合搜尋條件的DataViews取出
                        //Select->將上述的資料的每一個SortCount取出後建立另一個IEnumerable陣列
                        IEnumerable<int> _where = DataViews.Where(el => el.Profile == item.Profile && el.PartNumber == item.Number).Select(el => el.SortCount);

                        var count = _where.Aggregate((part1, part2) => part1 + part2);
                        if (count > 0)
                        {
                            listPart.AddRange(SinglePart.UnfoldPart(item, out List<bool> match, count));//展開物件並加入配料列表內
                            item.Match = match;
                            //DataViews.
                        }
                    }
                    ser.SetPart(profiles[i].GetHashCode().ToString(), new ObservableCollection<object>(steels));
                    listPart.Sort(Compare);//由大排到小
                    SinglePart[] weightsPatr = listPart.ToArray(); //權重比較的零件列表
                    for (int c = 0; c < weightsPatr.Length; c++)//寫入 index
                    {
                        weightsPatr[c].ChangeIndex(c);
                    }

                    SinglePart[] usePart = new SinglePart[weightsPatr.Length];//使用率的零件列表
                    SinglePart[] averagePart = new SinglePart[weightsPatr.Length];//數量評分的零件列表
                    SinglePart[] multipleusePart = new SinglePart[weightsPatr.Length];//使用率的分裂零件列表

                    Array.Copy(weightsPatr, usePart, weightsPatr.Length);//複製到使用率
                    Array.Copy(weightsPatr, averagePart, weightsPatr.Length);//複製到平均評分
                    Array.Copy(weightsPatr, multipleusePart, weightsPatr.Length);//複製到使用率

                    fixed (SinglePart* pUse = usePart, pWeights = weightsPatr, pAverage = averagePart, pmUse = multipleusePart)//指標
                    {
                        Population uPopulation = new Population(MatchSetting, pUse, usePart.Length, new UsageRate());//使用率配料
                        uPopulation.Run();
                        Population wPopulation = new Population(MatchSetting, pWeights, weightsPatr.Length, new Weights());//權重配料
                        wPopulation.Run();
                        Population aPopulation = new Population(MatchSetting, pAverage, averagePart.Length, new Average());//平均配置
                        aPopulation.Run();
                        Population umPopulation = new Population(MatchSetting, pmUse, multipleusePart.Length, new MultipleUsageRate());//分裂使用率配料
                        umPopulation.Run();
                        List<Population> _ = new List<Population>() { uPopulation, wPopulation, aPopulation, umPopulation }; //存入到列表內

                        //尋找最優化
                        double minValue = _[0].TotalSurplus(); //最少的餘料
                        int minIndex = 0; //最小值索引
                        for (int c = 1; c < _.Count; c++) //逐步展開配料結果的餘料
                        {
                            double value = _[c].TotalSurplus(); //餘料
                            if (minValue > value) //目前最小值大於餘料
                            {
                                minValue = value; //修改最小值
                                minIndex = c;//紀錄最小於列的索引位置
                            }
                        }
                        for (int q = 0; q < _[minIndex].Count; q++)
                        {
                            MaterialDataViews.Add(new MaterialDataView
                            {
                                MaterialNumber =  strNumber + startNumber.ToString().PadLeft(4, '0'), // 不足補0
                                Profile = profiles[i],
                            });
                            MaterialDataViews[MaterialDataViews.Count - 1].LengthList.AddRange(MatchSetting.MainLengths); //加入長度列表
                            MaterialDataViews[MaterialDataViews.Count - 1].LengthList.AddRange(MatchSetting.SecondaryLengths); //加入長度列表
                            MaterialDataViews[MaterialDataViews.Count - 1].LengthIndex = MaterialDataViews[MaterialDataViews.Count - 1].LengthList.FindIndex(el => _[minIndex][q].Length - MatchSetting.EndCut - MatchSetting.StartCut == el); //長度索引
                            int index = MaterialDataViews[MaterialDataViews.Count - 1].LengthIndex;
                            MaterialDataViews[MaterialDataViews.Count - 1].LengthStr = MaterialDataViews[MaterialDataViews.Count - 1].LengthList[index];
                            MaterialDataViews[MaterialDataViews.Count - 1].StartCut = MatchSetting.StartCut;
                            MaterialDataViews[MaterialDataViews.Count - 1].EndCut = MatchSetting.EndCut;
                            MaterialDataViews[MaterialDataViews.Count - 1].Cut = MatchSetting.Cut;
                            for (int c = 0; c < _[minIndex][q].PartNumber.Length; c++)
                            {
                                int dataViewIndex = DataViews.FindIndex(el => el.PartNumber == _[minIndex][q].PartNumber[c] && el.Match.FindIndex(el2 => el2 == true) != -1);
                                MaterialDataViews[MaterialDataViews.Count - 1].Parts.Add(DataViews[dataViewIndex]);
                                MaterialDataViews[MaterialDataViews.Count - 1].Material = DataViews[dataViewIndex].Material;
                                int matchIndex = DataViews[dataViewIndex].Match.IndexOf(el => el == true);
                                //bool aa = DataViews[dataViewIndex].Match[matchIndex];
                                DataViews[dataViewIndex].Match[matchIndex] = false;
                            }
                            //DataViews.Where(el => el.).ForEach(el => el.SortCount  = el.GetSortCount());
                            startNumber++;
                        }
                    }
                }
                ser.SetMaterialDataView(MaterialDataViews);
            }
        }

        private void AutoMatchAsyncV2()
        {
            // 2020.06.22  呂宗霖 新增IsNullOrEmpty條件
            MatchSetting.MainLengths = MainLength.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(el => Convert.ToDouble(el) - (MatchSettingStartCut+ MatchSettingEndCut)).ToList(); //1110 加入動作:先扣除MatchSettingStartCut+ MatchSettingEndCut CYH
            MatchSetting.SecondaryLengths = SecondaryLength.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(el => Convert.ToDouble(el) - (MatchSettingStartCut + MatchSettingEndCut)).ToList(); //1110 加入動作:先扣除MatchSettingStartCut+ MatchSettingEndCut CYH
            MatchSetting.PreCode = PreCode;
            //MatchSetting.StartNumber = StartNumber;

            unsafe
            {
                // 需排版之斷面規格
                var sortSteelType = DataViews.Where(x => x.SortCount > 0).Select(x => new { x.SteelType, x.Profile }).Distinct().ToList();
                
                List<string> sortProfile = DataViews.Where(x => x.SortCount > 0).Select(x => x.Profile).Distinct().ToList();
                // 需排版之零件
                List<string> sortPartNumber = DataViews.Where(x => x.SortCount > 0).Select(x => x.PartNumber).Distinct().ToList();
                if (sortPartNumber.Count <= 0 || sortProfile.Count <= 0)
                {
                    WinUIMessageBox.Show(null,
                    $"請選擇欲排版數量",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                    return;
                }

                STDSerialization ser = new STDSerialization();//序列化處理器
                                                              // obvm.allowType.Contains((OBJECT_TYPE)System.Enum.Parse(typeof(OBJECT_TYPE), el.ToString()))
                // 取得有開放且排版數>0之排版規格
                var a = ser.GetProfile();
                List<string> profiles = 
                    sortSteelType
                    .Where(el => !string.IsNullOrEmpty(el.Profile) &&
                           ObSettingVM.allowType.Contains((OBJECT_TYPE)el.SteelType) &&
                        sortProfile.Contains(el.Profile)
                    ).Select(el=>el.Profile)
                    .ToList();//模型有使用到的斷面規格

                //改寫判斷式 由SteelType找斷面規格
                //// 取得有開放且排版數>0之排版規格
                //var a = ser.GetProfile();
                //List<string> profiles = ser.GetProfile()
                //    .Where(el =>!string.IsNullOrEmpty(el) && el.Contains(
                //        ObSettingVM.allowType.Select(x => x.ToString()).ToArray()) &&
                //        sortProfile.Contains(el) 
                //    ).ToList();//模型有使用到的斷面規格
                // 在素材中，屬於前置碼PreCode的有幾筆
                string strNumber = MatchSetting.PreCode;
                var strings = MaterialDataViews.Where(el => el.MaterialNumber.Contains(strNumber));
                int startNumber = 1;
                if (strings.Count() != 0)
                {
                    List<int> value = new List<int>();
                    // 在素材中，屬於前置碼PreCode的資料
                    MatchCollection matches = Regex.Matches(strings.ElementAt(strings.Count() - 1).MaterialNumber, @"[0-9]+");
                    string result = string.Empty;

                    for (int i = 0; i < matches.Count; i++)
                    {
                        startNumber = Convert.ToInt32(matches[i].Value) + 1;
                    }
                }

                //startNumber = strings.Max(el => Convert.ToInt32(el));

                for (int i = 0; i < profiles.Count; i++)
                {
                    string partData = profiles[i].GetHashCode().ToString(); //資料名稱

                    ObservableCollection<SteelPart> steels = ser.GetPart(partData); //零件序列化檔案
                    if (steels == null)
                    {
                        continue;
                    }
                    List<SinglePart> listPart = new List<SinglePart>();//配料列表
                    var softedDataviews= DataViews.Where(x => x.SortCount > 0).ToList();
                    foreach (var item in softedDataviews)
                    {
                        //同一零件排複數個 要跳兩次
                        //1110 排版計算前添加鋸床切割損耗，DataViews中Part的Length此時已被變動 CYH

                        item.Length += MatchSetting.Cut;
                        foreach (var steel in steels.Where(x => x.Number == item.PartNumber))
                        {
                            //1110 排版計算前添加鋸床切割損耗 CYH
                            steel.Length += MatchSetting.Cut;

                           var _Findwhere = DataViews
                           .Where(el =>
                           el.Profile == steel.Profile &&
                           el.PartNumber == steel.Number &&
                           el.Length == steel.Length &&         // 2022/09/23 呂宗霖 新增
                           el.SortCount > 0);                 // 2022/09/23 呂宗霖 新增
                        
                           var _where = _Findwhere.Select(el => el.SortCount);

                            if (_where.Count() > 0)
                            {
                                var count = _where.Aggregate((total, next) => total + next);
                                if (count > 0)
                                {
                                    listPart.AddRange(SinglePart.UnfoldPart(steel, out List<bool> match, count));//展開物件並加入配料列表內
                                    steel.Match = match;
                                }
                            }
                            else
                            {

                            }

                            //1111 排版計算前添加鋸床切割損耗, 復原長度 CYH
                            steel.Length -= MatchSetting.Cut;
                        }
                        //1111 排版計算前添加鋸床切割損耗, 復原長度 CYH
                        item.Length -= MatchSetting.Cut;
                    }

                    ser.SetPart(profiles[i].GetHashCode().ToString(), new ObservableCollection<object>(steels));
                    listPart.Sort(Compare);//由大排到小
                    SinglePart[] weightsPatr = listPart.ToArray(); //權重比較的零件列表
                    for (int c = 0; c < weightsPatr.Length; c++)//寫入 index
                    {
                        weightsPatr[c].ChangeIndex(c);
                    }

                    SinglePart[] usePart = new SinglePart[weightsPatr.Length];//使用率的零件列表
                    SinglePart[] averagePart = new SinglePart[weightsPatr.Length];//數量評分的零件列表
                    SinglePart[] multipleusePart = new SinglePart[weightsPatr.Length];//使用率的分裂零件列表

                    Array.Copy(weightsPatr, usePart, weightsPatr.Length);//複製到使用率
                    Array.Copy(weightsPatr, averagePart, weightsPatr.Length);//複製到平均評分
                    Array.Copy(weightsPatr, multipleusePart, weightsPatr.Length);//複製到使用率

                    fixed (SinglePart* pUse = usePart, pWeights = weightsPatr, pAverage = averagePart, pmUse = multipleusePart)//指標
                    {
                        Population uPopulation = new Population(MatchSetting, pUse, usePart.Length, new UsageRate());//使用率配料
                        uPopulation.Run();
                        Population wPopulation = new Population(MatchSetting, pWeights, weightsPatr.Length, new Weights());//權重配料
                        wPopulation.Run();
                        Population aPopulation = new Population(MatchSetting, pAverage, averagePart.Length, new Average());//平均配置
                        aPopulation.Run();
                        Population umPopulation = new Population(MatchSetting, pmUse, multipleusePart.Length, new MultipleUsageRate());//分裂使用率配料
                        umPopulation.Run();
                        // 將四種方法算出來的結果存入List中進行比較
                        List<Population> _ = new List<Population>() { uPopulation, wPopulation, aPopulation, umPopulation }; //存入到列表內

                        //尋找最優化


                        //double minValue = _[0].TotalSurplus(); //最少的餘料
                        /*int minIndex = 0; //最小值索引
                        for (int c = 1; c < _.Count; c++) //逐步展開配料結果的餘料
                        {
                            double value = _[c].TotalSurplus(); //餘料
                            if (minValue > value) //目前最小值大於餘料
                            {
                                minValue = value; //修改最小值
                                minIndex = c;//紀錄最小於列的索引位置
                            }
                        }*/

                        var TotalSurplusMinValue= _.Min(x => x.TotalSurplus());
                        var MinPop = _.Find(x => (x.TotalSurplus() == TotalSurplusMinValue));
                        
                        for (int q = 0; q < MinPop.Count; q++)
                        {
                            var NewMaterial = new MaterialDataView
                            {
                                MaterialNumber = strNumber + startNumber.ToString().PadLeft(4, '0'), // 不足補0
                                Profile = profiles[i],

                                StartCut = MatchSetting.StartCut,
                                EndCut = MatchSetting.EndCut,
                                Cut = MatchSetting.Cut,
                            };
                            NewMaterial.LengthList.AddRange(MatchSetting.MainLengths); //加入長度列表
                            NewMaterial.LengthList.AddRange(MatchSetting.SecondaryLengths); //加入長度列表
                            NewMaterial.LengthIndex = NewMaterial.LengthList.FindIndex(el => MinPop[q].Length - MatchSetting.EndCut - MatchSetting.StartCut == el); //長度索引
                            int index = NewMaterial.LengthIndex;
                            NewMaterial.LengthStr = NewMaterial.LengthList[index] + (MatchSetting.EndCut + MatchSetting.StartCut); //1110 加入動作:將素材長度加回MatchSettingStartCut+ MatchSettingEndCut CYH

                            for (int c = 0; c < MinPop[q].PartNumber.Length; c++)
                            {
                                //1111 CYH dataViewIndex是從相同零件編號的零件群中找起，並不管零件與構件的對應關係，因此在排版設定中選擇的零件若在其同零件編號的後面，則配對後在排版設定中會顯示已配對的零件
                                //會從同零件編號群中的第一個開始標記，而不是一開始所選擇的零件

                                //20221222追加el.SortCount > 0  避免配對時配對到相同零件編號但構件編號不同的零件
                                int dataViewIndex = DataViews.FindIndex(el => el.SortCount > 0 && el.PartNumber == MinPop[q].PartNumber[c] && el.Match.Exists(el2 => el2 == true));
                                NewMaterial.Parts.Add(DataViews[dataViewIndex]);
                                NewMaterial.Material = DataViews[dataViewIndex].Material;

                                int matchIndex = DataViews[dataViewIndex].Match.IndexOf(el => el == true);
                                DataViews[dataViewIndex].Match[matchIndex] = false;
                                //20221222追加SortCount--
                                DataViews[dataViewIndex].SortCount--;
                            }

                            MaterialDataViews.Add(NewMaterial);
                            startNumber++;
                        }
                    }
                }
                ser.SetMaterialDataView(MaterialDataViews);
            }
        }
        #endregion
        #region 私有靜態
        /// <summary>
        /// 由大到小排序
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Compare(SinglePart a, SinglePart b)
        {
            if (a.Length < b.Length) //比較小的往後排
                return 1;
            else if (a.Length > b.Length) //比較大的往前排
                return -1;
            else
                return 0;
        }
        #endregion







    }






}

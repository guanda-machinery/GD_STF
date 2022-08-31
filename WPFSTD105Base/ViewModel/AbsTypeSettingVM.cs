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

namespace WPFSTD105
{
    /// <summary>
    /// 抽象排版設定
    /// </summary>
    public abstract class AbsTypeSettingVM : WPFBase.BaseViewModel
    {
        /// <summary>
        /// 控制排版結果是否顯示
        /// </summary>
        public bool ShowTypeResult { get; set; }
        /// <summary>
        /// 抽象排版設定
        /// </summary>
        public AbsTypeSettingVM()
        {
            STDSerialization ser = new STDSerialization();
            ObservableCollection<BomProperty> bomProperties = CommonViewModel.ProjectProperty.BomProperties; //報表屬性設定檔
            ObservableCollection<SteelAssembly> assemblies = ser.GetGZipAssemblies();//模型構件列表
            DeleteCommand = Delete();
            MaterialDataViews = ser.GetMaterialDataView();
            SureCommand = Sure();
            CancelCommand = Cancel();
            foreach (var profile in ser.GetProfile()) //逐步展開斷面規格
            {

                ObservableCollection<SteelPart> buffer = ser.GetPart(profile.GetHashCode().ToString()); //零件列表

                //只將 BH RH L TUBE BOX CH H LB([)加入到列表內
                if (buffer != null &&(
                    buffer[0].Type == OBJETC_TYPE.BH ||
                    buffer[0].Type == OBJETC_TYPE.RH ||
                    buffer[0].Type == OBJETC_TYPE.L ||
                    buffer[0].Type == OBJETC_TYPE.TUBE ||
                    buffer[0].Type == OBJETC_TYPE.BOX ||
                    buffer[0].Type == OBJETC_TYPE.CH ||
                    buffer[0].Type == OBJETC_TYPE.H ||
                    buffer[0].Type == OBJETC_TYPE.LB))
                {
                    foreach (var item in buffer) //逐步展開零件
                    {
                        if (item.Father != null)
                        {
                            for (int i = 0; i < item.Father.Count; i++)  //逐步展開零件  id or match
                            {
                                int index = assemblies.FindIndex(el => el.ID.Contains(item.Father[i])); //找出構件列表內是零件的 Father 位置
                                if (index == -1) //找不到物件
                                {
                                    throw new Exception("index 不可以是 -1");
                                }
                                int idIndex = assemblies[index].ID.IndexOf(item.Father[i]); //找出構件 id 所在的陣列位置
                                TypeSettingDataView view = new TypeSettingDataView(item, assemblies[index], idIndex, i);
                                view.SortCount =0;
                                int dataIndex = DataViews.IndexOf(view); //搜尋指定的物件
                                if (dataIndex == -1) //如果找不到物件
                                {
                                    DataViews.Add(view);
                                }
                                else
                                {
                                    DataViews[dataIndex].Add(item, i);
                                }
                            }
                        }
                        else
                        {
                            DataViews.Add(new TypeSettingDataView()
                            {
                                Profile = item.Profile,
                                Length = item.Length,
                                AssemblyNumber = item.Number,
                                PartNumber = item.Number,
                                Material = item.Material,
                                SortCount = 0,
                                Match = item.Match,
                            });
                        }
                    }
                }
            }
            AllSelectedGridCommand = AllSelectedGrid();// 選擇報表全部物件命令
            ReverseSelectedGridCommand = ReverseSelectedGrid();//反向選取命令
            UnselectSelectedGridCommand = UnselectSelectedGrid();//取消選取命令
            SaveMatchCommand = SaveMatch();
            AutoCommand = new WPFBase.RelayParameterizedCommand(obj =>
           {
               _GridControl = (GridControl)obj;
               LengthDodageControl = true;
               //AutoMatchAsync(obj);
               //GridControl grid = (GridControl)obj;
               //
           });
        }
        private GridControl _GridControl { get; set; }
        #region 命令
        /// <summary>
        /// 手動排版命令
        /// </summary>
        public ICommand ManualCommand { get; set; }
        /// <summary>
        /// 自動排版命令
        /// </summary>
        public WPFBase.RelayParameterizedCommand AutoCommand { get; set; }
        //private  WPFBase.RelayParameterizedCommand Auto()
        //{
        //    return 
        //}
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
        public ICommand DeleteCommand { get; set; }

        public WPFBase.RelayParameterizedCommand Delete()
        {
            return new WPFBase.RelayParameterizedCommand((el) =>
            {
                ObservableCollection<MaterialDataView> materialDataViews = new ObservableCollection<MaterialDataView>(MaterialDataViews);
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
                    materialDataViews.Remove(view);
                    //DataViews.ForEach(e => e.SortCount = e.GetSortCount());
                    DataViews = new ObservableCollection<TypeSettingDataView>(DataViews.ToList());
                    MaterialDataViews = new ObservableCollection<MaterialDataView>(materialDataViews);
                }
                ser.SetMaterialDataView(MaterialDataViews);
            });
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
        public ObservableCollection<MaterialDataView> SelectedMaterial { get; set; } = new ObservableCollection<MaterialDataView>();
        /// <summary>
        /// 報表視圖
        /// </summary>
        public ObservableCollection<TypeSettingDataView> DataViews { get; set; } = new ObservableCollection<TypeSettingDataView>();
        /// <summary>
        /// 素材組合列表
        /// </summary>
        public ObservableCollection<MaterialDataView> MaterialDataViews { get; set; } = new ObservableCollection<MaterialDataView>();
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
            MainLengths = new List<double>()
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
        #endregion
        #region 私有方法
        private string GetString(string str)
        {
            string result = str.Select(el => el >= 41 && el<= 122).ToString();
            return result;
        }
        /// <summary>
        /// 確定配料
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 確定配料
        /// </summary>
        /// <returns></returns>
        public ICommand Cancel()
        {
            return new WPFBase.RelayCommand(() =>
            {
                LengthDodageControl = false;
            });
        }
        /// <summary>
        /// 確定配料
        /// </summary>
        public ICommand SureCommand { get; set; }

        /// <summary>
        /// 確定配料
        /// </summary>
        /// <returns></returns>
        public ICommand Sure()
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
                LengthDodageControl =false;

            });
        }
        //public bool _Sure { get; set; }
        //public bool _Sure { get; set; }

        public bool LengthDodageControl { get; set; } = false;
        public string MainLength { get; set; } = "9000 10000 12000";
        public string SecondaryLength { get; set; } = "11000 13000 14000 15000";
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
                STDSerialization ser = new STDSerialization();//序列化處理器
                List<string> profiles = ser.GetProfile().Where(el => el.Contains("RH", "BH")).ToList();//模型有使用到的斷面規格
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
                        var _where = DataViews.Where(el => el.Profile == item.Profile && el.PartNumber == item.Number).Select(el => el.SortCount);
                        var agger = _where.Aggregate((part1, part2) => part1 + part2);
                        int count = agger;
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
                        double minValue = _[0].TotalSurplus(); //最少的於料
                        int minIndex = 0; //最小值索引
                        for (int c = 1; c < _.Count; c++) //逐步展開配料結果的於料
                        {
                            double value = _[c].TotalSurplus(); //於料
                            if (minValue > value) //目前最小值大於於料
                            {
                                minValue = value; //修改最小值
                                minIndex = c;//紀錄最小於列的索引位置
                            }
                        }
                        for (int q = 0; q < _[minIndex].Count; q++)
                        {
                            MaterialDataViews.Add(new MaterialDataView
                            {
                                MaterialNumber = strNumber + startNumber.ToString().PadLeft(4, '0'), // 不足補0
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

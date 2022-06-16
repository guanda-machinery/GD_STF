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
            MaterialDataViews = ser.GetMaterialDataView();
            foreach (var profile in ser.GetProfile()) //逐步展開斷面規格
            {
                ObservableCollection<SteelPart> buffer = ser.GetPart(profile.GetHashCode().ToString()); //零件列表
                //只將 BH RH L BOX CH 加入到列表內
                if (buffer[0].Type == OBJETC_TYPE.BH ||
                     buffer[0].Type == OBJETC_TYPE.RH ||
                     buffer[0].Type == OBJETC_TYPE.L ||
                     buffer[0].Type == OBJETC_TYPE.BOX ||
                     buffer[0].Type == OBJETC_TYPE.CH)
                {
                    foreach (var item in buffer) //逐步展開零件
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
                }
            }
            AllSelectedGridCommand = AllSelectedGrid();// 選擇報表全部物件命令
            ReverseSelectedGridCommand = ReverseSelectedGrid();//反向選取命令
            UnselectSelectedGridCommand = UnselectSelectedGrid();//取消選取命令
            AutoCommand = new WPFBase.RelayParameterizedCommand( (obj) =>
            {
                Task.Run(() => AutoMatchAsync(obj));
            });
        }
        #region 命令
        /// <summary>
        /// 手動排版命令
        /// </summary>
        public ICommand ManualCommand { get; set; }
        /// <summary>
        /// 自動排版命令
        /// </summary>
        public ICommand AutoCommand { get; set; }
        //private  WPFBase.RelayParameterizedCommand Auto()
        //{
        //    return 
        //}
        /// <summary>
        /// 顯示零件圖命令
        /// </summary>
        public WPFBase.RelayParameterizedCommand PartDrawingCommand { get; set; }
        /// <summary>
        /// 顯示零件圖
        /// </summary>
        /// <returns></returns>
        public WPFBase.RelayParameterizedCommand PartDrawing()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {

                //嵐: GraphWin 框架分離一下
                //string partNumber = el.ToString(); //零件名稱
                //STDSerialization ser = new STDSerialization(); //序列化處理器
                //ObservableCollection<DataCorrespond> dataCorrespond = ser.GetDataCorrespond(); //序列化列表
                //int indexDataCorrespond = dataCorrespond.FindIndex(data => data.Number == partNumber);//序列化的列表索引
                //if (indexDataCorrespond != -1) //有找到序列化物件
                //{
                //    DataCorrespond data = dataCorrespond[indexDataCorrespond]; //找到的物件
                //    NcTempList ncTemps = ser.GetNcTempList(); //NC 載入檔案
                //    NcTemp ncTemp = ncTemps.GetData(data.DataName);
                //    ser.SetNcTempList(ncTemps);
                //    GraphWin graphWin = null;
                //    if (ncTemp != null)
                //    {
                //        graphWin = new GraphWin(ncTemp);

                //    }
                //    else
                //    {
                //        graphWin = new GraphWin(data.DataName);
                //    }
                //    graphWin.Show();
                //    graphWin.Draw();
                //}
            });
        }
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
        #endregion

        #region 命令處理方法
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
        ///// <summary>
        ///// 顯示全部列表處理方法 (清除過濾器)
        ///// </summary>
        ///// <returns></returns>
        //protected WPFBase.RelayCommand DisplayAllList()
        //{
        //    return new WPFBase.RelayCommand(() =>
        //    {
        //        SteelParts = new ObservableCollection<SteelPart>(_SteelParts);
        //    });
        //}
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
        #endregion
        #region 私有方法
        private async Task AutoMatchAsync(object obj)
        {
            unsafe
            {
                MatchSetting matchSetting = new MatchSetting()//配料設定檔
                {
                    MainLengths = new List<double>()
                {
                    9000 ,
                    10000,
                    12000,
                },
                    SecondaryLengths = new List<double>()
                {
                    9500,
                    11000,
                    11500,
                    12500,
                    13000,
                    13500,
                    14000,
                    14500,
                    15000,
                },
                    Cut = 3,
                    EndCut = 13,
                    StartCut = 13,
                    StartNumber = "RH",
                    Price = 29.3f
                };

                STDSerialization ser = new STDSerialization();//序列化處理器
                List<string> profiles = ser.GetProfile().Where(el => el.Contains("RH")).ToList();//模型有使用到的斷面規格
                string strNumber = "RH";
                int startNumber = 1;
                for (int i = 0; i < profiles.Count; i++)
                {
                    string partData = profiles[i].GetHashCode().ToString(); //資料名稱
                    ObservableCollection<SteelPart> steels = new ObservableCollection<SteelPart>(ser.GetPart(partData).Select(el => el)); //零件序列化檔案
                    List<SinglePart> listPart = new List<SinglePart>();//配料列表
                    foreach (var item in steels)//將物件全部變成單一個體
                    {
                        listPart.AddRange(SinglePart.UnfoldPart(item, out List<bool> match));//展開物件並加入配料列表內
                        item.Match = match;
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
                        Population uPopulation = new Population(matchSetting, pUse, usePart.Length, new UsageRate());//使用率配料
                        uPopulation.Run();
                        Population wPopulation = new Population(matchSetting, pWeights, weightsPatr.Length, new Weights());//權重配料
                        wPopulation.Run();
                        Population aPopulation = new Population(matchSetting, pAverage, averagePart.Length, new Average());//平均配置
                        aPopulation.Run();
                        Population umPopulation = new Population(matchSetting, pmUse, multipleusePart.Length, new MultipleUsageRate());//分裂使用率配料
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
                            MaterialDataViews[MaterialDataViews.Count - 1].LengthList.AddRange(matchSetting.MainLengths); //加入長度列表
                            MaterialDataViews[MaterialDataViews.Count - 1].LengthList.AddRange(matchSetting.SecondaryLengths); //加入長度列表
                            MaterialDataViews[MaterialDataViews.Count - 1].LengthIndex = MaterialDataViews[MaterialDataViews.Count - 1].LengthList.FindIndex(el => _[minIndex][q].Length - matchSetting.EndCut - matchSetting.StartCut == el); //長度索引

                            for (int c = 0; c < _[minIndex][q].PartNumber.Length; c++)
                            {
                                int dataViewIndex = DataViews.FindIndex(el => el.PartNumber == _[minIndex][q].PartNumber[c] && el.Match.FindIndex(el2 => el2 == true) != -1);
                                MaterialDataViews[MaterialDataViews.Count - 1].Parts.Add(DataViews[dataViewIndex]);
                                MaterialDataViews[MaterialDataViews.Count - 1].Material = DataViews[dataViewIndex].Material;
                                int matchIndex = DataViews[dataViewIndex].Match.IndexOf(el => el == true);
                                bool aa = DataViews[dataViewIndex].Match[matchIndex];
                                DataViews[dataViewIndex].Match[matchIndex] = false;
                            }
                            startNumber++;
                        }
                    }
                }
                ser.SetMaterialDataView(MaterialDataViews);
                GridControl grid = (GridControl)obj;
                grid.RefreshData();
                ShowTypeResult = true; // 開啟辦公室軟體排版結果報表
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
        private static int Compare(SinglePart a, SinglePart b)
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

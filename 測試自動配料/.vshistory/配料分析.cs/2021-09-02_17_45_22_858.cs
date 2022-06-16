using GD_STD;
using GD_STD.Data;
using GD_STD.Phone;
using Ninject;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
using static WPFSTD105.ViewLocator;
using System.Collections;
namespace 測試自動配料
{
    [TestFixture]
    public class 配料分析
    {
        private static IEnumerable<string> GetBom()
        {
            //yield return "昇鋼金屬有限公司";
            //yield return "卜蜂";
            //yield return "南亞科技";
            //yield return "英發";
            //yield return "高雄港";
            //yield return "觀音運動中心";
            //yield return "南亞RH200X100";
            //yield return "卜蜂RH400X200";
            yield return "高雄港RH194X150";
        }
        /// <summary>
        /// 每一個測試單元都會進入這方法
        /// </summary>
        [SetUp]
        public void Setup()
        {

        }
        /// <summary>
        /// 單完測試結束
        /// </summary>
        [TearDown]
        public void Cleanup()
        {

        }
        [Test, TestCaseSource("GetBom")]
        public unsafe void 完整自動配料(string projectName)
        {
            MatchSetting matchSetting = new MatchSetting()//配料設定檔
            {
                MainLengths = new List<double>()
                {
                    9000,
                    10000,
                    12000,
                },
                SecondaryLengths = new List<double>()
                {
                    9500,
                    11000,
                    12000,
                    13000,
                    14000,
                    15000,
                },
                Cut = 3,
                EndCut = 10,
                StartCut = 10,
                StartNumber = "RH"
            };
            ApplicationViewModel.ProjectName = projectName;
            STDSerialization serialization = new STDSerialization();//序列化處理器
            List<string> profiles = serialization.GetProfile().Where(el => el.Contains("RH")).ToList();//模型有使用到的斷面規格
            for (int i = 0; i < profiles.Count; i++)
            {
                string partData = profiles[i].GetHashCode().ToString(); //資料名稱
                ObservableCollection<SteelPart> steels = new ObservableCollection<SteelPart>(serialization.GetPart(partData).Select(el => (SteelPart)el)); //零件序列化檔案
                List<SinglePart> listPart = new List<SinglePart>();//配料列表
                foreach (var item in steels)//將物件全部變成單一個體
                {
                    listPart.AddRange(SinglePart.UnfoldPart(item));//展開物件並加入配料列表內
                }
                listPart.Sort(Compare);//由大牌到小
                SinglePart[] weightsPatr = listPart.ToArray(); //權重比較的零件列表
                for (int c = 0; c < weightsPatr.Length; c++)//寫入 index
                {
                    weightsPatr[c].ChangeIndex(c);
                }
                SinglePart[] usePart = new SinglePart[weightsPatr.Length];//使用率的零件列表
                Array.Copy(weightsPatr, usePart, weightsPatr.Length);//複製到使用率
                fixed (SinglePart* pWeights = weightsPatr)//權重列表指標
                fixed (SinglePart* pUse = usePart)//使用率列表指標
                {
                    GD_STD.Data.Population uPopulation = new GD_STD.Data.Population(matchSetting, pUse, usePart.Length, new UsageRate());//使用率配料
                    uPopulation.Run();
                    GD_STD.Data.Population wPopulation = new GD_STD.Data.Population(matchSetting, pWeights, weightsPatr.Length, new Weights());//權重配料
                    wPopulation.Run();
                    Debug.WriteLine($"{profiles[i]}");
                    Debug.WriteLine($"使用率分析訊息 : ");
                    Debug.WriteLine($"{uPopulation.ToString()} 配料物件 : {usePart.Length}");
                    foreach (var item in uPopulation.GroupLength())
                    {
                        Debug.WriteLine($"長度 : {item.Key} 數量 : {item.Count()}");
                    }
                    Debug.WriteLine($"---------------------------------------------------------------");
                    Debug.WriteLine($"使用權重分析訊息 : ");
                    Debug.WriteLine($"{wPopulation.ToString()} 配料物件 : {weightsPatr.Length}");
                    foreach (var item in wPopulation.GroupLength())
                    {
                        Debug.WriteLine($"長度 : {item.Key} 數量 : {item.Count()}");
                    }
                    Debug.WriteLine($"---------------------------------------------------------------");
                    using (StreamWriter stream = new StreamWriter($@"{ApplicationVM.DirectoryModel()}\權重{profiles[i].Replace('*', 'X')}.csv")) //將配料寫入檔案中
                    {
                        for (int c = 0; c < wPopulation.Count; c++)
                        {
                            stream.WriteLine(wPopulation[c].ToCsvString(profiles[i]));
                        }
                        //foreach (var item in wPopulation.GroupLength())
                        //{
                        //    stream.WriteLine($"長度 : {item.Key} 數量 : {item.Count()}");
                        //    stream.WriteLine("-----------------");
                        //    foreach (var chr in item)
                        //    {
                        //        for (int q = 0; q < chr.DNA.Count; q++)
                        //        {
                        //            stream.WriteLine(weightsPatr[chr.DNA[q]]);
                        //        }
                        //        stream.WriteLine(chr.ToString());
                        //        stream.WriteLine("-----------------");
                        //    }
                        //}
                        //stream.WriteLine("---------------------------");
                    }
                    using (StreamWriter stream = new StreamWriter($@"{ApplicationVM.DirectoryModel()}\使用率{profiles[i].Replace('*', 'X')}.csv")) //將配料寫入檔案中
                    {
                        for (int c = 0; c < uPopulation.Count; c++)
                        {
                            stream.WriteLine(uPopulation[c].ToCsvString(profiles[i]));
                        }
                        //foreach (var item in uPopulation.GroupLength())
                        //{
                        //    stream.WriteLine($"長度 : {item.Key} 數量 : {item.Count()}");
                        //    stream.WriteLine("-----------------");
                        //    foreach (var chr in item)
                        //    {
                        //        for (int q = 0; q < chr.DNA.Count; q++)
                        //        {
                        //            stream.WriteLine(weightsPatr[chr.DNA[q]]);
                        //        }
                        //        stream.WriteLine(chr.ToString());
                        //        stream.WriteLine("-----------------");
                        //    }
                        //}
                        //stream.WriteLine("---------------------------");
                    }
                }
            }
        }
        [Test, TestCaseSource("GetBom")]
        public unsafe void 自動正常配料(string projectName)
        {
            ApplicationViewModel.ProjectName = projectName;
            STDSerialization serialization = new STDSerialization();//序列化處理器
            List<string> profiles = serialization.GetProfile().Where(el => el.Contains("RH")).ToList();//模型有使用到的斷面規格
            List<int> materialCount = new List<int>();//儲存素材數量
            List<int> partCount = new List<int>();//儲存零件數量
            List<string> profile = new List<string>();//儲存斷面規格
            List<double> surplus = new List<double>();//所有損耗
            List<double> length = new List<double>();//總米數
            List<Dictionary<string, int>> useInfo = new List<Dictionary<string, int>>();
            for (int q = 0; q < profiles.Count; q++)
            {
                useInfo.Add(new Dictionary<string, int>());
                profile.Add(profiles[q]);
                string partData = profiles[q].GetHashCode().ToString(); //資料名稱
                ObservableCollection<SteelPart> steels = new ObservableCollection<SteelPart>(serialization.GetPart(partData).Select(el => (SteelPart)el)); //零件序列化檔案
                List<SinglePart> listPart = new List<SinglePart>();//配料列表
                foreach (var item in steels)//將物件全部變成單一個體
                {
                    listPart.AddRange(SinglePart.UnfoldPart(item));//展開物件並加入配料列表內
                }
                listPart.Sort(Compare);//由大牌到小
                SinglePart[] arrayPatr = listPart.ToArray(); //複製陣列給指標
                for (int i = 0; i < arrayPatr.Length; i++)//寫入 index
                {
                    arrayPatr[i].ChangeIndex(i);
                }
                partCount.Add(arrayPatr.Length);
                MatchSetting matchSetting = new MatchSetting()//配料設定檔
                {
                    MainLengths = new List<double>()
                    {
                    9000,
                    10000,
                    12000,
                    },
                    SecondaryLengths = new List<double>()
                    {
                    9500,
                    11000,
                    12000,
                    13000,
                    14000,
                    15000,
                    },
                    Cut = 3,
                    EndCut = 10,
                    StartCut = 10,
                };

                using (StreamWriter stream = new StreamWriter($@"{ApplicationVM.DirectoryModel()}\{profiles[q].Replace('*', 'X')}.txt")) //將配料寫入檔案中
                {
                    stream.WriteLine("---------------------------------------");
                    fixed (SinglePart* ptr = arrayPatr)//將陣列轉換為指標
                    {
                        List<Chromosome> chromosomes = new List<Chromosome>(); //儲存素材基因
                        Population pop = new Population(matchSetting, ptr, arrayPatr.Length);//配料驅動
                        double currentSurplus = 0;
                        double currentM = 0; //總米數
                        while (arrayPatr.Where(el => !el.Match).Count() > 0) //持續運算到配料完成
                        {
                            pop.Initialize(); //初始化內部
                            Chromosome chromosome = pop.Suitable();//取得最適合的組合
                            stream.WriteLine(chromosome.ToString());//寫入基因字串到內容

                            for (int i = 0; i < chromosome.DNA.Count; i++) //逐步展開配置的素材
                            {
                                ptr[chromosome.DNA[i]].Match = true; //配料完成
                                stream.WriteLine(ptr[chromosome.DNA[i]].ToString()); //將配料完成的東西寫入到檔案中
                            }
                            stream.WriteLine("---------------------------------------");
                            chromosomes.Add(chromosome);//加入到儲存基因的集合
                            currentSurplus += chromosome.Surplus();
                            currentM += chromosome.Length;
                            if (useInfo[q].ContainsKey(chromosome.Length.ToString()))
                            {
                                useInfo[q][chromosome.Length.ToString()]++;
                            }
                            else
                            {
                                useInfo[q].Add(chromosome.Length.ToString(), 1);
                            }
                        }
                        materialCount.Add(chromosomes.Count);
                        surplus.Add(currentSurplus);
                        length.Add(currentM);
                    }
                }
            }
            for (int i = 0; i < materialCount.Count; i++)
            {
                Debug.WriteLine($"斷面規格 : {profile[i]} 素材數量 : {materialCount[i]} 零件數量 : {partCount[i]} 損耗 : {surplus[i]} 總米數 : {length[i]}");
                string useStr = string.Empty;
                foreach (var item in useInfo[i])
                {
                    useStr += $"素材長度 {item.Key} 使用 : {item.Value}\n";
                }
                Debug.WriteLine($"{useStr}");
            }
        }
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
        #region 基因
        /// <summary>
        /// 挑選適合的配對料件
        /// </summary>
        public unsafe class Population : List<Chromosome>
        {
            #region 公開屬性

            #endregion

            #region 私有屬性
            /// <summary>
            /// 配料設定
            /// </summary>
            private MatchSetting _Match;
            /// <summary>
            /// 亂數種子
            /// </summary>
            [NonSerialized()]
            private Random _Random = new Random();
            [NonSerialized()]
            private SinglePart* Ptr;
            private int _Count;
            #endregion
            /// <summary>
            /// 標準建構式
            /// </summary>
            /// <param name="match">配對料單設定</param>
            /// <param name="ptr">物件列表指標</param>
            public Population(MatchSetting match, SinglePart* ptr, int count)
            {
                _Match = match;
                Ptr = ptr;
                _Count = count;
            }

            #region 公開方法
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="popSize"></param>
            public void Initialize()
            {
                Clear();
                List<double> lengths = _Match.MainLengths;
                int index = 0;
                for (int i = 0; i < _Count; i++)
                {
                    if (!Ptr[i].Match)
                    {
                        index = i;
                        break;
                    }
                }
                for (int i = 0; i < lengths.Count; i++)
                {
                    this.Add(new Chromosome(lengths[i], _Match, index, Ptr, _Count));
                }
            }
            /// <summary>
            /// 取得最合適的物件
            /// </summary>
            /// <returns></returns>
            public Chromosome Suitable()
            {
                while (!this.TrueForAll(el => el.End))
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        if (!this[i].End)//如果尚未結束
                        {
                            SinglePart? part = this[i].SingleMatch();
                            if (part != null)
                            {
                                this[i].Add(part.Value.Index);
                            }
                        }
                    }
                }
                this.Sort(Compare);
                return this[0];
            }

            #endregion

            #region 私有方法
            /// <summary>
            /// 由大到小排序
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            private static int Compare(Chromosome a, Chromosome b)
            {
                if (a.Fitness() < b.Fitness()) //比較小的往後排
                    return 1;//陣列往前移一個
                else if (a.Fitness() > b.Fitness()) //比較大的往前排
                    return -1;//陣列往後移一個
                else
                    return 0;
            }
            #endregion
        }

        /// <summary>
        /// 與零件組合成的素材
        /// </summary>
        [Serializable]
        public unsafe class Chromosome : IMatchSetting
        {
            #region 公開屬性
            /// <summary>
            /// 素材長度
            /// </summary>
            public double Length { get; private set; }
            /// <inheritdoc/>
            public double Cut { get; private set; }
            /// <inheritdoc/>
            public double StartCut { get; private set; }
            /// <inheritdoc/>
            public double EndCut { get; private set; }
            /// <summary>
            /// 配置的索引位置
            /// </summary>
            public unsafe List<int> DNA { get; set; }
            /// <summary>
            /// 結束配對
            /// </summary>
            public bool End { get; set; }
            #endregion

            #region 私有屬性
            [NonSerialized]
            private SinglePart* Ptr;
            private int _Count;
            #endregion

            /// <summary>
            /// 標準建構式
            /// </summary>
            /// <param name="length"></param>
            /// <param name="setting"></param>
            /// <param name="singleParts"></param>
            public Chromosome(double length, IMatchSetting setting, int index, SinglePart* parts, int count)
            {
                Length = length + setting.EndCut + setting.StartCut; //素材總長是，長度 + 起始切除長度 + 尾部切除長度
                Cut = setting.Cut; //切割間隙
                StartCut = setting.StartCut;//起始切除長度
                EndCut = setting.EndCut;//尾部切除長度
                DNA = new List<int>() { index };
                Ptr = parts;
                _Count = count;
            }

            #region 公開方法
            /// <summary>
            /// 可能符合的料件
            /// </summary>
            /// <returns></returns>
            public List<SinglePart> Match()
            {
                double max = Length - UseLength(); //最大可配置長度
                List<SinglePart> result = new List<SinglePart>();
                for (int i = 0; i < _Count; i++)
                {
                    if (Ptr[i].Length < max + Cut && !Ptr[i].Match)
                    {
                        if (!result.Contains(Ptr[i]))
                        {
                            result.Add(Ptr[i]);
                        }
                    }
                }
                if (result.Count > 0)
                {
                    result.Sort(Compare);

                    return result;
                }
                else
                {
                    End = true; //結束配料
                    return null;
                }
            }
            /// <summary>
            /// 可能符合的料件
            /// </summary>
            /// <returns></returns>
            public SinglePart? SingleMatch()
            {
                double max = Length - UseLength(); //最大可配置長度
                SinglePart? result = null;
                for (int i = 0; i < _Count; i++)
                {
                    if (Ptr[i].Length < max + Cut && !Ptr[i].Match && !DNA.Contains(i))
                    {
                        result = Ptr[i];
                        return result;
                    }
                }
                if (result == null)
                    End = true; //結束配料

                return result;
            }
            public void Add(int index)
            {
                DNA.Add(index);
            }
            /// <summary>
            /// 取得使用長度
            /// </summary>
            /// <param name="singles"></param>
            /// <returns></returns>
            public double UseLength()
            {
                double result = 0;
                for (int i = 0; i < DNA.Count; i++)
                {
                    result += Ptr[DNA[i]].Length;
                }
                result += StartCut; //素材起始切除長度
                result += EndCut;//素材尾部切除長度
                if (DNA.Count != 0)//如果有零件
                    result += (DNA.Count + 1) * Cut; //計算所有素材的切割寬度

                return result;
            }
            /// <summary>
            /// 計算適合程度
            /// </summary>
            /// <returns></returns>
            public double Fitness()
            {
                if (UseLength() > Length)
                {
                    return 0;
                }
                else
                {
                    double use = UseLength() / Length; //使用率
                    //double odds = use / DNA.Count; //比數
                    return use;
                }
            }
            /// <summary>
            /// 計算餘料部分
            /// </summary>
            /// <param name="singles"></param>
            /// <returns></returns>
            public double Surplus()
            {
                double result = Length - UseLength(); //使用長度
                return result;
            }
            /// <inheritdoc/>
            public override string ToString()
            {
                return $@"配料長度 : {Length} 使用長度 : {UseLength()} 餘料 : {Surplus()} 適合程度 : {Fitness()}";
            }
            /// <summary>
            /// 複製
            /// </summary>
            /// <returns></returns>
            public Chromosome Clone()
            {
                using (Stream objectStream = new MemoryStream())
                {
                    //序列化物件格式
                    IFormatter formatter = new BinaryFormatter();
                    //將自己所有資料序列化
                    formatter.Serialize(objectStream, this);
                    //複寫資料流位置，返回最前端
                    objectStream.Seek(0, SeekOrigin.Begin);
                    //再將objectStream反序列化回去 
                    return (Chromosome)formatter.Deserialize(objectStream);
                }
            }
            #endregion

        }
        #endregion
    }
}

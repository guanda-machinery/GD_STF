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
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
using static WPFSTD105.ViewLocator;

namespace 測試自動配料
{
    [TestFixture]
    public class 配料分析
    {
        private static IEnumerable<string> GetBom()
        {
            //yield return "昇鋼金屬有限公司";
            yield return "卜蜂";
            //yield return "南亞科技";
            //yield return "英發";
            //yield return "高雄港";
            //yield return "觀音運動中心";
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
        public void 自動配料(string projectName)
        {
            ApplicationViewModel.ProjectName = projectName;
            STDSerialization serialization = new STDSerialization();//序列化處理器
            ObservableCollection<string> profiles = serialization.GetProfile();//模型有使用到的斷面規格
            string partData = profiles[0].GetHashCode().ToString(); //資料名稱
            ObservableCollection<SteelPart> steels = new ObservableCollection<SteelPart>(serialization.GetPart(partData).Select(el => (SteelPart)el)); //零件序列化檔案
            List<SinglePart> singleParts = new List<SinglePart>();//配料列表
            foreach (var item in steels)//將物件全部變成單一個體
            {
                singleParts.AddRange(SinglePart.UnfoldPart(item));//展開物件並加入配料列表內
            }
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
                }
            };

        }

        #region 基因
        /// <summary>
        /// 挑選適合的配對料件
        /// </summary>
        public class Population : List<Chromosome>
        {
            #region 公開屬性
            /// <summary>
            /// 零件列表
            /// </summary>
            public List<SinglePart> SingleParts;
            #endregion

            #region 私有屬性
            /// <summary>
            /// 配料設定
            /// </summary>
            private MatchSetting _Match { get; set; }
            /// <summary>
            /// 亂數種子
            /// </summary>
            private Random _Random = new Random();
            #endregion
            /// <summary>
            /// 標準建構式
            /// </summary>
            /// <param name="match">配對料單設定</param>
            /// <param name="singleParts">零件列表</param>
            /// <param name="popSize">幾個物件下去做比較</param>
            public Population(MatchSetting match, List<SinglePart> singleParts)
            {
                SingleParts = singleParts;
            }

            #region 公開方法
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="popSize"></param>
            public void Initialize(int popSize)
            {
                for (int i = 0; i < popSize; i++)
                {
                    int index = _Random.Next(0, _Match.MainLengths.Count); //隨機取得素材長度陣列位置
                    double length = _Match.MainLengths[index]; //抓取到的素材長度
                    this.Add(new Chromosome(length, _Match,  SingleParts));//加入到列表內
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

                }

            }
            #endregion

            #region 私有方法
            /// <summary>
            /// 排序
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            private static int Compare(Chromosome a, Chromosome b)
            {
                if (a.Fitness() < b.Fitness()) //比較的往後排
                    return 1;
                else if (a.Fitness() > b.Fitness()) //較大的往前排
                    return -1;
                else
                    return 0;
            }
            #endregion
        }

        /// <summary>
        /// 與零件組合成的素材
        /// </summary>
        public class Chromosome : IMatchSetting
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
            /// 配置的零件編號
            /// </summary>
            public List<SinglePart> DNA { get; private set; }
            /// <summary>
            /// 結束配對
            /// </summary>
            public bool End { get; set; }
            /// <summary>
            /// 零件列表
            /// </summary>
            public List<SinglePart> Singles { get; private set; }
            #endregion
            /// <summary>
            /// 標準建構式
            /// </summary>
            /// <param name="length"></param>
            /// <param name="setting"></param>
            /// <param name="singleParts"></param>
            public Chromosome(double length, IMatchSetting setting, List<SinglePart> singleParts)
            {
                Length = length + setting.EndCut + setting.StartCut; //素材總長是，長度 + 起始切除長度 + 尾部切除長度
                Cut = setting.Cut; //切割間隙
                StartCut = setting.StartCut;//起始切除長度
                EndCut = setting.EndCut;//尾部切除長度
                Singles = singleParts;
            }

            #region 私有屬性

            #endregion

            #region 公開方法
            public void Add(int index)
            {
                if (End)
                {
                    DNA.Add(Singles[index]);
                }
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
                    result += DNA[i].Length;
                }
                result += StartCut + Cut; //素材起始切除長度 + 上切割寬度
                result += EndCut + Cut;//素材尾部切除長度 + 上切割寬度
                if (DNA.Count != 0)//如果有零件
                    result += (DNA.Count - 1) * Cut; //計算所有素材的切割寬度

                return result;
            }
            /// <summary>
            /// 計算適合程度
            /// </summary>
            /// <returns></returns>
            public double Fitness()
            {
                double use = UseLength() / Length; //使用率
                return use;
            }
            /// <summary>
            /// 計算餘料部分
            /// </summary>
            /// <param name="singles"></param>
            /// <returns></returns>
            public double Surplus()
            {
                double use = UseLength(); //使用長度
                return Length - use;
            }
            /// <inheritdoc/>
            public override string ToString()
            {
                return $@"使用長度 : {UseLength()} 餘料 : {Surplus()} 適合程度 : {Fitness()}";
            }
            #endregion

        }
        #endregion
    }
}

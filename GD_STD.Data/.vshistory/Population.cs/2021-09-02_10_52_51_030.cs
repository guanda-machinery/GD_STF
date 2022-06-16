using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 挑選適合的配對料件
    /// </summary>
    public unsafe class Population
    {
        #region 公開屬性
        /// <summary>
        /// 加總長度
        /// </summary>
        public int TotalLength { get; private set; } = 0;
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
        /// <summary>
        /// 陣列數量
        /// </summary>
        private int _Count;
        /// <summary>
        /// 比較種子(種子集合)
        /// </summary>
        private List<Chromosome> Seed { get; set; }
        /// <summary>
        /// 評分處理函式
        /// </summary>
        [NonSerialized()]
        private IScore _Score;
        #endregion
        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="match">配對料單設定</param>
        /// <param name="ptr">物件列表指標</param>
        public Population(MatchSetting match, SinglePart* ptr, int count, ref IScore score)
        {
            _Match = match;
            Ptr = ptr;
            _Count = count;
            Seed = new List<Chromosome>();
        }

        #region 公開方法
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="popSize"></param>
        public void Initialize()
        {
            Seed.Clear();//比較種子
            List<double> lengths = _Match.MainLengths; //取得配料主要長度
            int index = 0;
            for (int i = 0; i < _Count; i++) //逐步看動態指標
            {
                if (!Ptr[i].Match)//如果配料完成
                {
                    index = i;
                    break;
                }
            }
            for (int i = 0; i < lengths.Count; i++)//長度配置到種子列表內
            {
                Seed.Add(new Chromosome(lengths[i], _Match, index, Ptr, _Count, ref _Score));//將主要配料長度加入到種子列表內
            }
        }
        /// <summary>
        /// 取得最合適的物件
        /// </summary>
        /// <returns></returns>
        public Chromosome Suitable()
        {
            while (!Seed.TrueForAll(el => el.End)) //查看所有種子都完成配料
            {
                for (int i = 0; i < Seed.Count; i++) //逐步查看種子是否配料完成
                {
                    if (!Seed[i].End)//如果尚未結束
                    {
                        SinglePart? part = Seed[i].SingleMatch(); //選擇單一的合適對象
                        if (part != null)
                        {
                            Seed[i].Add(part.Value.Index);//加入到種子DNA
                        }
                    }
                }
            }
            Seed.Sort(Compare);//排序適合程度大到小
            return Seed[0];//回傳最適合的種子
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
}

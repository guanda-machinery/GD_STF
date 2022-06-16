using System;
using System.Collections.Generic;
using System.Linq;

namespace GD_STD.Data
{
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
        private SinglePart* _Ptr;
        /// <summary>
        /// 陣列數量
        /// </summary>
        [NonSerialized()]
        private int _Count;
        /// <summary>
        /// 比較種子(種子集合)
        /// </summary>
        [NonSerialized()]
        private List<Chromosome> Seed;
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
        /// <param name="count">陣列數量</param>
        ///  <param name="score">評分</param>
        public Population(MatchSetting match, SinglePart* ptr, int count, IScore score)
        {
            _Match = match;
            _Ptr = ptr;
            _Count = count;
            Seed = new List<Chromosome>();
            _Score = score;
        }
        #region 公開方法
        /// <summary>
        /// 執行運算
        /// </summary>
        public void Run()
        {
            if (_Count == 0)//如果沒數量
                return;//終止運算
            while (Initialize(_Match.MainLengths)) // Initialize = true 才會執行
            {
                Suitable();//挑選最適合的物件並且加入
            }
        }

        /// <summary>
        /// 計算餘料
        /// </summary>
        /// <returns>列表內的所有餘料</returns>
        public double TotalSurplus()
        {
            double result = 0;
            for (int i = 0; i < Count; i++) //逐步查看集合
                result += this[i].Surplus();//加總餘料

            return result;
        }
        /// <summary>
        /// 合計長度
        /// </summary>
        /// <returns>列表內的所有長度</returns>
        public double TotalLength()
        {
            double result = 0;
            for (int i = 0; i < Count; i++) //計算所有素材長度
                result += this[i].Length;//加總素材長度

            return result;
        }
        /// <summary>
        /// 素材長度群組化
        /// </summary>
        /// <returns>有使用配料的素材長度</returns>
        public IEnumerable<IGrouping<double, Chromosome>> GroupLength()
        {
            var group =
                from el in this
                group el by el.Length into g
                orderby g.Key
                select g;
            foreach (var result in group) //疊代器
            {
                yield return result;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 儲存最合適的物件
        /// </summary>
        /// <returns></returns>
        public void Suitable()
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
                            Seed[i].Add(part.Value.Index);//加入到種子基因
                        }
                    }
                }
            }
            Seed.Sort(Compare);//排序適合程度大到小
            if (Seed[0].Fitness() == 0) //如果素材短於配料長度
            {
                Initialize(_Match.SecondaryLengths);
            }
            //else//如果素材沒有短於配料長度
            //{
            //    Seed[0].Number = _Match.StartNumber + this.Count.ToString().PadLeft(4, '0');
            //    this.Add(Seed[0]);//加入最適合的種子
            //    MatchDNA();
            //}
            Seed[0].Number = _Match.StartNumber + this.Count.ToString().PadLeft(4, '0');
            this.Add(Seed[0]);//加入最適合的種子
            MatchDNA();
        }
        /// <summary>
        /// 配置好的物件做標記
        /// </summary>
        public void MatchDNA()
        {
            for (int i = 0; i < Seed[0].DNA.Count; i++) //逐步查看基因
            {
                _Ptr[Seed[0].DNA[i]].Match = true;//匹配完成
            }
        }
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
                return 0;//保持目前位置
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lengths">素材長度</param>
        /// <returns></returns>
        private bool Initialize(List<double> lengths)
        {
            Seed.Clear();//比較種子
            int index = -1;
            for (int i = 0; i < _Count; i++) //逐步看動態指標
            {
                if (!_Ptr[i].Match)//如果配料完成
                {
                    index = i;
                    break;
                }
            }
            for (int i = 0; i < lengths.Count; i++)//長度配置到種子列表內
            {
                Seed.Add(new Chromosome(lengths[i], _Match, index, _Ptr, _Count, _Score));//將主要配料長度加入到種子列表內
            }
            return index == -1 ? false : true; //如果找不到配料的物件回傳 false
        }
        #endregion

        #region 複寫公開方法
        /// <inheritdoc/>
        public override string ToString()
        {
            return $@"配料總長度 : {TotalLength()} 使用數量 : {Count} 餘料 : {TotalSurplus()}";
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 零件組合成的素材
    /// </summary>
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
        /// <summary>
        /// 陣列位址
        /// </summary>
        [NonSerialized]
        private SinglePart* Ptr;
        /// <summary>
        /// 陣列數量
        /// </summary>
        private int _Count;
        /// <summary>
        /// 評分處理函式
        /// </summary>
        [NonSerialized()]
        private IScore _Score;
        #endregion

        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="length"></param>
        /// <param name="setting"></param>
        /// <param name="parts">陣列位址</param>
        /// <param name="count">陣列數量</param>
        ///  <param name="score">評分</param>
        public Chromosome(double length, IMatchSetting setting, int index, SinglePart* parts, int count, ref IScore score)
        {
            Length = length + setting.EndCut + setting.StartCut; //素材總長是，長度 + 起始切除長度 + 尾部切除長度
            Cut = setting.Cut; //切割間隙
            StartCut = setting.StartCut;//起始切除長度
            EndCut = setting.EndCut;//尾部切除長度
            DNA = new List<int>() { index };
            Ptr = parts;
            _Count = count;
            _Score = score;
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
        #endregion

        #region 私有方法
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

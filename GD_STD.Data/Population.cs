using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GD_STD.Data
{
    /// <summary>
    /// 挑選適合的配對料件
    /// </summary>
    /// <remarks>
    /// <para>GA 演算</para>
    /// <para>概念 : </para>
    /// <para>http://rportal.lib.ntnu.edu.tw:8080/server/api/core/bitstreams/0de9ce65-1ecf-4e55-9f64-cd9b2574c916/content</para>
    /// <para>最佳保留法</para>
    /// </remarks>
    public unsafe class Population : List<Chromosome>
    {
        #region 公開屬性
        #endregion

        #region 私有屬性
        /// <summary>
        /// 配料設定
        /// </summary>
        private MatchSetting _Match;
        ///// <summary>
        ///// 亂數種子
        ///// </summary>
        //[NonSerialized()]
        //private Random _Random = new Random();
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
        private IConfiguration _Score;
        #endregion
        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="match">配對料單設定</param>
        /// <param name="ptr">物件列表指標</param>
        /// <param name="count">陣列數量</param>
        ///  <param name="score">配置函式</param>
        public Population(MatchSetting match, SinglePart* ptr, int count, IConfiguration score)
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
            while (Initialize(_Match.MainLengths/*, _Match.SecondaryLengths*/)) // Initialize = true 才會執行
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
        /// 配置單一適合物件
        /// </summary>
        /// <param name="cycle">控制循環流程</param>
        /// <remarks>
        /// </remarks>
        private void Suitable(bool cycle = false)
        {
            while (!Seed.TrueForAll(el => el.End)) //查看所有種子都完成配料(交配方法)
            {
                _Score.Matching(ref Seed); //找尋適合物件
            }
            Seed.Sort(Compare);//排序適合程度大到小
            if ((Seed[0].Fitness() == 0 ||  Seed[0].Fitness() < 0.8) && !cycle) //如果素材短於配料長度或者分數低下
            {
                //重新規劃配料條件
                List<double> length1 = Seed[0].Fitness() == 0 ? _Match.SecondaryLengths : _Match.MainLengths; 
                List<double> length2 = Seed[0].Fitness() == 0 ? null : _Match.SecondaryLengths;

                Initialize(length1, length2); //重新初始化各類基因
                Suitable(true); //重新配置
                return; //收斂
            }
            Seed[0].Number = _Match.StartNumber + (this.Count + 1).ToString().PadLeft(4, '0');//給定起始編號
            for (int i = 0; i < Seed[0].DNA.Count; i++) //逐步查看基因
            {
                SinglePart _ = _Ptr[Seed[0].DNA[i]];//匹配完成
                _Ptr[Seed[0].DNA[i]].Match = true;//匹配完成
                _ = _Ptr[Seed[0].DNA[i]];//匹配完成
            }
            this.Add(Seed[0]);//加入最適合的種子
        }
        /// <summary>
        /// 由大到小排序
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>排序適應程度</returns>
        internal static int Compare(Chromosome a, Chromosome b)
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
        /// <param name="mainLengths">主要素材長度</param>
        /// <param name="secondaryLengths">次要素材長度</param>
        /// <returns>
        /// 初始化種子
        /// </returns>
        private bool Initialize(List<double> mainLengths, List<double> secondaryLengths = null)
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
            for (int i = 0; i < mainLengths.Count; i++)//長度配置到種子列表內
            {
                Seed.Add(new Chromosome(mainLengths[i], _Match, index, _Ptr, _Count, false, _Score));//將主要配料長度加入到種子列表內
            }
            if (secondaryLengths != null)
            {
                for (int i = 0; i < secondaryLengths.Count; i++)//長度配置到種子列表內
                {
                    Seed.Add(new Chromosome(secondaryLengths[i], _Match, index, _Ptr, _Count, true, _Score));//將主要配料長度加入到種子列表內
                }
            }

            return index == -1 ? false : true; //如果找不到配料的物件回傳 false
        }
        /// <summary>
        /// 將列表儲存到 CSV 格式 
        /// </summary>
        /// <param name="path">要新建的檔案路徑</param>
        /// <param name="profile">配料的斷面規格</param>
        public void SaveCsv(string path,string profile)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            using (StreamWriter stream = new StreamWriter(fileStream, Encoding.Default)) //將配料寫入檔案中
            {
                stream.WriteLine("素材編號,斷面規格,素材長度,切割長度,損耗,零件編號,零件長度");
                for (int c = 0; c < this.Count; c++)
                {
                    stream.WriteLine(this[c].ToCsvString(profile));
                }
            }
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

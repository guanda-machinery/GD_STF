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
        [NonSerialized()]
        public unsafe List<int> DNA;
        /// <summary>
        /// 零件編號
        /// </summary>
        public string[] PartNumber { get; set; }
        /// <summary>
        /// 結束配對
        /// </summary>
        public bool End
        {
            get => _End;
            set
            {
                _End = value;
                if (value)
                {
                    PartNumber = new string[DNA.Count]; //存取編號
                    for (int i = 0; i < PartNumber.Length; i++)//逐步加入到編號集合內
                    {
                        List<char> number = new List<char>();
                        for (int c = 0; c < 20; c++)//SinglePart Number 長度是 20 所以只能開 20
                        {
                            if (_Ptr[i].Number[c] != '\0') //過濾掉不需要的字元
                            {
                                number.Add(_Ptr[DNA[i]].Number[c]);
                            }
                        }
                        PartNumber[i] = new string(number.ToArray());
                    }
                }
            }
        }
        /// <summary>
        /// 素材編號
        /// </summary>
        public string Number { get; set; }
        #endregion

        #region 私有屬性
        /// <summary>
        /// 陣列位址
        /// </summary>
        [NonSerialized]
        private SinglePart* _Ptr;
        /// <summary>
        /// 陣列數量
        /// </summary>
        private int _Count;
        /// <summary>
        /// 評分處理函式
        /// </summary>
        [NonSerialized()]
        private IScore _Score;
        private bool _End;
        #endregion

        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="length">染色體長度</param>
        /// <param name="setting">切割長度</param>
        /// <param name="index">第一代基因陣列位置</param>
        /// <param name="parts">物件列表指標</param>
        /// <param name="count">陣列數量</param>
        ///  <param name="score">評分</param>
        public Chromosome(double length, IMatchSetting setting, int index, SinglePart* parts, int count, IScore score)
        {
            Length = length + setting.EndCut + setting.StartCut; //素材總長是，長度 + 起始切除長度 + 尾部切除長度
            Cut = setting.Cut; //切割間隙
            StartCut = setting.StartCut;//起始切除長度
            EndCut = setting.EndCut;//尾部切除長度
            DNA = new List<int>() { index };
            _Ptr = parts;
            _Count = count;
            _Score = score;
        }

        #region 公開方法
        /// <summary>
        /// 可能符合的料件
        /// </summary>
        /// <returns>回傳全部符合料件</returns>
        public List<SinglePart> Match()
        {
            double max = Length - UseLength(); //最大可配置長度
            List<SinglePart> result = new List<SinglePart>();//可配置的列表
            for (int i = 0; i < _Count; i++) //逐步看動態指標
            {
                if (_Ptr[i].Length < max + Cut && !_Ptr[i].Match) //找尋可配置的長度
                {
                    if (!result.Contains(_Ptr[i])) //如果列表內沒有這種長度
                    {
                        result.Add(_Ptr[i]);//加入物件
                    }
                }
            }
            if (result.Count > 0) //判斷是否可配料
            {
                result.Sort(Compare);//排序
                return result;//回傳整個列表
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
        /// <returns>回傳最符合料件</returns>
        public SinglePart? SingleMatch()
        {
            double max = Length - UseLength(); //最大可配置長度
            SinglePart? result = null; //配料結果
            for (int i = 0; i < _Count; i++)//逐步看動態指標
            {
                if (_Ptr[i].Length < max + Cut && !_Ptr[i].Match && !DNA.Contains(i)) //找尋可配置的長度
                {
                    result = _Ptr[i]; //指回傳找到第一個判斷是適合的物件
                    return result;
                }
            }
            if (result == null)
                End = true; //結束配料
            return result;
        }
        /// <summary>
        /// 加入基因
        /// </summary>
        /// <param name="index">陣列索引位置</param>
        public void Add(int index)
        {
            DNA.Add(index);
        }
        /// <summary>
        /// 取得使用長度
        /// </summary>
        /// <returns></returns>
        public double UseLength()
        {
            double result = 0;
            for (int i = 0; i < DNA.Count; i++)//逐步查看長度
            {
                result += _Ptr[DNA[i]].Length;
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
            return _Score.Fitness(this);
        }
        /// <summary>
        /// 計算餘料
        /// </summary>
        /// <returns>回傳餘料</returns>
        public double Surplus()
        {
            double result = Length - UseLength(); //使用長度
            return result;
        }
        /// <summary>
        /// 轉 Csv 文件
        /// </summary>
        /// <returns></returns>
        public string ToCsvString(string profile)
        {
            List<double> lengths = new List<double>();
            for (int i = 0; i < DNA.Count; i++)
            {
                lengths.Add(_Ptr[DNA[i]].Length);
            }
            string result = $"{Number},{profile},{Length},{UseLength()},{Length - UseLength()},{PartNumber[0]},{lengths[0]}";
            for (int i = 1; i < DNA.Count; i++)
            {
                result += $"\n,,,,,{PartNumber[i]},{lengths[i]}";
            }

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
                return 0;//保持目前位置
        }
        #endregion
    }
}

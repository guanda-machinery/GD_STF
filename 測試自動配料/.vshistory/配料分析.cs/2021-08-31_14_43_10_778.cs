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
        public unsafe void 自動配料(string projectName)
        {
            ApplicationViewModel.ProjectName = projectName;
            STDSerialization serialization = new STDSerialization();//序列化處理器
            ObservableCollection<string> profiles = serialization.GetProfile();//模型有使用到的斷面規格
            string partData = profiles[37].GetHashCode().ToString(); //資料名稱
            ObservableCollection<SteelPart> steels = new ObservableCollection<SteelPart>(serialization.GetPart(partData).Select(el => (SteelPart)el)); //零件序列化檔案
            List<SinglePart> singleParts = new List<SinglePart>();//配料列表
            int index = 0;
            foreach (var item in steels)//將物件全部變成單一個體
            {
                singleParts.AddRange(SinglePart.UnfoldPart(item, ref index));//展開物件並加入配料列表內
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
            SinglePart[] parts = singleParts.ToArray();
            singleParts.Sort(Compare);
            int size = Marshal.SizeOf(parts);
            byte[] result = new byte[size];
            IntPtr intPtr = Marshal.AllocHGlobal(size);

            for (int i = 0; i < parts.Length; i++)
            {
                fixed (void* p = &parts[i])
                {
                    int offset = sizeof(SinglePart) * i;
                    Marshal.WriteIntPtr(intPtr, offset, (IntPtr)p);
                }
            }
            SinglePart* AA = (SinglePart*)intPtr.ToPointer();
            var A =  AA[0].Length;
            List<Chromosome> chromosomes = new List<Chromosome>();
            while (singleParts.Count != 0)
            {

                Population pop = new Population(matchSetting, (SinglePart*)intPtr.ToPointer());
                pop.Initialize();
                Chromosome chromosome = pop.Suitable();
                chromosomes.Add(chromosome);
            }
            for (int i = 0; i < chromosomes.Count; i++)
            {
                Debug.WriteLine(chromosomes[i]);
            }
         Marshal.FreeHGlobal(intPtr);
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
        /// C++ _msize 函式，回傳動態記憶體大小
        /// </summary>
        /// <param name="handle"></param>
        /// <returns>IntPtr Size</returns>
        [DllImport("msvcrt.dll", SetLastError = true)]
        private static extern int _msize(IntPtr handle);
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
            private SinglePart* _SingleParts;
            private int _Size;
            #endregion
            /// <summary>
            /// 標準建構式
            /// </summary>
            /// <param name="match">配對料單設定</param>
            /// <param name="ptr">物件列表指標</param>
            public Population(MatchSetting match, SinglePart* ptr)
            {
                _Match = match;
                _SingleParts = ptr;
                _Size = _msize((IntPtr)ptr);
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
                SinglePart max = _SingleParts[0];
                for (int i = 0; i < lengths.Count; i++)
                {
                    this.Add(new Chromosome(lengths[i], _Match, 0, _SingleParts));
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
                            List<SinglePart> parts = this[i].Match();
                            if (parts != null)
                            {

                                //List<Chromosome> chromosomes = new List<Chromosome>();
                                //for (int c = 1; c < parts.Count; c++)
                                //{
                                //    this.Add(this[i].Clone());
                                //}
                                //return Suitable();
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
                    return 1;
                else if (a.Fitness() > b.Fitness()) //比較大的往前排
                    return -1;
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
            private SinglePart* ptr;
            #endregion

            /// <summary>
            /// 標準建構式
            /// </summary>
            /// <param name="length"></param>
            /// <param name="setting"></param>
            /// <param name="singleParts"></param>
            public Chromosome(double length, IMatchSetting setting, int index, SinglePart* parts)
            {
                Length = length + setting.EndCut + setting.StartCut; //素材總長是，長度 + 起始切除長度 + 尾部切除長度
                Cut = setting.Cut; //切割間隙
                StartCut = setting.StartCut;//起始切除長度
                EndCut = setting.EndCut;//尾部切除長度
                DNA = new List<int>() { index };
                ptr = parts;
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
                for (int i = 0; i < _msize((IntPtr)ptr) / Marshal.SizeOf(typeof(SinglePart)); i++)
                {
                    if (ptr[i].Length < max + Cut && !ptr[i].Match)
                    {
                        if (!result.Contains(ptr[i]))
                        {
                            result.Add(ptr[i]);
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
                //foreach (var item in (from el in Singles where el.Length < max select el))
                //{
                //    if (!result.Contains(item))
                //    {
                //        result.Add(item);
                //    }
                //}
                //if (result.Count() == 0)//如果可配置的料件為 0
                //{
                //    End = true; //結束配料
                //    return null;
                //}
                //else
                //{
                //    return result;
                //}
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
                    result += ptr[i].Length;
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
                if (use > 1)
                    return 0;
                else
                    return use;
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

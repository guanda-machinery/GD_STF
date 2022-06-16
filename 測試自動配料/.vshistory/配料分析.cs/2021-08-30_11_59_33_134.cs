using GD_STD;
using GD_STD.Data;
using Ninject;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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



            SELECT_ROULETTE _ = Roulette();

        }

        /// <summary>
        /// 選擇輪盤
        /// </summary>
        public enum SELECT_ROULETTE
        {
            /// <summary>
            /// 隨機配對
            /// </summary>
            ROULETTE,
            /// <summary>
            /// 選擇為邏輯配對
            /// </summary>
            LOGIC,
        }
        /// <summary>
        /// 隨機選擇，邏輯運配對或是隨機配對
        /// </summary>
        /// <returns></returns>
        public SELECT_ROULETTE Roulette()
        {
            double rv = random.NextDouble(); //亂數種子
            string strV = rv.ToString();
            Double value = Convert.ToDouble(strV[strV.Length - 1].ToString());
            return value % 2 == 0 ? SELECT_ROULETTE.ROULETTE : SELECT_ROULETTE.LOGIC;
        }

        [Test, TestCaseSource("GetBom")]
        public void 配對料單設定(string projectName)
        {
            ApplicationViewModel.ProjectName = projectName;

        }
        Random random = new Random();

        /// <summary>
        /// 與零件組合成的素材
        /// </summary>
        public class Chromosome : IMatchSetting
        {
            /// <inheritdoc/>
            public double Length { get; private set; }
            /// <inheritdoc/>
            public double StartCut { get; private set; }
            /// <inheritdoc/>
            public double EndCut { get; private set; }
            /// <inheritdoc/>
            public double Cut { get; private set; }
            public Chromosome()
            {

            }
            public double GetCurrentLength(List<SinglePart> singles)
            {

            }
            public Chromosome(double length, IMatchSetting setting)
            {
                Length = length;
                StartCut = setting.StartCut;
                EndCut = setting.EndCut;
                Cut = cut;
            }

            public List<int> DNA { get; set; }


            /// <summary>
            /// 變異個體
            /// </summary>
            /// <returns></returns>
            public int Mutate()
            {

            }
        }
    }
}

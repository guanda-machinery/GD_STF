using GD_STD.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WPFSTD105;
using static WPFSTD105.ViewLocator;
using System.Text;
using GD_STD.Data.MatchDI;

namespace 測試自動配料
{
    [TestFixture]
    public class 配料分析
    {
        private static IEnumerable<string> GeBom()
        {
            //yield return "昇鋼金屬有限公司";
            //yield return "卜蜂";
            //yield return "南亞科技";
            //yield return "英發";
            //yield return "高雄港";
            //yield return "觀音運動中心";
            //yield return "南亞RH200X100";
            //yield return "卜蜂RH400X200";
            //yield return "高雄港RH194X150";
            yield return "高雄港RH390X300";
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
        /// <summary>
        /// 完整配料
        /// </summary>
        /// <param name="projectName"></param>
        [Test, TestCaseSource("GetBom")]
        public unsafe void 完整配料(string projectName)
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
                    11500,
                    12000,
                    12500,
                    13000,
                    13500,
                    14000,
                    14500,
                    15000,
                },
                Cut = 3,
                EndCut = 10,
                StartCut = 10,
                StartNumber = "RH",
                Price = 29.3f
            };
            ApplicationViewModel.ProjectName = projectName;
            STDSerialization serialization = new STDSerialization();//
            List<string> profiles = serialization.GetProfile().Where(el => el.Contains("RH")).ToList();//模型有使用到的斷面規格
            for (int i = 0; i < profiles.Count; i++)
            {
                string partData = profiles[i].GetHashCode().ToString(); //資料名稱
                ObservableCollection<SteelPart> steels = new ObservableCollection<SteelPart>(serialization.GetPart(partData).Select(el => el)); //零件序列化檔案
                List<SinglePart> listPart = new List<SinglePart>();//配料列表
                foreach (var item in steels)//將物件全部變成單一個體
                {
                    listPart.AddRange(SinglePart.UnfoldPart(item, out List<bool> match));//展開物件並加入配料列表內
                    item.Match = match;
                }
                listPart.Sort(Compare);//由大牌到小
                SinglePart[] weightsPatr = listPart.ToArray(); //權重比較的零件列表
                for (int c = 0; c < weightsPatr.Length; c++)//寫入 index
                {
                    weightsPatr[c].ChangeIndex(c);
                }

                SinglePart[] usePart = new SinglePart[weightsPatr.Length];//使用率的零件列表
                SinglePart[] averagePart = new SinglePart[weightsPatr.Length];//數量評分的零件列表
                Array.Copy(weightsPatr, usePart, weightsPatr.Length);//複製到使用率
                Array.Copy(weightsPatr, averagePart, weightsPatr.Length);//複製到平均評分
                SinglePart[] multipleusePart = new SinglePart[weightsPatr.Length];//使用率的分裂零件列表
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
                    Debug.WriteLine($"使用平均分析訊息 : ");
                    Debug.WriteLine($"{aPopulation.ToString()} 配料物件 : {averagePart.Length}");
                    foreach (var item in aPopulation.GroupLength())
                    {
                        Debug.WriteLine($"長度 : {item.Key} 數量 : {item.Count()}");
                    }

                    Debug.WriteLine($"---------------------------------------------------------------");
                    Debug.WriteLine($"{profiles[i]}");
                    Debug.WriteLine($"分裂使用率分析訊息 : ");
                    Debug.WriteLine($"{umPopulation.ToString()} 配料物件 : {multipleusePart.Length}");
                    Debug.WriteLine($"---------------------------------------------------------------");

                    using (FileStream fileStream = new FileStream($@"{ApplicationVM.DirectoryModel()}\權重{profiles[i].Replace('*', 'X')}.csv", FileMode.Create))
                    using (StreamWriter stream = new StreamWriter(fileStream, Encoding.Default)) //將配料寫入檔案中
                    {
                        stream.WriteLine("素材編號,斷面規格,素材長度,切割長度,損耗,零件編號,零件長度");
                        for (int c = 0; c < wPopulation.Count; c++)
                        {
                            stream.WriteLine(wPopulation[c].ToCsvString(profiles[i]));
                        }
                    }

                    using (FileStream fileStream = new FileStream($@"{ApplicationVM.DirectoryModel()}\使用率{profiles[i].Replace('*', 'X')}.csv", FileMode.Create))
                    using (StreamWriter stream = new StreamWriter(fileStream, Encoding.Default)) //將配料寫入檔案中
                    {
                        stream.WriteLine("素材編號,斷面規格,素材長度,切割長度,損耗,零件編號,零件長度");
                        for (int c = 0; c < uPopulation.Count; c++)
                        {
                            stream.WriteLine(uPopulation[c].ToCsvString(profiles[i]));
                        }
                    }

                    using (FileStream fileStream = new FileStream($@"{ApplicationVM.DirectoryModel()}\平均{profiles[i].Replace('*', 'X')}.csv", FileMode.Create))
                    using (StreamWriter stream = new StreamWriter(fileStream, Encoding.Default)) //將配料寫入檔案中
                    {
                        stream.WriteLine("素材編號,斷面規格,素材長度,切割長度,損耗,零件編號,零件長度");
                        for (int c = 0; c < aPopulation.Count; c++)
                        {
                            stream.WriteLine(aPopulation[c].ToCsvString(profiles[i]));
                        }
                    }

                    using (FileStream fileStream = new FileStream($@"{ApplicationVM.DirectoryModel()}\分裂使用率{profiles[i].Replace('*', 'X')}.csv", FileMode.Create))
                    using (StreamWriter stream = new StreamWriter(fileStream, Encoding.Default)) //將配料寫入檔案中
                    {
                        stream.WriteLine("素材編號,斷面規格,素材長度,切割長度,損耗,零件編號,零件長度");
                        for (int c = 0; c < umPopulation.Count; c++)
                        {
                            stream.WriteLine(umPopulation[c].ToCsvString(profiles[i]));
                        }
                    }
                }
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
    }
}

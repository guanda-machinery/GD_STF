using System;
using System.Collections.Generic;
using System.Linq;

namespace GD_STD.Data.MatchDI
{
    /// <summary>
    /// 指定為使用率配置的分裂組合處理方法
    /// </summary>
    [Serializable]
    public class MultipleUsageRate : UsageRate
    {
        /// <summary>
        /// 配置多種組合適合物件
        /// </summary>
        /// <param name="seed"></param>
        public override void Matching(ref List<Chromosome> seed)
        {
            List<Chromosome> result = new List<Chromosome>();
            for (int i = 0; i < seed.Count; i++)
            {
                List<SinglePart> part = seed[i].Match(seed[seed.Count - 1].Fitness());
                for (int c = 0; c < part?.Count; c++)
                {
                    Chromosome chr = seed[i].Clone(); //有可能要加入的物件
                    chr.Add(part[c].Index); //先將陣列位置加入到物件，方便計算是否有重複組合
                    bool add = true;//控制加入物件的 bool
                    for (int q = 0; q < result.Count; q++) //逐步展開分裂結果
                    {
                        List<double> rLength = result[q].DNALength(); //分裂的物件長度
                        List<double> chrLength = chr.DNALength(); //有可能要加入物件的所有長度
                        rLength.Sort(); //排序長度
                        chrLength.Sort();//排序長度
                        if (!rLength.SequenceEqual(chrLength) && result[q].Length == chr.Length)
                        {
                            add = false;
                            break;
                        }
                    }
                    if (add)
                    {
                        result.Add(chr);
                    }
                    else
                    {
                        add = true;
                    }
                }
            }
            if (result.Count != 0)
            {
                seed.AddRange(result);
                seed.Sort(Population.Compare);
            }
            if (Convergence(seed))
            {
                seed.Sort(Population.Compare);
                seed = new List<Chromosome>() { seed[0] };
                max = 0;
                return;
            }
            else
            {
                Matching(ref seed);
            }
        }
        double max = 0;
        /// <summary>
        /// 收斂各代基因
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public bool Convergence(List<Chromosome> seed)
        {
            double fitness = seed[0].Fitness(); //適應值
            if (max < fitness) //如果記錄的值比較小
            {
                max = fitness; //改變紀錄值
                return false;//不收斂
            }
            else if (max == fitness) //收斂值與適應值相同
            {
                return true;//收斂
            }
            else
            {
                return false;//不收斂
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 指定為使用率配置的處理方法
    /// </summary>
    [Serializable]
    public class UsageRate : IConfiguration
    {
        /// <inheritdoc/>
        public virtual double Fitness(Chromosome chromosome)
        {
            double result = chromosome.UseLength() / chromosome.Length;//使用率
            if (result > 1) //如果使用空間大於長度
            {
                return 0;//評分回傳 0 
            }
            else
            {
                return result;//回傳使用率
            }
        }
        /// <summary>
        /// 配置單一適合物件
        /// </summary>
        /// <param name="pop">配料種子</param>
        public virtual void Matching(ref Population pop)
        {
            for (int i = 0; i < pop.Count; i++) //逐步查看種子是否配料完成
            {
                if (!pop[i].End)//如果尚未結束
                {
                    SinglePart? part = pop[i].SingleMatch(); //選擇單一的合適對象
                    if (part != null)
                    {
                        pop[i].Add(part.Value.Index);//加入到種子基因
                    }
                }
            }
        }
    }
    /// <summary>
    /// 指定為使用率配置的分裂組合處理方法
    /// </summary>
    [Serializable]
    public class MultipleUsageRate : UsageRate
    {
        /// <summary>
        /// 配置多種組合適合物件
        /// </summary>
        /// <param name="pop"></param>
        public override void Matching(ref Population pop)
        {
            pop.Sort(Population.Compare);//排序大到小
            List<Chromosome> result = new List<Chromosome>();
            for (int i = 0; i < pop.Count; i++)
            {
                List<SinglePart> part = pop[i].Match(pop[pop.Count - 1].Fitness());
                for (int c = 0; c < part?.Count; c++)
                {
                    result.Add(pop[i].Clone());
                    result[result.Count - 1].DNA.Add(part[c].Index);
                }
            }
            if (result.Count != 0)
            {
                result.Clear();
                result.AddRange(pop);
                //seed = result;
            }
        }
        double avg = 0;
        public bool Convergence(Population seed)
        {
            double _avg = seed.TotalSurplus() / seed.Count;
            if (_avg != avg)
            {
                avg = _avg;
                return false;
            }
            else
            {
                avg = 0;
                return true;
            }

        }
    }
}

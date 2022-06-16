using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

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
        /// <param name="seed">配料種子</param>
        public virtual void Matching(ref List<Chromosome> seed)
        {
            for (int i = 0; i < seed.Count; i++) //逐步查看種子是否配料完成
            {
                if (!seed[i].End)//如果尚未結束
                {
                    SinglePart? part = seed[i].SingleMatch(); //選擇單一的合適對象
                    if (part != null)
                    {
                        seed[i].Add(part.Value.Index);//加入到種子基因
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
        /// <param name="seed"></param>
        public override void Matching(ref List<Chromosome> seed)
        {

            seed.Sort(Population.Compare);//排序大到小
            List<Chromosome> result = new List<Chromosome>();
            List<Chromosome> convergence = new List<Chromosome>();
             seed.AsParallel().ForAll(el =>
             {
                 if (el.End)
                 {
                     convergence.Add(el);
                 }
             });
            for (int i = 0; i < seed.Count; i++)
            {
                List<SinglePart> part = seed[i].Match(seed[seed.Count - 1].Fitness(), seed);
                for (int c = 0; c < part?.Count; c++)
                {
                    result.Add(seed[i].Clone());
                    result[result.Count - 1].DNA.Add(part[c].Index);
                }
            }
            //if (Convergence(result))
            //{
            //    return;
            //}
            if (result.Count != 0)
            {
                seed = result;
            }
        }

        //public bool Convergence(List<Chromosome> seed)
        //{
        //    double _avg = 0;
        //    seed.ForEach(el => _avg += el.Fitness());
        //    return false;
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 權重評分
    /// </summary>
    public class Weights : UsageRate
    {
        /// <inheritdoc/>
        public override double Fitness(Chromosome chromosome)
        {
            double use = base.Fitness(chromosome);
            if (use > 0)
            {
                return (base.Fitness(chromosome) * 3 + chromosome.DNA.Count * 1) / (3 + 1);//計算權重 損耗比重3 數量比重1
            }
            else
            {
                return use;
            }
        }
    }
}

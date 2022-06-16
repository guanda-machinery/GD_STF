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
            return (base.Fitness(chromosome) * 2 + chromosome.DNA.Count *1) / (2+1);//計算權重 損耗比重2 數量比重1
        }
    }
}

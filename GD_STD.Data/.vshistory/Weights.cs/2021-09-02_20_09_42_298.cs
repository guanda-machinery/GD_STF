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
                return (base.Fitness(chromosome) * 1 + chromosome.DNA.Count * 2) / (1 + 2);
            }
            else
            {
                return use;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data.MatchDI
{
    /// <summary>
    /// 指定為權重配置的處理方法
    /// </summary>
    [Serializable]
    public class Weights : UsageRate
    {
        /// <inheritdoc/>
        public override double Fitness(Chromosome chromosome)
        {
            double use = base.Fitness(chromosome);
            if (use > 0)
            {
                double count = chromosome.DNA.Count * 2d;
                double result = (use * 1 + count) / 3;
                if (!chromosome.Mutation) //如果不是次要長度
                {
                    return result;
                }
                else
                {
                    return result - 0.05;
                }
            }
            else
            {
                return use;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 數量評分
    /// </summary>
    public class Average : IScore
    {
        /// <inheritdoc/>
        public double Fitness(Chromosome chromosome)
        {
            return chromosome.DNA.Count;
        }
    }
}

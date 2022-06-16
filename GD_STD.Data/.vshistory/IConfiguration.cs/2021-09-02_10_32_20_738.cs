using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 基因評分介面
    /// </summary>
    public interface IScore
    {
        /// <summary>
        /// 適合程度
        /// </summary>
        /// <returns></returns>
        double Fitness(Chromosome chromosome);
    }
}

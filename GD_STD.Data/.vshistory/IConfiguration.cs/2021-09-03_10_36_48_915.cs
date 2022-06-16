using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 基因配置介面
    /// </summary>
    public interface IConfiguration
    {
        //void Suitable(bool cycle = false, List<Chromosome> seed, )
        /// <summary>
        /// 適合程度
        /// </summary>
        /// <returns></returns>
        double Fitness(Chromosome chromosome);
    }
}

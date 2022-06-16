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
    public unsafe interface IConfiguration
    {
        /// <summary>
        /// 匹配單一物件 or 匹配多種組合物件
        /// </summary>
        /// <param name="seed"></param>
        void Matching (ref List<Chromosome> seed);
        /// <summary>
        /// 適合程度
        /// </summary>
        /// <returns></returns>
        double Fitness(Chromosome chromosome);
    }
}

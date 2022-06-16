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
        /// 配置單一適合物件，或是帶有分裂素材並找到
        /// </summary>
        /// <param name="cycle"></param>
        /// <param name="seed"></param>
        void Matching (ref List<Chromosome> seed, bool cycle = false);
        /// <summary>
        /// 適合程度
        /// </summary>
        /// <returns></returns>
        double Fitness(Chromosome chromosome);
    }
}

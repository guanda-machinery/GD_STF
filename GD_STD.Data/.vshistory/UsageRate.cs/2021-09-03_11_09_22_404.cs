using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 
    /// </summary>
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
        /// <param name="cycle">控制循環流程</param>
        /// <param name="seed">配料種子</param>
        public unsafe virtual void Matching (ref List<Chromosome> seed, bool cycle)
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
}

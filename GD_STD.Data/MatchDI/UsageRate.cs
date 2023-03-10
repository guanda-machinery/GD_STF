using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace GD_STD.Data.MatchDI
{
    /// <summary>
    /// 指定為使用率配置的處理方法
    /// </summary>
    [Serializable]
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
                if (!chromosome.Mutation) //如果不是次要長度
                {
                    return result;//回傳使用率
                }
                else
                {
                    return result -0.05;//回傳使用率
                }
            }
        }
        /// <summary>
        /// 配置單一適合物件
        /// </summary>
        /// <param name="seed">配料種子</param>
        public virtual void Matching(ref List<Chromosome> seed)
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

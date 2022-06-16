using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace GD_STD.Data
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
                return result;//回傳使用率
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
    /// <summary>
    /// 指定為使用率配置的分裂組合處理方法
    /// </summary>
    [Serializable]
    public class MultipleUsageRate : UsageRate
    {
        /// <summary>
        /// 配置多種組合適合物件
        /// </summary>
        /// <param name="seed"></param>
        public override void Matching(ref List<Chromosome> seed)
        {
            seed.Sort(Population.Compare);//排序大到小
            List<Chromosome> result = new List<Chromosome>();
            for (int i = 0; i < seed.Count; i++)
            {
                List<SinglePart> part = seed[i].Match(seed[seed.Count - 1].Fitness());
                for (int c = 0; c < part?.Count; c++)
                {

                    result.Add(seed[i].Clone());
                    result[result.Count - 1].DNA.Add(part[c].Index);
                }
            }
            if (Convergence(seed))
            {
                return;
            }
            if (result.Count != 0)
            {
                seed = result;
            }
        }
        List<Chromosome> convergence = new List<Chromosome>();
        public bool Convergence(List<Chromosome> seed)
        {
            if (convergence.Count == 0)
            {
                convergence = seed;
            }
        
            if (convergence[0].Fitness() == seed[0].Fitness() && seed.Count > 2000)
            {
                return true;
            }
            else
            {
                convergence = seed;
                return false;
            }
        }
        ///// <summary>
        ///// 配置多種組合適合物件
        ///// </summary>
        ///// <param name="seed"></param>
        //public override void Matching(ref List<Chromosome> seed)
        //{

        //    seed.Sort(Population.Compare);//排序大到小
        //    List<Chromosome> result = new List<Chromosome>(); //分裂結果
        //    for (int i = 0; i < seed.Count; i++) //逐步展開種子
        //    {
        //        List<SinglePart> part = seed[i].Match(seed[seed.Count - 1].Fitness()); //抓取最適合的物件
        //        for (int c = 0; c < part?.Count; c++) //逐步展開最合適的物件
        //        {
        //            Chromosome chr = seed[i].Clone(); //有可能要加入的物件
        //            chr.Add(part[c].Index); //先將陣列位置加入到物件，方便計算是否有重複組合
        //            bool add = true;//控制加入物件的 bool
        //            for (int q = 0; q < result.Count; q++) //逐步展開分裂結果
        //            {
        //                List<double> rLength = result[q].DNALength(); //分裂的物件長度
        //                List<double> chrLength = chr.DNALength(); //有可能要加入物件的所有長度
        //                rLength.Sort(); //排序長度
        //                chrLength.Sort();//排序長度
        //                if (true)
        //                {

        //                }
        //                for (int w = 0; w < rLength.Count; w++) //逐步展開分裂物件的長度
        //                {
        //                    if (chrLength.Contains(rLength[w])) //查看
        //                    {
        //                        add = false;
        //                    }
        //                    else
        //                    {
        //                        add = true;
        //                    }
        //                }
        //                if (!add) //如果不加入物件
        //                    break;//離開迴圈
        //            }
        //            if (add)
        //            {
        //                result.Add(chr);
        //            }
        //            else
        //            {
        //                add = true;
        //            }
        //        }
        //    }
        //    //if (Convergence(result))
        //    //{
        //    //    return;
        //    //}
        //    if (result.Count != 0)//分裂物件如果不等於0
        //    {
        //        seed = result; //分裂物件變成種子
        //    }
        //}

    }
}

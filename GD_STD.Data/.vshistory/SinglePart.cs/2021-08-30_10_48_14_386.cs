using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 單一零件，配料用類型
    /// </summary>
    public class SinglePart
    {
        /// <summary>
        /// 展開物件，變成單一個體
        /// </summary>
        /// <returns></returns>
        public static List<SinglePart> UnfoldPart(SteelPart part)
        {
            List<SinglePart> result = new List<SinglePart>();
            for (int i = 0; i < part.Count; i++)
            {
                result.Add(new SinglePart(part));
            }
            return result;
        }
        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="part"></param>
        public SinglePart(SteelPart part)
        {
            Length = part.Length;
            Profile = part.Profile;
        }
        /// <summary>
        /// 長度
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public string Profile { get; }
    }
}

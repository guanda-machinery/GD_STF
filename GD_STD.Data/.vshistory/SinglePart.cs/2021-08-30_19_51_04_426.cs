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
    [Serializable]
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
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SinglePart part &&
                   Length == part.Length &&
                   Number == part.Number;
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -487355075;
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Number);
            return hashCode;
        }

        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="part"></param>
        public SinglePart(SteelPart part)
        {
            Length = part.Length;
            Number = part.Number;
        }
        /// <summary>
        /// 長度
        /// </summary>
        public double Length;
        /// <summary>
        /// 編號
        /// </summary>
        public string Number;

        public static bool operator ==(SinglePart left, SinglePart right)
        {
            return EqualityComparer<SinglePart>.Default.Equals(left, right);
        }

        public static bool operator !=(SinglePart left, SinglePart right)
        {
            return !(left == right);
        }
    }
}

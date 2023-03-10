using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 單一零件，配料用類型
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack =1)]
    public unsafe struct SinglePart
    {
        /// <summary>
        /// 展開物件，變成單一個體
        /// </summary>
        /// <returns></returns>
        public static List<SinglePart> UnfoldPart(SteelPart part, ref int index)
        {
            List<SinglePart> result = new List<SinglePart>();
            for (int i = 0; i < part.Count; i++)
            {
                result.Add(new SinglePart(part, i));
                index++;
            }
            return result;
        }
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SinglePart part && Length == part.Length;
        }

        public override int GetHashCode()
        {
            return -2130075011 + Length.GetHashCode();
        }

        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="part"></param>
        public SinglePart(SteelPart part, int index)
        {
            Length = part.Length;
            Match = false;
            Index = index;
            for (int i = 0; i < part.Number.Length; i++)
                Number[i] = part.Number[i];


        }
        /// <summary>
        /// 陣列位置
        /// </summary>
        public int Index;
        /// <summary>
        /// 批配料單
        /// </summary>
        public bool Match;
        /// <summary>
        /// 長度
        /// </summary>
        public double Length;
        /// <summary>
        /// 編號
        /// </summary>
        public fixed char Number[20];
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

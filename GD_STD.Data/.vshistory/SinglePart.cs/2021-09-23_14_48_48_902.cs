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
    [StructLayout(LayoutKind.Auto)]
    public unsafe struct SinglePart
    {
        /// <summary>
        /// 展開物件，變成單一個體
        /// </summary>
        /// <param name="part"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<SinglePart> UnfoldPart(SteelPart part, int count = -1)
        {
            List<SinglePart> result = new List<SinglePart>();
            for (int i = 0; i < part.Count; i++)
            {
                result.Add(new SinglePart(part, i));
            }
            return result;
        }
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SinglePart part && Length == part.Length;
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return -2130075011 + Length.GetHashCode();
        }

        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="part"></param>
        /// <param name="index"></param>
        public SinglePart(SteelPart part, int index)
        {
            Length = part.Length;
            Match = false;
            Index = index;
            for (int i = 0; i < part.Number.Length; i++)
                Number[i] = part.Number[i];
        }
        /// <summary>
        /// 變更 index
        /// </summary>
        /// <param name="value"></param>
        public void ChangeIndex(int value)
        {
            this.Index = value;
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
        /// <inheritdoc/>
        public static bool operator ==(SinglePart left, SinglePart right)
        {
            return EqualityComparer<SinglePart>.Default.Equals(left, right);
        }
        /// <inheritdoc/>
        public static bool operator !=(SinglePart left, SinglePart right)
        {
            return !(left == right);
        }
        /// <summary>
        /// 回傳 <see cref="Index"/> 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = string.Empty;
            
            for (int i = 0; i < 20; i++)
            {
                str += Number[i].ToString();
            }
            return $"位置 : {Index} 長度 : {Length} 編號 : {str}";
        }
    }
}

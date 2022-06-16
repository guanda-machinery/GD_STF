using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// Tekla Bom 設定
    /// </summary>
    public class TeklaBomAttribute : Attribute
    {
        /// <summary>
        /// Tekla Bom 轉換物件
        /// </summary>
        /// <param name="index">指定屬性的報表欄位位置</param>
        public TeklaBomAttribute(int index)
        {
            Index = index;
        }
        /// <summary>
        /// 索引位置
        /// </summary>
        public int Index { get; set; }
    }
}

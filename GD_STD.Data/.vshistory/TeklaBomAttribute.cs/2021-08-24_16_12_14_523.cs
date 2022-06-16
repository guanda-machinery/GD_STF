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

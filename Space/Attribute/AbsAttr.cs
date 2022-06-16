using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Attribute
{
    /// <summary>
    /// 一個抽象的自定義屬性檔案
    /// </summary>
    public abstract class AbsAttr
    {
        /// <summary>
        /// 物件ID
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// 物件類型
        /// </summary>
        public OBJETC_TYPE Type { get; set; }
    }
}

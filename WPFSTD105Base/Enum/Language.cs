using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.Enum
{
    /// <summary>
    /// 語言
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// 英文
        /// </summary>
        [Description("English(英文)")]
        en,
        /// <summary>
        /// 泰文
        /// </summary>
        [Description("ไทย(泰文)")]
        th,
        /// <summary>
        /// 越南文
        /// </summary>
        [Description("Tiếng Việt(越南文)")]
        vn,
    }
}

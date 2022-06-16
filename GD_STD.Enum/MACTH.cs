using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MACTH' 的 XML 註解
    public enum MACTH
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MACTH' 的 XML 註解
    {
        /// <summary>
        /// 全部物件配料
        /// </summary>
        [Description("全部")]
        ALL,
        /// <summary>
        /// 部分
        /// </summary>
        [Description("部分")]
        SECTION,
        /// <summary>
        /// 不需要配料
        /// </summary>
        [Description("取消配料")]
        NO
    }
}

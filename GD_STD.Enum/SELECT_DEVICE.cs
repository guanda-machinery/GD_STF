using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    /// <summary>
    /// 選擇料架設備
    /// </summary>
    public enum SELECT_DEVICE : short
    {
        /// <summary>
        /// 不選擇
        /// </summary>
        NULL,
        /// <summary>
        /// 出口
        /// </summary>
        EXPORT,
        /// <summary>
        /// 入口
        /// </summary>
        ENTRANCE,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    /// <summary>
    /// 移動料架區塊
    /// </summary>
    public enum GROUP_DEVICE
    {
        /// <summary>
        /// 無狀態。沒有購買設備
        /// </summary>
        NULL = 0,
        /// <summary>
        /// 場內測試用
        /// </summary>
        TEST = 3,
        /// <summary>
        /// 前。離機台最近的4組移動料架
        /// </summary>
        FRONT = 4,
        /// <summary>
        /// 全部。共八組移動料架
        /// </summary>
        ALL = 8,
        /// <summary>
        /// 後。離機台最遠的4組移動料架
        /// </summary>
        REAR,
    }
}

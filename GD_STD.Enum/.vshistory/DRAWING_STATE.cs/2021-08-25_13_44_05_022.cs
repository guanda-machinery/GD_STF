using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    /// <summary>
    /// 圖紙狀態
    /// </summary>
    [Flags]
    public enum DRAWING_STATE
    {
        /// <summary>
        /// 沒有任何通知
        /// </summary>
        NULL = 0b_0000_0000,
        /// <summary>
        /// 新增數量
        /// </summary>
        INCREASE_COUNT = 0b_0000_0001,
        /// <summary>
        /// 減少數量
        /// </summary>
        REDUCE_COUNT = 0b_0000_0010,
        /// <summary>
        /// 圖面變更
        /// </summary>
        CHANGE = 0b_0000_0100,
        /// <summary>
        /// 新建圖面
        /// </summary>
        NEW = 0b_0000_1000,
    }
}

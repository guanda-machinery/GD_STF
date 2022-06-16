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
    public enum DRAWING_STATE
    {
        /// <summary>
        /// 沒有任何通知
        /// </summary>
        NULL = 0,
        /// <summary>
        /// 新增數量
        /// </summary>
        INCREASE_COUNT = 1,
        /// <summary>
        /// 減少數量
        /// </summary>
        REDUCE_COUNT = 2,
        /// <summary>
        /// 圖面變更
        /// </summary>
        CHANGE = 3,
        /// <summary>
        /// 新建圖面
        /// </summary>
        NEW = 4,
        /// <summary>
        /// 刪除物件
        /// </summary>
        DELETE = 5,
    }
}

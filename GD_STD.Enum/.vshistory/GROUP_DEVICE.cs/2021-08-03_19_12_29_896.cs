﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    //API文件: 新增enum
    /// <summary>
    /// 移動料架區塊
    /// </summary>
    public enum GROUP_DEVICE
    {
        /// <summary>
        /// 無狀態
        /// </summary>
        /// <remarks>
        /// 沒有購買設備
        /// </remarks>
        NULL = 0,
        /// <summary>
        /// 場內測試用
        /// </summary>
        TEST = 3,
        /// <summary>
        /// 前。
        /// </summary>
        /// <remarks>
        /// 離機台最近的4組移動料架
        /// </remarks>
        FRONT = 4,
        /// <summary>
        /// 全部
        /// </summary>
        /// <remarks>
        /// 共八組移動料架
        /// </remarks>
        ALL = 8,
        /// <summary>
        /// 前。
        /// </summary>
        /// <remarks>
        /// 離機台最遠的4組移動料架
        /// </remarks>
        REAR,
    }
}

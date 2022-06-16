using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STF
{
    /// <summary>
    /// 面板鑰匙孔
    /// </summary>
    /// <remarks>
    /// LOCK 人機介面會上鎖
    /// AUTO 自動加工
    /// MANUAL 可手動操作
    /// </remarks>
    public enum KEY_HOLE
    {
        /// <summary>
        /// 畫面上鎖
        /// </summary>
        LOCK,
        /// <summary>
        /// 自動加工
        /// </summary>
        AUTO,
        /// <summary>
        /// 手動操作
        /// </summary>
        MANUAL
    }
}
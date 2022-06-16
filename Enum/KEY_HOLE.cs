using System;

namespace GD_STD.Enum
{
    /// <summary>
    /// 面板鑰匙孔
    /// </summary>
    /// <remarks>
    /// LOCK 人機介面會上鎖
    /// AUTO 自動加工
    /// MANUAL 可手動操作
    /// </remarks>
    public enum KEY_HOLE : Int16
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
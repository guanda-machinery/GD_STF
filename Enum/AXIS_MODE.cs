using System;

namespace GD_STD.Enum
{
    /// <summary>
    /// 鑽頭的工作模式
    /// </summary>
    public enum AXIS_MODE : Int16
    {
        /// <summary>
        /// 鑽穿
        /// </summary>
        PIERCE,
        /// <summary>
        /// 打點
        /// </summary>
        POINT,
        /// <summary>
        /// 畫線
        /// </summary>
        LINE,
        /// <summary>
        /// 畫弧
        /// </summary>
        Arc,
        /// <summary>
        /// 畫圓
        /// </summary>
        Round,
    }
}
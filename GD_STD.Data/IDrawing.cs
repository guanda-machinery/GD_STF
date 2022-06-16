using GD_STD.Enum;
using System;

namespace GD_STD.Data
{
    /// <summary>
    /// 圖紙介面
    /// </summary>
    public interface IDrawing
    {
        /// <summary>
        /// 建立日期
        /// </summary>
        DateTime Creation { get; }
        /// <summary>
        /// 修改日期
        /// </summary>
        DateTime Revise { get; set; }
        /// <summary>
        /// 圖面狀態
        /// </summary>
        DRAWING_STATE State { get; set; }
    }
}
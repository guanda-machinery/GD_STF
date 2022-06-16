
using System;
using System.ComponentModel;

namespace GD_STD.Enum
{
    /// <summary>
    /// 軸向選擇
    /// </summary>
    public enum AXIS_SELECTED : Int16
    {
        /// <summary>
        /// 左軸
        /// </summary>
        [Description("左軸")]
        Left,
        /// <summary>
        /// 中軸
        /// </summary>
        [Description("中軸")]
        Middle,
        /// <summary>
        /// 右軸
        /// </summary>
        [Description("右軸")]
        Right,
    }
}

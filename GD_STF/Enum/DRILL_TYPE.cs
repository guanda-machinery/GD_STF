using System;
using System.ComponentModel;

namespace GD_STD.Enum
{
    /// <summary>
    /// 鑽頭類型
    /// </summary>
    public enum DRILL_TYPE : Int16
    {
        /// <summary>
        /// 鑽孔鑽頭
        /// </summary>
        [Description("鑽孔")]
        DRILL,
        /// <summary>
        /// 繪畫線段的鑽頭
        /// </summary>
        [Description("畫線")]
        PAINTED,
        /// <summary>
        /// 攻牙鑽頭
        /// </summary>
        [Description("攻牙")]
        TAPPING,
        /// <summary>
        /// 洗刀
        /// </summary>
        [Description("洗刀")]
        WASH
    }
}
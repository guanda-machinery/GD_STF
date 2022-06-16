using System;
using System.ComponentModel;

namespace GD_STD.Enum
{
    /// <summary>
    /// 主零件類型
    /// </summary>
    
    public enum OBJETC_TYPE
    {
        /// <summary>
        /// RH型鋼
        /// </summary>
        [Description("RH型鋼")]
        RH,
        /// <summary>
        /// 曹鐵
        /// </summary>
        [Description("槽鐵")]
        CH,
        /// <summary>
        /// L鐵
        /// </summary>
        [Description("角鐵")]
        L,
        /// <summary>
        /// 方型管
        /// </summary>
        [Description("方管")]
        BOX,
        /// <summary>
        /// BH型鋼
        /// </summary>
        [Description("BH型鋼")]
        BH,
        /// <summary>
        /// 多邊形鈑
        /// </summary>
        PLATE,
        /// <summary>
        /// C型鋼
        /// </summary>
        [Description("C型鋼")]
        C,
        /// <summary>
        /// 圓棒
        /// </summary>
        RB,
        /// <summary>
        /// 扁鐵
        /// </summary>
        FB,
        /// <summary>
        /// 圓管
        /// </summary>
        PIPE,
        /// <summary>
        /// 未知斷面
        /// </summary>
        Unknown,
    }
}

using System;
using System.ComponentModel;

namespace GD_STD.Enum
{
    /// <summary>
    /// 鑽頭等級
    /// </summary>
    public enum DRILL_LEVEL : Int16
    {
        /// <summary>
        /// HHS
        /// </summary>
        [Description("HHS")]
        LEVEL1,
        /// <summary>
        /// HHS油孔
        /// </summary>
        [Description("HHS油孔")]
        LEVEL2,
        /// <summary>
        /// 鎢化鋼刀片
        /// </summary>
        [Description("鎢化鋼刀片")]
        LEVEL3,
        /// <summary>
        /// 鎢化鋼刀片+
        /// </summary>
        [Description("鎢化鋼刀片+")]
        LEVEL4,
    }
}

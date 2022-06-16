using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STF
{
    /// <summary>
    /// 鑽頭類型
    /// </summary>
    public enum DRILL_TYPE 
    {
        /// <summary>
        /// 鑽孔鑽頭
        /// </summary>
        DRILL,
        /// <summary>
        /// 繪畫線段的鑽頭
        /// </summary>
        PAINTED,
        /// <remarks>攻牙鑽頭</remarks>
        TAPPING,
        /// <remarks>洗刀</remarks>
        WASH
    }
}
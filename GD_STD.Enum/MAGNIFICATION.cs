using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    /// <summary>
    /// 放大倍率
    /// </summary>
    public enum MAGNIFICATION : Int16
    {
        /// <summary>
        /// 等級1
        /// </summary>
        [Description("LEVLE1")]
        LEVLE1,
        /// <summary>
        /// 等級2
        /// </summary>
        [Description("LEVLE2")]
        LEVLE2,
        /// <summary>
        /// 等級3
        /// </summary>
        [Description("LEVLE3")]
        LEVLE3
    }
}

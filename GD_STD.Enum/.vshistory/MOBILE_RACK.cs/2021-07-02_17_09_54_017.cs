using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    /// <summary>
    /// 移動料架
    /// </summary>
    public enum MOBILE_RACK : Int16
    {
        /// <summary>
        /// 靜止
        /// </summary>
        NULL,
        /// <summary>
        /// 往外移動
        /// </summary>
        INSIDE,
        /// <summary>
        /// 往內移動
        /// </summary>
        OUTER,
    }
}

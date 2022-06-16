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
        /// 朝機台方向移或者下降
        /// </summary>
        INSIDE,
        /// <summary>
        /// 朝機台反方向移動或者上升
        /// </summary>
        OUTER,
    }
}

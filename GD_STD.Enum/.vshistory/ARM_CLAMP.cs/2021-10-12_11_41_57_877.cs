using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    /// <summary>
    /// 手臂夾爪
    /// </summary>
    public enum ARM_CLAMP : Int16
    {
        /// <summary>
        /// 水平
        /// </summary>
        [EnumMember(Value = "水平")]
        LEVEL,
        /// <summary>
        /// 垂直
        /// </summary>
        [EnumMember(Value = "垂直")]
        VERTICAL,
    }
}

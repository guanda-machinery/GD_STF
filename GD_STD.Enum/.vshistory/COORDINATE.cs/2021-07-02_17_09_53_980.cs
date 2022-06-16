using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    /// <summary>
    /// 座標選擇
    /// </summary>
    public enum COORDINATE : Int16
    {
        /// <summary>
        /// X軸向
        /// </summary>
        [Description("X軸")]
        X,
        /// <summary>
        /// Z軸向
        /// </summary>
        [Description("Z軸")]
        Z,
        /// <summary>
        /// Y軸向
        /// </summary>
        [Description("Y軸")]
        Y
    }
}

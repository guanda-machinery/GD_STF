using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STF
{
    /// <summary>
    /// 提供二維座標的介面
    /// </summary>
    public interface IAxis2D
    {
        /// <summary>
        /// 座標 X 
        /// </summary>
        double X { get; set; }
        /// <summary>
        /// 座標 Y 
        /// </summary>
        double Y { get; set; }

    }
}
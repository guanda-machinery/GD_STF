using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STD.IBase
{
    /// <summary>
    /// 提供三維座標的介面
    /// </summary>
    public interface IAxis3D : IAxis2D
    {
        /// <summary>
        /// 座標 Z
        /// </summary>
        double Z { get; set; }
    }
}
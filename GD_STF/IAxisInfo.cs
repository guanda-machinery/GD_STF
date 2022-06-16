using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STF
{
    /// <summary>
    /// 單一軸的軸向的資訊
    /// </summary>
    public interface ISingleAxisInfo : IAxis3D
    {
        /// <summary>
        /// 主軸速度
        /// </summary>
        double Rpm { get; set; }
    }
}
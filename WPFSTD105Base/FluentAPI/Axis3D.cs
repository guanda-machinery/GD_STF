using GD_STD.Base;
using GD_STD.Base.Additional;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.FluentAPI
{
    /// <summary>
    /// 三維座標
    /// </summary>
    [Serializable]
    public class Axis3D : IAxis3D
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public Axis3D()
        {

        }
        /// <summary>
        /// 用於轉換效果
        /// </summary>
        /// <param name="axis"></param>
        public Axis3D(IAxis3D axis)
        {
            X = axis.X;
            Y = axis.Y;
            Z = axis.Z;
        }

        /// <inheritdoc/>
        [MVVM("X", false)]
        public double X { get; set; }
        /// <inheritdoc/>
        [MVVM("Y", false)]
        public double Y { get; set; }
        /// <inheritdoc/>
        [MVVM("Z", false)]
        public double Z { get; set; }
    }
}

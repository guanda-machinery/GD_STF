using GD_STD.Base.Additional;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.FluentAPI
{
    /// <summary>
    /// 安全保護距離
    /// </summary>
    [Serializable]
    public class ProtectionDistance :  GD_STD.Base.IProtectionDistance
    {
        /// <inheritdoc/>
        [MVVM("X 向", false)]
        public double X { get; set; }
        /// <inheritdoc/>
        [MVVM("左右軸 Y 向", false)]
        public double LRY { get; set; }
        /// <inheritdoc/>
        [MVVM("中軸 Y 向", false)]
        public double MY { get; set; }
        /// <inheritdoc/>
        [MVVM("槽鐵與方管的 Y 軸保護", false)]
        public double U_And_BOX_Y_Protection_Length { get; set; }
    }
}

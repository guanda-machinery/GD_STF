using GD_STD.Base;
using GD_STD.Base.Additional;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;

namespace WPFSTD105.FluentAPI
{
    /// <summary>
    /// 軸向設定
    /// </summary>
    [Serializable]
    public class AxisSetting : IAxisSetting
    {
        /// <summary>
        /// 目前軸測刀量長的座標
        /// </summary>
        /// <remarks>
        /// 目前軸測刀量長的座標。
        /// <para>
        /// 調整模式 : 工程模式調整。
        /// </para>
        /// </remarks>
        [MVVM("測量刀長座標", true, "測量到長座標，請完整輸入 X,Y,Z")]
        public Axis3D MeasuringPosition { get; set; } = new Axis3D();
        /// <inheritdoc/>
        [MVVM("Z 軸行程總長", false)]
        public double TotalLength { get; set; }
        /// <inheritdoc/>
        [MVVM("扭力停止點", false, "打點時，接觸到物件的扭力。")]
        public short Torque { get; set; }
        /// <inheritdoc/>
        [MVVM("Y軸極限", false)]
        public double YAxisLimit { get; set; }
        /// <inheritdoc/>
        [MVVM("Z 軸原點到活動端距離", false, "Z 軸 0 點 到活動端和固定端接觸的距離，小於則有撞機可能")]
        public double OriginToSideLength { get; set; }
        /// <summary>
        /// 額定電流
        /// </summary>
        [MVVM("主軸額定電流", false)]
        public double ElectricalCurrent { get; set; }
    }
}

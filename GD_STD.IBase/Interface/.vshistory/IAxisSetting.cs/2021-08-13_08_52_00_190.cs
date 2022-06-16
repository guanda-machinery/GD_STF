using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 軸向設定介面
    /// </summary>
    public interface IAxisSetting
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
        Axis3D MeasuringPosition { get; set; }
        /// <summary>
        /// 目前軸 Z 向 0 點到測刀長 Sensor 距離。
        /// </summary>
        double SensorZero { get; set; }
        /// <summary>
        /// 目前軸 Z 軸行程總長
        /// </summary>
        double TotalLength { get; set; }
    }
}
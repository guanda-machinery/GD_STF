using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 軸向設定介面
    /// </summary>
    public interface IAxisSetting
    {
        /// <summary>
        /// 目前軸 Z 軸行程總長
        /// </summary>
        double TotalLength { get; set; }
        /// <summary>
        /// 扭力停止點 (打點用)
        /// </summary>
         short Torque { get; set; }
        /// <summary>
        /// Y軸極限
        /// </summary>
         double YAxisLimit { get; set; }
        /// <summary>
        /// Z 軸 0 點 到活動端 和 固定端 接觸的距離  
        /// 小於則有撞機可能
        /// </summary>
         double OriginToSideLength { get; set; }
    }
}
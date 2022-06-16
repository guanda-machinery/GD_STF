using System.Runtime.InteropServices;

namespace GD_STD.Base
{
    public interface ITraverse
    {
        /// <summary>
        /// 台車長度
        /// </summary>
        double CarLength { get; set; }
        /// <summary>
        /// 台車凸起區域厚度
        /// </summary>
        double CarRaisedLength { get; set; }
        /// <summary>
        /// 台車原點到定位點
        /// </summary>
        double OriginToLocationPoint { get; set; }
        /// <summary>
        /// 台車排序總空間
        /// </summary>
        double SortTotalLength { get; set; }
        /// <summary>
        /// 台車極限總行程
        /// </summary>
        double Limit { get; set; }
        ///// <summary>
        ///// 移動到料與料中間的尻料位置抓中心點的補正
        ///// </summary>
        //double PositiveSortCorrection { get; set; }
        ///// <summary>
        ///// 台車凸點在料與料的中心補正
        ///// </summary>
        //double ToArmSideCorrection { get; set; }
        /// <summary>
        /// 橫移料架電阻尺解析度 0.1,0.5,1.0 / 1 pulse
        /// </summary>
        double Resolution { get; set; }
        /// <summary>
        /// 安全間隙
        /// </summary>
        double SafetyGap { get; set; }
        /// <summary>
        /// 乘載上升的範圍_預設 30mm
        /// </summary>
        /// <remarks>
        /// 台車邊緣跟素材H切齊後要再多支撐面的長度
        /// </remarks>
        double RideRange { get; set; }
        ///// <summary>
        ///// 移動到料與料中間的尻料位置抓中心點的補正(反向)
        ///// </summary>
        //double ReverseSortCorrection { get; set; }
        /// <summary>
        /// 速度 1 補正
        /// </summary>
         double Speed1Correct { get; set; }
        /// <summary>
        /// 速度 2 補正
        /// </summary>
         double Speed2Correct { get; set; }

    }
}
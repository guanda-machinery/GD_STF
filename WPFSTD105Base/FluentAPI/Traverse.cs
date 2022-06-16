using GD_STD.Attribute;
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
    /// 移動料架參數設定
    /// </summary>
    [Serializable]
    public class Traverse : ITraverse
    {
        /// <summary>
        /// 台車長度
        /// </summary>
        [MVVM("臺車長度", false)]
        public double CarLength { get; set; }
        /// <summary>
        /// 台車凸起區域厚度
        /// </summary>
        [MVVM("台車凸起區域厚度", false)]
        public double CarRaisedLength { get; set; }
        /// <summary>
        /// 台車原點到定位點
        /// </summary>
        [MVVM("台車原點到定位點", false)]
        public double OriginToLocationPoint { get; set; }
        /// <summary>
        /// 台車排序總空間
        /// </summary>
        [MVVM("台車排序總空間", false)]
        public double SortTotalLength { get; set; }
        /// <summary>
        /// 台車極限總行程
        /// </summary>
        [MVVM("台車排序總空間", false)]
        public double Limit { get; set; }
        ///// <summary>
        ///// 移動到料與料中間的尻料位置抓中心點的補正
        ///// </summary>
        //[MVVM("排序校正 (正轉)", false)]
        //public double PositiveSortCorrection { get; set; }
        ///// <summary>
        ///// 台車凸點在料與料的中心補正
        ///// </summary>
        //[MVVM("台車凸點在料與料的中心補正", false)]
        //public double ToArmSideCorrection { get; set; }
        /// <summary>
        /// 橫移料架電阻尺解析度 0.1,0.5,1.0 / 1 pulse
        /// </summary>
        [MVVM("橫移料架電阻尺解析度", false)]
        public double Resolution { get; set; }
        /// <summary>
        /// 安全間隙
        /// </summary>
        [MVVM("安全間隙", false)]
        public double SafetyGap { get; set; }
        ///// <summary>
        ///// 移動到料與料中間的尻料位置抓中心點的補正(反向)
        ///// </summary>
        //[MVVM("排序校正 (逆轉)", false)]
        //public double ReverseSortCorrection { get; set; }
        /// <inheritdoc/>
        [MVVM("乘載上升的範圍", false, description: "車邊緣跟素材切齊後要再多支撐面的長度")]
        public double RideRange { get; set; }
        /// <summary>
        /// 速度 1 補正值
        /// </summary>
        [MVVM("速度 1 補正", false, description: "補正停止慣性")]
        public double Speed1Correct { get; set; }
        /// <summary>
        /// 速度 2 補正值
        /// </summary>
        [MVVM("速度 2 補正", false, description: "補正停止慣性")]
        public double Speed2Correct { get; set; }
    }
}

using GD_STD.Base.Additional;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 移動料架參數設定
    /// </summary>
    [DataContract]
    public struct Traverse : ITraverse
    {
        /// <summary>
        /// 台車長度
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double CarLength { get; set; }
        /// <summary>
        /// 台車凸起區域厚度
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double CarRaisedLength { get; set; }
        /// <summary>
        /// 台車原點到定位點
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double OriginToLocationPoint { get; set; }
        /// <summary>
        /// 台車排序總空間
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double SortTotalLength { get; set; }
        /// <summary>
        /// 台車極限總行程
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double Limit { get; set; }
        ///// <summary>
        ///// 移動到料與料中間的尻料位置抓中心點的補正(正向)
        ///// </summary>
        //[MVVM(false)]
        //[DataMember]
        //public double PositiveSortCorrection { get; set; }
        ///// <summary>
        ///// 台車凸點在料與料的中心補正
        ///// </summary>
        //[MVVM(false)]
        //[DataMember]
        //public double ToArmSideCorrection { get; set; }
        /// <summary>
        /// 橫移料架電阻尺解析度 0.1,0.5,1.0 / 1 pulse
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double Resolution { get; set; }
        /// <summary>
        /// 安全間隙
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double SafetyGap { get; set; }
        /// <summary>
        /// 乘載上升的範圍
        /// </summary>
        /// <remarks>
        /// 台車邊緣跟素材H切齊後要再多支撐面的長度
        /// </remarks>
        [MVVM(false)]
        [DataMember]
        public double RideRange { get; set; }
        ///// <summary>
        ///// 移動到料與料中間的尻料位置抓中心點的補正(反向)
        ///// </summary>
        //[MVVM(false)]
        //[DataMember]
        //public double ReverseSortCorrection { get; set; }
        /// <summary>
        /// 速度 1 補正
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double Speed1Correct { get; set; }
        /// <summary>
        /// 速度 2 補正
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double Speed2Correct { get; set; }
    }
}

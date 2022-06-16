using GD_STD.Base.Additional;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 可以規劃擺放移動料架的結構，
    /// Y 或 U 型 
    /// </summary>
    [DataContract]
    public struct ShapeTraverse
    {
        /// <summary>
        /// 台車長度
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double CarLength;
        /// <summary>
        /// 台車凸起區域厚度
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double CarRaisedLength;
        /// <summary>
        /// 台車原點到定位點
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double OriginToLocationPoint;
        /// <summary>
        /// 可排放物件的座標點位 ( 只有 X 軸 )。最大4個
        /// </summary>
        /// <remarks>
        /// 排放順序由電阻尺 0 點開始放置
        /// </remarks>
        [MVVM(false)]
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] ConveyorPosition;
        /// <summary>
        /// 台車極限總行程
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double Limit;
        ///// <summary>
        ///// 移動到料與料中間的尻料位置抓中心點的補正
        ///// </summary>
        //[MVVM(false)]
        //[DataMember]
        //public double PositiveSortCorrection;
        ///// <summary>
        ///// 台車凸點在料與料的中心補正
        ///// </summary>
        //[MVVM(false)]
        //[DataMember]
        //public double ToArmSideCorrection;
        /// <summary>
        /// 橫移料架電阻尺解析度 0.1,0.5,1.0 / 1 pulse
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double Resolution;
        /// <summary>
        /// 安全距離
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double Safety;
        /// <summary>
        /// 安全間隙
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double SafetyGap;
        /// <summary>
        /// 乘載上升的範圍_預設 30mm
        /// </summary>
        /// <remarks>
        /// 台車邊緣跟素材H切齊後要再多支撐面的長度
        /// </remarks>
        [MVVM(false)]
        [DataMember]
        public double RideRange;
        ///// <summary>
        ///// 移動到料與料中間的尻料位置抓中心點的補正(反向)
        ///// </summary>
        //[MVVM(false)]
        //[DataMember]
        //public double ReverseSortCorrection;
        /// <summary>
        /// 速度 1 補正
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double Speed1Correct;
        /// <summary>
        /// 速度 2 補正
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double Speed2Correct;
    }
}

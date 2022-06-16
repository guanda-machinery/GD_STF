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
        [DataMember]
        public double CarLength;
        /// <summary>
        /// 台車凸起區域厚度
        /// </summary>
        [DataMember]
        public double CarRaisedLength;
        /// <summary>
        /// 台車原點到定位點
        /// </summary>
        [DataMember]
        public double OriginToLocationPoint;
        /// <summary>
        /// 可排放物件的座標點位 ( 只有 X 軸 )。最大三個
        /// </summary>
        /// <remarks>
        /// 排放順序由電阻尺 0 點開始放置
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public double[] ConveyorPosition;
        /// <summary>
        /// 台車極限總行程
        /// </summary>
        [DataMember]
        public double Limit;
        /// <summary>
        /// 移動到料與料中間的尻料位置抓中心點的補正
        /// </summary>
        [DataMember]
        public double SortCorrection;
        /// <summary>
        /// 台車凸點在料與料的中心補正
        /// </summary>
        [DataMember]
        public double ToArmSideCorrection;
        /// <summary>
        /// 橫移料架電阻尺解析度 0.1,0.5,1.0 / 1 pulse
        /// </summary>
        [DataMember]
        public double Resolution;
        /// <summary>
        /// 安全距離
        /// </summary>
        [DataMember]
        public double Safety;
        /// <summary>
        /// 安全間隙
        /// </summary>
        [DataMember]
        public double SafetyGap;
    }
}

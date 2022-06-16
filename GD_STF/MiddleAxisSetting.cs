//using System.Runtime.Serialization;

//namespace GD_STD
//{
//    /// <summary>
//    /// 特殊軸設定
//    /// </summary>
//    [DataContract]
//    public struct MiddleAxisSetting : Base.IAxisSetting
//    {
//        /// <inheritdoc/>  
//        [DataMember]
//        public Axis3D MeasuringPosition { get; set; }
//        /// <inheritdoc/>
//        [DataMember]
//        public double SensorZero { get; set; }
//        /// <inheritdoc/>
//        [DataMember]
//        public double TotalLength { get; set; }
//        /// <summary>
//        /// 目前軸去指定借出軸量測的XYZ座標。
//        /// </summary>
//        [DataMember]
//        public Axis3D BorrowMeasuringPosition { get; set; }
//        /// <summary>
//        /// 目前軸去指定借出軸量測的 Z 向 0 點到測刀長 Sensor 距離。
//        /// </summary>
//        [DataMember]
//        public double BorrowSensorZero { get; set; }
//        /// <summary>
//        /// 當前軸 Z 向要往下走多少才能測到雷射範圍內的長度。
//        /// </summary>
//        [DataMember]
//        public double HighSensorZ { get; set; }
//    }
//}

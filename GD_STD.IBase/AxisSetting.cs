using GD_STD.Base;
using GD_STD.Base.Additional;
using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 軸向設定
    /// </summary>
    [DataContract]
    public struct AxisSetting : IAxisSetting
    {
        ///// <summary>
        ///// 建構式
        ///// </summary>
        ///// <param name="axisSetting"></param>
        ///// <param name="axis">測刀量長的座標</param>
        //public AxisSetting(IAxisSetting axisSetting, IAxis3D axis)
        //{
        //    MeasuringPosition = new Axis3D(axis);
        //    TotalLength = axisSetting.TotalLength;
        //    Torque = axisSetting.Torque;
        //    YAxisLimit = axisSetting.YAxisLimit;
        //    OriginToSideLength = axisSetting.OriginToSideLength;

        //}
        /// <summary>
        /// 目前軸測刀量長的座標
        /// </summary>
        /// <remarks>
        /// 目前軸測刀量長的座標。
        /// <para>
        /// 調整模式 : 工程模式調整。
        /// </para>
        /// </remarks>
        [MVVM(true)]
        [DataMember]
        public Axis3D MeasuringPosition { get; set; }

        /// <inheritdoc/>
        [MVVM(false)]
        [DataMember]
        public double TotalLength { get; set; }
        /// <summary>
        /// 扭力停止點 (打點用)
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public short Torque { get; set; }
        /// <summary>
        /// Y軸極限
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double YAxisLimit { get; set; }
        /// <summary>
        /// Z 軸 0 點 到活動端 和 固定端 接觸的距離  
        /// 小於則有撞機可能
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double OriginToSideLength { get; set; }
        /// <summary>
        /// 額定電流
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double ElectricalCurrent { get; set; }
        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}

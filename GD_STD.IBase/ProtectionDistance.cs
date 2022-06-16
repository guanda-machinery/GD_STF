using GD_STD.Base.Additional;
using System.Runtime.Serialization;
namespace GD_STD.Base
{
    /// <summary>
    /// 安全保護距離
    /// </summary>
    [DataContract]
    public struct ProtectionDistance : IProtectionDistance
    {
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="distance"></param>
        public ProtectionDistance(IProtectionDistance distance)
        {
            X = distance.X;
            LRY = distance.LRY;
            MY = distance.MY;
            U_And_BOX_Y_Protection_Length = distance.U_And_BOX_Y_Protection_Length;
        }
        /// <summary>
        /// 不可加工安全間隙 X 向
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double X { get; set; }
        /// <summary>
        /// 左右軸不可加工安全間隙 Y 向翼板 W 邊緣向內抓取
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double LRY { get; set; }
        /// <summary>
        /// 中軸不可加工安全間隙 Y 向上軸為翼板內側起始向內抓取
        /// </summary>
        [MVVM(false)]
        [DataMember]
        public double MY { get; set; }
        [MVVM(false)]
        [DataMember]
        public double U_And_BOX_Y_Protection_Length { get; set; }
    }
}
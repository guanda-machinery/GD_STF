using System.Runtime.Serialization;
namespace GD_STD.Phone
{
    public partial struct MechanicalSetting
    {
        /// <summary>
        /// 安全保護距離
        /// </summary>
        [DataContract]
        public struct ProtectionDistance
        {
            /// <summary>
            /// 不可加工安全間隙 X 向
            /// </summary>
            [DataMember]
            public double X { get; set; }
            /// <summary>
            /// 左右軸不可加工安全間隙 Y 向翼板 W 邊緣向內抓取
            /// </summary>
            [DataMember]
            public double LRY { get; set; }
            /// <summary>
            /// 中軸不可加工安全間隙 Y 向上軸為翼板內側起始向內抓取
            /// </summary>
            [DataMember]
            public double MY { get; set; }
        }
    }
}
using System.Runtime.Serialization;

namespace GD_STD.Base
{
    public interface IProtectionDistance
    {
        /// <summary>
        /// 不可加工安全間隙 X 向
        /// </summary>
        [DataMember]
        double X { get; set; }
        /// <summary>
        /// 左右軸不可加工安全間隙 Y 向翼板 W 邊緣向內抓取
        /// </summary>
        [DataMember]
        double LRY { get; set; }
        /// <summary>
        /// 中軸不可加工安全間隙 Y 向上軸為翼板內側起始向內抓取
        /// </summary>
        [DataMember]
        double MY { get; set; }
        /// <summary>
        /// 槽鐵與方管的 Y 軸保護
        /// </summary>
        [DataMember]
        double U_And_BOX_Y_Protection_Length { get; set; }
    }
}
using GD_STD.Enum;

namespace WPFSTD105.Attribute
{
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IGroupBoltsAttr' 的 XML 註解
    public interface IGroupBoltsAttr
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IGroupBoltsAttr' 的 XML 註解
    {
        /// <summary>
        /// 用戶輸入螺栓 X 向間距
        /// </summary>
        string dX { get; set; }
        /// <summary>
        /// 用戶輸入螺栓 Y 向間距
        /// </summary>
        string dY { get; set; }
        /// <summary>
        /// 起始點的孔位座標類型
        /// </summary>
        START_HOLE StartHole { get; set; }
        /// <summary>
        /// X 向螺栓數量
        /// </summary>
        int xCount { get; set; }
        /// <summary>
        /// Y 向螺栓數量
        /// </summary>
        int yCount { get; set; }
        /// <summary>
        /// 孔群類型
        /// </summary>
        GroupBoltsType groupBoltsType { get; set; }
    }
}
using GD_STD.Enum;

namespace WPFSTD105.Attribute
{
   
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IBoltAttr' 的 XML 註解
    public interface IBoltAttr
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IBoltAttr' 的 XML 註解
    {
        /// <summary>
        /// 螺栓直徑
        /// </summary>
        double Dia { get; set; }
        /// <summary>
        /// 實體面
        /// </summary>
        FACE Face { get; set; }
        /// <summary>
        /// 鑽孔類型
        /// </summary>
        AXIS_MODE Mode { get; set; }
        /// <summary>
        /// 尚未完成的圓柱體高度
        /// </summary>
        double t { get; set; }
        /// <summary>
        /// 絕對X座標
        /// </summary>
        double X { get; set; }
        /// <summary>
        /// 絕對Y座標
        /// </summary>
        double Y { get; set; }
        /// <summary>
        /// 絕對座標Z
        /// </summary>
        double Z { get; set; }
    }
}
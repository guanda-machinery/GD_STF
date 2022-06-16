using GD_STD.Enum;
using WPFSTD105.Model;

namespace WPFSTD105.Attribute
{
    /// <summary>
    /// 鋼構設定訊息
    /// </summary>
    public interface ISteelAttr
    {
        /// <summary>
        /// 構件編號
        /// </summary>
        string AsseNumber { get; set; }
        /// <summary>
        /// 前視圖切割線
        /// </summary>
        CutContour Front { get; }
        /// <summary>
        /// 高度
        /// </summary>
        float H { get; set; }
        /// <summary>
        /// 單位重
        /// </summary>
        float Kg { get; set; }
        /// <summary>
        /// 長度
        /// </summary>
        double Length { get; set; }
        /// <summary>
        /// 材質
        /// </summary>
        MATERIAL Material { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        int Number { get; set; }
        /// <summary>
        /// 零件編號
        /// </summary>
        string PartNumber { get; set; }
        /// <summary>
        /// 前視圖切割點位
        /// </summary>
        CutList PointFront { get; set; }
        /// <summary>
        /// 頂視圖切割點為
        /// </summary>
        CutList PointTop { get; set; }
        /// <summary>
        /// 斷面規格
        /// </summary>
        string Profile { get; set; }
        /// <summary>
        /// 腹板厚度
        /// </summary>
        float t1 { get; set; }
        /// <summary>
        /// 翼板厚度
        /// </summary>
        float t2 { get; set; }
        /// <summary>
        /// 頂視圖切割線
        /// </summary>
        CutContour Top { get; }
        /// <summary>
        /// 寬度
        /// </summary>
        float W { get; set; }
        /// <summary>
        /// 當前物件是主零件
        /// </summary>
        /// <remarks>
        /// 如果當前物件是主零件回傳 true，如果不是則回傳false。
        /// </remarks>
        bool IsMainPart { get;  }
        /// <summary>
        /// Tekla Part ID
        /// </summary>
        string TeklaPartID { get; set; }
        /// <summary>
        /// Tekla Assembly ID
        /// </summary>
        string TeklaAssemblyID { get; set; }
        /// <summary>
        /// 主件編號
        /// </summary>
        string MainPartNumber { get; set; }
    }
}
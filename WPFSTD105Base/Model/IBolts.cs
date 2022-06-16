using WPFSTD105.Attribute;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 螺栓介面
    /// </summary>
    public interface IBolts
    {
        /// <summary>
        /// 螺栓訊息
        /// </summary>
        GroupBoltsAttr Info { get; set; }
    }
}
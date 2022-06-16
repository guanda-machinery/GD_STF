using System.ComponentModel;

namespace GD_STD.Enum
{
    /// <summary>
    /// 實體面的方向
    /// </summary>
    public enum FACE
    {
        /// <summary>
        /// 頂面
        /// </summary>
        /// <remarks>
        /// 中軸
        /// </remarks>
        [Description("頂面")]
        TOP,
        /// <summary>
        /// 前面
        /// </summary>
        /// <remarks>
        /// 右軸
        /// </remarks>
        [Description("前面")]
        FRONT,
        /// <summary>
        /// 後面
        /// </summary>
        /// <remarks>
        /// 左軸
        /// </remarks>
        [Description("後面")]
        BACK
    }
}

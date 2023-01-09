using System.ComponentModel;

namespace GD_STD.Enum
{
    /// <summary>
    /// 孔群種類
    /// </summary>
    public enum GroupBoltsType
    {
        /// <summary>
        /// 矩形
        /// </summary>
        [Description("矩形")]
        Rectangle,
        /// <summary>
        /// 錯位向左
        /// </summary>
        [Description("錯位向左")]
        DisalignmentLeft,
        /// <summary>
        /// 錯位向右
        /// </summary>
        [Description("錯位向右")]
        DisalignmentRight,
        /// <summary>
        /// 左斜孔(左上右下)
        /// </summary>
        [Description("左斜孔")]
        HypotenuseLeft,
        /// <summary>
        /// 右斜孔(左下右上)
        /// </summary>
        [Description("右斜孔")]
        HypotenuseRight,
    }
}

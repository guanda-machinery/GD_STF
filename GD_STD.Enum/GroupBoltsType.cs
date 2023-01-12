using DevExpress.Mvvm.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using System.Xml.Linq;

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
        [Display(Name = "矩形", Description = "Rectangle")]
        Rectangle,
        /// <summary>
        /// 錯位向左
        /// </summary>
        //[Description("錯位向左")]
        [Display(Name = "錯位向左", Description = "DisalignmentLeft")]
        DisalignmentLeft,
        /// <summary>
        /// 錯位向右
        /// </summary>
        //[Description("錯位向右")]
        [Display(Name = "錯位向右", Description = "DisalignmentRight")]
        DisalignmentRight,
        /// <summary>
        /// 左斜孔(左上右下)
        /// </summary>
        //[Description("左斜孔")]
        [Display(Name = "左斜孔", Description = "HypotenuseLeft")]
        HypotenuseLeft,
        /// <summary>
        /// 右斜孔(左下右上)
        /// </summary>
        [Display(Name = "右斜孔", Description = "HypotenuseRight")]
        HypotenuseRight,
    }

}



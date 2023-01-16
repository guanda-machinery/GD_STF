using DevExpress.Mvvm.DataAnnotations;
using System;
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
        [Image(imageUri: (@"pack://application:,,,/GD_STD.Enum;component/ImageSVG/Drill_Rectangle.svg")), Display(Name = "矩形", Description = "常規加工")]
        Rectangle,
        /// <summary>
        /// 錯位向左
        /// </summary>
        //[Description("錯位向左")]
        [Image(imageUri: (@"pack://application:,,,/GD_STD.Enum;component/ImageSVG/Drill_DisalignmentLeft.svg")), Display(Name = "鑽孔(錯位A)", Description = "錯位向左")]
        DisalignmentLeft,
        /// <summary>
        /// 錯位向右
        /// </summary>
        //[Description("錯位向右")]
      [Image(imageUri: (@"pack://application:,,,/GD_STD.Enum;component/ImageSVG/Drill_DisalignmentRight.svg")), Display(Name = "鑽孔(錯位B)", Description = "錯位向右")]
       DisalignmentRight,
        /// <summary>
        /// 左斜孔(左上右下)
        /// </summary>
        //[Description("左斜孔")]
        [Image(imageUri: (@"pack://application:,,,/GD_STD.Enum;component/ImageSVG/Drill_HypotenuseLeft.svg")), Display(Name = "左斜孔", Description = "左上右下")]
        HypotenuseLeft,
        /// <summary>
        /// 右斜孔(左下右上)
        /// </summary>
        [Image(imageUri: (@"pack://application:,,,/GD_STD.Enum;component/ImageSVG/Drill_HypotenuseRight.svg")), Display(Name = "右斜孔", Description = "左下右上")]
        HypotenuseRight,
    }

}



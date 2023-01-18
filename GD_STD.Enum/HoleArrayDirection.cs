using DevExpress.Mvvm.DataAnnotations;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GD_STD.Enum
{
    /// <summary>
    /// 加工孔產生方向
    /// </summary>
    public enum ArrayDirection
    {
        /// <summary>
        /// 由左至右
        /// </summary>
        [Image(@"pack://application:,,,/GD_STD.Enum;component/ImageSVG/ArrowLtoR.svg"), Display(Name = "左至右", Description = "由左至右")]
        Left_To_Right,
        /// <summary>
        /// 由右至左
        /// </summary>
        [Image(@"pack://application:,,,/GD_STD.Enum;component/ImageSVG/ArrowRtoL.svg"), Display(Name = "右至左", Description = "由右至左")]
        Right_To_Left
    }
}

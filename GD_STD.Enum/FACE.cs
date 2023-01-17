using DevExpress.Mvvm.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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
        [Image(imageUri: @"pack://application:,,,/GD_STD.Enum;component/ImageSVG/SelectPlate_Front.svg"), Display(Name = "頂面", Description = "腹板")]
        TOP,
        /// <summary>
        /// 前面
        /// </summary>
        /// <remarks>
        /// 右軸
        /// </remarks>
        [Image(imageUri: @"pack://application:,,,/GD_STD.Enum;component/ImageSVG/SelectPlate_Top.svg"), Display(Name = "上面", Description = "右翼板")]
        FRONT,
        /// <summary>
        /// 後面
        /// </summary>
        /// <remarks>
        /// 左軸
        /// </remarks>
        [Image(imageUri: @"pack://application:,,,/GD_STD.Enum;component/ImageSVG/SelectPlate_Bottom.svg"), Display(Name = "下面", Description = "左翼板")]
        BACK,
        /// <summary>           
        /// 翼板同時選取
        /// </summary>
        [Image(imageUri: @"pack://application:,,,/GD_STD.Enum;component/ImageSVG/SelectPlate_Top_Bottom.svg"), Display(Name = "上下面", Description = "左右翼板")]
        FRONTandBack,















    }
}

using DevExpress.Mvvm.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GD_STD.Enum
{
    /// <summary>
    /// 起始點的孔位座標
    /// </summary>
    public enum START_HOLE
    {      
        /// <summary>  
        /// 起始點
        /// </summary>
        [Image(), Display(Name = "起始"), Description("起始")]
        START,
        /// <summary>
        /// 中間
        /// </summary>
        [Image(), Display(Name = "中間"), Description("中間")]
        MIDDLE,

    }
}

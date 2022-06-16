using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 鋼構輪廓
    /// </summary>
    public interface ISteelProfile
    {
        /// <summary>
        /// 高度
        /// </summary>
        float H { get; set; }
        /// <summary>
        /// 斷面規格名稱
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
        /// 寬度
        /// </summary>
        float W { get; set; }
        /// <summary>
        /// 物件類型
        /// </summary>
        OBJETC_TYPE Type { get; set; }
        /// <summary>
        /// 材質
        /// </summary>
        string Material { get; set; }
    }
}

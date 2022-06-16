using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GD_STD.IBase
{
    /// <summary>
    /// 各類型剛的斷面輪廓資訊
    /// </summary>
    /// <remarks></remarks>
    public interface IProfile
    {
        /// <summary>
        /// 高度
        /// </summary>
        float H { get; set; }
        /// <summary>
        /// 寬度
        /// </summary>
        float W { get; set; }
        /// <summary>
        /// 腹板厚度
        /// </summary>
        /// <remarks>腹板厚度</remarks>
        float t1 { get; set; }
        /// <summary>
        /// 翼板厚度
        /// </summary>
        float t2 { get; set; }
    }
}
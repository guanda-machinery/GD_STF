using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 矩形範圍
    /// </summary>
    public interface IRectangle
    {
        /// <summary>
        /// 高度
        /// </summary>
        [DataMember]
        float H { get; set; }
        /// <summary>
        /// 寬度
        /// </summary>
        [DataMember]
        float W { get; set; }
        /// <summary>
        /// 長度
        /// </summary>
        [DataMember]
        double Length { get; set; }
    }
}

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
    public interface IRectangle : IWeight
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

        /// <summary>
        /// 數量
        /// </summary>
        [DataMember]
        int Count { get; set; }
        /// <summary>
        /// 單件面積
        /// </summary>
        double UnitArea { get; set; }
        /// <summary>
        /// 總和面積
        /// </summary>
        double TotalArea { get; }
    }
}

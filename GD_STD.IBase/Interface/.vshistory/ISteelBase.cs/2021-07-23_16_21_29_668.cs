using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 鋼構通用參數
    /// </summary>
    public interface ISteelBase
    {
        /// <summary>
        /// 構件編號
        /// </summary>
        string AsseNumber { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        float H { get; set; }
        /// <summary>
        /// 單位重
        /// </summary>
        float Kg { get; set; }
        /// <summary>
        /// 長度
        /// </summary>
        double Length { get; set; }
        /// <summary>
        /// 材質
        /// </summary>
        MATERIAL Material { get; set; }
        ///// <summary>
        ///// 數量
        ///// </summary>
        //int Number { get; set; }
        /// <summary>
        /// 零件編號
        /// </summary>
        string PartNumber { get; set; }
        /// <summary>
        /// 斷面規格
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



#pragma warning disable CS1591 // 遺漏公用可見類型或成員的 XML 註解
        string GUID { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員的 XML 註解

    }
}

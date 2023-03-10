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
    public interface ISteelBase : IModelObjectBase, IProfile
    {
        /// <summary>
        /// 構件編號
        /// </summary>
        string AsseNumber { get; set; }
        /// <summary>
        /// 單位重
        /// </summary>MainPartNumber
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
        ///// <summary>
        ///// 取得零件編號
        ///// </summary>
        ///// <returns><see cref="PartNumbers"/> 所有字串組合</returns>
        //string GetPartNumber();
        ///// <summary>
        ///// 取得零件編號
        ///// </summary>
        ///// <returns><see cref="AsseNumbers"/> 所有字串組合</returns>
        //string GetAsseNumber();
        ///// <summary>
        ///// 當前物件是主零件
        ///// </summary>
        ///// <remarks>
        ///// 如果當前物件是主零件回傳 true，如果不是則回傳false。
        ///// </remarks>
        //bool IsMainPart { get; }
        ///// <summary>
        ///// Tekla Part ID
        ///// </summary>
        //string TeklaPartID { get; set; }
        ///// <summary>
        ///// Tekla Assembly ID
        ///// </summary>
        //string TeklaAssemblyID { get; set; }
        ///// <summary>
        ///// 主件編號
        ///// </summary>
        //string MainPartNumber { get; set; }
    }
}

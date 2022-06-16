using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 物件設定介面
    /// </summary>
    public interface IModelObjectBase
    {
        /// <summary>
        /// 物件ID
        /// </summary>
        Guid? GUID { get; set; }
        ///// <summary>
        ///// 物件類型
        ///// </summary>
        //OBJETC_TYPE Type { get; set; }
    }
}

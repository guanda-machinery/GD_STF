using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STD.Data
{
    /// <summary>
    /// 上一層物件資訊
    /// </summary>
    public interface IUpper
    {
        /// <summary>
        /// 上一層索引值
        /// </summary>
        int UpperID { get; set; }
    }
}
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFWindowsBase;

namespace GD_STD.Data
{
    /// <summary>
    /// 報表屬性
    /// </summary>
    [Serializable]
    public class BomProperty : BaseViewModel
    {
        /// <summary>
        /// 斷面規格類型
        /// </summary>
        public OBJECT_TYPE Type { get; set; }

        /// <summary>
        /// 採購
        /// </summary>
        /// <remarks>
        /// 需要採購回傳 true，不需要則回傳 false。
        /// </remarks>
        public bool Purchase { get; set; }
        /// <summary>
        /// 加工件
        /// </summary>
        /// <remarks>
        /// 需要加工回傳 true，不需要則回傳 false。
        /// </remarks>
        public bool Working { get; set; }

    }
}
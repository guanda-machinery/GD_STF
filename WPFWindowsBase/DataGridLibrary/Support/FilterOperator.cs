using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase.DataGridLibrary.Support
{
    /// <summary>
    /// 過濾運算符
    /// </summary>
    public enum FilterOperator
    {
        /// <summary>
        /// 未定義
        /// </summary>
        Undefined,
        /// <summary>
        /// 小於
        /// </summary>
        LessThan,
        /// <summary>
        /// 小於等於
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// 大於
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 大於等於
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// 等於
        /// </summary>
        Equals,
        /// <summary>
        /// 相同
        /// </summary>
        Like,
        /// <summary>
        /// 之間
        /// </summary>
        Between
    }
}

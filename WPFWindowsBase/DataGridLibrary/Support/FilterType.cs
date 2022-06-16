using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase.DataGridLibrary.Support
{
    /// <summary>
    /// 篩選器類型
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// 數字
        /// </summary>
        Numeric,
        /// <summary>
        /// 兩個數值之間
        /// </summary>
        NumericBetween,
        /// <summary>
        /// 文字
        /// </summary>
        Text,
        /// <summary>
        /// 列表
        /// </summary>
        List,
        Boolean,
        /// <summary>
        /// 日期時間
        /// </summary>
        DateTime,
        /// <summary>
        /// 兩個日期時間之間
        /// </summary>
        DateTimeBetween
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    /// <summary>
    /// 手機連接狀態
    /// </summary>
    /// <revisionHistory>
    ///     <revision date="2021-07-23" version="1.0.0.2" author="LogicYeh">
    ///         <list type="bullet">
    ///             <item>新增<see cref="PHONE_SATUS.INSERT_MATCH"/></item>
    ///         </list>
    ///     </revision>
    /// </revisionHistory> 
    public enum PHONE_SATUS : Int16
    {
        /// <summary>
        /// 只允許監控
        /// </summary>
        MONITOR,
        /// <summary>
        /// 手動操作等待 PC 給予回覆
        /// </summary>
        WAIT_MANUAL,
        /// <summary>
        /// 配對料單
        /// </summary>
        WAIT_MATCH,
        /// <summary>
        /// PC 端允許手動操作
        /// </summary>
        MANUAL,
        /// <summary>
        /// PC 允許配對料單
        /// </summary>
        MATCH,
        /// <summary>
        /// PC 允許插單配對
        /// </summary>
        INSERT_MATCH,
        /// <summary>
        /// 拒絕連線
        /// </summary>
        REFUSE,
    }
}

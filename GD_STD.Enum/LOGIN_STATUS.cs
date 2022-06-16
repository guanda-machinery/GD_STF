using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Enum
{
    /// <summary>
    /// 登入狀態
    /// </summary>
    public enum LOGIN_STATUS
    {
        NULL,
        /// <summary>
        /// 驗證碼錯誤
        /// </summary>
        TOKNE_ERROR,
        /// <summary>
        /// 帳號密碼錯誤
        /// </summary>
        Account_Number_ERROR,
        /// <summary>
        /// 伺服器無回應
        /// </summary>
        SERVER_ERROR,
        /// <summary>
        /// 登入成功
        /// </summary>
        SUCCESS
    }
}

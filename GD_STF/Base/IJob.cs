using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STD.IBase
{
    /// <summary>
    /// Codesys工作完成後的訊號
    /// </summary>
    /// <remarks>代表工作完成訊號</remarks>
    public interface IJob
    {
        /// <summary>
        /// 代表著此機械工作座標已經執行完畢
        /// </summary>
        bool Finish { get; set; }
    }
}
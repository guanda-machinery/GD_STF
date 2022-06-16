using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase.DataGridLibrary.Querying
{
    /// <summary>
    /// 過濾事件參數
    /// </summary>
    public class FilteringEventArgs
    {
        public Exception Error { get; set; }
        /// <summary>
        /// 過濾事件參數
        /// </summary>
        /// <param name="ex">錯誤訊息</param>
        public FilteringEventArgs(Exception ex)
        {
            Error = ex;
        }
    }
}

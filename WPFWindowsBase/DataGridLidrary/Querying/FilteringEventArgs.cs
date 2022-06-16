using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWindowsBase
{
    /// <summary>
    /// 過濾事件參數
    /// </summary>
    public class FilteringEventArgs : EventArgs
    {
        /// <summary>
        /// 錯誤收集
        /// </summary>
        public Exception Error { get; private set; }

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

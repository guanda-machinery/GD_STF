using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 非液壓油系統介面
    /// </summary>
    public interface IOill
    {
        /// <summary>
        /// 頻率/次數
        /// </summary>
        short Frequency { get; set; }
    }
}

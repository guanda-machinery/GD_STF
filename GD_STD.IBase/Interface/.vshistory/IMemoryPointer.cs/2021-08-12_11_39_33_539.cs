using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 指針介面
    /// </summary>
    public interface IPCMemoryPointer
    {
        /// <summary>
        /// 取得 Read 指針
        /// </summary>
        /// <returns></returns>
        IntPtr ReadIntPtr();
    }
}

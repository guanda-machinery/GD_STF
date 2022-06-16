using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// PC 寫入記憶體介面
    /// </summary>
    public interface IPCSharedMemory : ISharedMemory
    {
        /// <summary>
        /// 寫入 Codesys 的共享記憶體內
        /// </summary>
        void WriteMemory();
    }
}

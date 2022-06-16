using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Phone
{
    /// <summary>
    /// Phone 共享記憶體介面
    /// </summary>
    public interface ISharedMemory
    {
        /// <summary>
        /// Phone 讀取 Codesys 的共享記憶體
        /// </summary>
        void ReadMemory();
        /// <summary>
        /// Phone 寫入 Codesys 的共享記憶體內
        /// </summary>
        void WriteMemory();
    }
}

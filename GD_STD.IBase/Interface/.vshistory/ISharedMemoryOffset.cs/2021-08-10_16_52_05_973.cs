using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 共享記憶體介面
    /// </summary>
    public interface ISharedMemoryOffset
    {
        /// <summary>
        /// 讀取記憶體
        /// </summary>
        /// <typeparam name="T">從 Unmanaged 記憶體區塊封送處理資料到新配置的指定類型的 Managed 物件</typeparam>
        /// <param name="position">讀取位置</param>
        /// <param name="size">讀取長度</param>
        /// <returns></returns>
        T ReadMemory<T>(long position, int size) where T : struct;
    }
}

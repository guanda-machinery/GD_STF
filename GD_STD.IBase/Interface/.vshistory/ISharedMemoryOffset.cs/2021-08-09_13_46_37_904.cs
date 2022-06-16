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
        /// 讀取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        TResult ReadMemory<T, TResult>(long position, int size);
    }
}

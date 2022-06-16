using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 指針介面
    /// </summary>
    public interface IMemoryPointer
    {
        /// <summary>
        /// 取得 <see cref="MemoryMappedViewAccessor"/> 指針
        /// </summary>
        /// <returns></returns>
        MemoryMappedViewAccessor ReadView();

    }
}

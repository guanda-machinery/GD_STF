using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 記憶體例外狀況
    /// </summary>
    public sealed class MemoryException : Exception
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'MemoryException.MemoryException(string)' 的 XML 註解
        public MemoryException(string message) : base(message)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'MemoryException.MemoryException(string)' 的 XML 註解
        {

        }
    }
}

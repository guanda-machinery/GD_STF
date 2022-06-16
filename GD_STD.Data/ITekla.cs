using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// TEKLA 物件介面
    /// </summary>
    public interface ITekla
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ITekla.IsTekla' 的 XML 註解
        bool IsTekla { get; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ITekla.IsTekla' 的 XML 註解
    }
}

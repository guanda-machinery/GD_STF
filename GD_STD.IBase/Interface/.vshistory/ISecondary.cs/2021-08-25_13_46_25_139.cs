using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 次要物件
    /// </summary>
    public interface ISecondary
    {
        /// <summary>
        /// 上一層
        /// </summary>
        List<int> Father { get; set; }
    }
}

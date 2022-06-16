using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 計算適合程度介面
    /// </summary>
    public interface IScore
    {
        double Fitness();
    }
}

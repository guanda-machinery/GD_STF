using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// 有重量
    /// </summary>
    public interface IWeight
    {
        /// <summary>
        /// 單件重量
        /// </summary>
        [DataMember]
        double UnitWeight { get; set; }
        /// <summary>
        /// 總和重量
        /// </summary>
        [DataMember]
        double TotalWeight { get; }
    }
}

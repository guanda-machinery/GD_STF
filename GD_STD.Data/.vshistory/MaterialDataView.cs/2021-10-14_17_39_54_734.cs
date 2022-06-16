using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 素材組合
    /// </summary>
    [Serializable]
    public class MaterialDataView : IProfile
    {
        /// <summary>
        /// 素材編號
        /// </summary>
        public string MaterialNumber { get; set; }
        /// <inheritdoc/>
        public string Profile { get; set; }
        /// <summary>
        /// 素材長度
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// 損耗
        /// </summary>
        public double Loss { get; set; }
    }
}

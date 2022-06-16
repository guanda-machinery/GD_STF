using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 鋼構材質
    /// </summary>
    [Serializable]
    public class SteelMaterial
    {
        /// <summary>
        /// 材質名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 鈑密度
        /// </summary>
        public double PlateDensity { get; set; }
        /// <summary>
        /// 斷面積密度
        /// </summary>
        public double ProfileDensity { get; set; }
    }
}

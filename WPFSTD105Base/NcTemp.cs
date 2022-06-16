using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;

namespace WPFSTD105
{
    /// <summary>
    /// nc 實體設定檔
    /// </summary>
    [Serializable]
    public class NcTemp
    {
        /// <summary>
        /// 鋼構設定檔
        /// </summary>
        public SteelAttr SteelAttr { get; set; }
        /// <summary>
        /// 螺栓設定
        /// </summary>
        public List<GroupBoltsAttr> GroupBoltsAttrs { get; set; } 
    }
}

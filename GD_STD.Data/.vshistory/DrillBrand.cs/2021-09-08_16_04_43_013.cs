using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 鑽頭品牌
    /// </summary>
    [Serializable]
    public class DrillBrand : WPFWindowsBase.BaseViewModel, Base.IDrillParameter
    {
        /// <summary>
        /// 品牌名稱
        /// </summary>
        public string Name { get; set; }
        /// <inheritdoc/>
        public double Dia { get; set; }
        /// <inheritdoc/>
        public DRILL_TYPE DrillType { get; set; }
        /// <inheritdoc/>
        public float FeedQuantity { get; set; }
        /// <inheritdoc/>
        public double Rpm { get; set; }
        /// <summary>
        /// 切消速度
        /// </summary>
        public double Vc { get; set; }
    }
}

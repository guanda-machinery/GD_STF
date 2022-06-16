using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105
{
    /// <summary>
    /// 鑽頭品牌
    /// </summary>
    [Serializable]
    public class DrillBrand : GD_STD.Base.IDrillParameter
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
        /// 設定檔名稱
        /// </summary>
        public string DataName { get; set; }
        /// <summary>
        /// 切消速度
        /// </summary>
        public double Vc { get; set; }
        /// <summary>
        /// 鑽頭長度
        /// </summary>
        public float DrillLength { get; set; }
        /// <summary>
        /// 刀柄長度
        /// </summary>
        public float HolderLength { get; set; }
    }
    /// <summary>
    /// <see cref="DrillBrand"/>動態集合列表
    /// </summary>
    public class DrillBrands : ObservableCollection<DrillBrand>
    {

    }
}

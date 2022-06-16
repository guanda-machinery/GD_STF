using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 配料設定
    /// </summary>
    [Serializable]
    public class MatchSetting : IMatchSetting
    {
        /// <summary>
        /// 物件 type
        /// </summary>
        public OBJETC_TYPE Type { get; set; }
        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 起始編號
        /// </summary>
        public string StartNumber { get; set; }
        /// <summary>
        /// 主要長度
        /// </summary>
        public List<double> MainLengths { get; set; }
        /// <summary>
        /// 次要長度
        /// </summary>
        public List<double> SecondaryLengths { get; set; }
        /// <summary>
        /// 短料設定(小於等於)
        /// </summary>
        public double ShortMaterial { get; set; }
        /// <inheritdoc/>
        public double StartCut { get; set; }
        /// <inheritdoc/>
        public double EndCut { get; set; }
        /// <inheritdoc/>
        public double Cut { get; set; }

    }
}

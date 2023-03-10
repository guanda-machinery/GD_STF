using GD_STD.Attribute;
using GD_STD.Base;
using GD_STD.Base.Additional;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;

namespace WPFSTD105.FluentAPI
{
    /// <summary>
    /// 下壓夾具設定
    /// </summary>
    [Serializable]
    public class ClampDownSetting :  IFixture, IClampDown
    {
        /// <inheritdoc/>
        [MVVM("入口左側", false)]
        public double EntranceL { get; set; }
        /// <inheritdoc/>
        [MVVM("出口左側", false)]
        public double ExportL { get; set; }
        /// <inheritdoc/>
        [MVVM("入口右側", false)]
        public double EntranceR { get; set; }
        /// <inheritdoc/>
        [MVVM("出口右側", false)]
        public double ExportR { get; set; }
    }
}

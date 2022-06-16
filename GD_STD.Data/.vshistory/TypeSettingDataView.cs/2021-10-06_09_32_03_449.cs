using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 排版設定資料視圖
    /// </summary>
    public class TypeSettingDataView : ITypeSettingPartView
    {
        /// <inheritdoc/>
        public int Count { get; }
        /// <inheritdoc/>
        public DateTime Creation { get; }
        /// <inheritdoc/>
        public string DrawingName { get; set; }
        /// <inheritdoc/>
        public List<int> Father { get; set; }
        /// <inheritdoc/>
        public float H { get; set; }
        /// <inheritdoc/>
        public List<int> ID { get; set; }
        /// <inheritdoc/>
        public double Length { get; set; }
        /// <inheritdoc/>
        public bool Lock { get; set; }
        /// <inheritdoc/>
        public string Material { get; set; }
        /// <inheritdoc/>
        public string Number { get; set; }
        /// <inheritdoc/>
        public string Profile { get; set; }
        /// <inheritdoc/>
        public DateTime Revise { get; set; }
        /// <inheritdoc/>
        public int SortCount { get; set; }
        /// <inheritdoc/>
        public DRAWING_STATE State { get; set; }

    }
}

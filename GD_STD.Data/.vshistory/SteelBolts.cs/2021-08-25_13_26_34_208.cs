using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    public class SteelBolts : Base.IWeight , Base.ISecondary
    {
        [TeklaBom(1)]
        public string Profile { get; set; }
        /// <summary>
        /// 螺栓類型
        /// </summary>
        [TeklaBom(2)]
        public BOLT_TYPE Type { get; set; }
        /// <summary>
        /// 材質
        /// </summary>
        [TeklaBom(3)]
        public string Material { get; set; }
        /// <inheritdoc/>
        [TeklaBom(4)]
        public double UnitWeight { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Count { get; set; }
        /// <inheritdoc/>
        public double TotalWeight { get => UnitWeight * Count; }
        /// <inheritdoc/>
        public List<int> Father { get; set; }
    }
}

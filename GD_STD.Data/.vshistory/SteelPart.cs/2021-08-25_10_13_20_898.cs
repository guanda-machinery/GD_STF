using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STD.Data
{
    /// <summary>
    /// Tekla Part 訊息
    /// </summary>
    public class SteelPart : ITekla, IProfile, IPart
    {
        public SteelPart()
        {
            ID = new List<int>();
        }
        /// <inheritdoc/>
        [TeklaBom(1)]
        public List<int> ID { get; set; }
        [TeklaBom(2)]
        public string Number { get; set; }
        /// <inheritdoc/>
        [TeklaBom(3)]
        public string Profile { get; set; }
        [TeklaBom(4)]
        /// <inheritdoc/>
        public OBJETC_TYPE ProfileType { get; set; }
        [TeklaBom(5)]
        /// <inheritdoc/>
        public double Length { get; set; }
        /// <inheritdoc/>
        [TeklaBom(6)]
        public float H { get; set; }
        /// <inheritdoc/>
        [TeklaBom(7)]
        public float W { get; set; }
        /// <inheritdoc/>
        [TeklaBom(8)]
        public float t1 { get; set; }
        /// <inheritdoc/>
        [TeklaBom(9)]
        public float t2 { get; set; }
        /// <inheritdoc/>
        [TeklaBom(10)]
        public MATERIAL Material { get; set; }
        /// <inheritdoc/>
        [TeklaBom(11)]
        public double UnitWeight { get; set; }
        [TeklaBom(12)]
        public double UnitArea { get; set; }
        /// <inheritdoc/>
        [TeklaBom(13)]
        public string DrawingName { get; set; }
        /// <inheritdoc/>
        public double TotalWeight { get => UnitWeight * Count; }
        /// <inheritdoc/>
        public int Count { get; set; }
        /// <inheritdoc/>

        /// <inheritdoc/>
        public double TotalArea { get => UnitWeight * Count; }

    }
}
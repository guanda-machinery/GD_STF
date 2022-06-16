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
        /// <inheritdoc/>
        public float t1 { get; set; }
        /// <inheritdoc/>
        public float t2 { get; set; }
        /// <inheritdoc/>
        public float H { get; set; }
        /// <inheritdoc/>
        public float W { get; set; }
        /// <inheritdoc/>
        public string DrawingName { get; set; }
        /// <inheritdoc/>
        public double Length { get; set; }
        /// <inheritdoc/>
        public MATERIAL Material { get; set; }
        /// <inheritdoc/>
        public double UnitWeight { get; set; }
        /// <inheritdoc/>
        public double TotalWeight { get => UnitWeight * Count; }
        /// <inheritdoc/>
        public int Count { get; set; }
        /// <inheritdoc/>
        public double UnitArea { get; set; }
        /// <inheritdoc/>
        public double TotalArea { get => UnitWeight * Count; }
   
        
    }
}
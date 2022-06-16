using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STD.Data
{
    public class SteelPart : ITekla, IProfile
    {
        public SteelPart()
        {
            ID = new List<int>();
        }
        /// <inheritdoc/>
        public float t1 { get; set; }
        /// <inheritdoc/>
        public float t2 { get; set; }
        /// <inheritdoc/>
        public float H { get; set; }
        /// <inheritdoc/>
        public float W { get; set; }
        /// <inheritdoc/>
        public List<int> ID { get; set; }
        /// <inheritdoc/>
        public string DrawingName { get; set; }
        /// <inheritdoc/>
        public double Length { get; set; }
        /// <inheritdoc/>
        public MATERIAL Material { get; set; }
    }
}
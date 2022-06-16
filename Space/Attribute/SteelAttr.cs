using GD_STD.Enum;
using GD_STD.IBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Attribute
{
    /// <summary>
    /// 主要鋼構物件自定義資訊
    /// </summary>
    public class SteelAttr : AbsAttr, IPart
    {
        /// <summary>
        /// 素材長度
        /// </summary>
        public double Length { get; set; }
        public float H { get; set; }
        public float W { get; set; }
        public float t1 { get; set; }
        public float t2 { get; set; }
        public MATERIAL Material { get; set; }
        /// <summary>
        /// 零件編號
        /// </summary>
        public string PartNumber { get; set; }
        /// <summary>
        /// 構件編號
        /// </summary>
        public string AsseNumber { get; set; }
        /// <summary>
        /// 翼板弧度
        /// </summary>
        /// <remarks>
        /// 預設是曹鐵斜率 0.09
        /// </remarks>
        public double radian = 0.09;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 零件內加工之總數
    /// </summary>
    public class DrillBolts
    {
        /// <summary>
        /// 加工類型 目前僅只有"孔"
        /// </summary>
        public string WorkType { get; set; } = "孔";
        /// <summary>
        /// 面的方向
        /// </summary>
        public GD_STD.Enum.FACE Face { get; set; }
        /// <summary>
        /// 孔位數
        /// </summary>
        public int DrillHoleCount { get; set; }
        /// <summary>
        /// 孔直徑
        /// </summary>
        public double DrillHoleDiameter { get; set; }
    }
}

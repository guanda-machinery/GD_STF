using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data
{
    /// <summary>
    /// 報表 Part 訊息
    /// </summary>
    [Serializable]
    public class SteelCutSetting 
    {
        /// <summary>
        /// Tekla Part 訊息
        /// </summary>
        public SteelCutSetting()
        {
            GUID = Guid.NewGuid();
            face = FACE.TOP;
            DLX = 0;
            DLY = 0;
            DRX = 0;
            DRY = 0;
            URX = 0;
            URY = 0;
            ULX = 0;
            ULY = 0;
        }
        /// <summary>
        /// 零件GUID
        /// </summary>
        public Guid? GUID { get; set; } 
        /// <summary>
        /// 面
        /// </summary>
        public FACE face { get; set; }
        /// <summary>
        /// 左邊下緣
        /// </summary>
        public double DLX { get; set; }
        /// <summary>
        /// 左邊下緣
        /// </summary>
        public double DLY { get; set; }
        /// <summary>
        /// 右邊下緣
        /// </summary>
        public double DRX { get; set; }
        /// <summary>
        /// 右邊下緣
        /// </summary>
        public double DRY { get; set; }
        /// <summary>
        /// 左邊上緣
        /// </summary>
        public double ULX { get; set; }
        /// <summary>
        /// 左邊上緣
        /// </summary>
        public double ULY { get; set; }
        /// <summary>
        /// 右邊上緣
        /// </summary>
        public double URX { get; set; }
        /// <summary>
        /// 右邊上緣
        /// </summary>
        public double URY { get; set; }



    }
}
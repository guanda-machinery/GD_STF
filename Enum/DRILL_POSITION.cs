using System;

namespace GD_STD.Enum
{
    /// <summary>
    /// 刀庫選項
    /// </summary>
    public enum DRILL_POSITION : Int16
    {
        /// <summary>
        /// 左軸出料口刀庫
        /// </summary>
        /// <remarks>面對加工機出料左邊的軸向</remarks>
        EXPORT_L,
        /// <summary>
        /// 右軸出料口刀庫
        /// </summary>
        /// <remarks>面對加工機出料右邊的軸向</remarks>
        EXPORT_R,
        /// <summary>
        /// 中軸刀庫
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向</remarks>
        MIDDLE,
        /// <summary>
        /// 左軸入料口刀庫
        /// </summary>
        /// <remarks>面對加工機入料左邊的軸向</remarks>
        ENTRANCE_L,
        /// <summary>
        /// 右軸入料口刀庫
        /// </summary>
        /// <remarks>面對加工機入料右邊的軸向</remarks>
        ENTRANCE_R
    }
}

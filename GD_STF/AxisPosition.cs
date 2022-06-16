using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STF
{
    /// <summary>
    /// 目前三支主軸的 X,Y,Z 軸向的目前軸向座標
    /// </summary>
    /// <remarks>Codesys Memory 讀取</remarks>
    public struct AxisInfo
    {
        /// <summary>
        /// 中軸資訊
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向</remarks>
        public SingleAxisInfo Middle { get; set; }

        /// <summary>
        /// 左軸資訊
        /// </summary>
        /// <remarks>面對加工機出料左邊的軸向</remarks>
        public SingleAxisInfo Left { get; set; }

        /// <summary>
        /// 右軸資訊
        /// </summary>
        /// <remarks>面對加工機出料右邊的軸向</remarks>
        public SingleAxisInfo Right { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD
{
    /// <summary>
    /// 代表軸向選擇的操作動作
    /// </summary>
    public struct AxisAction : Base.IAxisAction
    {
        /// <summary>
        /// 主軸旋轉
        /// </summary>
        /// <remarks>
        /// 虛擬或實體面板的按鈕
        /// </remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool AxisRotation { get; set; }
        /// <summary>
        /// 主軸靜止
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool AxisStop { get; set; }
        /// <summary>
        /// 中心出水
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool AxisEffluent { get; set; }
        /// <summary>
        /// 鬆刀
        /// </summary>
        /// <remarks>虛擬或實體面板的按鈕</remarks>
        [DataMember]
        //[MarshalAs(UnmanagedType.I1)]
        public bool AxisLooseKnife { get; set; }
    }
}

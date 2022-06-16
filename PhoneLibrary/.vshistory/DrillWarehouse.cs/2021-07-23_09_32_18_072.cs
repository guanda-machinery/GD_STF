using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Phone
{
    /// <summary>
    /// 刀具庫  
    /// </summary>
    [DataContract]
    [Serializable]
    public struct DrillWarehouse
    {
        /// <summary>
        /// 左軸入料口刀庫，面對加工機入料左邊的軸向 D 刀庫。
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public DrillSetting[] LeftEntrance;
        /// <summary>
        /// 左軸出料口刀庫，面對加工機出料中間的軸向 B 刀庫。
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public DrillSetting[] LeftExport;
        /// <summary>
        /// 中軸刀庫，面對加工機出料中間的軸向 A 刀庫。
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public DrillSetting[] Middle;
        /// <summary>
        /// 右軸入料口刀庫，面對加工機入料右邊的軸向 E 刀庫。
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public DrillSetting[] RightEntrance;
        /// <summary>
        /// 右軸出料口刀庫，面對加工機出料中間的軸向 C 刀庫。
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public DrillSetting[] RightExport;
    }
}

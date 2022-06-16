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
    //API文件:
    /// <summary>
    /// 刀具庫  
    /// </summary>
    /// <revisionHistory>
    ///     <revision date="2021-07-23" version="1.0.0.2" author="LogicYeh">
    ///         <list type="bullet">
    ///             <item>變更數量 <see cref="LeftEntrance"/> 4 </item>
    ///             <item>變更數量 <see cref="LeftExport"/> 4 </item>
    ///             <item>變更數量 <see cref="Middle"/> 5 </item>
    ///             <item>變更數量 <see cref="RightEntrance"/> 4 </item>
    ///             <item>變更數量 <see cref="RightExport"/> 4 </item>
    ///         </list>
    ///     </revision>
    /// </revisionHistory>
    [DataContract]
    [Serializable()]
    public struct DrillWarehouse
    {
        /// <summary>
        /// 初始化刀庫
        /// </summary>
        /// <param name="mecOptional"></param>
        /// <returns></returns>
        public static DrillWarehouse Initialization(Phone.MecOptional mecOptional)
        {
            DrillWarehouse result = new DrillWarehouse();
            
            if (mecOptional.Middle)
                result.Middle = new DrillSetting[5];
            else
                result.Middle = new DrillSetting[1];

            if (mecOptional.LeftExport)
                result.LeftExport = new DrillSetting[4];
            else
                result.LeftExport = new DrillSetting[1];

            if (mecOptional.RightExport)
                result.RightExport = new DrillSetting[4];
            else
                result.RightExport = new DrillSetting[1];

            if (mecOptional.LeftEntrance)
                result.LeftEntrance = new DrillSetting[4];
            else
                result.LeftEntrance = new DrillSetting[0];

            if (mecOptional.RightEntrance)
                result.RightEntrance = new DrillSetting[4];
            else
                result.RightEntrance = new DrillSetting[0];

            return result;
        }
        
        /// <summary>
        /// 左軸入料口刀庫，面對加工機入料左邊的軸向 D 刀庫。
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public DrillSetting[] LeftEntrance;
        /// <summary>
        /// 左軸出料口刀庫，面對加工機出料中間的軸向 B 刀庫。
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public DrillSetting[] LeftExport;
        /// <summary>
        /// 中軸刀庫，面對加工機出料中間的軸向 A 刀庫。
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public DrillSetting[] Middle;
        /// <summary>
        /// 右軸入料口刀庫，面對加工機入料右邊的軸向 E 刀庫。
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public DrillSetting[] RightEntrance;
        /// <summary>
        /// 右軸出料口刀庫，面對加工機出料中間的軸向 C 刀庫。
        /// </summary>
        /// <remarks>指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 10</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public DrillSetting[] RightExport;
    }
}

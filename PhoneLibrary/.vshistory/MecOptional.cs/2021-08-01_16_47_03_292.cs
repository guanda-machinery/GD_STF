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
    /// 機械選配參數
    /// </summary>
    /// <remarks>
    /// <para>PC 寫入/讀取</para> 
    /// <para>Codesys 讀取</para>
    /// <para>Phone 讀取</para>
    /// </remarks>
    public struct MecOptional : IPhoneSharedMemory , IPCSharedMemory
    {
        /// <summary>
        /// 中軸刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機出料中間的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Middle;
        /// <summary>
        /// 左軸出料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機出料左邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool LeftExport;
        /// <summary>
        /// 右軸出料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機出料右邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool RightExport;
        /// <summary>
        /// 左軸入料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機入料左邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool LeftEntrance;
        /// <summary>
        /// 右軸入料口刀庫
        /// </summary>
        /// <remarks>如果有選配面對加工機入料右邊的軸向的刀庫。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool RightEntrance;
        //小霖: 變更邏輯
        /// <summary>
        /// 入口橫移料架選配
        /// </summary>
        /// <remarks>如果有選配橫移料架。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool EntranceTraverse;
        //小霖: 新增欄位
        /// <summary>
        /// 出口橫移料架
        /// </summary>
        /// <remarks>如果有選配橫移料架。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool ExportTraverse;
        /// <summary>
        /// 手臂自動夾料
        /// </summary>
        /// <remarks>如果有選配 手臂自動夾料。回傳 ture，沒有則回傳 false</remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HandAuto;
        //TODO: 尚未處理
        void ISharedMemory.ReadMemory()
        {
            throw new NotImplementedException();
        }

        void IPhoneSharedMemory.WriteMemory()
        {
            throw new NotImplementedException();
        }

        void IPCSharedMemory.WriteMemory()
        {
            throw new NotImplementedException();
        }
    }
}

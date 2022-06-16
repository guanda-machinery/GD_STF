﻿using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.MemoryHelper;
namespace GD_STD
{
    //小霖:
    /// <summary>
    /// 代表三軸追隨座標位置
    /// </summary>
    [DataContract]
    public struct MainAxisLocation : IPCSharedMemory
    {
        /// <summary>
        /// 左軸
        /// </summary>
        [DataMember]
        public Axis3D Left { get; set; }
        /// <summary>
        /// 中軸
        /// </summary>
        [DataMember]
        public Axis3D Middle { get; set; }
        /// <summary>
        /// 右軸
        /// </summary>
        [DataMember]
        public Axis3D Right { get; set; }

        void ISharedMemory.ReadMemory()
        {
            using (var memory = MainAxisLocationMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(MainAxisLocation)), MemoryMappedFileAccess.Read))
            {
                memory.Read<MainAxisLocation>(0, out MainAxisLocation result);
                this = result;
            }
        }

        void IPCSharedMemory.WriteMemory()
        {
            using (var memory = MainAxisLocationMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(MainAxisLocation)), MemoryMappedFileAccess.Write))
            {
                MainAxisLocation value = this;
                memory.Write<PanelButton>(0, ref panelButtom);
            }
        }
    }
}

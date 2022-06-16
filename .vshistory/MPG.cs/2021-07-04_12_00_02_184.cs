using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GD_STD.Base;
using static GD_STD.MemoryHelper;
namespace GD_STD
{
    /// <summary>
    /// 手搖輪
    /// </summary>
    [DataContract]
    public struct MPG: ISharedMemory
    {
        /// <summary>
        /// 軸向選擇
        /// </summary>
        [DataMember]
        public Enum.AXIS_SELECTED AxisSelected { get; set; }
        /// <summary>
        /// 軸向選擇
        /// </summary>
        [DataMember]
        public Enum.COORDINATE Coordinate { get; set; }
        /// <summary>
        /// 放大倍率
        /// </summary>
        [DataMember]
        public Enum.MAGNIFICATION Magnification { get; set; }

        void ISharedMemory.ReadMemory()
        {
            throw new NotImplementedException();
        }

        void ISharedMemory.WriteMemory()
        {
            using (var memory = MPGMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(MPG)), MemoryMappedFileAccess.Write))
            {
                MPG value = this;
                memory.Write<MPG>(0, ref value);
            }
        }
    }
}

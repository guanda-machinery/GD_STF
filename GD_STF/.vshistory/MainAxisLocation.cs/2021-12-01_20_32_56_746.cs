using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
            
        }

        void IPCSharedMemory.WriteMemory()
        {
            
        }
    }
}

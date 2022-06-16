using GD_STD.Base;
using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.MemoryHelper;
namespace GD_STD
{
    /// <summary>
    /// 目前三支主軸的 X,Y,Z 軸向的目前軸向座標，側壓資訊，下壓資訊，材料位置資訊
    /// </summary>
    /// <remarks>Codesys Memory 讀取</remarks>
    [DataContract]
    public struct AxisInfo : ISharedMemory
    {

        /// <summary>
        /// 中軸資訊
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向</remarks>
        [DataMember]
        public SingleAxisInfo Middle { get; set; }

        /// <summary>
        /// 左軸資訊
        /// </summary>
        /// <remarks>面對加工機出料左邊的軸向</remarks>
        [DataMember]
        public SingleAxisInfo Left { get; set; }

        /// <summary>
        /// 右軸資訊
        /// </summary>
        /// <remarks>面對加工機出料右邊的軸向</remarks>
        [DataMember]
        public SingleAxisInfo Right { get; set; }

        /// <summary>
        /// 送料手臂
        /// </summary>
        [DataMember]
        public SingleAxisInfo Hand { get; set; }
        /// <summary>
        /// 下壓夾具
        /// </summary>
        [DataMember]
        public ClampDown ClampDown { get; set; }
        /// <summary>
        /// 側壓夾具
        /// </summary>
        [DataMember]
        public SideClamp SideClamp { get; set; }
        /// <summary>
        /// 材料位置資訊
        /// </summary>
        [DataMember]
        public Material Material { get; set; }
        void ISharedMemory.ReadMemory()
        {
            using (var ax = AxisInfoMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(AxisInfo)), MemoryMappedFileAccess.Read))
            {
                AxisInfo axisInfo;
                ax.Read<AxisInfo>(0, out axisInfo);
                this = axisInfo;
            }
        }
        [Obsolete("AxisInfo 此功能不能寫入", true)]
        void ISharedMemory.WriteMemory()
        {
            using (var ax = AxisInfoMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(AxisInfo)), MemoryMappedFileAccess.Write))
            {
                AxisInfo axisInfo = new AxisInfo();
                ax.Write<AxisInfo>(0, ref axisInfo);
                this = axisInfo;
            }
            //throw new NotImplementedException();
        }
    }
}
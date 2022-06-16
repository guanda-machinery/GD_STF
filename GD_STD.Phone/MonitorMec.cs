using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GD_STD.IBase;
namespace GD_STD.Phone
{
    /// <summary>
    /// 機械監控
    /// </summary>
    /// <remarks>
    /// 記憶體名稱 MonitorMec
    /// <para>PC 寫入/讀取</para> 
    /// <para>Codesys 寫入/讀取</para>
    /// <para>Phone 讀取</para>
    /// </remarks>
    public struct MonitorMec : ISharedMemory
    {
        /// <summary>
        /// 開機累積使用時間
        /// </summary>
        /// <remarks>
        /// 備註 : 單位為秒數
        /// </remarks>
        public uint BootTime { get; set; }
        /// <summary>
        /// 加工累積支數
        /// </summary>
        public uint FinishNumber { get; set; }
        /// <summary>
        /// 加工累積重量
        /// </summary>
        public uint FinishKg { get; set; }
        /// <summary>
        /// 左邊鑽頭磨耗率
        /// </summary>
        public ushort DrillLeft { get; set; }
        /// <summary>
        /// 中間鑽頭磨耗率
        /// </summary>
        public ushort DrillMiddle { get; set; }
        /// <summary>
        /// 右邊鑽頭磨耗率
        /// </summary>
        public ushort DrillRight { get; set; }


        /// <inheritdoc/>
        void ISharedMemory.ReadMemory()
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc/>
        void ISharedMemory.WriteMemory()
        {
            throw new NotImplementedException();
        }
    }
}

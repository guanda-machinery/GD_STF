using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD
{
    /// <summary>
    /// 鋼印資料
    /// </summary>
    [Serializable]
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Stamp
    {
        /// <summary>
        /// 鋼印資料
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="content"></param>
        /// <param name="finish"></param>
        public Stamp(double startX = 0, byte[] content = null, bool finish = false)
        {
            Content = content == null ? new byte[20] : content;
            StartX = startX;
            Finish = finish;
        }
        /// <summary>
        /// 鋼印起始 X 座標
        /// </summary>
        /// <remarks>
        /// 第一個字位置
        /// </remarks>
        [DataMember]
        public double StartX { get; set; }
        /// <summary>
        /// 鋼印內容
        /// </summary>
        /// <remarks>
        /// 指示固定長度陣列中的元素數目或要匯出之字串中的字元數目長度為 20 
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Content;
        /// <summary>
        /// 完成訊號
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Finish;
    }
}

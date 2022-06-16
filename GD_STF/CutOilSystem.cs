using GD_STD.Base;
using System.Runtime.Serialization;

namespace GD_STD
{
    /// <summary>
    /// 切消油系統設定
    /// </summary>
    /// <remarks>Codesys Memory 讀取/寫入</remarks>
    [DataContract]
    public struct CutOilSystem : IOill
    {
        /// <summary>
        /// 頻率/次數
        /// </summary>
        public short Frequency { get; set; }
    }
}

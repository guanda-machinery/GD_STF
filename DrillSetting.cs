using GD_STD.Enum;
using GD_STD.IBase;
using System;
using System.Runtime.Serialization;

namespace GD_STD
{
    /// <summary>
    /// 鑽頭設定參數
    /// <para>可序列化的結構</para>
    /// </summary>
    /// <remarks>備註 : Codesys Memory 讀取/寫入</remarks>
    [Serializable()]
    [DataContract()]
    public struct DrillSetting : IDrillSetting, IOrder
    {
        /// <summary>
        /// 鑽頭直徑
        /// </summary>
        [DataMember]
        public double Dia { get; set; }
        /// <summary>
        /// 鑽頭等級(材質)
        /// </summary>
        [DataMember]
        public DRILL_LEVEL Level { get; set; }
        /// <summary>
        /// 主軸設定轉速
        /// </summary>
        [DataMember]
        public Int32 Rpm { get; set; }
        /// <summary>
        /// 鑽頭類型
        /// </summary>
        [DataMember]
        public DRILL_TYPE DrillType { get; set; }
        /// <summary>
        /// 鑽頭長度
        /// </summary>
        [DataMember]
        public float Length { get; set; }
        /// <summary>
        /// 鑽頭鑽過幾米內要通知換刀
        /// </summary>
        [DataMember]
        public short Limit { get; set; }
        /// <summary>
        /// 走刀量。一般指切削加工中工件或切削工具每鏇轉一周(如車削)或往返一次(稱雙行程，如刨削)時，工件或切削工具的相對移動距離。
        /// </summary>
        [DataMember]
        public float FeedQuantity { get; set; }
        /// <summary>
        /// 刀具位置
        /// <para>如果是放在刀庫內的刀具則回傳 刀具目前號碼 </para>
        /// </summary>
        [DataMember]
        public short Index { get; set; }

        /// <summary>
        /// 是目前使用的刀具。如果是回傳 true ，不是則回傳 false 。
        /// </summary>
        [DataMember]
        public bool IsCurrent { get; set; }
        /// <inheritdoc/>
        [DataMember]
        public Axis3D Position { get; set; }
    }
}
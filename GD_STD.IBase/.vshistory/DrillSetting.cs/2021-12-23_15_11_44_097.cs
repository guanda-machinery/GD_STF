using GD_STD.Enum;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 鑽頭設定參數
    /// <para>可序列化的結構</para>
    /// </summary>
    /// <remarks>備註 : Codesys Memory 讀取/寫入</remarks>
    [Serializable()]
    [DataContract()]
    public struct DrillSetting : IOrder
    {
        /// <summary>
        /// 刀具變更過
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Change;
        /// <summary>
        /// 鑽頭直徑
        /// </summary>
        [DataMember]
        public double Dia { get; set; }
        /// <summary>
        /// 鑽頭類型
        /// </summary>
        [DataMember]
        public DRILL_TYPE DrillType { get; set; }
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
        [MarshalAs(UnmanagedType.I1)]
        public bool IsCurrent;
        /// <summary>
        /// 鑽頭長度
        /// </summary>
        /// <remarks>
        /// 刀具長度不含刀柄
        /// </remarks>
        [DataMember]
        public float Length { get; set; }
        /// <summary>
        /// 刀柄長
        /// </summary>
        [DataMember]
        public float KnifeHandle { get; set; }
        /// <summary>
        /// 總刀長
        /// </summary>
        /// <remarks>
        /// 含刀柄
        /// </remarks>
        [DataMember]
        public float SumLength { get; set; }
        /// <summary>
        /// 鑽頭等級(材質)
        /// </summary>
        [DataMember]
        public DRILL_LEVEL Level { get; set; }
        /// <summary>
        /// 鑽頭鑽過幾米內要通知換刀
        /// </summary>
        [DataMember]
        public short Limit { get; set; }
        /// <summary>
        /// 刀具座標
        /// </summary>
        [DataMember]
        public Axis4D Position { get; set; }
        /// <summary>
        /// 主軸設定轉速
        /// </summary>
        [DataMember]
        public double Rpm { get; set; }
        /// <summary>
        /// 檢知器
        /// </summary>
        /// <remarks>
        /// 有刀具回傳 true，無刀具則回傳 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Sensor;
        /// <summary>
        /// 自動測量刀長失敗。
        /// </summary>
        /// <remarks>
        /// Codesys 測量刀長不在容許誤差內回傳 true，否則 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool AutoError;
        /// <summary>
        /// 測量刀長
        /// </summary>
        /// <remarks>
        /// 需要測量刀長回傳 true，不需要則 false。
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool TestLength;
        /// <summary>
        /// 設定檔案的陣列位置
        /// </summary>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public ushort SettingIndex;
    }
}
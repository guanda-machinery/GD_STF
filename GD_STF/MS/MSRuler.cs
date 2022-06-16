using GD_STD.Attribute;
using System.Runtime.Serialization;

namespace GD_STD.MS
{
    /// <summary>
    /// SQL Server  電阻尺表單
    /// </summary>
    [DataContract]
    [MSTable("MSRuler")]
    public class MSRuler : AbsMS
    {
        /// <summary>
        /// 電阻尺名稱
        /// </summary>
        [MSField("名稱", "Name")]
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 定位值
        /// </summary>
        [MSField("定位值", "Value")]
        [DataMember]
        public float Value { get; set; }
    }
}

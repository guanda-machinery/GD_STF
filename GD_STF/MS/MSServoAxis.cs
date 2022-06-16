using GD_STD.Attribute;
using System.Runtime.Serialization;

namespace GD_STD.MS
{
    /// <summary>
    /// SQL Server  伺服馬達表單
    /// </summary>
    [MSTable("MSServoAxis")]
    [DataContract]
    public class MSServoAxis : AbsMS
    {

        /// <summary>
        /// 軸向名稱
        /// </summary>
        [MSField("名稱", "Name")]
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 扭力值 
        /// </summary>
        [MSField("扭力", "Torque")]
        [DataMember]
        public float Torque { get; set; }
        /// <summary>
        /// 馬達位置
        /// </summary>
        [MSField("馬達位置", "Position")]
        [DataMember]
        public float Position { get; set; }
        /// <summary>
        /// 馬達速度
        /// </summary>
        [MSField("速度", "Speed")]
        [DataMember]
        public float Speed { get; set; }
    }
}

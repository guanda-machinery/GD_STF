using GD_STD.Attribute;
using System.Runtime.Serialization;
namespace GD_STD.MS
{
    /// <summary>
    /// SQL Server  IO 表單
    /// </summary>
    [DataContract]
    [MSTable("MSIO")]
    public sealed class MSIO : AbsMS
    {
        /// <summary>
        /// IO名稱
        /// </summary>
        [MSField("IO名稱", "Name")]
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 規則
        /// </summary>
        [MSField("規則", "Description")]
        [DataMember]
        public string Description { get; set; }
    }
}

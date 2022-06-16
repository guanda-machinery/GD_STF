using System.Runtime.Serialization;

namespace GD_STD.Attribute
{
    /// <summary>
    ///  一個抽象自訂附加屬性
    /// </summary>
    [DataContract]
    public abstract class AbsCustomAttribute : System.Attribute
    {
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public AbsCustomAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
        /// <summary>
        /// 名稱
        /// </summary>
        [DataMember]
        public string Name { get; protected set; }
        /// <summary>
        /// 說明
        /// </summary>
        [DataMember]
        public string Description { get; protected set; }
    }
}

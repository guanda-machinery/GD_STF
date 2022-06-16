using System;
using System.Runtime.Serialization;
namespace GD_STD.Attribute
{
    /// <summary>
    /// Codesys 附加屬性
    /// </summary>
    [DataContract]
    public sealed class CodesysAttribute : AbsCustomAttribute
    {

        /// <summary>
        /// 建立 Codesys 附加屬性
        /// </summary>
        /// <param name="name">附加屬性名稱</param>
        /// <param name="description">附加屬性說明</param>
        public CodesysAttribute(string name, string description = "") : base(name, description)
        {
        }
        /// <summary>
        /// 取得 Codesys 附加屬性名稱
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name">屬性名稱</param>
        /// <returns></returns>
        public static string GetCodesysName(Type type, string name)
        {
            return type.GetAttributeValue(name, (CodesysAttribute att) => att.Name);
        }
        /// <summary>
        /// 取得 Codesys 附加屬性說明
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name">屬性名稱</param>
        /// <returns></returns>
        public static string GetIODescription(Type type, string name)
        {
            return type.GetAttributeValue(name, (CodesysAttribute att) => att.Description);
        }
    }
}

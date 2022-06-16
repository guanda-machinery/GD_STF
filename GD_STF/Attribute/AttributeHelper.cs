using System;
using System.Linq;
using System.Reflection;

namespace GD_STD.Attribute
{
    
/// <summary>
    /// <see cref="System.Attribute"/> 擴展方法
    /// </summary>
    public static class AttributeHelper
    {
        /// <summary>
        /// 取得 Class 附加屬性內容 
        /// </summary>
        /// <example>
        /// 此示例顯示瞭如何調用 <see cref="GetAttributeValue{TAttribute, TValue}(Type, Func{TAttribute, TValue})"/> 方法
        /// <code>
        /// class Program
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         //回傳整個附加屬性
        ///         var a = new MSIO().GetType().GetAttributeValue((AbsCustomAttribute el) => el);
        ///         //回傳附加屬性裡面某個欄位
        ///         var b = new MSIO().GetType().GetAttributeValue((AbsCustomAttribute el) => el.Name);
        ///     }
        /// }
        /// [AbsCustomAttribute("myClass", "myClass Description")]
        /// public sealed class MSIO : AbsMS
        /// {
        ///     public string Field1 { get; set; }
        ///     public string Field2 { get; set; }
        /// }
        /// 
        /// public class AbsCustomAttribute : System.Attribute
        /// {
        ///     public AbsCustomAttribute(string name, string description)
        ///     {
        ///         Name = name;
        ///         Description = description;
        ///     }
        ///     public string Name { get; protected set; }
        ///     public string Description { get; protected set; }
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="TAttribute">標記的屬性檔類型</typeparam>
        /// <typeparam name="TValue">要回傳的值</typeparam>
        /// <param name="type">被標記的 Class type</param>
        /// <param name="valueSelector">要回傳<typeparamref name="TAttribute"/>的內容</param>
        /// <returns><typeparamref name="TValue"/></returns>
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : System.Attribute
        {
            var a = new int().GetType().GetAttributeValue((AbsCustomAttribute el) => el);
            var att = type.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default;
        }
        /// <summary>
        /// 取得 Class.Property 附加屬性內容
        /// </summary>
        /// <example>
        /// 此示例顯示瞭如何調用 <see cref="GetAttributeValue{TAttribute, TValue}(Type, string, Func{TAttribute, TValue})"/> 方法
        /// <code>
        /// class Program
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         //回傳整個附加屬性
        ///         var a = new MSIO().GetType().GetAttributeValue(Field1,(AbsCustomAttribute el) => el);
        ///         //回傳附加屬性裡面某個欄位
        ///         var b = new MSIO().GetType().GetAttributeValue("Field2",(AbsCustomAttribute el) => el.Name);
        ///     }
        /// }
        ///  
        /// public sealed class MSIO : AbsMS
        /// {
        ///     [AbsCustomAttribute("Field1", "Field1 Description")]
        ///     public string Field1 { get; set; }
        ///     [AbsCustomAttribute("Field1", "Field2 Description")]
        ///     public string Field2 { get; set; }
        /// }
        /// 
        /// public class AbsCustomAttribute : System.Attribute
        /// {
        ///     public AbsCustomAttribute(string name, string description)
        ///     {
        ///         Name = name;
        ///         Description = description;
        ///     }
        ///     public string Name { get; protected set; }
        ///     public string Description { get; protected set; }
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="TAttribute">標記的屬性檔類型</typeparam>
        /// <typeparam name="TValue">要回傳的值</typeparam>
        /// <param name="type">Class type</param>
        /// <param name="name">Property Name</param>
        /// <param name="valueSelector">要回傳<typeparamref name="TAttribute"/>的內容</param>
        /// <returns><typeparamref name="TValue"/></returns>
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, string name, Func<TAttribute, TValue> valueSelector) where TAttribute : System.Attribute
        {
            var property = type.GetProperty(name);
            var attribute = (TAttribute)property.GetCustomAttribute(typeof(TAttribute));
            if (attribute != null)
            {
                return valueSelector(attribute);
            }
            return default;
        }
    }
}

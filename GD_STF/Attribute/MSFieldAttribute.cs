using System;
using System.Runtime.Serialization;

namespace GD_STD.Attribute
{
    /// <summary>
    /// 代表 MSSQL 對應的欄位名稱
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [DataContract]
    public sealed class MSFieldAttribute : AbsCustomAttribute
    {
        /// <summary>
        /// 附加 MSSQL 對應的欄位名稱
        /// </summary>
        /// <param name="name">欄位名稱</param>
        /// <param name="description">參數名稱</param>
        public MSFieldAttribute(string name, string description) : base(name, description)
        {
            Description = $"@{description}";
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace GD_STD.Attribute
{
    /// <summary>
    /// 代表 MSSQL 欄位名稱的自訂屬性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [DataContract]
    public sealed class MSTableAttribute : AbsCustomAttribute
    {
        /// <summary>
        /// 附加 MSSQL 資料表名稱
        /// </summary>
        /// <param name="name">資料表名稱</param>
        /// <param name="description">說明</param>
        public MSTableAttribute(string name, string description = "") : base(name, description)
        {
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace GD_STD.Enum
{
    /// <summary>
    /// 
    /// 附加屬性
    /// </summary>
    [DataContract]
    public sealed class ErrorCodeAttribute : Attribute
    {
        /// <summary>
        /// 建立 ErrorCode 附加屬性
        /// </summary>
        /// <param name="description">ErrorCode說明</param>
        /// <param name="solution">ErrorCode解決方法</param>
        public ErrorCodeAttribute(string description, string solution = "")
        {
            Description = description;
            Solution = solution;
        }
        /// <summary>
        /// ErrorCode說明
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// ErrorCode解決方法
        /// </summary>
        public string Solution { get; }
    }
}

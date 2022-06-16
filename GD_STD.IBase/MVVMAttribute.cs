using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base.Additional
{
    /// <summary>
    /// 提供軟體顯示的附加屬性
    /// </summary>
    public class MVVMAttribute : System.Attribute
    {
        /// <summary>
        /// 提供軟體顯示的附加屬性
        /// </summary>
        /// <param name="IsGroup">群組化</param>
        /// <param name="reflection">反射此成員 true，不反射則 false</param>
        /// <param name="readOnly">唯讀</param>
        public MVVMAttribute(bool IsGroup, bool reflection = true, bool readOnly = true)
        {
            //Title=title;
            this.IsGroup=IsGroup;
            Reflection=reflection;
            this.ReadOnly=readOnly;
            //Description=description;
        }
        /// <summary>
        /// 提供軟體顯示的附加屬性
        /// </summary>
        /// <param name="title">標題</param>
        /// <param name="IsGroup">群組化</param>
        /// <param name="description">說明</param>
        /// <param name="reflection">反射此成員 true，不反射則 false</param>
        /// <param name="readOnly">唯讀</param>
        public MVVMAttribute(string title, bool IsGroup, string description = null, bool reflection = true, bool readOnly = true)
        {
            Title=title;
            this.IsGroup=IsGroup;
            Reflection=reflection;
            this.ReadOnly=readOnly;
            Description=description;
        }
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 建立群組化
        /// </summary>
        /// <remarks>
        /// 需要建立群組回傳 true，不需要則回傳false
        /// </remarks>
        public bool IsGroup { get; private set; }
        /// <summary>
        /// 反射成員
        /// </summary>
        public bool Reflection { get; private set; }
        /// <summary>
        /// 唯讀
        /// </summary>
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// 屬性說明
        /// </summary>
        public string Description { get; private set; }
    }
}

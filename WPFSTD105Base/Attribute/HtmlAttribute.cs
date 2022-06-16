using GD_STD.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.Attribute
{
    /// <summary>
    /// 代表從 Tekla 輸出 HTML 表格的附加屬性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HtmlAttribute : AbsCustomAttribute
    {
        /// <summary>
        /// 附加到Tekla 輸出 HTML 表格的對應欄位
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public HtmlAttribute(string name, string description = null) : base(name, description)
        {
        }
    }
}

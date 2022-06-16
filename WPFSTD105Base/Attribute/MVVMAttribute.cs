//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WPFSTD105.Attribute
//{
//    /// <summary>
//    /// 提供軟體顯示的附加屬性
//    /// </summary>
//    public class MVVMAttribute : System.Attribute
//    {
//        /// <summary>
//        /// 提供軟體顯示的附加屬性
//        /// </summary>
//        /// <param name="IsGroup">群組化</param>
//        public MVVMAttribute(bool IsGroup)
//        {
//            //Title=title;
//            this.IsGroup=IsGroup;
//            //Description=description;
//        }
//        /// <summary>
//        /// 提供軟體顯示的附加屬性
//        /// </summary>
//        /// <param name="title">標題</param>
//        /// <param name="IsGroup">群組化</param>
//        /// <param name="description">說明</param>
//        public MVVMAttribute(string title, bool IsGroup, string description = null)
//        {
//            Title=title;
//            this.IsGroup=IsGroup;
//            Description=description;
//        }
//        /// <summary>
//        /// 標題
//        /// </summary>
//        public string Title { get; private set; } = null;
//        /// <summary>
//        /// 建立群組化
//        /// </summary>
//        /// <remarks>
//        /// 需要建立群組回傳 true，不需要則回傳false
//        /// </remarks>
//        public bool IsGroup { get; private set; }
//        /// <summary>
//        /// 屬性說明
//        /// </summary>
//        public string Description { get; private set; } = null;
//    }
//}

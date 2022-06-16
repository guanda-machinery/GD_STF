using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 專案訊息
    /// </summary>
    public class ProjectInfo
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Html("ProjectName")]
        public string Name { get; set; }
        /// <summary>
        /// 號碼
        /// </summary>
        [Html("ProjectNumber")]
        public string Number { get; set; }
        /// <summary>
        /// 輸出日期
        /// </summary>
        [Html("DateTim")]
        public string Date { get; set; }
    }
}

using DevExpress.Utils.Extensions;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAttribute = WPFSTD105.Attribute.HtmlAttribute;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace WPFSTD105.Tekla
{
    /// <summary>
    /// 從 Tekla 匯出的 HTML 報表處理方法
    /// </summary>
    public class TeklaHtemlFactory
    {
        /// <summary>
        /// 專案名稱
        /// </summary>
        public string ProjectName { get; private set; }
        /// <summary>
        /// 報表輸出日期
        /// </summary>
        public DateTime DateTim { get; private set; }
        /// <summary>
        /// 專案號碼
        /// </summary>
        public string ProjectNumber { get; private set; }
        /// <summary>
        /// 處理 Tekla CSV 表格
        /// </summary>
        /// <param name="bomPath">輸入報表路徑</param>
        /// <param name="modelPath"></param>
        public TeklaHtemlFactory(string bomPath, string modelPath)
        {

        }
        #region 私有方法
        #endregion

        #region 公用方法
        #endregion
    }
}

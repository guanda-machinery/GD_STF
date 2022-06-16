using DevExpress.Utils.Extensions;
using GD_STD.Data;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
        /// 構件資訊列表
        /// </summary>
        private ObservableCollection<SteelAssembly> SteelAssemblies = new ObservableCollection<SteelAssembly>();

        /// <summary>
        /// 處理 Tekla CSV 表格
        /// </summary>
        /// <param name="bomPath">輸入報表路徑</param>
        public TeklaHtemlFactory(string bomPath)
        {
            StreamReader reader = new StreamReader(bomPath);
        }
        #region 私有方法

        #endregion

        #region 公用方法

        #endregion
    }
}

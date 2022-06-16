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
using GD_STD;
using GD_STD.Enum;
using System.Windows;

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
        /// 報表屬性
        /// </summary>
        private ObservableCollection<BomProperty> BomProperty = new ObservableCollection<BomProperty>();
        /// <summary>
        /// 斷面規格列表
        /// </summary>
        private ObservableCollection<string> ProfileList;
        /// <summary>
        /// 主件
        /// </summary>
        private SteelAssembly Father;
        /// <summary>
        /// GD_STD.Data Assembly  
        /// </summary>
        private Assembly assembly = Assembly.Load("GD_STD.Data");
        /// <summary>
        /// 處理 Tekla CSV 表格
        /// </summary>
        /// <param name="bomPath">輸入報表路徑</param>
        public TeklaHtemlFactory(string bomPath)
        {
            ProfileList = SerializationHelper.GZipDeserialize<ObservableCollection<string>>(ApplicationVM.ProfileList());
            using (StreamReader reader = new StreamReader(bomPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (ToObject(line))
                    {
                        MessageBox.Show($"是否確定離開此頁面，離開後將不儲存相關設定。", "通知", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                        return;
                    }
                }
            }
        }
        #region 私有方法
        private bool ToObject(string str)
        {
            string[] data = str.Split(','); //資料表內容

            for (int i = 0; i < data.Length; i++)
            {

            }
        }
        #endregion

        #region 公用方法

        #endregion
    }
}

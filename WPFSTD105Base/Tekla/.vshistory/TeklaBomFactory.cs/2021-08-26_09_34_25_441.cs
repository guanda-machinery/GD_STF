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
using GD_STD.Base;
using System.Threading.Tasks;

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
        ///// <summary>
        ///// 報表屬性
        ///// </summary>
        //private ObservableCollection<BomProperty> BomProperty = new ObservableCollection<BomProperty>();
        /// <summary>
        /// 斷面規格列表
        /// </summary>
        private ObservableCollection<string> ProfileList;
        /// <summary>
        /// 構件
        /// </summary>
        private SteelAssembly Father;
        /// <summary>
        /// GD_STD.Data.dll Assembly  
        /// </summary>
        private Assembly assembly = Assembly.Load("GD_STD.Data");
        /// <summary>
        /// 處理 Tekla CSV 表格
        /// </summary>
        /// <param name="bomPath">輸入報表路徑</param>
        public TeklaHtemlFactory(string bomPath)
        {
            //斷面規格列表
            ProfileList = SerializationHelper.GZipDeserialize<ObservableCollection<string>>(ApplicationVM.ProfileList());
            //讀取報表資料流
            using (StreamReader reader = new StreamReader(bomPath))
            {
                string line;//資料行內容
                int number = 0;//資料行位置
                while ((line = reader.ReadLine()) != null)
                {
                    if (ToObject(line,) == null)
                    {
                        return;
                    }
                    else
                    {

                    }
                    number++;
                }
            }
        }
        #region 私有方法
        /// <summary>
        /// 報表資料行轉物件
        /// </summary>
        /// <param name="str">資料行內容</param>
        /// <param name="number">資料行位置</param>
        /// <returns>轉換成功回傳 true, 失敗則回傳false。</returns>
        private object ToObject(string str, int number)
        {
            string[] data = str.Split(','); //資料表內容
            string strTtype = data[0];//反射類型在報表逗點第一次出現前的欄位
            Type type = assembly.GetType(strTtype); //找尋報表指定 type
            //反射不到 type 
            if (type == null)
            {
                MessageBox.Show($"轉換失敗因 {strTtype} 不是有效物件。", "通知", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                return null;
            }
            object result = Activator.CreateInstance(type); // 建立指定 type 物件
            bool error = false;//引發錯誤
            Parallel.For(0, data.Length, i =>
            {
                IEnumerable<FieldInfo> fieldInfos = type.GetFields().Where(el => el.GetCustomAttribute<TeklaBomAttribute>().Index == i);
                try
                {
                    FieldInfo info = fieldInfos.ElementAt(0);
                    object value = Convert.ChangeType(data[i], info.FieldType);
                    info.SetValue(result, value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex}。在資料行 {number} 。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    error = true;
                    log4net.LogManager.GetLogger($"報表資料行在第 {number} 行轉換失敗").ErrorFormat(ex.Message + "\n" + DateTime.Now.ToString() + "\n", ex.StackTrace);
                }
            });
            //如果引發例外外狀
            if (error)
            {
                return null;
            }
            else
            {
                return result;
            }
        }
        #endregion

        #region 公用方法

        #endregion
    }
}

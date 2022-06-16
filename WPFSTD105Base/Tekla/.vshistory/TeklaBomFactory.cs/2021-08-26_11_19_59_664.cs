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
        #region 私有屬性
        /// <summary>
        /// 構件資訊列表
        /// </summary>
        private ObservableCollection<SteelAssembly> SteelAssemblies = new ObservableCollection<SteelAssembly>();
        /// <summary>
        /// 螺栓的列表
        /// </summary>
        private Dictionary<string, SteelBolts> _BoltsKeyValuePairs = new Dictionary<string, SteelBolts>();
        /// <summary>
        /// 單零件列表
        /// </summary>
        private Dictionary<string, SteelPart> _PartKeyValuePairs = new Dictionary<string, SteelPart>();
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
        #endregion
        /// <summary>
        /// 處理 Tekla CSV 表格
        /// </summary>
        /// <param name="bomPath">輸入報表路徑</param>
        public TeklaHtemlFactory(string bomPath)
        {
            //斷面規格列表
            ProfileList = SerializationHelper.GZipDeserialize<ObservableCollection<string>>(ApplicationVM.ProfileList());
            //讀取報表資料流
            using (StreamReader reader = new StreamReader(bomPath, System.Text.Encoding.Default))
            {
                string line;//資料行內容
                int number = 0;//資料行位置
                //讀取資料行
                while ((line = reader.ReadLine()) != null)
                {
                    //標頭 0~2 是模型資訊
                    if (number > 2)
                    {
                        string[] data = line.Split(','); //資料表欄位陣列
                        string strTtype = data[0];//反射類型在報表逗點第一次出現前的欄位
                        Type type = assembly.GetType($"GD_STD.Data.{strTtype}"); //找尋報表指定 type
                        //反射不到 type 
                        if (type == null)
                        {
                            MessageBox.Show($"轉換失敗因 {strTtype} 不是有效物件。", "通知", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                            return;
                        }
                        object result = Activator.CreateInstance(type); // 建立指定 type 物件
                        bool obj = GetObject(data, number, result); //轉換物件
                        if (obj == false) //判斷物件轉換是否有成功
                        {
                            return;
                        }
                        else
                        {

                        }
                    }
                    number++;
                }
            }
        }
        #region 私有方法
        /// <summary>
        /// 報表資料行轉物件
        /// </summary>
        /// <param name="data">資料欄位內容</param>
        /// <param name="number">資料行位置</param>
        /// <param name="obj">寫入物件</param>
        /// <returns>轉換成功回傳 true, 失敗則回傳false。</returns>
        private bool GetObject(string[] data, int number, object obj)
        {
            bool error = false;//引發錯誤
            for (int i = 1; i < data.Length; i++)
            {
                string strValue = data[i].Trim();
                IEnumerable<PropertyInfo> propertyInfos = obj.GetType().GetProperties().Where(el =>
                        el.GetCustomAttribute<TeklaBomAttribute>() != null && el.GetCustomAttribute<TeklaBomAttribute>().Index == i);//收尋對應欄位
                try
                {
#if DEBUG
                    //欄位是唯一不然設中斷點
                    if (propertyInfos.Count() > 1)
                    {
                        Debugger.Break();
                    }
#endif
                    PropertyInfo info = propertyInfos.ElementAt(0);//收尋到的對應欄位只有唯一個
                    
                    if (info.PropertyType.IsEnum)
                    {
                        try
                        {
                            object value = System.Enum.Parse(info.PropertyType, strValue);//反射出屬性 TYPE ，並轉換要賦予的值
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    else
                    {
                        object value = Convert.ChangeType(strValue, info.PropertyType);//轉換報表內的參數
                        if (info.PropertyType.GetInterfaces().Contains(typeof(IList)))
                        {
                            Type[] typeArgs = info.PropertyType.GetGenericArguments();//取得封閉泛型類型的型別引數
                            Type constructed = info.PropertyType.MakeGenericType(typeArgs); //用類型陣列的項目取代目前泛型類型定義的型別參數，並傳回代表所得結果建構類型的 System.Type 物件。
                            IList list = (IList)Activator.CreateInstance(constructed);//反射出集合
                            list.Add(value);//加入到集合內
                        }
                        else
                        {
                            info.SetValue(obj, value);//存到物件內
                        }
                    }
               

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex}。在資料行 {number} 。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    error = true;
                    log4net.LogManager.GetLogger($"報表資料行在第 {number} 行轉換失敗").ErrorFormat(ex.Message + "\n" + DateTime.Now.ToString() + "\n", ex.StackTrace);
                }
            }
            //平行運算
            //            Parallel.For(1, data.Length, i =>
            //            {
            //                IEnumerable<PropertyInfo> propertyInfos = obj.GetType().GetProperties().Where(el =>
            //                            el.GetCustomAttribute<TeklaBomAttribute>() != null && el.GetCustomAttribute<TeklaBomAttribute>().Index == i);//收尋對應欄位
            //                try
            //                {
            //#if DEBUG
            //                    //欄位是唯一不然設中斷點
            //                    if (propertyInfos.Count() > 1)
            //                    {
            //                        Debugger.Break();
            //                    }
            //#endif
            //                    PropertyInfo info = propertyInfos.ElementAt(0);//收尋到的對應欄位只有唯一個
            //                    object value = Convert.ChangeType(data[i].Trim(), info.PropertyType);//轉換報表內的參數
            //                    info.SetValue(obj, value);//存到物件內
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show($"{ex}。在資料行 {number} 。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
            //                    error = true;
            //                    log4net.LogManager.GetLogger($"報表資料行在第 {number} 行轉換失敗").ErrorFormat(ex.Message + "\n" + DateTime.Now.ToString() + "\n", ex.StackTrace);
            //                }
            //            });
            //如果引發例外外狀
            if (error)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 公用方法

        #endregion
    }
}

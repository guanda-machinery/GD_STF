using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using HtmlAttribute = WPFSTD105.Attribute.HtmlAttribute;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace WPFSTD105.Tekla
{
    /// <summary>
    /// 從 Tekla 匯出的 HTML 報表處理方法
    /// </summary>
    public class TeklaHtemlFactory 
    {
        private HtmlDocument _doc = new HtmlDocument();
        /// <summary>
        /// 處理 Tekla HTML 表格
        /// </summary>
        /// <param name="path"></param>
        public TeklaHtemlFactory(string path)
        {
            _doc.Load(path, Encoding.Default);//讀取 tekla html 報表
            //偵測是否是 GUANDA 製作的報表
            var logo = _doc.DocumentNode.SelectNodes("//meta").ToList().FirstOrDefault(el => el.GetAttributeValue("CONTENT", "") == "GUANDA");
            if (logo == null)
            {
                throw new Exception("無法讀取檔案 : 此表格有可能是非來自 Tekla Structures 所輸出，或者，報表作者不是來自 GUANDA");
            }
        }
        /// <summary>
        /// 逐步取得資料行內容
        /// </summary>
        /// <param name="htmlNodeCollection"></param>
        /// <param name="type">要反射的物件 type</param>
        /// <param name="filter">過濾器相關訊息</param>
        /// <returns></returns>
        private object Cell(HtmlNodeCollection htmlNodeCollection, Type type, Dictionary<Type, List<string>> filter)
        {
            object result = Activator.CreateInstance(type);//創建類型
            htmlNodeCollection.ToList().
                    ForEach(tr =>
                    {
                        if (tr.ChildNodes.Count > 0 && result != null)
                        {
                            tr.SelectNodes("td").ToList().
                            ForEach(td =>
                            {
                                if (result != null)
                                {
                                    var value = td.GetAttributeValue("name", "");
                                    if (value != "")
                                    {
                                        var propertyInfos = type.GetProperties().
                                            Where(info => info.GetCustomAttribute(typeof(HtmlAttribute)) != null && ((HtmlAttribute)info.GetCustomAttribute(typeof(HtmlAttribute))).Name == value);//以附加屬性的標籤去找相對應的欄位
                                        string strValue = Regex.Replace(td.InnerText, @"<[^>]+>|&nbsp;", "").Trim();//將 html 格式的空白字元拿掉
                                        //判斷集合是否有物件
                                        if (propertyInfos.Count() != 0)
                                        {
                                            PropertyInfo propertyInfo = propertyInfos.ElementAt(0);
                                            object setValue = null; //存取值
                                            //判斷轉換的值是不是 Enum 類型
                                            if (propertyInfo.PropertyType.IsEnum)
                                            {
                                                //判斷此屬性是否有需要過濾
                                                if (filter.ContainsKey(propertyInfo.PropertyType))
                                                {
                                                    if (filter[propertyInfo.PropertyType].Contains(strValue))
                                                    {
                                                        result = null;
                                                        return;
                                                    }
                                                    setValue = System.Enum.Parse(propertyInfo.PropertyType, strValue);//反射出屬性 TYPE ，並轉換要賦予的值
                                                }
                                            }
                                            else if (propertyInfo.PropertyType.IsGenericType)
                                            {
                                                if (propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IList)))
                                                {
                                                    Type[] typeArgs = propertyInfo.PropertyType.GetGenericArguments();//取得封閉泛型類型的型別引數
                                                    Type constructed = propertyInfo.PropertyType.MakeGenericType(typeArgs); //用類型陣列的項目取代目前泛型類型定義的型別參數，並傳回代表所得結果建構類型的 System.Type 物件。
                                                    IList list = (IList)Activator.CreateInstance(constructed);//反射出集合
                                                    list.Add(Convert.ChangeType(strValue, propertyInfo.PropertyType));//加入到集合內
                                                    setValue = list;
                                                }
                                                Debugger.Break();
                                            }
                                            else
                                            {
                                                setValue = Convert.ChangeType(strValue, propertyInfo.PropertyType);//反射出屬性 TYPE ，並轉換要賦予的值
                                            }
                                            propertyInfo.SetValue(result, setValue);//賦予值
                                        }
                                    }
                                }
                            });
                        }
                    });
            return result;
        }
        /// <summary>
        /// Tekla HTML 報表轉換成 .NET 物件
        /// </summary>
        /// <param name="type">要反射的物件 type </param>
        /// <param name="tableName">資料表格名稱</param>
        /// <param name="filter">過濾器相關訊息</param>
        /// <returns></returns>
        public List<object> ToObject(Type type, string tableName, Dictionary<Type, List<string>> filter = null)
        {
            List<object> result = new List<object>();
            //取得表格節點
            _doc.DocumentNode.SelectNodes("//table").
                Where(table => table.GetAttributeValue("name", "") == tableName).ToList().//只選擇指定資料表
                   ForEach(tableContent =>
                   {
                       HtmlNodeCollection htmlNodes = null;
                       var tbodyNodes = tableContent.SelectNodes("tbody");//共用欄位參數
                       if (tbodyNodes != null)
                       {
                           tbodyNodes.ToList().
                              ForEach(tbody =>
                              {
                                  htmlNodes = tbody.ChildNodes;
                              });
                       }
                       tableContent.SelectNodes("tfoot").ToList().//資料群組
                           ForEach(tfoot =>
                           {
                               if (htmlNodes != null)
                               {
                                   tfoot.AppendChildren(htmlNodes);//加入共用欄位參數
                               }
                               object val = Cell(tfoot.ChildNodes, type, filter);
                               if (val != null)
                               {
                                   result.Add(val);
                               }
                           });
                   });
            return result;
        }
    }
}

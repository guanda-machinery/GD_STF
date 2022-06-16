using GD_STD.Base;
using GD_STD.Phone;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAttribute = WPFSTD105.Attribute.HtmlAttribute;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using static GD_STD.MemoryHelper;
using GD_STD.Enum;
using GD_STD;
using System.ServiceModel;
using GD_STD.Data;
using WPFSTD105;
using System.Collections.ObjectModel;
using WPFSTD105.ViewModel;

namespace ConsoleApp3
{
    public struct aaa
    {
        public aaa(int[] vs)
        {
            Array = vs;
        }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] Array;
    }
    class Program
    {
        public static MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateOrOpen("Global\\aaaa", Marshal.SizeOf(typeof(aaa)));
        public static MemoryMappedFile charMappedFile = MemoryMappedFile.CreateOrOpen("Global\\charaw", Marshal.SizeOf(typeof(aaa)));
        public static Operating Operating = new Operating();
        public static Memor.WriteMemorClient ClientWrite = new Memor.WriteMemorClient();
        public static Memor.ReadMemorClient ClientRead = new Memor.ReadMemorClient();
        List<string> profile = new List<string> { "RH300X150X6.5X9", "RH300X300X10X15", "RH400X400X13X21" };
        List<string> partNumber = new List<string> { "H300", "H484", "H811", "S100", "AD4V1", "SWDF" };
        List<string> AssNumber = new List<string> { "SC1-1", "SC1-2", "SB1-2", "SB1-3" };
        unsafe static void Main(string[] args)
        {
            MonitorWork _ = MonitorWork.Initialization();

            //ObservableCollection<DrillBrand> drillBrands = new ObservableCollection<DrillBrand>();
            STDSerialization ser = new STDSerialization();
            ProjectProperty project = new ProjectProperty();
            ser.TEST(project);
            ser.TESTa();
        }

        /// <summary>
        /// 逐步取得資料行內容
        /// </summary>
        /// <param name="htmlNodeCollection"></param>
        /// <param name="type">要反射的 type</param>
        /// <param name="filter">過濾器相關訊息</param>
        /// <returns></returns>
        static object Cell(HtmlNodeCollection htmlNodeCollection, Type type, Dictionary<Type, List<string>> filter = null)
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
                                        var propertyInfo = type.GetProperties().
                                            Where(info => info.GetCustomAttribute(typeof(HtmlAttribute)) != null && ((HtmlAttribute)info.GetCustomAttribute(typeof(HtmlAttribute))).Name == value);//以附加屬性的標籤去找相對應的欄位
                                        string strValue = Regex.Replace(td.InnerText, @"<[^>]+>|&nbsp;", "").Trim();//將 html 格式的空白字元拿掉
                                        if (propertyInfo.Count() != 0)
                                        {
                                            object setValue = null;
                                            if (propertyInfo.ElementAt(0).PropertyType.IsEnum)
                                            {
                                                if (filter.ContainsKey(propertyInfo.ElementAt(0).PropertyType))
                                                {
                                                    if (filter[propertyInfo.ElementAt(0).PropertyType].Contains(strValue))
                                                    {
                                                        result = null;
                                                        return;
                                                    }
                                                    setValue = Enum.Parse(propertyInfo.ElementAt(0).PropertyType, strValue);//反射出屬性 TYPE ，並轉換
                                                }
                                            }
                                            else
                                            {
                                                setValue = Convert.ChangeType(strValue, propertyInfo.ElementAt(0).PropertyType);//反射出屬性 TYPE ，並轉換
                                            }
                                            propertyInfo.ElementAt(0).SetValue(result, setValue);//存取值
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
        /// <param name="doc"></param>
        /// <param name="type">要反射的 type</param>
        /// <param name="tableName">資料表格名稱</param>
        /// <param name="filter">過濾器相關訊息</param>
        /// <returns></returns>
        private static List<object> ToObject(HtmlDocument doc, Type type, string tableName, Dictionary<Type, List<string>> filter = null)
        {
            List<object> result = new List<object>();
            //取得表格節點
            doc.DocumentNode.SelectNodes("//table").
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

            //專案訊息
            //doc.DocumentNode.SelectNodes("//table").
            //    Where(table => table.GetAttributeValue("name", "") == tableName).ToList().
            //        ForEach(row =>
            //        {
            //            object obj = Activator.CreateInstance(type);//創建類型
            //            object vObj = obj.DeepClone();
            //            bool save = false; //儲存
            //            foreach (var td in row.SelectNodes("tr"))
            //            {
            //                foreach (var cell in td.SelectNodes("td"))
            //                {
            //                    var value = cell.GetAttributeValue("name", "");
            //                    if (value != "")
            //                    {
            //                        var propertyInfo = type.GetProperties().
            //                            Where(info => info.GetCustomAttribute(typeof(HtmlAttribute)) != null && ((HtmlAttribute)info.GetCustomAttribute(typeof(HtmlAttribute))).Name == value);//以附加屬性的標籤去找相對應的欄位
            //                        string strValue = Regex.Replace(cell.InnerText, @"<[^>]+>|&nbsp;", "").Trim();//將 html 格式的空白字元拿掉
            //                        if (propertyInfo.Count() != 0)
            //                        {
            //                            object setValue = null;
            //                            if (propertyInfo.ElementAt(0).PropertyType.IsEnum)
            //                            {
            //                                setValue = Enum.Parse(propertyInfo.ElementAt(0).PropertyType, strValue);//反射出屬性 TYPE ，並轉換
            //                            }
            //                            else
            //                            {
            //                                setValue = Convert.ChangeType(strValue, propertyInfo.ElementAt(0).PropertyType);//反射出屬性 TYPE ，並轉換
            //                            }
            //                            propertyInfo.ElementAt(0).SetValue(vObj, setValue);//存取值
            //                            save = true;
            //                        }
            //                    }
            //                }
            //                if (save)
            //                {
            //                    result.Add(obj);
            //                }
            //            }

            //        });

            return result;
        }
    }
    static class a
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (var item in sequence) action(item);
        }
        public static object DeepClone(this object obj)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //序列化物件格式
                IFormatter formatter = new BinaryFormatter();
                //將自己所有資料序列化
                formatter.Serialize(objectStream, obj);
                //複寫資料流位置，返回最前端
                objectStream.Seek(0, SeekOrigin.Begin);
                //再將objectStream反序列化回去 
                return formatter.Deserialize(objectStream);
            }
        }
    }
}

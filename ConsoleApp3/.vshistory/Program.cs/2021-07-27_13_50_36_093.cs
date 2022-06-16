using GD_STD.Phone;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using HtmlAttribute = WPFSTD105.Attribute.HtmlAttribute;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace ConsoleApp3
{
    class Program
    {
        public static Operating Operating = new Operating();
        static void Main(string[] args)
        {
            string _ = File.ReadAllText(@"C:\Users\User\source\repos\GD_STF\ConsoleApp3\bin\Debug\OK_Test_1.txt");
            Console.WriteLine(_.Length);
            Console.ReadLine();
            //GD_STD.Phone.MemoryHelper.OpenSharedMemory(); //需要使用系統管理員
            //Operating = GD_STD.Phone.SharedMemory<Operating>.GetValue(); ;
            //Operating.OpenApp = true;
            //Console.WriteLine("輸入要求 ...... ");
            //Operating.Satus = (PHONE_SATUS)(Convert.ToInt16(Console.ReadLine()));
            //GD_STD.Phone.SharedMemory<Operating>.SetValue(Operating);

            //            List<DrillParameter> drillParameters = new List<DrillParameter>();

            //            string _ = @"WIDIA,27,3.5,74.22012644,875,0.24
            //WIDIA,27,5.5,82.3631346,971,0.34
            //WIDIA,27,7.5,95.42587685,1125,0.4
            //WIDIA,27,9.5,112.3904772,1325,0.43
            //Kennametal,25,3.5,65.97344573,840,0.25
            //Kennametal,25,5.5,86.39379797,1100,0.3
            //Kennametal,25,7.5,98.17477042,1250,0.36
            //Kennametal,25,9.5,104.1437965,1326,0.43";
            //            foreach (var item in _.Split('\n'))
            //            {
            //                string[] vs = item.Split(',');
            //                string Level = string.Empty;
            //                switch (vs[2])
            //                {
            //                    case "3.5":
            //                        Level = "Level1";
            //                        break;
            //                    case "5.5":
            //                        Level = "Level2";
            //                        break;
            //                    case "7.5":
            //                        Level = "Level3";
            //                        break;
            //                    case "9.5":
            //                        Level = "Level4";
            //                        break;
            //                    default:
            //                        break;
            //                }
            //                drillParameters.Add(new DrillParameter()
            //                {
            //                    Name = vs[0],
            //                    DataName = Level,
            //                    Dia = Convert.ToDouble(vs[1]),
            //                    Vc = Convert.ToDouble(vs[3]),
            //                    DrillType = DRILL_TYPE.DRILL,
            //                    Rpm = Convert.ToDouble(vs[4]),
            //                    FeedQuantity = Convert.ToSingle(vs[5]),
            //                    IsReadOnly = true
            //                });
            //            }
            //HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            //doc.LoadHtml(File.ReadAllText(@"C:\Users\User\Desktop\GUANDA_Personal_Format.xls", Encoding.Default));
            //var logo = doc.DocumentNode.SelectNodes("//meta").ToList().FirstOrDefault(el => el.GetAttributeValue("CONTENT", "") == "GUANDA");

            //Dictionary<string, Dictionary<string, List<SteelAttr>>> keyValuePairs = new Dictionary<string, Dictionary<string, List<SteelAttr>>>();
            //IList<object> list1 = ToObject(doc, typeof(ProjectInfo), "ProjectInfo");
            //Dictionary<Type, List<string>> filter = new Dictionary<Type, List<string>>();
            //filter.Add(typeof(OBJETC_TYPE), new List<string>() { OBJETC_TYPE.RH.ToString(), OBJETC_TYPE.Unknown.ToString() });
            //IList<object> list2 = ToObject(doc, typeof(SteelAttr), "AssemblyInfo", filter);
            //list2.ToList().
            //    ForEach(el =>
            //    {
            //        SteelAttr steelAttr = (SteelAttr)el;
            //        if (!keyValuePairs.ContainsKey(steelAttr.PartNumber))
            //        {
            //            Dictionary<string, List<SteelAttr>> keyValues = new Dictionary<string, List<SteelAttr>>();
            //            List<SteelAttr> steelAttrs = new List<SteelAttr>();
            //            steelAttrs.Add(steelAttr);
            //            keyValues.Add(steelAttr.AsseNumber, steelAttrs);
            //            keyValuePairs.Add(steelAttr.PartNumber, keyValues);
            //        }
            //        else
            //        {
            //            if (!keyValuePairs[steelAttr.PartNumber].ContainsKey(steelAttr.AsseNumber))
            //            {
            //                List<SteelAttr> steelAttrs = new List<SteelAttr>();
            //                steelAttrs.Add(steelAttr);
            //                keyValuePairs[steelAttr.PartNumber].Add(steelAttr.AsseNumber, steelAttrs);
            //            }
            //            else
            //            {
            //                keyValuePairs[steelAttr.PartNumber][steelAttr.AsseNumber].Add(steelAttr);
            //            }
            //        }
            //    });

            ////專案訊息
            //doc.DocumentNode.SelectNodes("//table").
            //    Where(title => title.GetAttributeValue("name", "") == "ProjectInfo").ToList().
            //        ForEach(table => table.SelectNodes("tr").ToList().
            //            ForEach(row => row.SelectNodes("td").ToList().
            //                ForEach(td =>
            //                {
            //                    var value = td.GetAttributeValue("name", "");
            //                    if (value != "")
            //                    {
            //                        var propertyInfo = type.GetProperties().
            //                            Where(info => ((HtmlAttribute)info.GetCustomAttribute(typeof(HtmlAttribute))).Name == value);//以附加屬性的標籤去找相對應的欄位
            //                        string strValue = Regex.Replace(td.InnerText, @"<[^>]+>|&nbsp;", "").Trim();//將 html 格式的空白字元拿掉
            //                        object setValue = Convert.ChangeType(strValue, propertyInfo.ElementAt(0).PropertyType);//反射出屬性 TYPE ，並轉換
            //                        propertyInfo.ElementAt(0).SetValue(obj, setValue);//存取值
            //                    }
            //                })));

            //ProjectInfo projectInfo = (ProjectInfo)obj;
            //ForEach(row =>
            //{
            //    row.SelectNodes("td").ToList().ForEach(td => { })
            //    foreach (HtmlNode cell in row.SelectNodes("td"))
            //        projectName = cell.GetAttributeValue("name", "") != "ProjectName" ? "" : cell.GetAttributeValue("name", "");
            //}
            //foreach (HtmlNode table in )
            //{
            //    foreach (HtmlNode row in table.SelectNodes("tr"))
            //    {
            //        string str = string.Empty;
            //        foreach (HtmlNode cell in row.SelectNodes("th|td"))
            //            str += Regex.Replace(cell.InnerText, @"<[^>]+>|&nbsp;", "").Trim();
            //        Console.WriteLine(str);
            //    }
            //}
            Console.Read();
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
        /// <param name="type">要反射的 type </param>
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

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
using DevExpress.Utils;
using DevExpress.Data.Extensions;
using WPFSTD105.Attribute;
using WPFSTD105;
namespace WPFSTD105.Tekla
{
    /// <summary>
    /// 從 Tekla 匯出的 HTML 報表處理方法
    /// </summary>
    public class TeklaBomFactory
    {
        #region 公開屬性
        /// <summary>
        /// 有使用過的斷面規格
        /// </summary>
        public ObservableCollection<string> ProfileList = new ObservableCollection<string>();
        /// <summary>
        /// 構件資訊列表
        /// </summary>
        public ObservableCollection<SteelAssembly> SteelAssemblies { get; private set; } = new ObservableCollection<SteelAssembly>();
        /// <summary>
        /// 次件列表
        /// </summary>
        public Dictionary<string, ObservableCollection<object>> KeyValuePairs { get; private set; } = new Dictionary<string, ObservableCollection<object>>();
        /// <summary>
        /// 軟體缺少的斷面規格列表
        /// </summary>
        public Dictionary<OBJETC_TYPE, ObservableCollection<SteelAttr>> Profile { get; private set; } = new Dictionary<OBJETC_TYPE, ObservableCollection<SteelAttr>>();
        /// <summary>
        /// 軟體缺少的材質
        /// </summary>
        public ObservableCollection<SteelMaterial> Material { get; set; } = new ObservableCollection<SteelMaterial>();

        #endregion

        #region 私有屬性
        /// <summary>
        /// 構件
        /// </summary>
        private SteelAssembly Father;
        /// <summary>
        /// GD_STD.Data.dll Assembly  
        /// </summary>
        private Assembly assembly = Assembly.Load("GD_STD.Data");
        /// <summary>
        /// 報表路徑
        /// </summary>
        private string _BomPath { get; set; }
        /// <summary>
        /// 有增加斷面規格
        /// </summary>
        private bool _LackProfile { get; set; }
        /// <summary>
        /// 有增加過材質
        /// </summary>
        private bool _LackMaterial { get; set; }
        #endregion
        /// <summary>
        /// 處理 Tekla CSV 表格
        /// </summary>
        /// <param name="bomPath">輸入報表路徑</param>
        public TeklaBomFactory(string bomPath)
        {
            _BomPath = bomPath;
            Material = SerializationHelper.GZipDeserialize<ObservableCollection<SteelMaterial>>(ApplicationVM.FileMaterial()); //系統內材質序列化檔案
            Profile.Add(OBJETC_TYPE.BH, SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\BH.inp")); //加入 BH 字典
            Profile.Add(OBJETC_TYPE.RH, SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\RH.inp"));//加入 RH 字典
            Profile.Add(OBJETC_TYPE.BOX, SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\BOX.inp"));//加入 BOX 字典
            Profile.Add(OBJETC_TYPE.CH, SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\CH.inp"));//加入 CH 字典
            Profile.Add(OBJETC_TYPE.L, SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\L.inp"));//加入 L 字典
        }

        #region 私有方法
        //除錯用
#if DEBUG && _DEBUG
        /// <summary>
        /// 報表資料行轉物件
        /// </summary>
        /// <param name="data">資料欄位內容</param>
        /// <param name="number">資料行位置</param>
        /// <param name="obj">寫入物件</param>
        /// <returns>引發錯誤回傳true , 未引發錯誤回傳 false。</returns>
        private bool GetObject(string[] data, int number, object obj)
        {
            bool error = false;//引發錯誤
            for (int i = 1; i < data.Length; i++)
            {
                string strValue = data[i].Trim();
                IEnumerable<PropertyInfo> propertyInfos = obj.GetType().GetProperties().Where(el =>
                        el.GetCustomAttribute<TeklaBomAttribute>() != null && el.GetCustomAttribute<TeklaBomAttribute>().Index == i);//收尋對應欄位
                object setValue = null;
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
                            setValue = System.Enum.Parse(info.PropertyType, strValue);//反射出屬性 TYPE ，並轉換要賦予的值
                        }
                        catch (ArgumentException ex)
                        {
                            setValue = System.Enum.Parse(info.PropertyType, "Unknown");//反射出屬性 TYPE ，並轉換要賦予 Unknown
                            Debugger.Break();
                        }
                        finally
                        {
                            info.SetValue(obj, setValue);//存到物件內
                        }
                    }
                    else if (info.PropertyType.IsGenericType) //如果是泛型
                    {
                        if (info.PropertyType.GetInterfaces().Contains(typeof(IList))) //如果是包含 IList 型別
                        {
                            Type genericType = info.PropertyType.GetGenericTypeDefinition();//泛型類型
                            Type[] typeArgs = info.PropertyType.GetGenericArguments();//取得 T 型別
                            setValue = Convert.ChangeType(strValue, typeArgs[0]);//反射出屬性 TYPE ，並轉換要賦予的值
                            Type constructed = genericType.MakeGenericType(typeArgs); //用類型陣列的項目取代目前泛型類型定義的型別參數，並傳回代表所得結果建構類型的 System.Type 物件。
                            MethodInfo methodInfo = info.PropertyType.GetMethod("Add");//調用方法
                            methodInfo.Invoke(info.GetValue(obj), new object[] { setValue });
                        }
                        else
                        {
                            Debugger.Break();
                        }
                    }
                    else
                    {
                        setValue = Convert.ChangeType(strValue, info.PropertyType);//反射出屬性 TYPE ，並轉換要賦予的值
                        info.SetValue(obj, setValue);//存到物件內
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}。在資料行 {number} 。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    error = true;
                    log4net.LogManager.GetLogger($"報表資料行在第 {number} 行轉換失敗").ErrorFormat(ex.Message + "\n" + DateTime.Now.ToString() + "\n", ex.StackTrace);
                }
            }

            return error;
        }
#elif DEBUG || TRACE
        /// <summary>
        /// 報表資料行轉物件
        /// </summary>
        /// <param name="data">資料欄位內容</param>
        /// <param name="number">資料行位置</param>
        /// <param name="obj">寫入物件</param>
        /// <returns>引發錯誤回傳true , 未引發錯誤回傳 false。</returns>
        private bool GetObject(string[] data, int number, object obj)
        {
            bool error = false;//引發錯誤
            //平行運算
            Parallel.For(1, data.Length, i =>
            {
                string strValue = data[i].Trim();
                IEnumerable<PropertyInfo> propertyInfos = obj.GetType().GetProperties().Where(el =>
                        el.GetCustomAttribute<TeklaBomAttribute>() != null && el.GetCustomAttribute<TeklaBomAttribute>().Index == i);//收尋對應欄位
                object setValue = null;
                try
                {
                    PropertyInfo info = propertyInfos.ElementAt(0);//收尋到的對應欄位只有唯一個
                    if (info.PropertyType.IsEnum)
                    {
                        try
                        {
                            setValue = System.Enum.Parse(info.PropertyType, strValue);//反射出屬性 TYPE ，並轉換要賦予的值
                        }
                        catch (ArgumentException ex)
                        {
                            setValue = System.Enum.Parse(info.PropertyType, "Unknown");//反射出屬性 TYPE ，並轉換要賦予 Unknown
                            Debug.WriteLine($"{info.PropertyType} 找不到 {strValue}");
                        }
                        finally
                        {
                            info.SetValue(obj, setValue);//存到物件內
                        }
                    }
                    else if (info.PropertyType.IsGenericType) //如果是泛型
                    {
                        if (info.PropertyType.GetInterfaces().Contains(typeof(IList))) //如果是包含 IList 型別
                        {
                            Type genericType = info.PropertyType.GetGenericTypeDefinition();//泛型類型
                            Type[] typeArgs = info.PropertyType.GetGenericArguments();//取得 T 型別
                            setValue = Convert.ChangeType(strValue, typeArgs[0]);//反射出屬性 TYPE ，並轉換要賦予的值
                            Type constructed = genericType.MakeGenericType(typeArgs); //用類型陣列的項目取代目前泛型類型定義的型別參數，並傳回代表所得結果建構類型的 System.Type 物件。
                            MethodInfo methodInfo = info.PropertyType.GetMethod("Add");//調用方法
                            methodInfo.Invoke(info.GetValue(obj), new object[] { setValue });
                        }
                        else
                        {
                            Debugger.Break();
                        }
                    }
                    else
                    {
                        setValue = Convert.ChangeType(strValue, info.PropertyType);//反射出屬性 TYPE ，並轉換要賦予的值
                        info.SetValue(obj, setValue);//存到物件內
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}。在資料行 {number} 。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    error = true;
                    log4net.LogManager.GetLogger($"報表資料行在第 {number} 行轉換失敗").ErrorFormat(ex.Message + "\n" + DateTime.Now.ToString() + "\n", ex.StackTrace);
                }
            });
            //如果引發例外外狀
            return error;
        }

#endif

        #endregion

        #region 公用方法
        /// <summary>
        /// 載入報表物件
        /// </summary>
        public void Load()
        {
            try
            {
                //讀取報表資料流
                using (StreamReader reader = new StreamReader(_BomPath, System.Text.Encoding.Default))
                {
                    string errorString = string.Empty;//錯誤提示
                    string line;//資料行內容
                    int number = 0;//資料行位置
                    //讀取資料行
                    while ((line = reader.ReadLine()) != null)
                    {
                        //標頭 0~2 是模型資訊
                        if (number > 2 && line != "")
                        {
                            string[] data = line.Split(','); //資料表欄位陣列
                            string strTtype = data[0].Replace("\f", "");//反射類型在報表逗點第一次出現前的欄位
                            Type type = assembly.GetType($"GD_STD.Data.{strTtype}"); //找尋報表指定 type

                            if (type == null) //反射不到 type 結束處理
                            {
                                MessageBox.Show($"轉換失敗因 {strTtype} 不是有效物件。資料行在{number}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                                return;//離開函式
                            }

                            object obj = Activator.CreateInstance(type); // 建立指定 type 物件
                            bool error = GetObject(data, number, obj); //轉換物件
                            Type objType = obj.GetType();

                            if (error) //如果轉換失敗
                            {
                                return; //離開函式
                            }
                            else if (objType == typeof(SteelAssembly))//如果是構件
                            {
                                Father = (SteelAssembly)obj;//存取物件報表是樹狀圖的所以需要用到此物件的部分資訊
                                var steels = SteelAssemblies.Where(el => el.Number == Father.Number); //只篩選構件編號
                                if (steels.Count() > 0) //如果找到相同物件
                                {
                                    if (steels.ElementAt(0).GetHashCode() == Father.GetHashCode()) //使用雜湊確認合併物件
                                    {
                                        steels.ElementAt(0).Merge(Father);//合併物件
                                    }
                                    else
                                    {
                                        MessageBox.Show($"雜湊不相符，在資料行 {number} 。\n請檢查報表是否有相同物件，\n但長度 or 寬度 or 高度 or 單重 or 單面積 or 圖紙名稱不相符。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                                        return;
                                    }
                                }
                                else //如果找不到相同物件
                                {
                                    SteelAssemblies.Add(Father);//加入物件
                                }
                            }
                            else
                            {
                                string key = ((IProfile)obj).Profile; //字典 key 
                                if (KeyValuePairs.ContainsKey(key))//如果找到相同的 key
                                {
                                    int index = 0;//索引位置
                                    if (obj.GetType() == typeof(SteelPart)) //如果類型是 SteelPart
                                    {
                                        index = KeyValuePairs[key].FindIndex(el => el.GetType() == typeof(SteelPart) && ((SteelPart)el).Number == ((SteelPart)obj).Number);//找出字典檔內物件
                                        SteelPart part = (SteelPart)obj; //轉換單零件
                                        //判斷需要加入的斷面規格類型
                                        if (part.Type == OBJETC_TYPE.BH ||
                                            part.Type == OBJETC_TYPE.BOX ||
                                            part.Type == OBJETC_TYPE.CH ||
                                            part.Type == OBJETC_TYPE.RH ||
                                            part.Type == OBJETC_TYPE.L)
                                        {
                                            if (Profile[part.Type].FindIndex(el => el.Profile == part.Profile) ==-1)//如果模型找不到相同的斷面規格
                                            {
                                                /*自動新增斷面規格*/
                                                SteelAttr att = new SteelAttr((SteelPart)part); //轉換斷面規格設定檔
                                                Profile[part.Type].Add(att);//加入到列表內
                                                _LackProfile = true;
                                            }
                                            if (Material.FindIndex(el => el.Name == part.Material) == -1) //如果沒有模型材質列表沒有相對應的材質
                                            {
                                                Material.Add(new SteelMaterial { Name = part.Material });//加入到材質
                                                _LackMaterial = true;
                                            }
                                        }
                                    }
                                    else //如果類型是 SteelBolt
                                    {
                                        index = KeyValuePairs[key].FindIndex(el => el.GetType() == typeof(SteelBolts) && ((SteelBolts)el).Profile == ((SteelBolts)obj).Profile);//找出字典檔內物件
                                    }
                                    if (index == -1) //如果找不到物件
                                    {
                                        KeyValuePairs[key].Add(obj);//將物件加入字典
                                    }
                                    else
                                    {
                                        if (KeyValuePairs[key][index].GetHashCode() == obj.GetHashCode())//使用雜湊確認合併物件
                                        {
                                            ((IMerge)KeyValuePairs[key][index]).Merge(obj);//合併物件
                                        }
                                        else
                                        {
                                            MessageBox.Show($"雜湊不相符，在資料行 {number} 。\n請檢查報表是否有相同物件，\n但長度 or 寬度 or 高度 or 單重 or 單面積 or 圖紙名稱不相符。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                                            return;
                                        }
                                    }
                                }
                                else//如果找不到相同的 key
                                {
                                    KeyValuePairs.Add(key, new ObservableCollection<object> { obj });
                                    if (obj.GetType() == typeof(SteelPart))//只存入 SteelPart 的 Profile 
                                        ProfileList.Add(key);
                                }
                            }
                        }
                        number++;
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"{ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                return;
            }
        }
        /// <summary>
        /// 需要加入斷面規格
        /// </summary>
        /// <returns></returns>
        public bool LackProfile()
        {
            return _LackProfile;
        }
        /// <summary>
        /// 需要加入材質
        /// </summary>
        /// <returns></returns>
        public bool LackMaterial()
        {
            return _LackMaterial;
        }
        #endregion
    }
}

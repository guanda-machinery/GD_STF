using devDept.Geometry;
using GD_STD.Base.Additional;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFWindowsBase;
namespace WPFSTD105
{
    /// <summary>
    /// 擴展
    /// </summary>
    public static class Expand
    {
   
        /// <summary>
        /// 結構與類型映射到對應的屬性或欄位中
        /// </summary>
        /// <typeparam name="TResult">要被寫入的反射物件</typeparam>
        /// <param name="value">要讀取的物件</param>
        /// <param name="type">被映射的 Type</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static TResult Reflection<TResult>(object value, Type type)
        {
            object result = Activator.CreateInstance(type); // 建立指定 type 物件
            Type vType = value.GetType();//讀取的物件Type

            var rInfo = vType.GetProperties()
                .AsEnumerable()
                .Union(vType.GetFields()
                .Where(el => el.GetCustomAttribute<MVVMAttribute>().Reflection == true)
                .Select(el => (MemberInfo)el));//讀取的物件屬性列表

            rInfo.ForEach(readInfo =>
            {
                MemberInfo writeInfo = type.GetProperty(readInfo.Name) != null ? (MemberInfo)type.GetProperty(readInfo.Name) : (MemberInfo)type.GetField(readInfo.Name); //要寫入的欄位 or 屬性
                if (writeInfo != null)
                {
                    Type writeInfoType = writeInfo.GetType(); //取得 writeInfo Type (屬性 or 欄位)
                    Type readInfoType = readInfo.GetType(); //取得 readInfo Type (屬性 or 欄位)
                    if (writeInfo != null)
                    {
                        MVVMAttribute mvvmAttribute = writeInfo.GetCustomAttribute<MVVMAttribute>(); //屬性附加參數
                        if (mvvmAttribute != null) //如果找不到附加屬性
                        {
                            object rValue = GetValue(readInfo, value); //要寫入的值
                            if (!mvvmAttribute.IsGroup) //如果這不是自訂結構或類類型
                            {
                                SetValue(writeInfo, result, rValue); //寫入參數
                            }
                            else
                            {
                                Type nextType = GetValue(writeInfo, result).GetType(); //下一個要寫入的 type
                                SetValue(writeInfo, result, Reflection<object>(rValue, nextType));
                            }

                        }
                    }
                    //else
                    //{
                    //    throw new Exception($"{typeof(TResult).ToString()} 找不到對應的 {readInfo.Name}。");
                    //}
                }
            });
            return (TResult)result;
        }
        /// <summary>
        /// 取得成員的值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="data"></param>
        /// <param name="setValue">要存取的值</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static void SetValue(MemberInfo info, object data, object setValue)
        {
            switch (info.MemberType)
            {
                case MemberTypes.TypeInfo:
                case MemberTypes.Custom:
                case MemberTypes.NestedType:
                case MemberTypes.All:
                case MemberTypes.Method:
                case MemberTypes.Constructor:
                case MemberTypes.Event:
                    throw new Exception($"不支援 {info.MemberType}");
                case MemberTypes.Field:
                    FieldInfo field = (FieldInfo)info;
                    field.SetValue(data, setValue);
                    break;
                case MemberTypes.Property:
                    PropertyInfo property = (PropertyInfo)info;
                    property.SetValue(data, setValue);
                    break;
                default:
                    throw new Exception($"不支援 {info.MemberType}");
            }
        }
        /// <summary>
        /// 取得成員的值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object GetValue(MemberInfo info, object data)
        {
            switch (info.MemberType)
            {
                case MemberTypes.TypeInfo:
                case MemberTypes.Custom:
                case MemberTypes.NestedType:
                case MemberTypes.All:
                case MemberTypes.Method:
                case MemberTypes.Constructor:
                case MemberTypes.Event:
                    throw new Exception($"不支援 {info.MemberType}");
                case MemberTypes.Field:
                    FieldInfo field = (FieldInfo)info;
                    return field.GetValue(data);
                case MemberTypes.Property:
                    PropertyInfo property = (PropertyInfo)info;
                    return property.GetValue(data);
                default:
                    throw new Exception($"不支援 {info.MemberType}");
            }
        }
        /// <summary>
        /// <see cref="System.Drawing.Color"/> To <see cref="System.Windows.Media.Color"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Drawing.Color Color(this System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.R, color.G, color.B); ;
        }
        /// <summary>
        /// <see cref="System.Windows.Media.Color"/> To <see cref="System.Drawing.Color"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color Color(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromRgb(color.R, color.G, color.B); ;
        }
        /// <summary>
        /// 傳回值，這個值表示指定的子字串是否會出現在這個字串陣列內。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="values">要搜尋的字串陣列。</param>
        /// <returns>如果 true 參數出現在這個字串內，或是 value 為空字串 ("")，則為 value，否則為 false。</returns>
        public static bool Contains(this string str, params string[] values)
        {
            foreach (var item in values)
            {
                if (str.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        ///// <summary>
        ///// 轉換 NC 座標
        ///// </summary>
        ///// <param name="p1"></param>
        ///// <returns></returns>
        //public static NcPoint3D ConvertToNcPoint3D(this Point3D p1)
        //{
        //    return new NcPoint3D(p1.X, p1.Y, p1.Z);
        //}
    }
}

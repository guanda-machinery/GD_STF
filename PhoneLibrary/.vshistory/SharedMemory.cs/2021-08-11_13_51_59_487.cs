using GD_STD.Base;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static GD_STD.Phone.MemoryHelper;
namespace GD_STD.Phone
{
    /// <summary>
    /// 受保護的操作 Codesys 記憶體的處理方法
    /// </summary>
    /// <revisionHistory>
    ///     <revision date="2021-07-23" version="1.0.0.2" author="LogicYeh">
    ///         <list type="bullet">
    ///             <item>刪除</item> 
    ///             <item>SharedMemory&lt;T&gt;.SetValue(T)</item>
    ///             <item>SharedMemory&lt;T&gt;.GetValue() </item>
    ///             <item>新增</item>
    ///             <item><see cref="GetValue{T}"/> </item>
    ///             <item><see cref="SetValue{T}(T)"/></item>
    ///             <item></item>
    ///         </list>
    ///     </revision>
    /// </revisionHistory>
    public static class SharedMemory
    {
        /// <summary>
        /// 讀取記憶體
        /// </summary>
        /// <typeparam name="T">讀取記憶體結構型態</typeparam>
        /// <returns>讀取 <typeparamref name="T"/> Codesys 的共享記憶體結構</returns>
        public static T GetValue<T>() where T : ISharedMemory, new()
        {
            T result = new T();
            result.ReadMemory();
            return result;
        }

        /// <summary>
        /// 寫入記憶體
        /// </summary>
        /// <typeparam name="T">寫入記憶體結構型態</typeparam>
        /// <param name="value">要寫入的值</param>
        public static void SetValue<T>(T value) where T : struct, IPhoneSharedMemory
        {
            value.WriteMemory();
        }
        /// <summary>
        /// 寫入參數到記憶體指定位元中
        /// </summary>
        /// <typeparam name="T">記憶體結構型態</typeparam>
        /// <typeparam name="TVaule">寫入的結構型別</typeparam>
        /// <param name="position">會在此處開始寫入存取子的位元組數。</param>
        /// <param name="value">要寫入的結構。</param>
        /// <returns><typeparamref name="T"/></returns>
        public static void SetValue<T>(long position, byte[] value) where T : IPhoneSharedMemoryOffset, new() 
        {
            T result = new T();
            result.WriteMemory(position, value);
        }
        /// <summary>
        /// 讀取記憶體
        /// </summary>
        /// <typeparam name="T">繼承 <see cref="ISharedMemoryOffset"/> 類型</typeparam>
        /// <typeparam name="TResult">從 Unmanaged 記憶體區塊封送處理資料到新配置的指定類型的 Managed 物件</typeparam>
        /// <param name="position">要在存取子中開始讀取的位元組數。</param>
        /// <param name="size">要從存取子讀取的類型 T 的結構數目。</param>
        /// <returns></returns>
        public static TResult GetValue<T, TResult>(long position, int size) where T : ISharedMemoryOffset, new() where TResult : struct
        {
            T result = new T();
            return result.ReadMemory<TResult>(position, size);
        }


        /// <summary>
        /// 取得 type 欄位的 offset 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static long GetMemoryOffset(Type type, string fieldName)
        {
            if (!Ban)
                throw new Exception("使用失敗,因參數調用失敗。");

            long result = 0;
            string mainField;
            string nextField;
            GetFieldString(fieldName, out mainField, out nextField);

            MatchCollection matches = Regex.Matches(mainField, @"(\W[\w*[0-9a-zA-Z])");
            if (matches.Count == 1)
            {
                int index = mainField.IndexOf('[');
                mainField = mainField.Substring(0, index);
                FieldInfo fieldInfos = type.GetField(mainField);
                int arrayIndex;//指定欄位的陣列位置
                string strArrayIndex = matches[0].Value.Substring(1);
                int.TryParse(strArrayIndex, out arrayIndex);
                result += fieldInfos.FieldType.GetElementType() == typeof(bool) ? sizeof(bool) * arrayIndex : Marshal.SizeOf(fieldInfos.FieldType.GetElementType()) * arrayIndex;
                object nextObj = Activator.CreateInstance(fieldInfos.FieldType.GetElementType());
                result += GetMemoryOffset(nextObj.GetType(), nextField);
            }
            else if (nextField != null)
            {
                FieldInfo fieldInfos = type.GetField(mainField);
                object nextObj = Activator.CreateInstance(fieldInfos.FieldType);
                result += GetMemoryOffset(nextObj.GetType(), nextField);
            }
            else
            {
                result += Marshal.OffsetOf(type, mainField).ToInt64();
            }
            return result;
        }

        private static void GetFieldString(string fieldName, out string mainField, out string nextField)
        {
            string[] vs = fieldName.Split('.');
            mainField = vs[0];
            nextField = null;
            //如果有下一層
            if (vs.Length >= 2)
                nextField = vs.ToList().GetRange(1, vs.Length - 1).Aggregate((str1, str2) => $"{str1}.{str2}");
        }
        ///// <summary>
        ///// 下一個欄位
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="fieldName"></param>
        ///// <returns></returns>
        //public static long Next(Type type, string fieldName)
        //{
        //    object obj = Activator.CreateInstance(type);
        //    long result = 0;
        //    string[] split;
        //    string nextField;
        //    GetFieldString(fieldName, out split, out nextField);
        //    if (fieldName != "")
        //    {
        //        result += GetMemoryOffset((IPhoneSharedMemoryOffset)obj, fieldName);
        //    }
        //    return result;
        //}
    }
}

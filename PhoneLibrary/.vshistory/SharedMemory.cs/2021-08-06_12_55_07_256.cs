using GD_STD.Base;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

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
        public static void SetValue<T>(T value) where T : IPhoneSharedMemory, new()
        {
            value.WriteMemory();
        }
        /// <summary>
        /// 寫入記憶體
        /// </summary>
        /// <typeparam name="T">記憶體結構型態</typeparam>
        /// <typeparam name="TVaule">寫入的結構型別</typeparam>
        /// <param name="position">會在此處開始寫入存取子的位元組數。</param>
        /// <param name="value">要寫入的結構。</param>
        /// <param name="valueOffset">要從 T 位元組中開始寫入的索引</param>
        /// <param name="count">要從 T 位元組中要寫入的結構數目</param>
        /// <returns><typeparamref name="T"/></returns>
        public static T SetValue<T, TVaule>(long position, TVaule value, int valueOffset, int count) where T : IPhoneSharedMemoryOffset, ISharedMemory, new() where TVaule : struct, IPCSharedMemoryOffset
        {
            T result = new T();
            result.WriteMemory(position, value, valueOffset, count);
            result.ReadMemory();
            return result;
        }
        /// <summary>
        /// 物件序列化 <see cref="byte"/>[] 
        /// </summary>
        /// <param name="obj">物件</param>
        /// <returns></returns>
        public static byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;

            int size = Marshal.SizeOf(obj);
            byte[] result = new byte[size];
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, intPtr, true);
            Marshal.Copy(intPtr, result, 0, size);
            Marshal.FreeHGlobal(intPtr);
            return result;
        }
        /// <summary>
        /// 將<see cref="byte"/>[] 轉 <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">反序列化的結構</typeparam>
        /// <param name="data">序列化資料</param>
        /// <returns></returns>
        public static T FromByteArray<T>(byte[] data)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(data, 0, intPtr, size);
            T result = (T)Marshal.PtrToStructure(intPtr, typeof(T));
            Marshal.FreeHGlobal(intPtr);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static long GetMemoryOffset<T>(T obj, string fieldName)
        {
            if (obj.GetType().GetInterface($"{nameof(IPhoneSharedMemoryOffset)}") == null)
            {
                throw new Exception($"{obj.GetType()} 找不到 {nameof(IPhoneSharedMemoryOffset)} 的介面 ");
            }
            long result = 0;

            FieldInfo[] fieldInfos = obj.GetType().GetFields();
            string[] split;
            string nextField;
            GetFieldString(fieldName, out split, out nextField);

            foreach (var field in fieldInfos)
            {
                if (field.FieldType.IsArray)
                {
                    int arrayLength;//欄位陣列的總長
                    int arrayIndex;//指定欄位的陣列位置
                    Assembly assembly = field.FieldType.Assembly;
                    Type arrayType = assembly.GetType(field.FieldType.FullName.Replace("[]", ""));

                    arrayLength = obj.ArrayLength(field.Name);
                    MatchCollection matches = Regex.Matches(split[0], @"[0-9]+?");

                    int.TryParse(matches[matches.Count - 1].Value, out arrayIndex);
                    result += Marshal.SizeOf(arrayType) * arrayIndex;
                    if (split[0] == $"{field.Name}[{arrayIndex}]")
                    {
                        if (nextField != "")
                        {
                            IPhoneSharedMemoryOffset nextObj = (IPhoneSharedMemoryOffset)Activator.CreateInstance(arrayType);
                            return result += GetMemoryOffset(nextObj, nextField);
                        }
                        return result;
                    }
                }
                if (field.Name == fieldName)
                {
                    return result;
                }
                else
                {
                    result += Marshal.SizeOf(field.FieldType);
                }
            }
            return result;
        }

        private static void GetFieldString(string fieldName, out string[] split, out string nextField)
        {
            split = fieldName.Split('.');
            nextField = string.Empty;
            //如果有下一層
            if (split.Length >= 2)
                nextField = split.ToList().GetRange(1, split.Length - 1).Aggregate((str1, str2) => $"{str1}.{str2}");
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

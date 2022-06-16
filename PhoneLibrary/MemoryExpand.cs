using GD_STD.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.Phone.MemoryHelper;
namespace GD_STD.Phone
{
    /// <summary>
    /// 記憶體擴展方法
    /// </summary>
    public static class MemoryExpand
    {
        /// <summary>
        /// 文字轉 <see cref="byte"/>[]
        /// </summary>
        /// <param name="str"></param>
        /// <param name="arrayLength"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str, int arrayLength)
        {
            if (str.Length > arrayLength)
            {
                throw new MemoryException("內容長度不可以大於 arrayLength。");
            }
            byte[] result = new byte[arrayLength];

            byte[] vs = str.ToArray().ToByteArray();

            Array.Copy(vs, result, vs.Length);

            return result;
        }
        /// <summary>
        /// 文字轉 <see cref="UInt16"/>[]
        /// </summary>
        /// <param name="str">內容</param>
        /// <param name="arrayLength">陣列長度</param>
        /// <returns></returns>
        public static ushort[] GetUInt16(this string str, int arrayLength)
        {
#if !LogicYeh
            if (!Ban)
                throw new Exception("使用失敗,因不是測試專案。");
#endif
            if (str.Length > arrayLength)
            {
                throw new MemoryException("內容長度不可以大於 arrayLength。");
            }
            ushort[] result = new ushort[arrayLength];

            for (int i = 0; i < str.Length; i++)
            {
                result[i] = str[i];
                Debug.WriteLine(result[i]);
            }

            return result;
        }
        /// <summary>
        ///  獲取 <see cref="Type"/> 指定欄位 <see cref="MarshalAsAttribute.SizeConst"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns>回傳 Unmanaged 陣列長度</returns>
        public static int ArrayLength(this Type type, string fieldName)
        {
#if !LogicYeh
            if (!Ban)
                throw new Exception("使用失敗,因不是測試專案。");
#endif
            var field = type.GetField(fieldName);

            if (field.FieldType.IsArray)
            {
                MarshalAsAttribute marshalAsAttribute = field.GetCustomAttribute<MarshalAsAttribute>();
                return marshalAsAttribute.SizeConst;
            }
            throw new MemoryException($"錯誤，此 {fieldName} 欄位不是陣列");
        }
        /// <summary>
        ///  <typeparamref name="T"/> 轉換 <see cref="byte"/>[]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToByteArray<T>(this T obj)
        {
            lock (obj)
            {
#if !LogicYeh
            if (!Ban)
                throw new Exception("使用失敗,因不是測試專案。");
#endif
                if (obj == null)
                    return null;

                int size;
                IntPtr intPtr;
                byte[] result;
                if (obj.GetType().IsArray)
                {
                    var array = ((IEnumerable)obj).Cast<object>();
                    Type arrayType = obj.GetType().GetElementType();
                    size = typeof(bool) == obj.GetType().GetElementType() ? sizeof(bool) : Marshal.SizeOf(obj.GetType().GetElementType()) * array.Count();
                    result = new byte[size];
                    intPtr = Marshal.AllocHGlobal(size);
                    for (int i = 0; i < array.Count(); i++)
                    {
                        byte[] value = array.ElementAt(i).ToByteArray();
                        for (int c = 0; c < value.Count(); c++)
                        {
                            Marshal.WriteByte(intPtr, i * Marshal.SizeOf(arrayType) + c, value[c]);
                        }
                    }
                }
                else if (obj.GetType() == typeof(string))
                {
                    string str = obj.ToString();
                    ushort[] uStr = str.GetUInt16(str.Length);
                    result = uStr.ToByteArray();
                    return result;
                }
                else
                {
                    size = Marshal.SizeOf(obj.GetType());
                    result = new byte[size];
                    intPtr = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(obj, intPtr, true);
                }
                Marshal.Copy(intPtr, result, 0, size);
                Marshal.FreeHGlobal(intPtr);
                return result;
            }
        }
        /// <summary>
        /// <see cref="byte"/>[] 轉換 <typeparamref name="T"/>
        /// </summary>
        /// <typeparamref name="T"/>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T FromByteArray<T>(this byte[] data)
        {
#if !LogicYeh
            if (!Ban)
                throw new Exception("使用失敗,因不是測試專案。");
#endif
            int size = Marshal.SizeOf(typeof(T));
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(data, 0, intPtr, size);
            T result = (T)Marshal.PtrToStructure(intPtr, typeof(T));
            Marshal.FreeHGlobal(intPtr);
            return result;
        }

        /// <summary>
        /// 取得欄位 byte[] 長度
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName">欄位名稱</param>
        /// <returns></returns>
        public static int GetSizeof(this Type type, string fieldName)
        {
#if !LogicYeh
            if (!Ban)
                throw new Exception("使用失敗,因不是測試專案。");
#endif
            var field = type.GetField(fieldName);
            if (field.FieldType.IsArray)
            {
                return Marshal.SizeOf(field.FieldType.GetElementType()) * type.ArrayLength(fieldName);
            }
            else
            {
                return Marshal.SizeOf(field.FieldType);
            }
        }
    }
}

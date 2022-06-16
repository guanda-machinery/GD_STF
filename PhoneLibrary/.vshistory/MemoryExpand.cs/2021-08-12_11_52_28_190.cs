using GD_STD.Base;
using System;
using System.Collections;
using System.Collections.Generic;
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
        ///  獲取 <typeparamref name="T"/> 指定欄位 <see cref="MarshalAsAttribute.SizeConst"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns>回傳 Unmanaged 陣列長度</returns>
        public static int ArrayLength<T>(this T obj, string fieldName)
        {
            if (!Ban)
                throw new Exception("使用失敗,因不是測試失敗。");

            var field = obj.GetType().GetField(fieldName);

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
            if (!Ban)
                throw new Exception("使用失敗,因不是測試失敗。");

            if (obj == null)
                return null;

            int size;
            byte[] result = null;
            IntPtr intPtr;
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
        ///// <summary>
        /////  <typeparamref name="T"/> 轉換 <see cref="byte"/>[]
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static byte[] ToByteArray<T>(this T obj)
        //{
        //    if (obj == null)
        //        return null;

        //    int size = Marshal.SizeOf(obj);
        //    byte[] result = new byte[size];
        //    IntPtr intPtr = Marshal.AllocHGlobal(size);
        //    Marshal.StructureToPtr(obj, intPtr, true);
        //    Marshal.Copy(intPtr, result, 0, size);
        //    Marshal.FreeHGlobal(intPtr);
        //    return result;
        //}
        /// <summary>
        /// <see cref="byte"/>[] 轉換 <typeparamref name="T"/>
        /// </summary>
        /// <typeparamref name="T"/>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T FromByteArray<T>(this byte[] data)
        {
            if (!Ban)
                throw new Exception("使用失敗,因不是測試失敗。");

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
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldName">欄位名稱</param>
        /// <returns></returns>
        public static int GetSizeof<T>(this T obj, string fieldName) where T : struct
        {
            if (!Ban)
                throw new Exception("使用失敗,因不是測試失敗。");

            var field = obj.GetType().GetField(fieldName);
            if (field.FieldType.IsArray)
            {
                return Marshal.SizeOf(field.FieldType.GetElementType()) * obj.ArrayLength(fieldName);
            }
            else
            {
                return Marshal.SizeOf(field.FieldType);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
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
        /// <see cref="byte"/>[] 轉換 <typeparamref name="T"/>
        /// </summary>
        /// <typeparamref name="T"/>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T FromByteArray<T>(this byte[] data)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr intPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(data, 0, intPtr, size);
            T result = (T)Marshal.PtrToStructure(intPtr, typeof(T));
            Marshal.FreeHGlobal(intPtr);

            return result;
        }

    }
}

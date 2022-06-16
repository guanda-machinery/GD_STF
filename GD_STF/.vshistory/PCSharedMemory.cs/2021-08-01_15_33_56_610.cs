using System;
using System.Runtime.InteropServices;

namespace GD_STD
{
    /// <summary>
    /// 操作 Codesys 記憶體的處理方法
    /// </summary>
    /// <typeparam name="T">要寫入或讀取記憶體的物件</typeparam>
    public static class SharedMemory<T> where T : Base.ISharedMemory, new()
    {
        /// <summary>  
        /// 讀取記憶體
        /// </summary>
        /// <returns>讀取 <typeparamref name="T"/> Codesys 的共享記憶體</returns>
        public static T GetValue()
        {
            T result = new T();
            result.ReadMemory();
            return result;
        }
        /// <summary>
        /// 寫入記憶體
        /// </summary>
        /// <param name="Value">要寫入的值</param>
        public static void SetValue(T Value)
        {
            Value.WriteMemory();
        }
        /// <summary>
        /// 物件轉為 <see cref="byte"/>[] 
        /// </summary>
        /// <param name="obj">物件</param>
        /// <returns></returns>
        public static byte[] ToByteArray(T obj)
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
        /// <param name="data"></param>
        /// <returns></returns>
        public static T FromByteArray(byte[] data)
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
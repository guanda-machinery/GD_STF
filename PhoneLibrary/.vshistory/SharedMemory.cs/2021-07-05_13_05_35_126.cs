using GD_STD.Phone;
using System;
using System.Runtime.InteropServices;

namespace GD_STD.Phone
{
    /// <summary>
    /// 操作 Codesys 記憶體的處理方法
    /// </summary>
    /// <typeparam name="T">要寫入或讀取記憶體的物件</typeparam>
    public static class SharedMemory<T> where T : ISharedMemory, new()
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
    }
}

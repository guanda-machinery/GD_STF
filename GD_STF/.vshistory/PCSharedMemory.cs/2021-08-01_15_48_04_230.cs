using System;
using System.Runtime.InteropServices;

namespace GD_STD.Base
{
    /// <summary>
    /// 不受到保護的操作 Codesys 記憶體的處理方法
    /// </summary>
    public static partial class SharedMemory
    {
        /// <summary>
        /// 寫入記憶體
        /// </summary>
        /// <typeparam name="T">寫入記憶體結構型態</typeparam>
        /// <param name="value">要寫入的值</param>
        public static void SetValue<T>(T value) where T : IPCSharedMemory, new()
        {
            value.WriteMemory();
        }
        ///// <summary>  
        ///// 讀取記憶體
        ///// </summary>
        ///// <returns>讀取 <typeparamref name="T"/> Codesys 的共享記憶體</returns>
        //public static T GetValue()
        //{
        //    T result = new T();
        //    result.ReadMemory();
        //    return result;
        //}
        ///// <summary>
        ///// 寫入記憶體
        ///// </summary>
        ///// <param name="Value">要寫入的值</param>
        //public static void SetValue(T Value)
        //{
        //    Value.WriteMemory();
        //}

    }
}
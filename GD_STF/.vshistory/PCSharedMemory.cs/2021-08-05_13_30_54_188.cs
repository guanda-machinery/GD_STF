using GD_STD.Base;
using System;
using System.Runtime.InteropServices;

namespace GD_STD
{
    /// <summary>
    /// 不受到保護的操作 Codesys 記憶體的處理方法
    /// </summary>
    public static class PCSharedMemory
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
        /// <summary>
        /// 讀取記憶體
        /// </summary>
        /// <typeparam name="T">讀取記憶體結構型態</typeparam>
        /// <returns>
        /// 讀取 <typeparamref name="T"/> Codesys 的共享記憶體結構
        /// <para><see cref="SharedMemory.GetValue{T}"/>相同</para>
        /// </returns>
        public static T GetValue<T>() where T : ISharedMemory, new()
        {
            T result = new T();
            result.ReadMemory();
            return result;
        }
    }
}
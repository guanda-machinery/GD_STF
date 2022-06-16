using GD_STD.Base;
using GD_STD.Phone;
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
        /// <summary>
        /// 寫入參數到記憶體指定位元中
        /// </summary>
        /// <typeparam name="T">記憶體結構型態</typeparam>
        /// <typeparam name="TVaule">寫入的結構型別</typeparam>
        /// <param name="position">會在此處開始寫入存取子的位元組數。</param>
        /// <param name="value">要寫入的結構。</param>
        /// <returns><typeparamref name="T"/></returns>
        public static T SetValue<T, TVaule>(long position, TVaule value) where T : IPCSharedMemoryOffset, ISharedMemory, new() where TVaule : struct
        {
            T result = new T();
            result.WriteMemory<TVaule>(position, value);
            result.ReadMemory();
            return result;
        }
    }
}
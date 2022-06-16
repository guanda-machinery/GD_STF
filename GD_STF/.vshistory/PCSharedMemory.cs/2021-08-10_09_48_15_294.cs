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
        /// <param name="position">會在此處開始寫入存取子的位元組數。</param>
        /// <param name="value">要寫入的結構。</param>
        /// <returns><typeparamref name="T"/></returns>
        public static void SetValue<T>(long position, object value) where T : IPCSharedMemoryOffset, new()
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
        /// <returns>
        /// 讀取 <typeparamref name="T"/> Codesys 的共享記憶體結構
        /// <para><see cref="SharedMemory.GetValue{T, TResult}(long, int)"/>相同</para>
        /// </returns>
        public static TResult GetValue<T, TResult>(long position, int size) where T : ISharedMemoryOffset, new() 
        {
            T result = new T();
            return result.ReadMemory<TResult>(position, size);
        }
    }
}
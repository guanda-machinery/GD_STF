using GD_STD.Phone;
using System;
using System.Runtime.InteropServices;

namespace GD_STD.Phone
{
    /// <summary>
    /// 操作 Codesys 記憶體的處理方法
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
    ///             <item><see cref="SetValue{T, TVaule}(long, TVaule)"/></item>
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
        public static void SetValue<T>(T value) where T : ISharedMemory, new()
        {
            value.WriteMemory();
        }
        /// <summary>
        /// 寫入記憶體
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TVaule"></typeparam>
        /// <param name="position"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T SetValue<T, TVaule>(long position, TVaule value) where T : ISharedMemoryOffset, ISharedMemory, new() where TVaule : struct
        {
            T result = new T();
            result.WriteMemory(position, value);
            result.ReadMemory();
            return result;
        }
    }
    ///// <summary>
    ///// 操作 Codesys 記憶體的處理方法
    ///// </summary>
    ///// <typeparam name="T">要寫入或讀取記憶體的物件</typeparam>
    //public static class SharedMemoryOffset<T, TValue> where T : ISharedMemoryOffset<T, TValue>, new()
    //{
    //    /// <summary>  
    //    /// 讀取記憶體
    //    /// </summary>
    //    /// <returns>讀取 <typeparamref name="T"/> Codesys 的共享記憶體</returns>
    //    public static T GetValue()
    //    {
    //        T result = new T();
    //        result.ReadMemory();
    //        return result;
    //    }
    //    /// <summary>
    //    /// 寫入記憶體
    //    /// </summary>
    //    /// <param name="Value">要寫入的值</param>
    //    public static void SetValue(T Value)
    //    {
    //        Value.WriteMemory();
    //    }
    //}
}

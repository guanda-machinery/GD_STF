using GD_STD.Base;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static GD_STD.Phone.MemoryHelper;
namespace GD_STD.Phone
{
    /// <summary>
    /// 受保護的操作 Codesys 記憶體的處理方法
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
    ///             <item><see cref="GetValue{T, TResult}(long, int)"/> </item>
    ///             <item><see cref="SetValue{T}(long, byte[])"/> </item>
    ///             <item></item>
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
        public static void SetValue<T>(T value) where T : struct, IPhoneSharedMemory
        {
            value.WriteMemory();
        }
        /// <summary>
        /// 寫入參數到記憶體指定位元中
        /// </summary>
        /// <typeparam name="T">記憶體結構型態</typeparam>
        /// <param name="position">會在此處開始寫入存取子的位元組數。</param>
        /// <param name="value">要寫入的結構。</param>
        /// <returns><typeparamref name="T"/></returns>
        public static void SetValue<T>(long position, byte[] value) where T : IPhoneSharedMemoryOffset, new()
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
        /// <returns></returns>
        public static TResult GetValue<T, TResult>(long position, int size) where T : ISharedMemoryOffset, new() where TResult : struct
        {
            T result = new T();
            return result.ReadMemory<TResult>(position, size);
        }
        /// <summary>
        /// 取得讀取指針
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static MemoryMappedViewAccessor GetReadView<T>() where T : struct, IMemoryPointer
        {
#if !LogicYeh
            if (!Ban)
                throw new Exception("使用失敗,因不是測試專案。");
#endif
            T result = new T();
            return result.ReadView();
        }
        private static MemoryMappedViewAccessor _View;
        /// <summary>
        /// 讀取 <typeparamref name="T"/> 記憶體內值定的位置，並使用 unsafe 轉換 <see cref="char"/>[] 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="offset"></param>
        /// <param name="readLength"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static char[] ReadChar<T>(long offset, int readLength, string fieldName) where T : struct, IMemoryPointer
        {
#if !LogicYeh
            if (!Ban)
                throw new Exception("使用失敗,因不是測試專案。");
#endif
            //using (MemoryMappedViewAccessor view = GetReadView<T>())
            //{
            IntPtr intPtr = _View.SafeMemoryMappedViewHandle.DangerousGetHandle(); //記憶體指標
            FieldInfo info = typeof(T).GetField(fieldName);
            if (info == null)
                throw new Exception("找不到指定欄位。");

            Type elementType = typeof(T).GetField(fieldName).FieldType.GetElementType(); //陣列節點 type
            int elementSize = Marshal.SizeOf(elementType); //陣列節點 sizeof
            char[] buffer = new char[readLength]; //讀取資料長度
            unsafe
            {
                byte* ptr = (byte*)intPtr.ToPointer(); //void*
                for (int i = 0; i < readLength; i++)  //逐步執行讀取記憶體內的 byte
                {
                    fixed (void* p = &buffer[i]) //  p 指標指向 buffer[i]
                    {
                        GetVoid(p, ptr, offset, elementSize * i);
                        //byte* p1 = (byte*)p;//切出 buffer[i] byte位置
                        //for (int c = 0; c < elementSize; c++) //塞入到 
                        //{
                        //    p1[c] = ptr[offset + i * elementSize + c];
                        //}
                    }
                }
            }
            _View.Dispose();
            return buffer;
            //}
        }
        /// <summary>
        /// 取得無型別指標
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ptr"></param>
        /// <param name="offset"></param>
        /// <param name="elementSize"></param>
        public static unsafe void GetVoid(void* p, byte* ptr, long offset, int elementSize)
        {
#if !LogicYeh
            if (!Ban)
                throw new Exception("使用失敗,因不是測試專案。");
#endif
            byte* p1 = (byte*)p;//切出 buffer[i] byte位置
            for (int c = 0; c < elementSize; c++) //塞入到 
            {
                p1[c] = ptr[offset + elementSize + c];
            }
        }
        /// <summary>
        /// 取得 type 欄位的 offset 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static long GetMemoryOffset(Type type, string fieldName)
        {
#if !LogicYeh
            if (!Ban)
                throw new Exception("使用失敗,因不是測試專案。");
#endif
            long result = 0;
            GetFieldString(fieldName, out string mainField, out string nextField);

            MatchCollection matches = Regex.Matches(mainField, @"(\W[\w*[0-9a-zA-Z])");
            if (matches.Count == 1)
            {
                int index = mainField.IndexOf('[');
                mainField = mainField.Substring(0, index);
                FieldInfo fieldInfos = type.GetField(mainField);
                string strArrayIndex = matches[0].Value.Substring(1);
                int.TryParse(strArrayIndex, out int arrayIndex); //指定欄位的陣列位置
                result += fieldInfos.FieldType.GetElementType() == typeof(bool) ? sizeof(bool) * arrayIndex : Marshal.SizeOf(fieldInfos.FieldType.GetElementType()) * arrayIndex;
                object nextObj = Activator.CreateInstance(fieldInfos.FieldType.GetElementType());
                result += GetMemoryOffset(nextObj.GetType(), nextField);
            }
            else if (nextField != null)
            {
                FieldInfo fieldInfos = type.GetField(mainField);
                object nextObj = Activator.CreateInstance(fieldInfos.FieldType);
                result += GetMemoryOffset(nextObj.GetType(), nextField);
            }
            else
            {
                result += Marshal.OffsetOf(type, mainField).ToInt64();
            }
            return result;
        }

        private static void GetFieldString(string fieldName, out string mainField, out string nextField)
        {
#if !LogicYeh
            if (!Ban)
                throw new Exception("使用失敗,因不是測試專案。");
#endif
            string[] vs = fieldName.Split('.');
            mainField = vs[0];
            nextField = null;
            //如果有下一層
            if (vs.Length >= 2)
                nextField = vs.ToList().GetRange(1, vs.Length - 1).Aggregate((str1, str2) => $"{str1}.{str2}");
        }

    }
}

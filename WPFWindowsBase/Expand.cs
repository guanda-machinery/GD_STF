using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Management;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace WPFWindowsBase
{
    /// <summary>
    /// 外部擴展
    /// </summary>
    public static class Expand
    {
    
        /// <summary>
        /// 在<see cref="IEnumerable{T}"/>的每一個項目上執行指定之動作。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action">要在 System.Collections.Generic.List`1 的每一個項目上執行的 System.Action`1 委派。</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }
        /// <summary>
        /// 創建匿名 <see cref="List{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static List<T> CreateAnonymousList<T>(params T[] elements)
        {
            return new List<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Task<T> StartSTATask<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            Thread thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        ///// <summary>
        ///// <see cref="DataSet"/>轉換byte[] 字節
        ///// </summary>
        ///// <param name="vs"></param>
        ///// <param name="ds"></param>
        //public static byte[] DataToByte(this List<byte> vs, DataSet ds)
        //{
        //    byte[] bArrayResult = null;
        //    ds.RemotingFormat = SerializationFormat.Binary;
        //    MemoryStream ms = new MemoryStream();
        //    IFormatter bf = new BinaryFormatter();
        //    bf.Serialize(ms, ds);
        //    bArrayResult = ms.ToArray();
        //    ms.Close();
        //    ms.Dispose();
        //    return bArrayResult;
        //}
        ///// <summary>
        ///// byte[] 字節轉 <see cref="DataSet"/>
        ///// </summary>
        ///// <param name="bArrayResult"></param>
        ///// <returns></returns>
        //public static DataSet ByteToDataSet(this List<byte> bArrayResult)
        //{
        //    DataSet dsResult = new DataSet();
        //    MemoryStream ms = new MemoryStream(bArrayResult.ToArray());
        //    IFormatter bf = new BinaryFormatter();
        //    object obj = bf.Deserialize(ms);
        //    dsResult = (DataSet)obj;
        //    ms.Close();
        //    ms.Dispose();
        //    return dsResult;
        //}
    }
}

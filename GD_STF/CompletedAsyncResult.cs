using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GD_STD
{
    /// <summary>
    /// 代表非同步作業的狀態
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class CompletedAsyncResult<T> : IAsyncResult
    {
        T data;
        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="data"></param>
        public CompletedAsyncResult(T data)
        {
            this.data = data;
        }
        /// <summary>
        /// 回傳資料
        /// </summary>
        [DataMember]
        public T Data
        {
            get { return data; }
        }

        /// <inheritdoc/>
        [DataMember]
        public object AsyncState
        {
            get { return (object)data; }
        }
        /// <inheritdoc/>
        [DataMember]
        public WaitHandle AsyncWaitHandle
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        /// <inheritdoc/>
        [DataMember]
        public bool CompletedSynchronously
        {
            get { return true; }
        }
        /// <inheritdoc/>
        [DataMember]
        public bool IsCompleted
        {
            get { return true; }
        }
    }
}

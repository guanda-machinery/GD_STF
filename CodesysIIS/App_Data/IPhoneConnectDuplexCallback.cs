using System;
using System.Collections.Generic;
using System.ServiceModel;
using GD_STD.Enum;
using GD_STD.Phone;
using Wsdl;

namespace CodesysIIS
{
    /// <summary>
    /// 手機連線雙向通道
    /// </summary>
    public interface IPhoneConnectDuplexCallback
    {
        /// <summary>
        /// 回復 Phone 操作功能請求
        /// </summary>
        /// <param name="operating">請求連線狀態</param>
        [OperationContract(IsOneWay = true)]
        void RequestConnection(Operating operating);
        /// <summary>
        /// 回復序列化索引標籤內的 <see cref="WorkMaterial"/> 資訊
        /// </summary>
        /// <param name="index"></param>
        [OperationContract(IsOneWay = true)]
        void SerializationIndex(short[] index);
        /// <summary>
        /// 回復序列 <see cref="WorkMaterial"/>[<see cref="MonitorWork.Current"/>]
        /// </summary>
        /// <param name="current"></param>
        [OperationContract(IsOneWay = true)]
        void SerializationCurrent(short current);
        /// <summary>
        /// 回復 Login 登入訊息
        /// </summary>
        /// <param name="login"></param>
        [OperationContract(IsOneWay = true)]
        void RequestLogin(Login login); 
    }
}

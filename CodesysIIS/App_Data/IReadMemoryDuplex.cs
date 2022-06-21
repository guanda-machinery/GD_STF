using GD_STD.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wsdl;

namespace CodesysIIS
{
    /// <summary>
    /// 讀取 Codesys 連線服務合約
    /// </summary>
    [ServiceContract(Namespace = "http://Codesys.ReadMemoryDuplex", SessionMode = SessionMode.Required, CallbackContract = typeof(IReadMemoryDuplexCallback))]
    public interface IReadMemoryDuplex
    {
        /// <summary>
        /// 取得面板狀態
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void GetPanel();
        /// <summary>
        /// 取得 host 目前訊息
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void GetHost();
        /// <summary>
        /// 取得追隨座標
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void GetMainAxisLocation();
        ///// <summary>
        ///// 暫停面板狀態
        ///// </summary>
        //[OperationContract(IsOneWay = true)]
        //void StopPanel(bool value);
        ///// <summary>
        ///// 暫停 host 目前訊息
        ///// </summary>
        //[OperationContract(IsOneWay = true)]
        //void StopHost(bool value);
        /// <summary>
        /// 刷新 Token
        /// </summary>
        [OperationContract(IsOneWay = false)]
        string GetToken();
        /// <summary>
        /// 啟動遠端登入監聽
        /// </summary>
        /// <param name="run">執行遠端登入監聽</param>
        [OperationContract(IsOneWay = false)]
        void StartLogin(bool run);
    }
    public interface IReadMemoryDuplexCallback
    {
        /// <summary>
        /// 發送面板狀態
        /// </summary>
        /// <summary>
        /// Phone 操作功能請求
        /// </summary>
        /// <param name="panelButton">請求連線狀態</param>
        [OperationContract(IsOneWay = true)]
        void SendPanel(GD_STD.PanelButton panelButton);
        /// <summary>
        /// 發送 Host 訊息
        /// </summary>
        /// <param name="host"></param>
        [OperationContract(IsOneWay = true)]
        void SendHost(GD_STD.Host host);
        /// <summary>
        /// 發送主軸座標
        /// </summary>
        /// <param name="axisLocation"></param>
        [OperationContract(IsOneWay = true)]
        void SendMainAxisLocation(GD_STD.MainAxisLocation axisLocation);
        /// <summary>
        /// 發送登入狀態
        /// </summary>
        /// <param name="login"></param>
        [OperationContract(IsOneWay = true)]
        void SendLogin(Login login);
        ///// <summary>
        ///// 發送等待加工料件
        ///// </summary>
        ///// <param name="workMaterials"></param>
        //[OperationContract(IsOneWay = true)]
        //void SendDrills(WorkMaterial[] workMaterials);
        ///// <summary>
        ///// 發送等待加工料件
        ///// </summary>
        ///// <param name="index"></param>
        //[OperationContract(IsOneWay = true)]
        //void SendIndex(ushort[] index);
    }
}

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
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IMonitorDuplex"。
    [ServiceContract(Namespace = "http://Codesys.MonitorDuplex", SessionMode = SessionMode.Required, CallbackContract = typeof(IMonitorDuplexCallback))]
    public interface IMonitorDuplex
    {
        /// <summary>
        /// 修改監控部分參數
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        [OperationContract(IsOneWay = false)]
        void SetMonitorWorkOffset(byte[] value, [WsdlParamOrReturnDocumentation("偏移量")] long offset);
        ///// <summary>
        ///// 取得加工資訊
        ///// </summary>
        ///// <param name="index">陣列索引位置</param>
        ///// <returns></returns>
        [OperationContract(IsOneWay = false)]
        WorkMaterial GetWorkMaterial(int index);
        ///// <summary>
        ///// 取得 index 列表
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //[OperationContract]
        //[WsdlDocumentation("取得 Index 列表")]
        //[return: WsdlParamOrReturnDocumentation("Index[]")]
        //short[] GetIndexList(int index);
        /// <summary>
        /// 取得 index 列表
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void GetIndex();
    }
    public interface IMonitorDuplexCallback
    {
        /// <summary>
        /// 發送 <see cref="MonitorWork.Index"/>
        /// </summary>
        /// <param name="index"></param>
        [OperationContract(IsOneWay = true)]
        void SendIndex(short[] index);
        /// <summary>
        ///  發送 <see cref="MonitorWork.Current"/>、 
        ///  <see cref="MonitorWork.EntranceOccupy"/>、 
        ///  <see cref="MonitorWork.ExportOccupy1"/>、 
        ///  <see cref="MonitorWork.ExportOccupy2"/>
        /// </summary>
        /// <param name="current"></param>
        /// <param name="enOccupy"></param>
        /// <param name="exOccupy1"></param>
        /// <param name="exOccupy2"></param>
        [OperationContract(IsOneWay = true)]
        void SendOther(short current, double enOccupy, double exOccupy1, double exOccupy2);
    }
}

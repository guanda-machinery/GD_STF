using GD_STD;
using GD_STD.Phone;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Services;
using Wsdl;
using DrillWarehouse = GD_STD.Phone.DrillWarehouse;

namespace CodesysIIS
{
    /// <summary>
    /// 寫入 Codesys 共享記憶體的服務介面
    /// </summary>
    [ServiceContract(Namespace = "http://Codesys.Memory")]
    [WsdlDocumentation("寫入 Codesys 共享記憶體的服務介面")]
    public interface IWriteMemor
    {
        /// <summary>
        /// 修改 Codesys 人機操控面板 IO 狀態
        /// </summary>
        /// <param name="value">要修改的值</param>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "SetPanel")]
        [WsdlDocumentation("修改 Codesys 修改人機操控面板按 IO 狀態")]
        void SetPanel([WsdlParamOrReturnDocumentation("要修改的值")] PanelButton value);

        /// <summary>
        /// 修改 Codesys 刀庫設定
        /// </summary>
        /// <param name="value">要修改的值</param>
        [OperationContract]
        [WsdlDocumentation("修改 Codesys 刀庫設定")]
        void SetDrillWarehouse([WsdlParamOrReturnDocumentation("要修改的值")] GD_STD.DrillWarehouse value);

        /// <summary>
        /// 修改 Codesys 完整油壓系統設定
        /// </summary>
        /// <param name="value">要修改的值</param>
        [OperationContract]
        [WsdlDocumentation("修改 Codesys 完整油壓系統設定")]
        void SetOill([WsdlParamOrReturnDocumentation("要修改的值")] OillSystem value);

        /// <summary>
        /// 修改 Codesys 液壓油系統參數
        /// </summary>
        /// <param name="value">要修改的值</param>
        [OperationContract]
        [WsdlDocumentation("修改 Codesys 液壓油系統參數")]
        void SetHydraulic([WsdlParamOrReturnDocumentation("要修改的值")] OillSystem value);
        /// <summary>
        /// 修改 Codesys 潤滑油系統參數
        /// </summary>
        /// <param name="value">要修改的值</param>
        [OperationContract]
        [WsdlDocumentation("修改 Codesys 潤滑油系統參數")]
        void SetLubricant([WsdlParamOrReturnDocumentation("要修改的值")] OillSystem value);
        /// <summary>
        /// 修改 Codesys 切削油系統參數
        /// </summary>
        /// <param name="value">要修改的值</param>
        [OperationContract]
        [WsdlDocumentation("修改 Codesys 切削油系統參數")]
        void SetCut([WsdlParamOrReturnDocumentation("要修改的值")] OillSystem value);
        /// <summary>
        /// 修改 PC 主機狀態
        /// </summary>
        /// <param name="value">要修改的值</param>
        [OperationContract]
        [WsdlDocumentation("修改 PC 主機狀態")]
        void SetHost([WsdlParamOrReturnDocumentation("要修改的值")] Host value);
        /// <summary>
        /// 修改 Codesys 斷電保持數值
        /// </summary>
        /// <param name="value"></param>
        [OperationContract]
        [WsdlDocumentation("修改 Codesys 斷電保持數值")]
        void SetOutage([WsdlParamOrReturnDocumentation("修改 Codesys 斷電保持數值")] Outage value);
        /// <summary>
        /// 修改 APP 手動操作參數
        /// </summary>
        /// <param name="value"></param>
        [OperationContract]
        [WsdlDocumentation("修改 APP 手動操作")]
        void SetAPPStruct([WsdlParamOrReturnDocumentation("修改 APP 手動操作參數")] GD_STD.Phone.APP_Struct value);
        /// <summary>
        /// 修改 APP 及時監控參數
        /// </summary>
        /// <param name="value"></param>
        [OperationContract]
        [WsdlDocumentation("修改 APP 及時監控參數")]
        void SetAPPMonitorMec([WsdlParamOrReturnDocumentation("修改 APP 及時監控參數")] GD_STD.Phone.MonitorMec value);
        /// <summary>
        /// 修改機械參數設定
        /// </summary>
        /// <param name="value"></param>
        [OperationContract]
        [WsdlDocumentation("修改機械參數設定")]
        void SetMechanicalSetting([WsdlParamOrReturnDocumentation("修改機械設定參數")] GD_STD.Phone.MechanicalSetting value);
        /// <summary>
        /// 修改手機連線狀態
        /// </summary>
        /// <param name="value"></param>
        [OperationContract]
        [WsdlDocumentation("修改手機連線狀態")]
        void SetPhoneOperating([WsdlParamOrReturnDocumentation("修改連線狀態參數")] GD_STD.Phone.Operating value);
        /// <summary>
        /// 修改選配參數
        /// </summary>
        /// <param name="value"></param>
        [OperationContract]
        [WsdlDocumentation("修改手機連線狀態")]
        void SetMecOptional([WsdlParamOrReturnDocumentation("修改選配參數")] GD_STD.Phone.MecOptional value);
        /// <summary>
        /// 修改加工監控部分參數
        /// </summary>
        /// <param name="value">修改參數</param>
        /// <param name="offset">偏移量</param>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "SetMonitorWorkOffset")]
        [WsdlDocumentation("修改加工監控部分參數")]
        void SetMonitorWorkOffset([WsdlParamOrReturnDocumentation("修改參數")] byte[] value, [WsdlParamOrReturnDocumentation("偏移量")] long offset);
        /// <summary>
        /// 修改 <see cref="GD_STD.Phone.WorkMaterial"/>
        /// </summary>
        /// <param name="value">修改參數</param>
        /// <param name="offset">偏移量</param>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "SetWorkMaterial")]
        [WsdlDocumentation("修改 GD_STD.Phone.WorkMaterial")]
        void SetWorkMaterial([WsdlParamOrReturnDocumentation("修改參數")] GD_STD.Phone.WorkMaterial[] value, [WsdlParamOrReturnDocumentation("偏移量")] long offset);
        /// <summary>
        /// 修改手機連線狀態
        /// </summary>
        /// <param name="value"></param>
        [OperationContract]
        [WsdlDocumentation("修改手機連線狀態")]
        void SetLogin([WsdlParamOrReturnDocumentation("修改連線登入狀態")] GD_STD.Phone.Login value);
        [OperationContract]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IWriteMemor.SetAndroidTest(AndroidTest)' 的 XML 註解
        void SetAndroidTest(AndroidTest value);
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IWriteMemor.SetAndroidTest(AndroidTest)' 的 XML 註解
    }
}

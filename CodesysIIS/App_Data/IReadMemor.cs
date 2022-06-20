using GD_STD;
using GD_STD.Phone;
using System.ServiceModel;
using System.ServiceModel.Web;
using Wsdl;

namespace CodesysIIS
{
    /// <summary>
    /// 讀取 Codesys 共享記憶體的服務介面
    /// </summary>
    [ServiceContract(Namespace = "http://Codesys.Memory")]
    [WsdlDocumentation("讀取 Codesys 共享記憶體的服務介面")]
    public interface IReadMemor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WsdlDocumentation("取得工作其他參數")]
        [return: WsdlParamOrReturnDocumentation("回傳當前值，出入口的占用長度。")]
        GD_STD.Base.WorkOther GetWorkOther();
        /// <summary>
        /// 取得加工陣列 Index
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WsdlDocumentation("取得工作陣列 index")]
        [return: WsdlParamOrReturnDocumentation("回傳所有陣列除了 -1 以後的值")]
        short[] GetIndex();
        /// <summary>
        /// 取得目前 Codesys 人機操控面板 IO 狀態
        /// </summary>
        /// <returns>目前 Codesys 操控面板 IO 狀態</returns>
        [OperationContract]
        [WsdlDocumentation("鑽頭斷電前是否接觸工件")]
        [return: WsdlParamOrReturnDocumentation("接觸回傳 true，沒有接觸則回傳 false。")]
        bool GetMaterialTouch();
        /// <summary>
        /// 取得目前 Codesys 人機操控面板 IO 狀態
        /// </summary>
        /// <returns>目前 Codesys 操控面板 IO 狀態</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetPanel")]
        [WsdlDocumentation("取得目前 Codesys 人機操控面板 IO 狀態")]
        [return: WsdlParamOrReturnDocumentation("取得目前 Codesys 人機操控面板 IO 狀態")]
        PanelButton GetPanel();

        /// <summary>
        /// 取得 Codesys 刀庫設定
        /// </summary>
        /// <returns>用戶在 Codesys 設定的刀庫</returns>
        [OperationContract]
        [WsdlDocumentation("取得 Codesys 刀庫設定")]
        [return: WsdlParamOrReturnDocumentation("用戶在 Codesys 設定的刀庫")]
        GD_STD.DrillWarehouse GetDrillWarehouse();

        /// <summary>
        /// 取得 Codesys 油壓系統參數
        /// </summary>
        /// <returns>用戶在 Codesys 設定的油壓系統參數</returns>
        [OperationContract]
        [WsdlDocumentation("取得 Codesys 油壓系統參數")]
        [return: WsdlParamOrReturnDocumentation("用戶在 Codesys 設定的完整油壓系統參數")]
        OillSystem GetOill();

        /// <summary>
        /// 取得 Codesys 液壓油系統參數
        /// </summary>
        /// <returns>用戶在 Codesys 設定的液壓油系統參數</returns>
        [OperationContract]
        [WsdlDocumentation("取得 Codesys 液壓油系統參數")]
        [return: WsdlParamOrReturnDocumentation("用戶在 Codesys 設定的液壓油系統參數")]
        HydraulicSystem[] GetHydraulic();

        /// <summary>
        /// 取得 Codesys 潤滑油系統參數
        /// </summary>
        /// <returns>用戶在 Codesys 設定的潤滑油系統參數</returns>
        [OperationContract]
        [WsdlDocumentation("取得 Codesys 潤滑油系統參數")]
        [return: WsdlParamOrReturnDocumentation("用戶在 Codesys 設定的潤滑油系統參數")]
        LubricantSystem GetLubricant();

        /// <summary>
        /// 取得 Codesys 切削油系統參數
        /// </summary>
        /// <returns>用戶在 Codesys 設定的切削油系統參數</returns>
        [OperationContract]
        [WsdlDocumentation("取得 Codesys 切削油系統參數")]
        [return: WsdlParamOrReturnDocumentation("用戶在 Codesys 設定的切削油系統參數")]
        CutOilSystem GetCut();
        //TODO:先註解等待小霖完成
        ///// <summary>
        ///// 取得 Input 訊號
        ///// </summary>
        ///// <returns>目前 Codesys input 訊號</returns>
        //[OperationContract]
        //[WsdlDocumentation("取得 Input 訊號")]
        //[return: WsdlParamOrReturnDocumentation("目前 Codesys input 訊號")]
        //Input GetInput();
        //TODO:先註解等待小霖完成
        ///// <summary>
        ///// 取得 Output 訊號
        ///// </summary>
        ///// <returns>目前 Codesys Output 訊號</returns>
        //[OperationContract]
        //[WsdlDocumentation("取得 Output 訊號")]
        //[return: WsdlParamOrReturnDocumentation("目前 Codesys Output 訊號")]
        //Output GetOutput();

        /// <summary>
        /// 取得 Codesys 軸向訊息
        /// </summary>
        /// <returns>目前 Codesys Output 訊號</returns>
        [OperationContract]
        [WsdlDocumentation("取得目前 Codesys 軸向訊息")]
        [return: WsdlParamOrReturnDocumentation("目前 Codesys 軸向訊息")]
        AxisInfo GetAxisInfo();

        /// <summary>
        /// 取得 PC 與 Codesys 主機交握狀態
        /// </summary>
        /// <returns>目前 PC 與 Codesys 主機交握狀態</returns>
        [OperationContract]
        [WsdlDocumentation("取得 PC 與 Codesys 主機交握狀態")]
        [return: WsdlParamOrReturnDocumentation("目前 PC 與 Codesys 主機交握狀態")]
        Host GetHost();
        /// <summary>
        /// 取得斷電保持數值
        /// </summary>
        /// <returns>目前的斷電保持數值</returns>
        [OperationContract]
        [WsdlDocumentation("取得斷電保持數值")]
        [return: WsdlParamOrReturnDocumentation("目前的斷電保持數值")]
        Outage GetOutage();
        /// <summary>
        /// 取得其他錯誤代碼
        /// </summary>
        /// <returns>文字錯誤代碼</returns>
        [OperationContract]
        [WsdlDocumentation("取得其他錯誤代碼")]
        [return: WsdlParamOrReturnDocumentation("文字錯誤代碼")]
        string GetUnknownCode();
        /// <summary>
        /// 取得 APP 手動操作
        /// </summary>
        /// <returns>APP 手動操作狀態</returns>
        [OperationContract]
        [WsdlDocumentation("取得 APP 手動操作")]
        [return: WsdlParamOrReturnDocumentation("取得 APP 手動操作")]
        GD_STD.Phone.APP_Struct GetAPP_Struct();
        ///// <summary>
        ///// 取得 APP 加工監控"
        ///// </summary>
        ///// <returns>APP 加工監控參數</returns>
        //[OperationContract]
        //[WsdlDocumentation("取得 APP 加工監控")]
        //[return: WsdlParamOrReturnDocumentation("APP 加工監控參數")]
        //GD_STD.Phone.MonitorWork GetAPPMonitorWork();

        /// <summary>
        /// 取得 APP 及時監控
        /// </summary>
        /// <returns>APP 及時監控參數</returns>
        [OperationContract]
        [WsdlDocumentation("取得 APP 及時監控")]
        [return: WsdlParamOrReturnDocumentation("APP 及時監控參數")]
        GD_STD.Phone.MonitorMec GetAPPMonitorMec();

        /// <summary>
        /// 取得 APP 控制訊號
        /// </summary>
        /// <returns>APP 控制訊號</returns>
        [OperationContract]
        [WsdlDocumentation("取得 APP 控制訊號")]
        [return: WsdlParamOrReturnDocumentation("APP 控制訊號")]
        GD_STD.Phone.Operating GetOperating();
        /// <summary>
        /// 取得專案名稱
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WsdlDocumentation("取得專案名稱")]
        [return: WsdlParamOrReturnDocumentation("如果沒專案回傳 null，有專案則回傳 MonitorWork.ProjectName。")]
        string GetProjectName();
        /// <summary>
        /// 取得 <see cref="GD_STD.Phone.MonitorWork.WorkMaterial"/> 列表索引位置
        /// </summary>
        /// <returns><see cref="GD_STD.Phone.MonitorWork.WorkMaterial"/>[index]</returns>
        [OperationContract]
        [WsdlDocumentation("取得 GD_STD.Phone.MonitorWork.WorkMaterial 列表索引位置")]
        [return: WsdlParamOrReturnDocumentation("GD_STD.Phone.MonitorWork.WorkMaterial[index]")]
        GD_STD.Phone.WorkMaterial GetWorkMaterial(ushort index);
        ///// <summary>
        ///// 取得加工陣列內容
        ///// </summary>
        ///// <param name="offset">偏移量</param>
        ///// <param name="size">讀取大小</param>
        //object GetMonitorWorkOffset([WsdlParamOrReturnDocumentation("偏移量")] long offset, [WsdlParamOrReturnDocumentation("讀取大小")] int size);
        /// <summary>
        /// 取得即時訊息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WsdlDocumentation("取得即時訊息")]
        [return: WsdlParamOrReturnDocumentation("Codesys 回復訊息 Unicode UTF-16")]
        ushort[] GetInstantMessage();
        [OperationContract]
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IReadMemor.GetAndroidTest()' 的 XML 註解
        AndroidTest GetAndroidTest();
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IReadMemor.GetAndroidTest()' 的 XML 註解
    }
}

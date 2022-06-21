using GD_STD.Enum;
using System.ServiceModel;
using Wsdl;

namespace CodesysIIS
{
    /// <summary>
    /// 手機連線服務合約
    /// </summary>
    [ServiceContract(Namespace = "http://Codesys.PhoneConnectDuplex", SessionMode = SessionMode.Required, CallbackContract = typeof(IPhoneConnectDuplexCallback))]
    [WsdlDocumentation("手機連線服務合約")]
    public interface IPhoneConnectDuplex
    {
        /// <summary>
        /// 控制聆聽端是否繼續監聽工作
        /// </summary>
        /// <param name="start">啟動傳入 true，暫停則傳入false。</param>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("控制聆聽端是否繼續監聽工作")]
        void RunListening([WsdlParamOrReturnDocumentation("啟動傳入 true，暫停則傳入false")] bool start);

        /// <summary>
        /// 設定在指定的毫秒數內暫止聆聽的執行緒。
        /// </summary>
        /// <param name="millisecondsTimeout">暫止執行緒的毫秒數。 </param>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("設定在指定的毫秒數內暫止聆聽的執行緒")]
        void SetSleepListening([WsdlParamOrReturnDocumentation("暫止執行緒的毫秒數")] int millisecondsTimeout);
        /// <summary>
        /// 回復 Phone 連線請求
        /// </summary>
        /// <param name="satus">User 回復 Phone 的請求</param>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("回復 Phone 連線請求")]
        void ReplyConnect([WsdlParamOrReturnDocumentation("User 回復 Phone 的請求")] PHONE_SATUS satus);
        /// <summary>
        /// 等待手機登入訊息。
        /// </summary>
        /// <param name="start">執行續狀態</param>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("等待手機登入帳號")]
        void WaitLogin([WsdlParamOrReturnDocumentation("啟動傳入 true，暫停則傳入 false。")] bool start);

    }
}
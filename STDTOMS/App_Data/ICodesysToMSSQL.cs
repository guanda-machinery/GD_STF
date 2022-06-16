using GD_STD.MS;
using System.ServiceModel;
using System.ServiceModel.Web;
namespace STDTOMS
{
    /// <summary>
    /// Codesys 資料傳送到 MSSQL
    /// </summary>
    [ServiceContract]
    public interface ICodesysToMSSQL
    {
        /// <summary>
        /// 插入 IO資訊到 MSIO 表內
        /// </summary>
        /// <param name="ms"></param>
        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "InsertMSIO")]
        void InsertMSIO(MSIO ms);

        /// <summary>
        /// 插入 IO資訊到 MSAxisMain 表內
        /// </summary>
        /// <param name="ms"></param>
        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "InsertMSAxisMain")]
        void InsertMSAxisMain(MSAxisMain ms);
        /// <summary>
        /// 插入 IO資訊到 MSAxisMain 表內
        /// </summary>
        /// <param name="ms"></param>
        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "InsertMSServoAxis")]
        void InsertMSServoAxis(MSServoAxis ms);
        /// <summary>
        /// 插入 IO資訊到 MSRuler 表內
        /// </summary>
        /// <param name="ms"></param>
        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "InsertMSRuler")]
        void InsertMSRuler(MSRuler ms);
    }
}

using GD_STD;
using GD_STD.Enum;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using Wsdl;

namespace CodesysIIS
{
    /// <summary>
    /// 創建 IIS 與 Codesys 共享記憶體的服務合約
    /// </summary>
    [ServiceContract(Namespace = "http://Codesys.Memory")]
    [WsdlDocumentation("創建 IIS 與 Codesys 共享記憶體的服務合約")]
    public interface IMemor
    {
        [OperationContract]
        void IniWork();
        /// <summary>
        /// 創建與 Codesys  共享的記憶體
        /// </summary>
        /// <param name="host"></param>
        /// <param name="company"></param>
        /// <returns>回傳目前 PC 與 Codesys 主機狀態</returns>
        [OperationContract]
        [WsdlDocumentation("創建與 Codesys  共享的記憶體")]
        [return: WsdlParamOrReturnDocumentation("回傳 PC 與 Codesys 本機狀態")]
        Host Create([WsdlParamOrReturnDocumentation("PC 狀態")] Host host, [WsdlParamOrReturnDocumentation("公司名稱")] string company);
        /// <summary>
        /// 取得斷電保持電量
        /// </summary>
        /// <returns>回傳主機斷電保持的電量</returns>
        [OperationContract]
        [WsdlDocumentation("取得斷電保持電量")]
        [return: WsdlParamOrReturnDocumentation("回傳主機斷電保持的電量")]
        BAVT BAVT();
        /// <summary>
        /// 取得加工監控指定欄位 offset 
        /// </summary>
        /// <returns>回傳主機斷電保持的電量</returns>
        [OperationContract]
        [WsdlDocumentation("取得加工監控指定欄位 offset")]
        [return: WsdlParamOrReturnDocumentation("offset")]
        int GetMonitorWorkOffset(string FieldName);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wsdl;

namespace HMI
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IExternalImport"。
    [ServiceContract]
    public interface IExternalImport
    {
        /// <summary>
        /// 讀取資料夾內資料夾
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("控制聆聽端是否繼續監聽工作")]
        void ReadDirectory(string path);

    }
    public interface ImportDuplexCallback
    {
        /// <summary>
        /// 回饋資料夾路徑
        /// </summary>
        /// <returns></returns>
        string ResponseDirectory();

    }
}

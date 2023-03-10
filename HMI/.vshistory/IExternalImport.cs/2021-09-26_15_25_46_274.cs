using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wsdl;

namespace HMI
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IExternalImport"。
    [ServiceContract(Namespace = "http://HMI.ExternalImportDuplex", SessionMode = SessionMode.Required, CallbackContract = typeof(IImportDuplexCallback))]
    public interface IExternalImport
    {
        /// <summary>
        /// 取得根目錄
        /// </summary>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("取得遠端根目錄")]
        void RootDirectory();
        /// <summary>
        /// 讀取資料夾內資料夾
        /// </summary>
        /// <param name="path">Client 要讀取資料夾的路徑</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("取得遠端資料夾目錄")]
        void ReadDirectory([WsdlParamOrReturnDocumentation("要讀取資料夾的路徑")] string path);
        /// <summary>
        /// 創建資料流
        /// </summary>
        /// <param name="path">Client 要儲存的路徑</param>
        /// <param name="projectName">Client 專案名稱</param>
        /// <param name="length">Client 發送的 byte[] 長度</param>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("創建資料流")]
        void CreateFile(
            [WsdlParamOrReturnDocumentation("要儲存的路徑")] string path,
            [WsdlParamOrReturnDocumentation("專案名稱")] string projectName,
            [WsdlParamOrReturnDocumentation("發送的 byte[] 長度")] int length);
        /// <summary>
        /// 寫入資料流
        /// </summary>
        /// <param name="data">Client 發送的資料</param>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("寫入資料流")]
        void WriteFile([WsdlParamOrReturnDocumentation("要發送的資料")] byte[] data);

    }
    /// <summary>
    /// 匯入檔案雙向通道
    /// </summary>
    public interface IImportDuplexCallback
    {
        /// <summary>
        /// 遠端創建檔案結果
        /// </summary>
        /// <param name="result">創建完成 true，重複檔案 false。</param>
        void CreateFileResult(bool result);
        /// <summary>
        ///  資料夾路徑
        /// </summary>
        /// <param name="path">回傳路徑</param>
        [OperationContract(IsOneWay = true)]
        void ResponseDirectory(string[] path);
        /// <summary>
        /// 寫入資料流位置
        /// </summary>
        /// <param name="position">Server 寫入完成的位置</param>
        /// <param name="schedule">上傳進度</param>
        [OperationContract(IsOneWay = true)]
        void WriteStream(long position, long schedule);
        /// <summary>
        /// 衝突比對
        /// </summary>
        /// <param name="list">衝突物件</param>
        [OperationContract(IsOneWay = true)]
        void Conflict(SteelPart[] list);
        /// <summary>
        /// 結束寫入資料流
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void EndFile();
    }
}

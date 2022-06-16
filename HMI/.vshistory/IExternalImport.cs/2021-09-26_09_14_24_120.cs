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
    [ServiceContract(Namespace = "http://Codesys.PhoneConnectDuplex", SessionMode = SessionMode.Required, CallbackContract = typeof(ImportDuplexCallback))]
    public interface IExternalImport
    {
        /// <summary>
        /// 回傳 client 通道
        /// </summary>
        ImportDuplexCallback Callback { get; }
        /// <summary>
        /// 伺服器儲存檔案的路徑
        /// </summary>
        string SavePath { get; set; }
        /// <summary>
        /// 上傳進度
        /// </summary>
        int Schedule { get; set; }
        /// <summary>
        /// 上傳檔案資料 byte 長度
        /// </summary>
        int DataLength { get; set; }
        /// <summary>
        /// 用戶傳送的資料流 (壓縮檔)
        /// </summary>
        FileStream File { get; set; }
        /// <summary>
        /// 讀取資料夾內資料夾
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("取得遠端資料夾目錄")]
        void Directory(string path);
        /// <summary>
        /// 創建資料流
        /// </summary>
        /// <param name="path">創建資料流的路徑</param>
        /// <param name="path">資料流長度</param>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("創建資料流")]
        void CreateFile(string path, int length);
        /// <summary>
        /// 寫入資料流
        /// </summary>
        /// <param name="data"></param>
        /// <param name="writePosition">Client 寫入的位置</param>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("寫入資料流")]
        void WriteFile(byte[] data, int writePosition);
        /// <summary>
        /// 結束寫入資料流
        /// </summary>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("結束寫入資料流")]
        void EndFile();
    }
    public interface ImportDuplexCallback
    {
        /// <summary>
        /// 遠端創建資料夾的結果
        /// </summary>
        /// <param name="result"></param>
        void CreateStreamResult(bool result);
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
        [OperationContract(IsOneWay = true)]
        void WriteStream(int position);
        /// <summary>
        /// 衝突比對
        /// </summary>
        /// <param name="list">衝突物件</param>
        [OperationContract(IsOneWay = true)]
        void Conflict(SteelPart[] list);
    }
}

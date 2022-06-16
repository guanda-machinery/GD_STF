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
    [ServiceContract]
    public interface IExternalImport
    {
        /// <summary>
        /// 儲存路徑
        /// </summary>
        string _SavePath { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        FileStream Stream { get; set; }
        /// <summary>
        /// 讀取資料夾內資料夾
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = true)]
        [WsdlDocumentation("取得遠端資料夾目錄")]
        void Directory(string path);
    }
    public interface ImportDuplexCallback
    {
        /// <summary>
        ///  資料夾路徑
        /// </summary>
        /// <param name="path">回傳路徑</param>
        [OperationContract(IsOneWay = true)]
        void ResponseDirectory(string[] path);
        /// <summary>
        /// 寫入資料流
        /// </summary>
        /// <param name="data"></param>
        [OperationContract(IsOneWay = true)]
        void WriteStream(byte[] data);
        /// <summary>
        /// 衝突比對
        /// </summary>
        /// <param name="list"></param>
        [OperationContract(IsOneWay = true)]
        void Conflict(SteelPart[] list);
    }
}

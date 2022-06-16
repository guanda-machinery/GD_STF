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
    /// <summary>
    /// 辦公室檔案匯出到加工機台
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ExternalImport : IExternalImport
    {
        /// <summary>
        /// 回傳 client 通道
        /// </summary>
        IImportDuplexCallback _Callback { get; }
        /// <summary>
        /// 伺服器儲存檔案的路徑
        /// </summary>
        string _SavePath { get; set; }
        /// <summary>
        /// 專案名稱
        /// </summary>
        string _ProjectName { get; set; }
        /// <summary>
        /// 上傳進度
        /// </summary>
        int _Schedule { get; set; }
        /// <summary>
        /// 上傳檔案資料 byte 長度
        /// </summary>
        int _DataLength { get; set; }
        /// <summary>
        /// Client 傳送的資料流 (壓縮檔)
        /// </summary>
        FileStream File { get; set; }
        /// <inheritdoc/>
        public void CreateFile(string path, string projectName, int length)
        {
            _SavePath = path; //存取路徑
            _DataLength = length; //資料長度
            _ProjectName = projectName; //專案名稱
            if (!Directory.Exists($@"{_SavePath}\{projectName}"))//沒有相同檔案
            {
                File = new FileStream();
                _Callback.CreateFileResult(true);
            }
            else //有相同檔案
            {
                _Callback.CreateFileResult(false);
            }
        }
        /// <inheritdoc/>
        public void ReadDirectory(string path)
        {
            _Callback.ResponseDirectory(Directory.GetDirectories(path)); //回傳目前路徑內部的資料夾給 Client 端
        }
        /// <inheritdoc/>
        public void WriteFile(byte[] data, int writePosition)
        {
            
        }
    }
}

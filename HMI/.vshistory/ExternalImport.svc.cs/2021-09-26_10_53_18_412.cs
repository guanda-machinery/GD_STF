using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wsdl;
using static GD_STD.ServerLogHelper;
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
        private IImportDuplexCallback _Callback { get; }
        /// <summary>
        /// 伺服器儲存檔案的路徑
        /// </summary>
        private string _SavePath { get; set; }
        /// <summary>
        /// 專案名稱
        /// </summary>
        private string _ProjectName { get; set; }
        /// <summary>
        /// 暫存檔案名稱
        /// </summary>
        private string _BufferName { get; set; }
        /// <summary>
        /// 上傳進度
        /// </summary>
        private int _Schedule
        { get; set; }
        /// <summary>
        /// 上傳檔案資料 byte 長度
        /// </summary>
        private int _DataLength { get; set; }
        /// <summary>
        /// Client 傳送的資料流 (壓縮檔)
        /// </summary>
        private FileStream _File { get; set; }

        public void CreateFile([WsdlParamOrReturnDocumentation("要儲存的路徑")] string path,
                               [WsdlParamOrReturnDocumentation("專案名稱")] string projectName,
                               [WsdlParamOrReturnDocumentation("發送的 byte[] 長度")] int length)
        {
            throw new NotImplementedException();
        }

        public void ReadDirectory([WsdlParamOrReturnDocumentation("要讀取資料夾的路徑")] string path)
        {
            throw new NotImplementedException();
        }

        public void WriteFile([WsdlParamOrReturnDocumentation("要發送的資料")] byte[] data)
        {
            throw new NotImplementedException();
        }
        ///// <inheritdoc/>
        //public void CreateFile(string path, string projectName, int length)
        //{
        //    if (string.IsNullOrEmpty(path))
        //        throw new ArgumentException($"'{nameof(path)}' 不可為 Null 或空白。", nameof(path));
        //    if (string.IsNullOrEmpty(projectName))
        //        throw new ArgumentException($"'{nameof(projectName)}' 不可為 Null 或空白。", nameof(projectName));
        //    try
        //    {
        //        _SavePath = path; //存取路徑
        //        _ProjectName = projectName;//專案名稱
        //        _DataLength = length; //資料長度
        //        _BufferName = Guid.NewGuid().ToString();//暫存檔案名稱
        //        _File = new FileStream($@"{_SavePath}\{_BufferName}.db", FileMode.Create);//創建壓縮檔
        //        bool result = true;
        //        if (Directory.Exists($@"{_SavePath}\{projectName}"))//有相同檔案
        //        {
        //            result = false;
        //        }

        //        WriteInfo(WriteMemorLog, "沒有相同檔案", $@"儲存路徑：{_SavePath}\n專案名稱：{_ProjectName}\n資料長度：{_DataLength}\n暫存檔案名稱：{_BufferName}");
        //        _Callback.CreateFileResult(result); //傳送結果給 Client
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteError(WriteMemorLog, ex);
        //        throw;
        //    }

        //}
        ///// <inheritdoc/>
        //public void ReadDirectory(string path)
        //{
        //    string[] result = Directory.GetDirectories(path); //取得指定路徑的資料夾
        //    WriteInfo(WriteMemorLog, result.Aggregate((str1, str2) => $@"{str1}\n{str2}")); //寫入所有資料夾
        //    _Callback.ResponseDirectory(result); //回傳目前路徑內部的資料夾給 Client 端
        //}
        ///// <inheritdoc/>
        //public void WriteFile(byte[] data)
        //{
        //    long current = _File.Position; //資料流的目前位置
        //    _File.Write(data, 0, data.Length);//寫入文件
        //    WriteInfo(WriteMemorLog, $@"資料目前位置：{current}\n寫入位置：{_File.Position}"); //目前文件寫入位置

        //}
    }
}
